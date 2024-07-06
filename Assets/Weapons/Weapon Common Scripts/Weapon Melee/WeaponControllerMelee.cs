using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllerMeleeBase : Weapon
{
    private Animator animator;
    private bool attackBlocked;
    private CapsuleCollider2D blade;
    private DamageController damageController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        damageController = GetComponent<DamageController>();
        damageController.Damage = damage;

        blade = GetComponent<CapsuleCollider2D>();
        if (blade != null )
        {
            blade.enabled = false;
        }

        Debug.Log("Me loaded");
    }

    public override void Attack()
    {
        if (attackBlocked)
            return;

        animator.SetTrigger("Attack");
        blade.enabled = true;

        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(cooldown);
        attackBlocked = false;
        blade.enabled = false;
    }
}
