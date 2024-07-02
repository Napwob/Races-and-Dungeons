using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private float speedX, speedY;

    private Vector2 input;

    Rigidbody2D rb;

    private Animator animator;

    private EquipedController equiped;

    private void Awake()
    {
        Vector2 position = transform.position;
        position.y = 0;
        position.x = 0;
        transform.position = position;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        equiped = GetComponentInChildren<EquipedController>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        input.y = Input.GetAxisRaw("Vertical") * moveSpeed;
        Vector2 pointerInput = GetPointerInput();

        equiped.PointerPosition = pointerInput;

        var rotation = transform.rotation;
        if (pointerInput.x - transform.position.x < 0)
        {
            rotation.y = -180;
        }
        else
        if (pointerInput.x - transform.position.x > 0)
        {
            rotation.y = 0;
        }
        transform.root.rotation = rotation;

        rb.velocity = input;

        if (input != Vector2.zero)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }

    public Vector2 GetPointerInput()
    { 
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
