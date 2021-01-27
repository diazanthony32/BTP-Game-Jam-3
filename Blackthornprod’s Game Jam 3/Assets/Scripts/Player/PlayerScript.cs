using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//=================================================
// The main player script
//=================================================
public class PlayerScript : MonoBehaviour
{
    [Header("Scripts")]
    [Space(5)]
    //Store a reference to all the sub player scripts
    [SerializeField]
    internal PlayerInputScript inputScript;

    [SerializeField]
    internal PlayerMovementScript movementScript;

    [SerializeField]
    internal PlayerCollisionScript collisionScript;


    [Header("Health Settings")]
    [Space(5)]
    //main player properties
    [SerializeField]
    internal int health;


    //component references
    internal Animator anim;
    internal Rigidbody2D rb2d;

    //other references
    internal string currentState;

    private void Awake()
    {
        //print("Main PlayerScript Awake");
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

    }
    // Start is called before the first frame update
    void Start()
    {
        //print("Main PlayerScript Starting");
    }

    //=================================================/
    //State management
    //=================================================/
    internal void ChangeState(string newState)
    {
        if (newState != currentState)
        {
            Debug.Log("Triggering: \"" + newState + "\" Animation!");
            //anim.Play(newState);
            anim.SetTrigger(newState);
            currentState = newState;
        }  
    }

}
