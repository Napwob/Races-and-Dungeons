using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : MonoBehaviour
{
    private Animator animator;
    public float delay = 0.15f;
    private bool attackBlocked;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (attackBlocked)
            return;

        animator.SetTrigger("Attack");
        attackBlocked = true;
        StartCoroutine(DelayAttack());
        
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }
}
