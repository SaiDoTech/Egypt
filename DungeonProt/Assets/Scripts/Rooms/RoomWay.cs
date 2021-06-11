using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomWay : MonoBehaviour
{
    public string PlayerTag = "Player";
    private Room parentRoom; // ���� ���������� ��������� � ���, ��� ����� ����� �������� �������
    private void Start()
    {
        parentRoom = gameObject.GetComponentInParent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag))
        {
            Debug.Log("Way activated");
            parentRoom.DetectWay(gameObject);
        }
    }
}
