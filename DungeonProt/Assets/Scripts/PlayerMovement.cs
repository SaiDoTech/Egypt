using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float modeSpeed = 2f; //скорость бега

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movement;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // Нажатия для x
        movement.y = Input.GetAxisRaw("Vertical"); // Нажатия для y

        animator.SetFloat("Speed", movement.sqrMagnitude); // выставление скорости в состоянии (не является скоростью)
        animator.SetFloat("Horizontal", movement.x); //установить в состояниях нажатия на клавиши по оси x и y
        animator.SetFloat("Vertical", movement.y);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * modeSpeed * Time.fixedDeltaTime);
    }
}
