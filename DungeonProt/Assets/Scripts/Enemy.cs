using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool Alive;
    public int DeathFrames;
    void Start()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!Alive)
        {
            if (DeathFrames != 0)
            {
                DeathFrames--;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Alive) Die();
    }

    private void Die()
    {
        Alive = false;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }
}