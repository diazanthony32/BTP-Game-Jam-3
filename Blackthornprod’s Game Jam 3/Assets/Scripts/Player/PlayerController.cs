using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    internal List<CinemachineVirtualCamera> gameCameras;

    //main player properties
    [SerializeField]
    internal List<GameObject> connectedBodies;

    //states
    internal readonly int COMBINED = 0;
    internal readonly int SPLIT = 1;
    public int STATE;

    const float bodyRadiusCheck = 1.5f; // Radius of the overlap circle to determine if close enough to merge


    public PlayerScript currentPlayerScript = null;


    private void Awake()
    {
        CombineBody();
    }

    // Update is called once per frame
    void Update()
    {
        BodyMorph();
    }

    void BodyMorph()
    {
        if (currentPlayerScript.inputScript.bodyMorph)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(currentPlayerScript.transform.position, bodyRadiusCheck);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (STATE == SPLIT)
                {
                    if (colliders[i].gameObject.CompareTag("Half-Player") && colliders[i].gameObject != currentPlayerScript.gameObject)
                    {
                        Debug.Log("Combining Body!");
                        CombineBody();
                        break;
                    }
                    else {
                        Debug.Log("Swapping Body!");
                        SwapBody();
                        break;
                    }

                }
                else if (STATE == COMBINED)
                {
                    Debug.Log("Splitting Body!");
                    SplitBody();
                    break;
                }
                else
                {
                    Debug.Log("Can't do anyhing chief...");
                }
                
            }
            
        }
    }

    void CombineBody()
    {
        //Debug.Log("Combine Body");

        //currentPlayerScript.movementScript.m_FacingRight;
        foreach (GameObject sides in connectedBodies)
        {
            if (currentPlayerScript && sides != currentPlayerScript.gameObject &&(currentPlayerScript.transform.localScale.x != sides.transform.localScale.x)) {
                sides.GetComponent<PlayerMovementScript>().Flip();
            }
        }


        currentPlayerScript = this.GetComponent<PlayerScript>();
        //currentPlayerScript.inputScript = this.GetComponent<PlayerInputScript>();
        currentPlayerScript.inputScript.playerScript = currentPlayerScript;

        currentPlayerScript.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        currentPlayerScript.GetComponent<Collider2D>().enabled = true;

        // reset player to center of bodies
        currentPlayerScript.transform.position = new Vector3(
            (connectedBodies[0].transform.position.x + (connectedBodies[1].transform.position.x - connectedBodies[0].transform.position.x) / 2),
            (connectedBodies[0].transform.position.y + (connectedBodies[1].transform.position.y - connectedBodies[0].transform.position.y) / 2), 0);

        //moves side bodies to the center
        foreach (GameObject sides in connectedBodies)
        {
            sides.GetComponent<Rigidbody2D>().isKinematic = true;
            sides.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            sides.GetComponent<Collider2D>().enabled = false;
            sides.GetComponent<PlayerScript>().enabled = false;
            sides.transform.localPosition = Vector3.zero;
        }

        SwapCamera(0);

        //gameCamera.Follow.transform(currentPlayerScript.gameObject.transform);

        STATE = COMBINED;
    }

    void SplitBody()
    {
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        this.GetComponent<Collider2D>().enabled = false;
    
        currentPlayerScript.inputScript.playerScript = null;
        this.GetComponent<PlayerScript>().enabled = false;
        //currentPlayerScript.inputScript = null;

        foreach (GameObject sides in connectedBodies)
        {
            sides.GetComponent<Rigidbody2D>().isKinematic = false;
            sides.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

            sides.GetComponent<Collider2D>().enabled = true;
            sides.GetComponent<PlayerScript>().enabled = true;
        }

        currentPlayerScript = connectedBodies[0].GetComponent<PlayerScript>();
        //currentPlayerScript.inputScript = this.GetComponent<PlayerInputScript>();
        currentPlayerScript.inputScript.playerScript = currentPlayerScript;

        //connectedBodies[0].GetComponent<PlayerScript>().enabled = true;
        SwapCamera(1);
        STATE = SPLIT;
    }

    void SwapCamera(int camera)
    {
        foreach (CinemachineVirtualCamera cam in gameCameras)
        {
            cam.Priority = 10;
        }
        gameCameras[camera].MoveToTopOfPrioritySubqueue();
    }

    void SwapBody()
    {
        int index = connectedBodies.IndexOf(currentPlayerScript.gameObject);
        
        if (index < connectedBodies.Count-1)
        {
            //Debug.Log("Next Body");
            //connectedBodies.IndexOf(currentPlayerScript.gameObject);

            currentPlayerScript = connectedBodies[index+1].GetComponent<PlayerScript>();
            //currentPlayerScript.inputScript = this.GetComponent<PlayerInputScript>();
            currentPlayerScript.inputScript.playerScript = currentPlayerScript;

            //connectedBodies[0].GetComponent<PlayerScript>().enabled = true;
            SwapCamera(index + 2);
        }
        else {
            //connectedBodies.IndexOf(currentPlayerScript.gameObject);

            currentPlayerScript = connectedBodies[0].GetComponent<PlayerScript>();
            //currentPlayerScript.inputScript = this.GetComponent<PlayerInputScript>();
            currentPlayerScript.inputScript.playerScript = currentPlayerScript;

            //connectedBodies[0].GetComponent<PlayerScript>().enabled = true;
            SwapCamera(1);
        }
        
    }

    private void ResetBodies()
    {
        
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3(
            (connectedBodies[0].transform.position.x + (connectedBodies[1].transform.position.x - connectedBodies[0].transform.position.x) / 2),
            (connectedBodies[0].transform.position.y + (connectedBodies[1].transform.position.y - connectedBodies[0].transform.position.y) / 2), 0), 0.5f);

        //(connectedBodies[0].position.x + (connectedBodies[1].position.x - connectedBodies[0].position.x) / 2);
    }
}
