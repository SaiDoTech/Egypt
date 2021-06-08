using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float modeSpeed = 2f; //�������� ����
    public float modeRoll = 3f; //�������� ��������

    public Rigidbody2D rb;
    public Animator animator;
    Vector2 movement;
    Vector2 movementRoll;
    float lastIdleState = 0; // � ����� ������� �������� ������� 0 - �����, 1 - ����, 2 - ������, 3 - �����
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); //������� ������ �� x
        movement.y = Input.GetAxisRaw("Vertical"); //������� ������ �� y
 
        animator.SetFloat("Speed", movement.sqrMagnitude); // ����������� �������� � ��������� (�� �������� ���������)

        if (animator.GetFloat("Speed") == 0) //���� ����������� �����������, ������������� � ���������� ������� ����������� �����
        {
            animator.SetFloat("IdleState", lastIdleState); 
        }
        else { //�������� ������� ����������� �����������
            if (movement.x != 0)
                lastIdleState =  (movement.x>0)?2:3;
            else lastIdleState = (movement.y>0)?0:1;
        }

        animator.SetFloat("Horizontal", movement.x); //���������� � ���������� ������� �� ������� �� ��� x � y
        animator.SetFloat("Vertical", movement.y);

        bool roll = Input.GetButtonDown("Roll"); 
        if (roll) 
        {

            animator.SetFloat("IdleState", lastIdleState); //������������� ����������� �����
            setDirectionRoll();
            animator.SetBool("Roll", true); //���������� � ��������� ������� �� true
            
        }
    }
    void FixedUpdate() 
    {
        if (!animator.GetBool("Roll"))
        rb.MovePosition(rb.position + movement * modeSpeed * Time.fixedDeltaTime);

    }

    void setDirectionRoll() 
    {
        if (movement.x != 0 || movement.y != 0) //������� �� ����� ����
            movementRoll = movement; 
        else //������� ����� �����
        {
            movementRoll.x = (lastIdleState == 2) ? 1 : (lastIdleState == 3) ? -1 : 0;
            movementRoll.y = (lastIdleState == 0) ? 1 : (lastIdleState == 1) ? -1 : 0;
        }
    }

    void moveRoll() //������� event ��� ����������� �� ������ ���� �������� ��������
    {

        rb.MovePosition(rb.position + movementRoll * modeRoll * Time.fixedDeltaTime);
        
    }
    void stopRoll() //������� event ��� ����������� ��������
    {
        animator.SetBool("Roll", false);
    }


}
