using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    Collider2D[] colliders;
    [SerializeField]
    Animator animator;
    public override void Fire(Transform shootpoint,float range,bool isPlayer)
    {
        StartCoroutine(PerformMeleeAttack( shootpoint,range, isPlayer));
    }

    IEnumerator PerformMeleeAttack(Transform shootpoint,float range,bool isPlayer)
    {
        canFire = false;
        animator.SetTrigger(Constants.attackTrigger);
        Attack(shootpoint,range,isPlayer);
        yield return new WaitForSeconds(coolDown);
        canFire = true;
        yield return null;
    }

    private void Attack(Transform shootpoint,float range,bool isPlayer)
    {

        if (isPlayer)
        {
            colliders = Physics2D.OverlapCircleAll(shootpoint.position, range, LayerMask.GetMask(Constants.enemyLayer));
        }
        else
        {
            colliders = Physics2D.OverlapCircleAll(shootpoint.position, range, LayerMask.GetMask(Constants.playerLayer));
        }
        Debug.Log(colliders.Length);
        if (colliders.Length > 0)
        {
            foreach(Collider2D collider in colliders)
            {
                Character character = collider.GetComponent<Character>();
                Debug.Log("Damage");
                character.ApplyDamage(damage);
            }
        }
    }
}
