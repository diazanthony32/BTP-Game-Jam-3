using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportback : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {

            Vector3 temp = other.transform.position;
            temp.x -= 5;

            Vector3 temp2 = Camera.main.transform.position;
            temp2.x -= 5;

            other.transform.position = temp;
            Camera.main.transform.position = temp2;


        }
    }
}
