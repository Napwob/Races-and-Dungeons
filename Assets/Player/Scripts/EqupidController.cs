using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }

    private MeleeWeaponController weapon;

    private void Awake()
    {
        weapon = GetComponentInChildren<MeleeWeaponController>();
    }

    void Update()
    {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (direction.x < 0 ) 
        {
            scale.y = -1;
        }
        else
        if (direction.x > 0 ) 
        {
            scale.y = 1;
        }
        transform.localScale = scale;

        if (Input.GetMouseButtonDown(0))
            weapon.Attack();
    }
}
