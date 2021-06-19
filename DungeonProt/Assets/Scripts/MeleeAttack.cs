using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.3f;
    public int damage = 20;

    // cause no animation yet
    private bool inAttack;
    public int attackTicks = 10;
    private int attackTicksLeft;

    void Start()
    {
        inAttack = false;
        attackTicksLeft = attackTicks;
    }

    void Update()
    {
        if (!inAttack && Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        AttackFixedUpdate();
    }

    private void Attack()
    {
        //animation lol
        AttackAnimation();

        //detect enemy
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //damage
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    // cause no animation yet
    private void AttackAnimation()
    {
        attackPoint.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        inAttack = true;
    }

    private void AttackFixedUpdate()
    {
        if (inAttack)
        {
            attackTicksLeft--;
            if (attackTicksLeft <= 0)
            {
                inAttack = false;
                attackTicksLeft = attackTicks;
                attackPoint.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
