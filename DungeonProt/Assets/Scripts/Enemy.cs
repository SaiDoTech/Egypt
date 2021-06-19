using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    // cause no animation yet
    private bool inHurt;
    public int hurtTicks = 20;
    private int hurtTicksLeft;

    void Start()
    {
        currentHealth = maxHealth;
        inHurt = false;
    }

    void Update()
    {

    }

    // cause no animation yet
    private void FixedUpdate()
    {
        TakeDamageFixedUpdate();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //animation lol
        TakeDamageAnimation();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // cause no animation yet
    private void TakeDamageAnimation()
    {
        if (currentHealth > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            inHurt = true;
            hurtTicksLeft = hurtTicks;
        }
    }

    private void TakeDamageFixedUpdate()
    {
        if (inHurt)
        {
            if (--hurtTicksLeft == 0)
            {
                inHurt = false;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    private void Die()
    {
        //animation lol
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        //disable
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        gameObject.transform.parent.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
