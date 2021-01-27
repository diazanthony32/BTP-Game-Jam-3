using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//=================================================
// The code in this script manages Collision checks
//=================================================
public class DungeonPlayerCollisionScript : MonoBehaviour
{
    //reference to the main player script
    [SerializeField]
    DungeonPlayerScript playerScript;

    private void OnTriggerEnter(Collider trigger)
    {
        if (!playerScript.inControl) return;

        //Debug.Log("Entered Collision!");

        if (trigger.gameObject.CompareTag("DoesDamage"))
        {
            playerScript.ChangeState("Die");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!playerScript.inControl) return;

        //Debug.Log("Entered Collision!");

        if (collision.gameObject.CompareTag("DoesDamage"))
        {
            playerScript.ChangeState("Die");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!playerScript.inControl) return;


        //if (collision.gameObject.CompareTag("DoesDamage"))
        //{
        //    playerScript.ChangeState("Player_happy");
        //}
    }

}
