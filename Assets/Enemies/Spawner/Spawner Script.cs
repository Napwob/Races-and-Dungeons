using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPrefab;

    // Start is called before the first frame update
    void Start()
    {
        int entityToSpawn = Random.Range(0, spawnPrefab.Length);
        GameObject toSpawn = spawnPrefab[entityToSpawn];
        Instantiate(toSpawn, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
