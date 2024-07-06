using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Generation Parameters")]
    [Range(1, 30)] public int roomNumber = 10;
    [SerializeField] public bool randomCorridors;
    [Range(1, 60)] public int corridorsNumber = 20;
    [Range(4, 200)] public int dungeonSize = 50;

    [Header("Tilemaps and Tiles")]
    [SerializeField] private Tilemap floor;
    [SerializeField] private Tile[] floorTiles;
    [SerializeField] private Tilemap walls;
    [SerializeField] private Tile[] wallTiles;

    [Header("Enemies")]
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private bool spawnEnemies;
    [SerializeField][Range(0, 100)] private int enemiesSpawnChance = 80;

    private static bool playerRoomAdded;

    private struct Coord
    {
        public int x, y;
    }

    void Start()
    {
        bool[,] dungeonArray = new bool[dungeonSize, dungeonSize];
        Coord[] roomCoords = new Coord[roomNumber];

        // Generate rooms
        GenerateRooms(dungeonArray, roomCoords);

        // Generate corridors
        GenerateCorridors(dungeonArray, roomCoords);

        // Draw the dungeon
        DrawDungeon(dungeonArray);
    }

    void GenerateRooms(bool[,] array, Coord[] roomCoords)
    {
        for (int i = 0; i < roomNumber; i++)
        {
            SetRoomLocation(array, roomCoords, i);
        }
    }

    void SetRoomLocation(bool[,] array, Coord[] roomCoords, int roomIndex)
    {
        bool isSquareFree = false;

        if (!playerRoomAdded)
        {
            // Place the player's starting room in the center
            int startX = array.GetLength(0) / 2;
            int startY = array.GetLength(1) / 2;

            for (int i = startX - 1; i <= startX + 1; i++)
            {
                for (int j = startY - 1; j <= startY + 1; j++)
                {
                    array[i, j] = true;
                }
            }

            roomCoords[roomIndex].x = startX;
            roomCoords[roomIndex].y = startY;
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
                roomCoords[roomIndex].x = x + sizeX / 2;
                roomCoords[roomIndex].y = y + sizeY / 2;
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

    void GenerateCorridors(bool[,] array, Coord[] roomCoords)
    {
        int selectedCorNumber = randomCorridors ? corridorsNumber : roomCoords.Length - 1;
        for (int i = 0; i < selectedCorNumber; i++)
        {
            int fromRoomIndex;
            int toRoomIndex;

            if (randomCorridors)
            {
                fromRoomIndex = UnityEngine.Random.Range(0, roomCoords.Length - 1);
                while (true)
                {
                    toRoomIndex = UnityEngine.Random.Range(0, roomCoords.Length - 1);
                    if (fromRoomIndex != toRoomIndex)
                        break;
                }
            }
            else
            {
                fromRoomIndex = i;
                toRoomIndex = i + 1;
            }

            Coord fromRoom = roomCoords[fromRoomIndex];
            Coord toRoom = roomCoords[toRoomIndex];

            while (true)
            {
                SetCorridorSegment(array, fromRoom.x, fromRoom.y);
                if (fromRoom.x > toRoom.x)
                {
                    fromRoom.x--;
                }
                else if (fromRoom.x < toRoom.x)
                {
                    fromRoom.x++;
                }
                else if (fromRoom.y > toRoom.y)
                {
                    fromRoom.y--;
                }
                else if (fromRoom.y < toRoom.y)
                {
                    fromRoom.y++;
                }

                if (fromRoom.x == toRoom.x && fromRoom.y == toRoom.y)
                    break;
            }
        }
    }

    void SetCorridorSegment(bool[,] array, int x, int y)
    {
        for (int i = x; i < x + 2; i++)
        {
            for (int j = y; j < y + 2; j++)
            {
                array[i, j] = true;
            }
        }
    }

    bool CheckSquareFree(bool[,] array, int x, int y, int sizeX, int sizeY)
    {
        for (int i = x; i < x + sizeX; i++)
        {
            for (int j = y; j < y + sizeY; j++)
            {
                if (i >= array.GetLength(0) ||
                    j >= array.GetLength(1) ||
                    array[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }

    void DrawDungeon(bool[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                int tileNumber = UnityEngine.Random.Range(0, floorTiles.Length);
                if (array[i, j])
                {
                    floor.SetTile(new Vector3Int(i - array.GetLength(0) / 2, j - array.GetLength(1) / 2, 0), floorTiles[tileNumber]);

                    if (spawnEnemies && UnityEngine.Random.Range(0, 100) > enemiesSpawnChance)
                    {
                        GameObject toSpawn = spawners[0];
                        Vector3 spawnPosition = new Vector3(i - array.GetLength(0) / 2, j - array.GetLength(1) / 2, 0);
                        Instantiate(toSpawn, spawnPosition, Quaternion.identity);
                    }
                }
            }
        }

        // Draw walls based on floor tiles
        DrawWalls();
    }

    void DrawWalls()
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
                        floor.HasTile(new Vector3Int(x - 1, y + 1, 0)))
                    {
                        walls.SetTile(pos, wallTiles[4]);
                    }
                    else if (walls.HasTile(new Vector3Int(x + 1, y, 0)) &&
                             walls.HasTile(new Vector3Int(x, y + 1, 0)) &&
                             floor.HasTile(new Vector3Int(x + 1, y + 1, 0)))
                    {
                        walls.SetTile(pos, wallTiles[5]);
                    }
                    else if (walls.HasTile(new Vector3Int(x + 1, y, 0)) &&
                             walls.HasTile(new Vector3Int(x, y - 1, 0)) &&
                             !walls.HasTile(new Vector3Int(x + 1, y - 1, 0)) &&
                             !floor.HasTile(new Vector3Int(x - 1, y, 0)) &&
                             !floor.HasTile(new Vector3Int(x, y + 1, 0)))
                    {
                        walls.SetTile(pos, wallTiles[0]);
                    }
                    else if (walls.HasTile(new Vector3Int(x - 1, y, 0)) &&
                             walls.HasTile(new Vector3Int(x, y - 1, 0)) &&
                             !walls.HasTile(new Vector3Int(x - 1, y - 1, 0)) &&
                             !floor.HasTile(new Vector3Int(x + 1, y, 0)) &&
                             !floor.HasTile(new Vector3Int(x, y + 1, 0)))
                    {
                        walls.SetTile(pos, wallTiles[1]);
                    }
                }
                else
                {
                    if (floor.HasTile(new Vector3Int(x, y + 1, 0)) &&
                        floor.HasTile(new Vector3Int(x, y - 1, 0)))
                    {
                        walls.SetTile(pos, wallTiles[3]);
                    }
                    else if (floor.HasTile(new Vector3Int(x + 1, y, 0)) &&
                             floor.HasTile(new Vector3Int(x - 1, y, 0)))
                    {
                        walls.SetTile(pos, wallTiles[8]);
                    }
                    else if (walls.HasTile(new Vector3Int(x - 1, y, 0)) &&
                             walls.HasTile(new Vector3Int(x, y + 1, 0)) &&
                             floor.HasTile(new Vector3Int(x + 1, y, 0)) &&
                             floor.HasTile(new Vector3Int(x, y - 1, 0)))
                    {
                        walls.SetTile(pos, wallTiles[3]);
                    }
                    else if (walls.HasTile(new Vector3Int(x + 1, y, 0)) &&
                             walls.HasTile(new Vector3Int(x, y - 1, 0)))
                    {
                        walls.SetTile(pos, wallTiles[6]);
                    }
                    else if (walls.HasTile(new Vector3Int(x - 1, y, 0)) &&
                             walls.HasTile(new Vector3Int(x, y - 1, 0)))
                    {
                        walls.SetTile(pos, wallTiles[7]);
                    }
                }
            }
        }
    }
}
