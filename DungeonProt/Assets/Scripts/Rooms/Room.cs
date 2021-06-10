using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject WayU;
    public GameObject WayR;
    public GameObject WayD;
    public GameObject WayL;

    public int x;
    public int y;

    public delegate void ChangeRoomEventHandler(int x, int y);
    public static event ChangeRoomEventHandler ChangeRoomEvent;

    public void DetectWay(GameObject way)
    {
        ChangeRoomEvent(1,1);
    }
}
