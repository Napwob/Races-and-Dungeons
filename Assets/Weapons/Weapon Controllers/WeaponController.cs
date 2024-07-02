using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Animator animator;
    public float delay = 0.15f;
    private bool attackBlocked;

    public bool distanceWeapon;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;

    [SerializeField] private Vector3 startPosition;

    [SerializeField] private int Damage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Debug.Log("Me loaded");
    }

    public void Attack(Vector2 direction)
    {
        if (attackBlocked)
            return;

        animator.SetTrigger("Attack");

        if (distanceWeapon)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position + startPosition, Quaternion.identity);
            BulletCotroller bulletScript = bullet.GetComponent<BulletCotroller>();
            bulletScript.damage = Damage;
            bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed;
        }

        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }
}
