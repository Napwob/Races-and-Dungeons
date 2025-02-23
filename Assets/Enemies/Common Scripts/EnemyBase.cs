using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int health;

    private Animator animator;
    private PlayerFollow PlayerFollow;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private Weapon weapon;

    private status enemyStatus;
    enum status
    {
        None,
        onFire,
        onDamage,
        onPoison,
        onIce
    };

    private void Awake()
    {
        animator = GetComponent<Animator>();
        PlayerFollow = GetComponent<PlayerFollow>();
        circleCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rbSprite = GetComponent<SpriteRenderer>();

        enemyStatus = status.None;  
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            Debug.Log("Triggered");
            int Damage = collision.gameObject.GetComponent<DamageController>().Damage;

            health -= Damage;
            enemyStatus = status.onDamage;

            return;
        }

        if (collision.gameObject.CompareTag("Fire"))
        {
            Debug.Log("Triggered");
            return;
        }

        if (collision.gameObject.CompareTag("Poison"))
        {
            Debug.Log("Triggered");
            return;
        }

        if (collision.gameObject.CompareTag("Ice"))
        {
            int Damage = collision.gameObject.GetComponent<DamageController>().Damage;

            health -= Damage;
            enemyStatus = status.onIce;

            Debug.Log("Triggered");
            return;
        }
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
        PlayerFollow.enabled = false;
        circleCollider.enabled = false;
        animator.SetTrigger("isDead");
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rbSprite.sortingOrder = 1;

        StartCoroutine(FadeOut(0.5f));
    }

    void Update()
    {
        if (enemyStatus != status.None)
        {
            switch (enemyStatus)
            {
                case status.onFire:
                    rbSprite.color = Color.yellow;
                    StartCoroutine(DelayDamage(0.5f));
                    break;
                case status.onDamage:
                    rbSprite.color = Color.red;
                    StartCoroutine(DelayDamage(0.15f));
                    break;
                case status.onPoison:
                    rbSprite.color = Color.green;
                    StartCoroutine(DelayDamage(1f));
                    break;
                case status.onIce:
                    rbSprite.color = Color.blue;
                    StartCoroutine(DelayDamage(2f));
                    break;
                default:
                    Debug.LogError("No such status");
                    break;
            }
        }

        if (health < 1) 
            Death();
    }

    private IEnumerator DelayDamage(float delay)
    {
        PlayerFollow.stunned = true;
        yield return new WaitForSeconds(delay);
        PlayerFollow.stunned = false;
        rbSprite.color = Color.white;
        enemyStatus = status.None;
    }
}
