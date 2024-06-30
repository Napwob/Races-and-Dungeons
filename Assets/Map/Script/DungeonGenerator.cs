using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DungeonGenerator : MonoBehaviour
{
    [Range(1, 30)] public int roomNumber;

    [SerializeField] public bool randomCorridors;
    [Range(1, 60)] public int coridorsNumber;

    [Range(4, 200)] public int dungeonSize;

    [SerializeField] private Tilemap floor;
    [SerializeField] private Tile[] floorTiles;
    [SerializeField] private Tilemap walls;

    [SerializeField] private Tile[] wallTiles;

    [SerializeField] private Tilemap obejcts;

    [SerializeField] private GameObject[] spawner;

    private static bool playerRoomAdded;
    [SerializeField] private bool spawnEnemies;

    struct coord
    {
        public int x, y;
    }

    private void drawWalls()
    {
        BoundsInt groundBounds = floor.cellBounds;

        for (int x = groundBounds.xMin; x < groundBounds.xMax; x++)
        {
            for (int y = groundBounds.yMin; y < groundBounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (floor.HasTile(pos))
                {
                    if (!floor.HasTile(new Vector3Int(x - 1, y, 0)))
                    {
                        walls.SetTile(new Vector3Int(x - 1, y, 0), wallTiles[0]);
                    }
                    if (!floor.HasTile(new Vector3Int(x + 1, y, 0)))
                    {
                        walls.SetTile(new Vector3Int(x + 1, y, 0), wallTiles[1]);
                    }
                    if (!floor.HasTile(new Vector3Int(x, y - 1, 0)))
                    {
                        walls.SetTile(new Vector3Int(x, y - 1, 0), wallTiles[2]);
                    }
                    if (!floor.HasTile(new Vector3Int(x, y + 1, 0)))
                    {
                        walls.SetTile(new Vector3Int(x, y + 1, 0), wallTiles[3]);
                    }
                }
            }
        }

        BoundsInt wallBounds = walls.cellBounds;

        for (int x = wallBounds.xMin; x < wallBounds.xMax; x++)
        {
            for (int y = wallBounds.yMin; y < wallBounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (!walls.HasTile(pos))
                {
                    if (walls.HasTile(new Vector3Int(x - 1, y, 0)) && 
                        walls.HasTile(new Vector3Int(x, y + 1, 0)) &&
                        floor.HasTile(new Vector3Int(x - 1, y + 1, 0))
                        )
                    {
                        walls.SetTile(pos, wallTiles[4]);
                        continue;
                    }

                    if (walls.HasTile(new Vector3Int(x + 1, y, 0)) &&
                        walls.HasTile(new Vector3Int(x, y + 1, 0)) &&
                        floor.HasTile(new Vector3Int(x + 1, y + 1, 0))
                        )
                    {
                        walls.SetTile(pos, wallTiles[5]);
                        continue;
                    }

                    if (walls.HasTile(new Vector3Int(x + 1, y, 0)) &&
                        walls.HasTile(new Vector3Int(x, y - 1, 0)) &&
                        !walls.HasTile(new Vector3Int(x + 1, y - 1, 0)) &&
                        !floor.HasTile(new Vector3Int(x - 1, y, 0)) &&
                        !floor.HasTile(new Vector3Int(x, y + 1, 0))
                        )
                    {
                        walls.SetTile(pos, wallTiles[0]);
                        continue;
                    }

                    if (walls.HasTile(new Vector3Int(x - 1, y, 0)) &&
                        walls.HasTile(new Vector3Int(x, y - 1, 0)) &&
                        !walls.HasTile(new Vector3Int(x - 1, y - 1, 0)) &&
                        !floor.HasTile(new Vector3Int(x + 1, y, 0)) &&
                        !floor.HasTile(new Vector3Int(x, y + 1, 0))
                        )
                    {
                        walls.SetTile(pos, wallTiles[1]);
                        continue;
                    }


                }
                else
                {
                    if (floor.HasTile(new Vector3Int(x, y + 1, 0)) &&
                        floor.HasTile(new Vector3Int(x, y - 1, 0))
                        )
                    {
                        walls.SetTile(pos, wallTiles[3]);
                        continue;
                    }

                    if (floor.HasTile(new Vector3Int(x + 1, y, 0)) &&
                        floor.HasTile(new Vector3Int(x - 1, y, 0))
                        )
                    {
                        walls.SetTile(pos, wallTiles[8]);
                        continue;
                    }

                    if (walls.HasTile(new Vector3Int(x - 1, y, 0)) &&
                        walls.HasTile(new Vector3Int(x, y + 1, 0)) &&
                        floor.HasTile(new Vector3Int(x + 1, y, 0)) &&
                        floor.HasTile(new Vector3Int(x, y - 1, 0))
                        )
                    {
                        walls.SetTile(pos, wallTiles[3]);
                        continue;
                    }

                    if (walls.HasTile(new Vector3Int(x + 1, y, 0)) &&
                        walls.HasTile(new Vector3Int(x, y - 1, 0)))
                        
                    {
                        walls.SetTile(pos, wallTiles[6]);
                        continue;
                    }

                    if (walls.HasTile(new Vector3Int(x - 1, y, 0)) &&
                        walls.HasTile(new Vector3Int(x, y - 1, 0)))
                        
                    {
                        walls.SetTile(pos, wallTiles[7]);
                        continue;
                    }

                }
            }
        }
    }

    static void setCorridorSegment(bool[,] array, int x, int y)
    {
        for (int i = x; i < x + 2; i++)
            for (int j = y; j < y + 2; j++)
                array[i, j] = true;
    }

    static bool CheckSquareFree(bool[,] array,
                                int x,
                                int y,
                                int sizeX,
                                int sizeY)
    {
        for (int i = x; i < x + sizeX; i++)
        {
            for (int j = y; j < y + sizeY; j++)
            {
                if (i >= array.GetLength(0) ||
                    j >= array.GetLength(1) ||
                    array[i, j] == true)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void setRoomLocation(bool[,] array, coord[] roomCoord, int roomNumber)
    {
        bool isSquareFree = false;

        if (!playerRoomAdded)
        {
            for (int i = array.GetLength(0) / 2 - 2; i < array.GetLength(0) / 2 + 2; i++)
            {
                for (int j = array.GetLength(1) / 2 - 2 / 2; j < array.GetLength(1) / 2 + 2; j++)
                {
                    array[i, j] = true;
                }
            }
            roomCoord[roomNumber].x = array.GetLength(0) / 2;
            roomCoord[roomNumber].y = array.GetLength(1) / 2;
            playerRoomAdded = true;

            return;
        }

        while (!isSquareFree)
        {
            int x = UnityEngine.Random.Range(0, array.GetLength(0));
            int y = UnityEngine.Random.Range(0, array.GetLength(1));

            int sizeX = UnityEngine.Random.Range(5, 20);
            int sizeY = UnityEngine.Random.Range(5, 20);

            isSquareFree = CheckSquareFree(array, x, y, sizeX, sizeY);
            if (isSquareFree)
            {
                roomCoord[roomNumber].x = x + sizeX / 2;
                roomCoord[roomNumber].y = y + sizeY / 2;
                for (int i = x; i < x + sizeX; i++)
                {
                    for (int j = y; j < y + sizeY; j++)
                    {
                        array[i, j] = true;
                    }
                }
            }
        }
    }

    void setCorridors(bool[,] array, coord[] roomCoord)
    {
        int selectedCorNumber = randomCorridors ? coridorsNumber : roomCoord.Length - 1;
        for (int i = 0; i < selectedCorNumber; i++)
        {
            int fromRoomIndex;
            int toRoomIndex;

            if (randomCorridors)
            {
                fromRoomIndex = UnityEngine.Random.Range(0, roomCoord.Length - 1);
                while (true)
                {
                    toRoomIndex = UnityEngine.Random.Range(0, roomCoord.Length - 1);
                    if (fromRoomIndex != toRoomIndex)
                        break;
                }
            }
            else
            {
                fromRoomIndex = i;
                toRoomIndex = i + 1;
            }
            
            coord fromRoom = roomCoord[fromRoomIndex];
            coord toRoom = roomCoord[toRoomIndex];

            while (true)
            {
                setCorridorSegment(array, fromRoom.x, fromRoom.y);
                if (fromRoom.x > toRoom.x)
                {
                    fromRoom.x--;
                    continue;
                }
                if (fromRoom.x < toRoom.x)
                {
                    fromRoom.x++;
                    continue;
                }

                if (fromRoom.y > toRoom.y)
                {
                    fromRoom.y--;
                    continue;
                }
                if (fromRoom.y < toRoom.y)
                {
                    fromRoom.y++;
                    continue;
                }

                if (fromRoom.x == toRoom.x && fromRoom.y == toRoom.y)
                    break;
            }
        }
    }

    void drawFloor(bool[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                int tileNumber = UnityEngine.Random.Range(0, floorTiles.Length);
                if (array[i, j])
                {
                    floor.SetTile(new Vector3Int(i - array.GetLength(0) / 2,
                                                 j - array.GetLength(1) / 2, 0),
                                                 floorTiles[tileNumber]);
                    if (spawnEnemies && UnityEngine.Random.Range(0, 10) > 8)
                    {
                        GameObject toSpawn = spawner[0];
                        Vector3 spawnPosition = new Vector3(i - array.GetLength(0) / 2, j - array.GetLength(1) / 2, 0);
                        Instantiate(toSpawn, spawnPosition, Quaternion.identity);
                    }
                }
            }
        }
    }

    void Start()
    {
        bool[,] array = new bool[dungeonSize, dungeonSize];
        coord[] roomCoord = new coord[roomNumber];

        for (int i = 0; i < roomNumber; i++)
            setRoomLocation(array, roomCoord, i);

        setCorridors(array, roomCoord);

        drawFloor(array);
        drawWalls();
    }
}
