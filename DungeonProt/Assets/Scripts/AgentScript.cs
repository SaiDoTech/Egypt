using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour
{
    [SerializeField] Transform target;

    private NavMeshAgent agent;
    public Animator animator;
    Vector3 prevPosition;
    Vector3 deltaPosition;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform; //��������� ������� ������

        agent = GetComponent<NavMeshAgent>();//��������� ������ navmesh
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);//����������, ���� ���� �����(������� �����)


      /*  deltaPosition = agent.nextPosition - prevPosition; // ������� ������ �����������

        animator.SetFloat("Horizontal", deltaPosition.x); //���������� � ���������� ������ �����������
        animator.SetFloat("Vertical", deltaPosition.y);
        if (deltaPosition.x != 0f || deltaPosition.y != 0f) //���������� ��������� �������� ��� ������ ����������� ��������� �� 0
            animator.SetFloat("Speed", 1f);
        else
            animator.SetFloat("Speed", 0f);
        prevPosition = agent.nextPosition; //���������� ������� ��� ����������*/   //�� �������

    }
    
}
