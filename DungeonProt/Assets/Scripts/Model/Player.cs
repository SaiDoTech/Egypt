using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,
                      IUnit,
                      IMovable,
                      IHealable,
                      IWizzard

{
    public int Health { get; set; } = 100;
    public int MaxHealth { get; set; } = 100;
    public float MoveSpeed { get; set; } = 4;
    public int ManaPoints { get; set; } = 100;
    public int MaxMana { get; set; } = 100;
    public ISpell spell { get; set; } = null;

    private Rigidbody2D rb;

    private Animator animator;
    private float lastIdleState = 0;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    public void Die()
    {
    }

    public void Heal(int hp)
    {
        Health += hp;

        if (Health > MaxHealth)
            Health = MaxHealth;
    }

    public void Move(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction * MoveSpeed * Time.fixedDeltaTime);

        animator.SetFloat("Speed", direction.sqrMagnitude); // ����������� �������� � ��������� (�� �������� ���������)
        if (direction.sqrMagnitude == 0) //���� ����������� �����������, ������������� � ���������� ������� ����������� �����
        {
            animator.SetFloat("IdleState", lastIdleState);
        }
        else
        { //�������� ������� ����������� �����������
            if (direction.x != 0)
                lastIdleState = (direction.x > 0) ? 2 : 3;
            else lastIdleState = (direction.y > 0) ? 0 : 1;
        }
        animator.SetFloat("Horizontal", direction.x); //���������� � ���������� ������� �� ������� �� ��� x � y
        animator.SetFloat("Vertical", direction.y);
    }

    public void RegenMana(int mp)
    {
        ManaPoints += mp;

        if (ManaPoints > MaxMana)
            ManaPoints = MaxMana;
    }

    public void TakeDamage(int damage)
    {
        if (damage >= Health)
            Die();
        else
            Health -= damage;
    }
}
