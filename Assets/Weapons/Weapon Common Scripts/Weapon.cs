using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int damage;
    public float cooldown;
    public Vector2 direction {  get; set; }

    // Абстрактный метод attack, который должен быть реализован в наследниках
    public abstract void Attack();
}
