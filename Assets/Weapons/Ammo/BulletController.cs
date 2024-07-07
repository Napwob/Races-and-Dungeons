using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
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
