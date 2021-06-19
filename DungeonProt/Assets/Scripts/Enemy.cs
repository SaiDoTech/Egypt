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
            Debug.Log("blue");
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            inHurt = true;
            hurtTicksLeft = hurtTicks;
        }
    }

    private void TakeDamageFixedUpdate()
    {
        if (inHurt && currentHealth > 0)
        {
            if (--hurtTicksLeft == 0)
            {
                inHurt = false;
                Debug.Log("white");
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    private void Die()
    {
        //animation lol
        Debug.Log("red");
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        //disable
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        gameObject.transform.parent.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
