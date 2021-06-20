using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode
{
    // ������� �������
    public Room Room { get; }

    // ������ �������� ������
    public List<RoomNode> Children { get; }

    public RoomNode(Room room)
    {
        Room = room;
        Children = new List<RoomNode>();
    }

    // ��������� ����� �������� �������.
    public RoomNode AddChildren(RoomNode node)
    {
        Children.Add(node);
        return this;
    }
}
