using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HitPoints;
    public int DeathFrames;
    void Start()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (HitPoints <= 0)
        {
            if (DeathFrames != 0)
            {
                DeathFrames--;
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
        if (collision.gameObject.tag == "Weapon" && HitPoints > 0)
        {
            HitPoints -= 10;
            if (HitPoints <= 0)
            {
                Die();
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
        if (collision.gameObject.tag == "Weapon" && HitPoints > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void Die()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Destroy(gameObject.GetComponent<CapsuleCollider2D>());
        Destroy(gameObject.transform.parent.gameObject.GetComponent<CapsuleCollider2D>());
    }
}
