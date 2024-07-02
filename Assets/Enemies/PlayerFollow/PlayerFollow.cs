using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerFollow : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    private GameObject player;
    private Rigidbody2D rb;

    private Animator animator;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance > 1 && distance < 5)
        {
            Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
            rb.velocity = direction * speed;

            var rotation = transform.rotation;
            if (direction.x < 0)
            {
                rotation.y = -180;
            }
            else
            if (direction.x > 0)
            {
                rotation.y = 0;
            }
            transform.root.rotation = rotation;
            if (animator)
                animator.SetBool("isMoving", true);
        }
        else
        {
            rb.velocity = Vector2.zero;
            if (animator)
                animator.SetBool("isMoving", false);
        }
    }
}
