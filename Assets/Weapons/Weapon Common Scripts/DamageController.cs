using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DamageController : MonoBehaviour
{
    public int Damage { set; get; }

    void Start()
    {
        Debug.Log("Weapon damage: " + Damage);
    }
}
