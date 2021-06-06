using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public MapBuider mapBuider;

    private Room currentRoom;
    private Room[,] rooms;
    private Vector3 target;

    void Start()
    {
        currentRoom = mapBuider.StartRoom;
        target = currentRoom.transform.position;
    }

    private void Update()
    {
        rooms = mapBuider.spawnedRooms;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!currentRoom.DoorD.activeSelf)
            {
                changePosition(rooms[currentRoom.x, currentRoom.y-1]);
                Debug.Log("Down");
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!currentRoom.DoorU.activeSelf)
            {
                changePosition(rooms[currentRoom.x, currentRoom.y+1]);
                Debug.Log("Up");
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!currentRoom.DoorL.activeSelf)
            {
                changePosition(rooms[currentRoom.x-1, currentRoom.y]);
                Debug.Log("Left");
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!currentRoom.DoorR.activeSelf)
            {
                changePosition(rooms[currentRoom.x+1, currentRoom.y]);
                Debug.Log("Right");
            }
        }

        gameObject.transform.position = Vector3.Lerp(transform.position, target, 2.55f*Time.deltaTime);
    }

    private void changePosition(Room nextRoom)
    {
        var room = nextRoom.gameObject;
        currentRoom = nextRoom;

        target = new Vector3(room.transform.position.x, room.transform.position.y, -10);
        //gameObject.transform.position = new Vector3 (room.transform.position.x, room.transform.position.y, -10);
    }
}
