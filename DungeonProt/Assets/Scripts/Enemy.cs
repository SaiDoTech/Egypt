using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public float repulseRange = 1f;
    private Vector3 vectorRepulse;
    private Vector3 startRepulsePosition;

    // cause no animation yet
    private bool inHurt;
    public int hurtTicks = 0;
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
            hurtTicksLeft = hurtTicks;
            inHurt = true;

            vectorRepulse = 2 * gameObject.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position; // calc position to repulse
            startRepulsePosition = gameObject.transform.position;
        }
    }

    private void TakeDamageFixedUpdate()
    {
        if (inHurt && currentHealth > 0)
        {
            if (--hurtTicksLeft == 0)
            {
                inHurt = false;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().MovePosition(Vector3.Lerp(startRepulsePosition, repulseRange * vectorRepulse, ((float)(hurtTicks - hurtTicksLeft)) / hurtTicks)); // Lerp repulse
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

        gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
