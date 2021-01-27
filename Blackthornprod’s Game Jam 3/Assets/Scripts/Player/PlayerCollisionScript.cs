using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//=================================================
// The code in this script manages Collision checks
//=================================================
public class PlayerCollisionScript : MonoBehaviour
{
    //reference to the main player script
    [SerializeField]
    PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        

        //print("PlayerCollisionScript Starting:"+ playerScript.rb2d);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        //if (collision.gameObject.CompareTag("DoesDamage"))
        //{
        //    playerScript.ChangeState("Player_sad");
        //}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        

        //if (collision.gameObject.CompareTag("DoesDamage"))
        //{
        //    playerScript.ChangeState("Player_happy");
        //}
    }

}
