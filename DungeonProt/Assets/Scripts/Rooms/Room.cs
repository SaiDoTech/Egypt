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
        if (way != null)
        {
            int nextX = x;
            int nextY = y;

            if (way == WayU)
            {
                nextX = x;
                nextY = y + 1;
            }
            else if (way == WayR)
            {
                nextX = x + 1;
                nextY = y;
            }
            else if (way == WayD)
            {
                nextX = x;
                nextY = y - 1;
            }
            else if (way == WayL)
            {
                nextX = x - 1;
                nextY = y;
            }

            ChangeRoomEvent(nextX, nextY);
        }
    }
}
