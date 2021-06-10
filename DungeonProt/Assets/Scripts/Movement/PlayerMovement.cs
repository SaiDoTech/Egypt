using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; //�������� ����

    private Rigidbody2D rb;
    private Animator animator;
    float lastIdleState = 0; // � ����� ������� �������� ������� 0 - �����, 1 - ����, 2 - ������, 3 - �����
    private Vector2 movement;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // ������� ��� x
        movement.y = Input.GetAxisRaw("Vertical"); // ������� ��� y
        animator.SetFloat("Speed", movement.sqrMagnitude); // ����������� �������� � ��������� (�� �������� ���������)
        if (movement.sqrMagnitude == 0) //���� ����������� �����������, ������������� � ���������� ������� ����������� �����
        {
            animator.SetFloat("IdleState", lastIdleState);
        }
        else
        { //�������� ������� ����������� �����������
            if (movement.x != 0)
                lastIdleState = (movement.x > 0) ? 2 : 3;
            else lastIdleState = (movement.y > 0) ? 0 : 1;
        }
        animator.SetFloat("Horizontal", movement.x); //���������� � ���������� ������� �� ������� �� ��� x � y
        animator.SetFloat("Vertical", movement.y);
    }

    private void FixedUpdate()
    {
        if (movement.x != 0 && movement.y != 0)
        {
            movement.x = movement.x * (float)Math.Sqrt(2) / 2;
            movement.y = movement.y * (float)Math.Sqrt(2) / 2;
        }
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
