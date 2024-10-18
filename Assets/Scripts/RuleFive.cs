using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class RuleFive : MonoBehaviour
{
    public float timeRemaining = 10;
    bool isGuarding = false;
    [SerializeField] GameObject baby;
    bool startControl = false;

    public Camera subCamera;
    public Camera playerCamera;
    public GameObject eye;
    public GameObject witchesHand;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (startControl)
        {
            if (!isGuarding && timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                //Debug.Log("GUARDA here" + timeRemaining);
            }

            if (timeRemaining < 1)
            {
                ShowBed();
                Invoke("ShowHand", 2f);
                Invoke("DestroyBaby", 4f);
                Invoke("LoadGO", 9f);
            }
        }

    }

    public void ShowBed()
    {
        subCamera.enabled = true;
        playerCamera.enabled = false;
        Cursor.visible = false;
        eye.SetActive(false);
        //nanny.SetActive(false);
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>(); // Find the player movement script
        if (playerMovement != null)
        {
            playerMovement.SetMovementState(false); // Disable movement
        }
        
    }
        void ShowHand()
        {
            witchesHand.SetActive(true);
        }

        //voids to invoke
        void DestroyBaby()
        {
            Destroy(baby);
            Destroy(witchesHand);
        }

        void LoadGO()
        {
            SceneManager.LoadScene("GameOverScene");
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("TIMER"))
            {
                startControl = true;
            }
            else
                if (other.CompareTag("GUARDA"))
            {
                //stop and reset countdown
                Debug.Log("GUARDA okay");
                isGuarding = true;
                timeRemaining = 10;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("GUARDA"))
            {
                //start countdown
                Debug.Log("GUARDA madonnna");
                isGuarding = false;
            }
        }
    }
