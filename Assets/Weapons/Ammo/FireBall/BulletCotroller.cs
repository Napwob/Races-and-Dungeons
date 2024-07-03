using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCotroller : MonoBehaviour
{
    public int Damage { set; get; }

    void Awake()
    {
        Destroy(gameObject, 4);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
       // if (collision.gameObject.CompareTag("Enemy"))
           // Destroy(collision.gameObject);
       if (!collision.gameObject.CompareTag("Player"))
            Destroy(gameObject);
    }
}
