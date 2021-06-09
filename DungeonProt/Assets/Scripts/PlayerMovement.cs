using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float modeSpeed = 2f; //�������� ����

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
        movement.x = Input.GetAxisRaw("Horizontal"); // ������� ��� x
        movement.y = Input.GetAxisRaw("Vertical"); // ������� ��� y

        animator.SetFloat("Speed", movement.sqrMagnitude); // ����������� �������� � ��������� (�� �������� ���������)
        animator.SetFloat("Horizontal", movement.x); //���������� � ���������� ������� �� ������� �� ��� x � y
        animator.SetFloat("Vertical", movement.y);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * modeSpeed * Time.fixedDeltaTime);
    }
}
