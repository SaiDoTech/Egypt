using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMelee : MonoBehaviour
{
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.J))
        {
            Quaternion rotationZ = Quaternion.AngleAxis(20, Vector3.forward);
            transform.rotation *= rotationZ;
        }
    }
}
