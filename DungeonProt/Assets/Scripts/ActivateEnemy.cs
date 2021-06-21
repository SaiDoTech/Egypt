using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ActivateEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public RoomManager roomManager;
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        var enemies = roomManager.currentRoom.transform.GetComponentsInChildren<Transform>().Where(t => t.tag == "Enemy").ToArray();
        foreach (var enemy in enemies)
        {
            enemy.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            enemy.gameObject.GetComponent<AgentScript>().enabled = true;
        }
    }
}
