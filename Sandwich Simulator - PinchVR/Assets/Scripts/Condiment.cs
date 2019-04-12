using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condiment : MonoBehaviour {

    private CondimentHandler condimentHandler;
    private SoundManager soundManager;
    private GameObject player;

    public bool wackyMode = false;

	// Use this for initialization
	void Start ()
    {
        // Initialize
        player = GameObject.Find("Player");
        soundManager = FindObjectOfType<SoundManager>();
        condimentHandler = FindObjectOfType<CondimentHandler>();
	}

    private void Update()
    {
        // Scale quick fix
        //if(transform.parent != null)
        //    if(transform.parent.name == "BreadLeft" || transform.parent.name == "BreadRight")
        //        transform.localScale = new Vector3(0.7407407f, 0.25f, 0.8333334f);
    }


    public void EventDebug(string msg)
    {
        // Event debuger
        // Used to test when an event gets called
        Debug.Log(msg,gameObject);
    }

    public void DropCondiment()
    {
        RaycastHit hit;
        // Raycast used to see where the condiment needs to be placed
            // Raycast from camera to straight forwards
        if (Physics.Raycast(player.transform.GetChild(0).position, player.transform.GetChild(0).forward, out hit, Mathf.Infinity))
        {
            // If the raycast hit something
            condimentHandler.currentCondiment.transform.position = hit.point + Vector3.up * .5f;
        }

        // Diable gravity to ensure condiment doesnt fall while in hand
        condimentHandler.currentCondiment.GetComponent<Rigidbody>().useGravity = true;

        // Release condiment
        condimentHandler.currentCondiment.transform.parent = null;
        condimentHandler.currentCondiment = null;

        // Scale issue fix (rotation on axis affects scale)
        condimentHandler.currentCondiment.transform.localRotation = Quaternion.Euler(Vector3.zero);

        // Sound
        soundManager.PlaySound("putdown");

    }

    // Allows player to pick up a condiment that was spawned in.
    public void GrabCondiment()
    {

        if (condimentHandler.currentCondiment == null)
        {
            // Set up currentCondiment to hold clicked condiment
            condimentHandler.currentCondiment = gameObject;

            // Don't let the condiment fall
            condimentHandler.currentCondiment.GetComponent<Rigidbody>().useGravity = false;
            condimentHandler.currentCondiment.transform.SetParent(player.transform.GetChild(0));

            // Hardcode hand position
            condimentHandler.currentCondiment.transform.localPosition = new Vector3(0.206f, -.22f, .428f);
            condimentHandler.currentCondiment.transform.localEulerAngles = new Vector3(0.2f, 0.005f, .2f);

            // Sound
            soundManager.PlaySound("pickup");
        }
        else
        {
            Debug.Log("Condiment in hand already", condimentHandler.currentCondiment);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Parenting and tagging used for endgame sandwich put together
        if (collision.gameObject.CompareTag("BreadLeft") && !condimentHandler.finishGame)
        {
            gameObject.tag = "BreadLeft";
            transform.SetParent(condimentHandler.breadLeft);

            if (!wackyMode)
            {
                transform.localScale = new Vector3(0.8333334f, 0.8333334f, 0.3500001f);
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }
        else if (collision.gameObject.CompareTag("BreadRight") && !condimentHandler.finishGame)
        {
            gameObject.tag = "BreadRight";
            transform.SetParent(condimentHandler.breadRight);

            // want to play with funny parenting scale issues? go on every condiment prefab and enable wacky mode
            if (!wackyMode)
            {
                transform.localScale = new Vector3(0.8333334f, 0.8333334f, 0.3500001f);
                transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
            }

        }
    }

    
}
