using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private void OnEnable()
    {
        Room.ChangeRoomEvent += Room_ChangeCurrentRoom;
    }

    private void Room_ChangeCurrentRoom(int x, int y)
    {

    }

    private void OnDisable()
    {
        Room.ChangeRoomEvent -= Room_ChangeCurrentRoom;
    }
}
