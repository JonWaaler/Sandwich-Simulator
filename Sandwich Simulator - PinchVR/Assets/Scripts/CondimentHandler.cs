using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CondimentHandler : MonoBehaviour {

    // Condiment that is in player hand (can only have 1)
    public GameObject currentCondiment;
    public Transform breadRight, breadLeft;
    private SoundManager soundManager;
    private GameObject player;
    private Bounds rightBreadBounds;
    public bool finishGame = false;
    public GameObject winScreen;
    private float t_lerpTimer = 0;

    private void Start()
    {
        player = GameObject.Find("Player");
        soundManager = FindObjectOfType<SoundManager>();
        //breadRight = GameObject.Find("BreadRight").transform;
        //breadLeft = GameObject.Find("BreadLeft").transform;
    }

    private void Update()
    {
        // Debug
        Debug.DrawRay(player.transform.GetChild(0).position, player.transform.GetChild(0).forward * 200);

        if (finishGame)
        {
            FinishSanwich(t_lerpTimer);

            t_lerpTimer += Time.deltaTime;
        }
    }

    // This creates a new condiment
    // Used for the main lettuce, tomato and cheese selections
    public void GrabNewCondiment(GameObject condiment)
    {        
        if(currentCondiment == null)
        {
            // Create a new condiment, and set current condiment
            GameObject condimentInst = Instantiate(condiment);
            currentCondiment = condimentInst;

            // Don't let the condiment fall
            condimentInst.GetComponent<Rigidbody>().useGravity = false;
            condimentInst.transform.SetParent(player.transform.GetChild(0));

            // Set condiment to hand position
            currentCondiment.transform.localPosition = new Vector3(0.206f, -.22f, .428f);
            currentCondiment.transform.localEulerAngles = new Vector3(0.2f, 0.005f, .2f);
            soundManager.PlaySound("pickup");
        }
        else
        {
            Debug.Log("Condiment in hand already", currentCondiment);
        }
    }

    public void EventDebug(string msg)
    {
        Debug.Log(msg, gameObject);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void DropCondiment()
    {

        RaycastHit hit;
        // Raycast from camera to straight forwards
        if (Physics.Raycast(player.transform.GetChild(0).position, player.transform.GetChild(0).forward, out hit, Mathf.Infinity))
        {
            // If the raycast hit something
            currentCondiment.transform.position = hit.point + Vector3.up;            
        }

        currentCondiment.GetComponent<Rigidbody>().useGravity = true;

        currentCondiment.transform.parent = null;
        currentCondiment = null;

        soundManager.PlaySound("putdown");

    }

    public void ActivateEndSequence()
    {
        finishGame = true;
    }

    private void FinishSanwich(float lerpTimer)
    {
        // (Looks wise)
        // Use floating "hands" for fun
        // Use stick through whole left piece for immersion

        // (Functionality)
        // Take left bread slice Disable all children gravity and collider?
        breadLeft.GetComponent<Rigidbody>().useGravity = false;
        breadLeft.GetComponent<BoxCollider>().enabled = false;
        for (int i = 0; i < breadLeft.childCount; i++)
        {
            breadLeft.GetChild(i).GetComponent<Rigidbody>().useGravity = false;
            breadLeft.GetChild(i).GetComponent<BoxCollider>().enabled = false;
            rightBreadBounds.Encapsulate(breadLeft.GetChild(i).position);
        }
        // lerp bread parent up in the air based on size of rightBread
        if (lerpTimer < 1)
        {
            breadRight.position = Vector3.Lerp(new Vector3(1.021f, breadRight.transform.position.y, -0.291f), new Vector3(1.146f, breadRight.transform.position.y, -0.005f), lerpTimer);
            breadLeft.position = Vector3.Lerp(new Vector3(1.021f, 1.01f, 0.258f), new Vector3(1.146f, 2, -0.005f), lerpTimer);
        }

        // rotate parent upside down
        if (lerpTimer > 1 && lerpTimer < 2)
        {
            breadLeft.rotation = Quaternion.Lerp(Quaternion.Euler(Vector3.right * 90), Quaternion.Euler(Vector3.left * 90), lerpTimer - 1);
        }
        else if (lerpTimer > 2)
        {
            // enable all childrens disabled component back and drop bread
            for (int i = 0; i < breadLeft.childCount; i++)
            {
                breadLeft.GetChild(i).GetComponent<Rigidbody>().useGravity = true;
                breadLeft.GetChild(i).GetComponent<BoxCollider>().enabled = true;
            }
            breadLeft.GetComponent<Rigidbody>().useGravity = true;
            breadLeft.GetComponent<BoxCollider>().enabled = true;

            winScreen.SetActive(true);
            //print("Game Over");
        }        
    }

    public void TextTutorial(GameObject video)
    {
        video.SetActive(true);
    }

    public void CloseTutorial(GameObject video)
    {
        video.SetActive(false);
    }

    public void HighlightCondiment(GameObject condimentBloom)
    {
        condimentBloom.SetActive(true);
    }

    public void RemoveHighlightCondiment(GameObject condimentBloom)
    {
        condimentBloom.SetActive(false);
    }
}
