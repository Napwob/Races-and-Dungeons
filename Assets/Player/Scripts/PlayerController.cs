using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private int playerHealth;

    public float moveSpeed;
    private float speedX, speedY;

    private Vector2 input;

    Rigidbody2D rb;

    private Animator animator;

    private EquipedController equiped;
    private SpriteRenderer rbSprite;

    public HealthBarController healthBar;

    private void Start()
    {
        Vector2 position = transform.position;
        position.y = 0;
        position.x = 0;
        transform.position = position;

        playerHealth = 20;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        equiped = GetComponentInChildren<EquipedController>();
        rbSprite = GetComponent<SpriteRenderer>();

        healthBar = FindObjectOfType<HealthBarController>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            rbSprite.color = Color.red;
            playerHealth--;
            healthBar.updateHealthBar(playerHealth);
            StartCoroutine(DelayDamage(0.15f));
        }
    }

    private IEnumerator DelayDamage(float delay)
    {
        yield return new WaitForSeconds(delay);
        rbSprite.color = Color.white;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();

        if (playerHealth == 0)
            Death();
    }

    private void HandleMovement()
    {
        input.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        input.y = Input.GetAxisRaw("Vertical") * moveSpeed;
        rb.velocity = input;
        animator.SetBool("isMoving", input != Vector2.zero);
    }

    private void HandleRotation()
    {
        Vector2 pointerInput = GetPointerInput();
        equiped.PointerPosition = pointerInput;

        transform.rotation = Quaternion.Euler(0f, pointerInput.x < transform.position.x ? 180f : 0f, 0f);
    }

    private IEnumerator FadeOut(float fadeTime)
    {
        float timer = fadeTime;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            float alpha = timer / fadeTime;
            Color objectColor = renderer.color;
            objectColor.a = alpha;
            renderer.color = objectColor;

            yield return null;
        }

        Destroy(gameObject);
    }

    void Death()
    {
        animator.SetTrigger("isDead");
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rbSprite.sortingOrder = 1;

        StartCoroutine(FadeOut(1f));
    }

    public Vector2 GetPointerInput()
    { 
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
