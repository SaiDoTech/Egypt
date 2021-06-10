using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public RoomManager RoomManager;
    private Vector3 target;

    void Start()
    {
        target = RoomManager.currentRoom.transform.position + new Vector3(0,0,-10);
    }

    private void Update()
    {
        target = RoomManager.currentRoom.transform.position + new Vector3(0, 0, -10);
        gameObject.transform.position = Vector3.Lerp(transform.position, target, 2.55f*Time.deltaTime);
    }
}
