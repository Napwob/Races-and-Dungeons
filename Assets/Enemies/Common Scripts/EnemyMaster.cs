using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaster : MonoBehaviour
{
    public static int enemyCount {  get; set; }

    private void Awake()
    {
        enemyCount++;
        Debug.Log("enemyCount: " + enemyCount);
    }
}
