using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthStatus : MonoBehaviour
{
    [SerializeField] private int health;

    private Animator animator;
    PlayerFollow scriptToDisable;
    CircleCollider2D circleCollider;
    Rigidbody2D rb;
    SpriteRenderer rbSprite;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        scriptToDisable = GetComponent<PlayerFollow>();
        circleCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rbSprite = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletCotroller bulletScript = collision.gameObject.GetComponent<BulletCotroller>();
            if (bulletScript != null)
                health -= bulletScript.damage;

            Debug.Log("bulletScript damage: " + bulletScript.damage);
            Debug.Log("healthe: " + health);
        }
    }
    void Update()
    {
        if (health == 0)
        {
            scriptToDisable.enabled = false;
            circleCollider.enabled = false;
            animator.SetTrigger("isDead");
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rbSprite.sortingOrder = 1;
        }
    }
}
