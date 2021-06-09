using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapBuider : MonoBehaviour
{
    public int MapWidth;
    public int MapHeight;
    public int RoomCount;

    public Room[] RoomPrefabs;
    public Room StartRoom;

    public Room[,] spawnedRooms;

    void Start()
    {
        spawnedRooms = new Room[MapHeight, MapWidth];

        StartRoom.x = spawnedRooms.GetLength(0) / 2;
        StartRoom.y = spawnedRooms.GetLength(1) / 2;
        spawnedRooms[StartRoom.x, StartRoom.y] = StartRoom;

        for (int i = 0; i < RoomCount; i++)
        {
            PlaceOneRoom();
        }
    }

    private void PlaceOneRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();

        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }

        Room newRoom = Instantiate(RoomPrefabs[Random.Range(0, RoomPrefabs.Length)]);

        int limit = 500;
        while (limit-- > 0)
        {
            Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));

            if (ConnectToSomething(newRoom, position))
            {
                newRoom.transform.position = new Vector3((position.x - (spawnedRooms.GetLength(0) / 2)) * 17.8f, (position.y - (spawnedRooms.GetLength(1) / 2)) * 10, 0);
                spawnedRooms[position.x, position.y] = newRoom;
                // Немного быдлокода
                newRoom.x = position.x;
                newRoom.y = position.y;

                break;
            }
        }
    }

    private bool ConnectToSomething(Room room, Vector2Int p)
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (room.WayU != null && p.y < maxY && spawnedRooms[p.x, p.y + 1]?.WayD != null) neighbours.Add(Vector2Int.up);
        if (room.WayD != null && p.y > 0 && spawnedRooms[p.x, p.y - 1]?.WayU != null) neighbours.Add(Vector2Int.down);
        if (room.WayR != null && p.x < maxX && spawnedRooms[p.x + 1, p.y]?.WayL != null) neighbours.Add(Vector2Int.right);
        if (room.WayL != null && p.x > 0 && spawnedRooms[p.x - 1, p.y]?.WayR != null) neighbours.Add(Vector2Int.left);

        if (neighbours.Count == 0) return false;

        Vector2Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];

        if (selectedDirection == Vector2Int.up)
        {
            room.WayU.SetActive(true);
            selectedRoom.WayD.SetActive(true);
        }
        else if (selectedDirection == Vector2Int.down)
        {
            room.WayD.SetActive(true);
            selectedRoom.WayU.SetActive(true);
        }
        else if (selectedDirection == Vector2Int.right)
        {
            room.WayR.SetActive(true);
            selectedRoom.WayL.SetActive(true);
        }
        else if (selectedDirection == Vector2Int.left)
        {
            room.WayL.SetActive(true);
            selectedRoom.WayR.SetActive(true);
        }

        return true;
    }
}
