using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{
    public float Cooldown;
    public float Distance;
    public float Speed;
    public float DestinationMultiplier;
    public Transform CursorTransform;
    public LayerMask layerMask;

    float cooldownTimer;
    bool blinking = false;
    Vector3 destination;
    ParticleSystem trail;

    void Start()
    {
        trail = transform.Find("Trail").GetComponent<ParticleSystem>();
        cooldownTimer = Cooldown;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Blink();
        }

        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;
        else
            cooldownTimer = Cooldown;

        if (blinking)
        {
            var dist = Vector3.Distance(transform.position, destination);
            if (dist > 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * Speed);
            }
            else
                blinking = false;
        }
    }

    void Blink()
    {
        trail.Play();

        destination = CursorTransform.position;

        blinking = true;
    }
}
