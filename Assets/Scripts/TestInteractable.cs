using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Events.UnityAction;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class TestInteractable : Interactable
{
    [SerializeField] GameObject letterView;
    [SerializeField] GameObject[] lettersRules;
    [SerializeField] Light candleLight;

    private TMP_Text interactText;
    private bool canOpenDoor = false;
    private GameObject doorParent;
    private GameObject bathDoorParent;

    public GameObject witchReflection;
    public AudioSource doorAudioSource;

    private void Start()
    {
        
        interactAction.SetActive(false);
        subCamera.enabled = false;
        playerCamera.enabled = true;
        eye.SetActive(true);
        exitBtn.SetActive(false);
        nanny.SetActive(true);
        if (gameObject.tag == "LOCK")
        {
            gameObject.SetActive(false);
        }
    }

    public override void OnFocus()
    {
        Debug.Log("LOOKING AT " + gameObject.name);

        if (gameObject.tag == "MIRROR" && GameManager.instance.currentLevel == 1)
        {
            ShowWitch();
        }
        else if (gameObject.tag == "CROCE" && gameObject.transform.rotation.x <= 0 && GameManager.instance.currentLevel == 3)
        {
            interactAction.SetActive(false);
            Debug.Log("what croce one " + gameObject.transform.rotation.x);
        }
        //else if (gameObject.tag == "DOORONE" && canOpenDoor == false)
        //{
        //    interactAction.SetActive(false);
        //    Debug.Log("doorcheck no action " + canOpenDoor);
        //}
        else if (gameObject.tag == "LOCK" && GameManager.instance.currentLevel == 1)
        {
            interactAction.SetActive(true);
            Debug.Log("what croce two " + gameObject.transform.rotation.x);
        } else if ((gameObject.tag == "TOYS" || gameObject.tag == "CANDLE") && GameManager.instance.currentLevel == 2)
        {
            interactAction.SetActive(true);
        }
        else if ((gameObject.tag == "CROCE" || gameObject.tag == "WITCH") && GameManager.instance.currentLevel == 3)
        {
            interactAction.SetActive(true);
        } else if (gameObject.tag == "BED" || gameObject.tag == "LETTER" || gameObject.tag == "DOORONE" || gameObject.tag == "DOORTWO")
        {
            interactAction.SetActive(true);
        }





    }

    public override void OnInteract()
    {
        
        interactAction.SetActive(false);

        switch (gameObject.tag)
        {
            case "BED":
                EnterSubscene();
                break;
            case "TOYS":
                if (GameManager.instance.currentLevel == 2)
                {
                    EnterSubscene();
                }
                
                break;
            case "LETTER":
                ShowLetter();
                break;
            case "CANDLE":
                if (GameManager.instance.currentLevel == 2)
                {
                    SwitchLight();
                }
                    
                break;
            case "CROCE":
                if (GameManager.instance.currentLevel == 3)
                {
                    TurnCroce();
                }
                break;
            case "DOORONE":
                OpenDoorOne();
                break;
            case "DOORTWO":
                OpenDoorTwo();
                break;
            case "WITCH":
                if (GameManager.instance.currentLevel == 3)
                {
                    SceneManager.LoadScene("FinalScene");
                }
                    
                break;
 
                
        }
    }

    public override void OnLoseFocus()
    {
        Debug.Log("STOPPED LOOKING AT " + gameObject.name);
        interactAction.SetActive(false);
    }

    public void EnterSubscene()
    {
        subCamera.enabled = true;
        playerCamera.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        eye.SetActive(false);
        exitBtn.SetActive(true);
        //nanny.SetActive(false);
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>(); // Find the player movement script
        if (playerMovement != null)
        {
            playerMovement.SetMovementState(false); // Disable movement
        }

        ActivateExitButton(ExitSubscene);
    }

    public void ExitSubscene()
    {
        subCamera.enabled = false;
        playerCamera.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        eye.SetActive(true);
        exitBtn.SetActive(false);
        //nanny.SetActive(true);
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>(); // Find the player movement script
        if (playerMovement != null)
        {
            playerMovement.SetMovementState(true); // Disable movement
        }
    }

     void ActivateExitButton(Action method)
    {
        if (exitBtn != null)
        {
            var button = exitBtn.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {

                button.onClick.AddListener(new UnityEngine.Events.UnityAction(method));


            }
        }
    }

    public void ShowLetter()
    {
        exitBtn.SetActive(true);
        ActivateExitButton(HideLetter);

        // Hide all letter rules first
        foreach (GameObject letter in lettersRules)
        {
            letter.SetActive(false);
        }

        // Show the letter corresponding to the current level
        int currentLevel = GameManager.instance.currentLevel;
        if (currentLevel >= 1 && currentLevel <= lettersRules.Length)
        {
            lettersRules[currentLevel - 1].SetActive(true); // Activate letter for the current level
        }

        letterView.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }


    public void HideLetter()
    {
        letterView.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        exitBtn.SetActive(false);
        canOpenDoor = true;
        Debug.Log("doorcheck now here " + canOpenDoor);
    }

    public void SwitchLight()
    {
        if (candleLight.intensity == 0)
        {
            candleLight.intensity = 3;
        } else if (candleLight.intensity > 0)
        {
            candleLight.intensity = 0;
        }
    }

    public void ShowWitch()
    {
        Debug.Log("MIRRORRRR");
        StartGame gm = FindObjectOfType<StartGame>(); 
        if (gm != null && gm.witchReflects == true)
        {
            witchReflection.SetActive(true);
            Invoke("RemoveWitch", 1f);
        }
       
    }

    public void RemoveWitch()
    {
        Destroy(witchReflection);
        
    }

    public void TurnCroce()
    {
        Debug.Log("why rotaiting croce " + gameObject.transform.rotation.x);
        if (gameObject.transform.rotation.x > 0)
        {
            gameObject.transform.Rotate(-180f, 0f, 0f, Space.World);
            Debug.Log("rotaiting croce " + gameObject.transform.rotation.x);
        }
    }

    public void OpenDoorOne()
    {
        //if (canOpenDoor)
        //{
        doorParent = GameObject.FindGameObjectWithTag("DOORHOLDER");
        Debug.Log("doorParent " + doorParent.transform.rotation.y);

        if (doorParent.transform.rotation.y <= 0)
        {
            if (gameObject.tag == "LOCK")
            {
                gameObject.SetActive(false);
            }
            doorParent.transform.Rotate(0f, 270f, 0f, Space.World);
            Debug.Log("doorParent a " + doorParent.transform.rotation.y);
            if (doorAudioSource != null)
            {
                doorAudioSource.Play();
            }
        } else
        {
            doorParent.transform.Rotate(0f, -270f, 0f, Space.World);
            Debug.Log("doorParent b " + doorParent.transform.rotation.y);
            //activate lock
            if (gameObject.tag == "LOCK")
            {
                gameObject.SetActive(true);
            }
            if (doorAudioSource != null)
            {
                doorAudioSource.Play();
            }
        }
        

        //} else
        //{
        //    Debug.Log("doorcheck cannot open");
        //}
    }

    public void OpenDoorTwo()
    {
        
        bathDoorParent = GameObject.FindGameObjectWithTag("Bathdoorholder");
        Debug.Log("bath doorParent start " + bathDoorParent.transform.rotation.y);

        if (bathDoorParent.transform.rotation.y <= 0)
        {
            bathDoorParent.transform.Rotate(0f, 290f, 0f, Space.World);
            Debug.Log("bath doorParent open " + bathDoorParent.transform.rotation.y);
            if (doorAudioSource != null)
            {
                doorAudioSource.Play();
            }
        }
        else
        {
            bathDoorParent.transform.Rotate(0f, -290f, 0f, Space.World);
            Debug.Log("bath doorParent close " + bathDoorParent.transform.rotation.y);
            if (doorAudioSource != null)
            {
                doorAudioSource.Play();
            }
        }


        
    }
}
