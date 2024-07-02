using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WeaponSettings : MonoBehaviour
{
    [SerializeField] private int Damage;

    void Start()
    {
        Debug.Log("Weapon damage: " + Damage);
    }
}
