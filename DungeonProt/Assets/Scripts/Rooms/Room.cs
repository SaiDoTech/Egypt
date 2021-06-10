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

    public delegate void ChangeRoomEventHandler(int x, int y, int nexSpawnerInd);
    public static event ChangeRoomEventHandler ChangeRoomEvent;

    public Transform[] playerSpawners;
    public GameObject PlayerSpawners;

    private void Start()
    {
        playerSpawners = PlayerSpawners.GetComponentsInChildren<Transform>();
    }

    public void DetectWay(GameObject way)
    {
        if (way != null)
        {
            int nextX = x;
            int nextY = y;
            int nextSpawner = 0;

            if (way == WayU)
            {
                nextX = x;
                nextY = y + 1;
                nextSpawner = 4;
            }
            else if (way == WayR)
            {
                nextX = x + 1;
                nextY = y;
                nextSpawner = 1;
            }
            else if (way == WayD)
            {
                nextX = x;
                nextY = y - 1;
                nextSpawner = 2;
            }
            else if (way == WayL)
            {
                nextX = x - 1;
                nextY = y;
                nextSpawner = 3;
            }

            ChangeRoomEvent(nextX, nextY, nextSpawner);
        }
    }
}
