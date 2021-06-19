using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayers;
    private new Rigidbody2D rigidbody;

    public float attackRange = 0.3f;
    public int damage = 20;

    // cause no animation yet
    private bool inAttack;
    public int attackTicks = 20;
    private int attackTicksLeft;

    void Start()
    {
        inAttack = false;
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!inAttack && Input.GetMouseButton(0))
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
        // detecting direction
        CorrectAttackPoint();

        // freeze for animation lol
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        // animation lol
        AttackAnimation();

        // detect enemy
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // damage
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    // correcting attacPoint.position considering cursor
    private void CorrectAttackPoint()
    {
        Vector3 worldSpaceMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float dx = worldSpaceMousePosition.x - gameObject.transform.position.x;
        float dy = worldSpaceMousePosition.y - gameObject.transform.position.y;

        Vector3 position = attackPoint.localPosition;

        if (Mathf.Abs(dx) > Mathf.Abs(dy))
        {
            position.x = Mathf.Sign(dx) * 0.5f;
        }
        else
        {
            position.y = Mathf.Sign(dy) * 0.5f;
        }

        attackPoint.localPosition = position;
    }

    // cause no animation yet
    private void AttackAnimation()
    {
        attackPoint.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        attackTicksLeft = attackTicks;
        inAttack = true;
    }

    private void AttackFixedUpdate()
    {
        if (inAttack)
        {
            if (--attackTicksLeft <= 0)
            {
                inAttack = false;
                attackPoint.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                attackPoint.localPosition = Vector3.zero;
                rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
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
