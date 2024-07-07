using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponControllerRangeBase : Weapon
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10;

    [SerializeField] private Vector3 startPosition;

    private Animator animator;
    private bool attackBlocked;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Debug.Log("Me loaded");
    }

    void shootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position + startPosition, Quaternion.identity);
        bullet.transform.localScale = bulletPrefab.transform.localScale;
        DamageController bulletScript = bullet.GetComponent<DamageController>();
        bulletScript.Damage = damage;
        bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed;
    }

    public override void Attack()
    {
        if (attackBlocked)
            return;

        animator.SetTrigger("Attack");
        shootBullet();
        attackBlocked = true;

        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(cooldown);
        attackBlocked = false;
    }
}
