using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//=================================================
// The code in this script Checks for button input
//=================================================
public class DungeonPlayerInputScript : MonoBehaviour
{

    [SerializeField]
    internal DungeonPlayerScript playerScript;

    internal float x;
    internal float y;
    internal bool jumping;
    internal bool crouching;

    internal bool split;

    // Start is called before the first frame update
    void Start()
    {
        //print("PlayerInputScript Starting");
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerScript.inControl) return;
        //if (playerScript != this.GetComponent<PlayerController>().currentPlayerScript) return;

        //This is a Ternary Operation that chooses a message based if the selected Key is Being Pressed
        //isLeftPressed = Input.GetKey(KeyCode.A) ? true : false;
        //isRightPressed = Input.GetKey(KeyCode.D) ? true : false;

        //isUpPressed = Input.GetKey(KeyCode.Space) ? true : false;
        //isDownPressed = Input.GetKey(KeyCode.S) ? true : false;


        x = (Input.GetAxisRaw("Horizontal"));
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);

        split = Input.GetKeyDown(KeyCode.R);

        //Debug.Log(jumping);

    }
}
