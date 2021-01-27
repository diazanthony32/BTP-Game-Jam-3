using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//=================================================
// The main player script
//=================================================
public class DungeonPlayerScript : MonoBehaviour
{
    [Header("Scripts")]
    [Space(5)]
    //Store a reference to all the sub player scripts
    [SerializeField]
    internal DungeonPlayerInputScript inputScript;

    [SerializeField]
    internal DungeonPlayerMovementScript movementScript;

    [SerializeField]
    internal DungeonPlayerCollisionScript collisionScript;


    [Header("Health Settings")]
    
    //main player properties
    [SerializeField]
    internal int health = 4;

    [SerializeField]
    internal Sprite[] wholePlayerSprites;

    [SerializeField]
    internal Sprite[] leftPlayerSprites;

    [SerializeField]
    internal Sprite[] rightPlayerSprites;

    [SerializeField]
    internal List<PhysicMaterial> physicMaterials;

    [SerializeField]
    internal GameObject previousLife;

    internal string side;

    internal bool inControl = true;

    //component references
    internal Animator anim;
    internal Rigidbody rb;
    internal SpriteRenderer sr;

    //other references
    internal string currentState;

    private void Awake()
    {
        //print("Main PlayerScript Awake");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();

    }
    // Start is called before the first frame update
    void Start()
    {
        //if(previousLife)
        //sr.sprite = playerSprites[health];
        //print("Main PlayerScript Starting");
    }

    private void Update()
    {
        if (!inControl) return;

        if (inputScript.split && health > 1)
        {
            Split();
        }
        
        if (currentState == "Die") {
            Die();
        }
    }

    //=================================================/
    //State management
    //=================================================/
    internal void ChangeState(string newState)
    {
        if (newState != currentState)
        {
            Debug.Log("Triggering: \"" + newState + "\" Animation!");

            //anim.ResetTrigger(currentState);
            anim.SetTrigger(newState);

            currentState = newState;
        }  
    }

    internal void SetFloat(float newFloat)
    {
        //if (newState != currentState)
        //{
        //    Debug.Log("Triggering: \"" + newState + "\" Animation!");

        anim.SetFloat("Speed", newFloat);
        //    anim.SetTrigger(newState);

        //    currentState = newState;
        //}
    }

    void Split() {

        Debug.Log("Splitting Life!");

        this.health /= 2;

        // Spawns a new Player Prefab
        GameObject newPlayer = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerDungeon"));

        newPlayer.GetComponent<DungeonPlayerScript>().previousLife = this.gameObject;
        newPlayer.GetComponent<DungeonPlayerScript>().health = this.health;

        // sets the sides for the sprites
        if (side == null)
        {
            side = "left";
            newPlayer.GetComponent<DungeonPlayerScript>().side = "right";
        }
        else {
            newPlayer.GetComponent<DungeonPlayerScript>().side = side;
        }

        // sets correct sprite top to bottom
        if (side == "left")
        {
            // sets current player to the correct sprite state (Left/bottom)
            if (health == 2)
            {
                sr.sprite = leftPlayerSprites[0];
                newPlayer.GetComponent<DungeonPlayerScript>().sr.sprite = rightPlayerSprites[0];

            }
            else
            {
                sr.sprite = leftPlayerSprites[2];
                // sets current player to the correct sprite state (Right/Top)
                newPlayer.GetComponent<DungeonPlayerScript>().sr.sprite = leftPlayerSprites[1];

            }
        }
        else if (side == "right")
        {
            // sets current player to the correct sprite state (Left/bottom)
            if (health == 2)
            {
                sr.sprite = rightPlayerSprites[0];
                newPlayer.GetComponent<DungeonPlayerScript>().sr.sprite = leftPlayerSprites[0];

            }
            else
            {
                sr.sprite = rightPlayerSprites[2];
                // sets current player to the correct sprite state (Right/Top)
                newPlayer.GetComponent<DungeonPlayerScript>().sr.sprite = rightPlayerSprites[1];

            }
        }

        // Places new Player Prefab in front of the player                                                                      // Some Placement protection detection needed eventually
        Vector3 temp = this.transform.position;
        temp.z -= 1.0f;
        newPlayer.transform.position = temp;

        // Swaps control and player camera over to the new player spawn
        Camera.main.GetComponent<Cinemachine.CinemachineBrain>().ActiveVirtualCamera.Follow = newPlayer.transform;

        //Disables the Current Player
        this.inControl = false;
        this.GetComponent<Collider>().material = physicMaterials[1];

        // this is to prevent the split button being forever enabled due to swapping body
        inputScript.split = false;
    }

    void Die()
    {
        if (this.previousLife)
        {
            // Swaps control and player camera over to the new player spawn
            Camera.main.GetComponent<Cinemachine.CinemachineBrain>().ActiveVirtualCamera.Follow = previousLife.transform;
            previousLife.GetComponent<DungeonPlayerScript>().inControl = true;
            previousLife.GetComponent<Collider>().material = physicMaterials[0];
        }
        else {
            Debug.Log("Game Over...");
        }

        GameObject.Destroy(this.gameObject);

    }

}
