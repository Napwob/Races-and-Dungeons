using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthStatus : MonoBehaviour
{
    [SerializeField] private int health;

    private Animator animator;
    private PlayerFollow PlayerFollow;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;

    private status enemyStatus;
    enum status
    {
        None,
        onFire,
        onDamage,
        onPoison
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            BulletCotroller bulletScript = collision.gameObject.GetComponent<BulletCotroller>();
            if (bulletScript != null)
            {
                health -= bulletScript.damage;
                enemyStatus = status.onDamage;
            }

            Debug.Log("bulletScript damage: " + bulletScript.damage);
            Debug.Log("healthe: " + health);
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
                    break;
                case status.onDamage:
                    rbSprite.color = Color.red;
                    StartCoroutine(DelayDamage(0.15f));
                    break;
                case status.onPoison:
                    rbSprite.color = Color.green;
                    break;
                default:
                    Debug.LogError("No such status");
                    break;
            }
        }

        if (health == 0) 
            Death();
    }

    private IEnumerator DelayDamage(float delay)
    {
        yield return new WaitForSeconds(delay);
        rbSprite.color = Color.white;
        enemyStatus = status.None;
    }
}
