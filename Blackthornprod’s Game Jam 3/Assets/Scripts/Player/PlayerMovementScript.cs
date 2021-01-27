using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//=================================================
// The code in this script managers all movement details
//=================================================
public class PlayerMovementScript : MonoBehaviour
{
    //reference to the main player script
    [SerializeField]
    PlayerScript playerScript;

    
    [Header("Movement Settings")]
    [Space(5)]

    //main player properties
    [SerializeField]
    internal float movementSpeed;

    // Amount of force added when the player jumps.
    [SerializeField]
    private float m_JumpForce = 400f;

    // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, 1)][SerializeField]
    private float m_CrouchSpeed = .36f;

    // How much to smooth out the movement
    [Range(0, .3f)] [SerializeField]
    private float m_MovementSmoothing = .05f;

    // Whether or not a player can steer while jumping;
    [SerializeField]
    internal bool m_AirControl = false;

    // A mask determining what is ground to the character
    [SerializeField]
    internal LayerMask m_WhatIsGround;

    // A position marking where to check for ceilings
    [SerializeField]
    internal Transform m_CeilingCheck;

    // A position marking where to check if the player is grounded.
    [SerializeField]
    internal Transform m_GroundCheck;

    // A collider that will be disabled when crouching
    [SerializeField]
    internal Collider2D m_CrouchDisableCollider;

    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;


    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    internal bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        //print("PlayerMovementScript Starting");
        playerScript.rb2d.velocity = new Vector2(0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentPlayerScript;
        //if (playerScript != GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentPlayerScript) return;

        //if (playerScript.inputScript.currentPlayerScript != playerScript) return;

        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }

        //if (playerScript != GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentPlayerScript) return;
            Movement();
    }

    void Movement()
    {
        // Move our character
        Move(playerScript.inputScript.x * Time.fixedDeltaTime, playerScript.inputScript.crouching, playerScript.inputScript.jumping);
        playerScript.inputScript.jumping = false;
    }

    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            // If crouching
            if (crouch)
            {
                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, playerScript.rb2d.velocity.y);
            // And then smoothing it out and applying it to the character
            playerScript.rb2d.velocity = Vector3.SmoothDamp(playerScript.rb2d.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
                //playerScript.ChangeState("Walk");
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
                //playerScript.ChangeState("Walk");
            }

            if (move !=0)
            {
                playerScript.ChangeState("Walk");
            }
        }

        //Debug.Log(m_Grounded);
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            //playerScript.rb2d.AddForce(new Vector2(0f, m_JumpForce));
            Vector2 tempVelocity = playerScript.rb2d.velocity;
            tempVelocity.y = m_JumpForce;
            playerScript.rb2d.velocity = tempVelocity;

            playerScript.ChangeState("Jumping");
        }

        if (playerScript.rb2d.velocity.y < 0)
        {
            //Debug.Log("Fast Decent");
            playerScript.rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerScript.rb2d.velocity.y > 0 && !jump)
        {
            //Debug.Log("Low Jump");
            playerScript.rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        }

        if (playerScript.rb2d.velocity == Vector2.zero && !crouch) {
            playerScript.ChangeState("Idle");
        }
        //Debug.Log(playerScript.rb2d.velocity);

    }

    public void Flip()
    {
        //if (this.GetComponent<PlayerController>()) {
            // Switch the way the player is labelled as facing.
            //m_FacingRight = !m_FacingRight;

            //foreach (GameObject body in this.GetComponent<PlayerController>().connectedBodies) {
            //    body.GetComponent<PlayerMovementScript>().m_FacingRight = !body.GetComponent<PlayerMovementScript>().m_FacingRight;
            //    // Switch the way the player is labelled as facing.
                
            //    // Multiply the player's x local scale by -1.
            //    Vector3 bodyScale = body.transform.localScale;
            //    bodyScale.x *= -1;
            //    body.transform.localScale = bodyScale;
            //}
          //  return;
        //}

        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
