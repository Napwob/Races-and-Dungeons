using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float delay = 0.15f;

    [SerializeField] private bool distanceWeapon;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10;

    [SerializeField] private Vector3 startPosition;

    public int Damage;

    private Animator animator;
    private bool attackBlocked;
    private CapsuleCollider2D blade;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        blade = GetComponent<CapsuleCollider2D>();
        if (blade != null )
        {
            blade.enabled = false;
        }

        Debug.Log("Me loaded");
    }

    void shootBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position + startPosition, Quaternion.identity);
        BulletCotroller bulletScript = bullet.GetComponent<BulletCotroller>();
        bulletScript.Damage = Damage;
        bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed;
    }

    public void Attack(Vector2 direction)
    {
        if (attackBlocked)
            return;

        animator.SetTrigger("Attack");

        if (distanceWeapon)
            shootBullet(direction);
        else
            blade.enabled = true;

        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
        blade.enabled = false;
    }
}
