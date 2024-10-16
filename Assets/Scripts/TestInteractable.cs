using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Events.UnityAction;
using System;
using TMPro;

public class TestInteractable : Interactable
{
    [SerializeField] GameObject letterView;
    [SerializeField] Light candleLight;

    private TMP_Text interactText;
    private bool canOpenDoor = false;
    private GameObject doorParent;

    private void Start()
    {
        interactAction.SetActive(false);
        subCamera.enabled = false;
        playerCamera.enabled = true;
        eye.SetActive(true);
        exitBtn.SetActive(false);
        nanny.SetActive(true);
        
    }

    public override void OnFocus()
    {
        Debug.Log("LOOKING AT " + gameObject.name);

        if (gameObject.tag == "MIRROR")
        {
            ShowWitch();
        }
        else if (gameObject.tag == "CROCE" && gameObject.transform.rotation.x <= 0)
        {
            interactAction.SetActive(false);
            Debug.Log("what croce one " + gameObject.transform.rotation.x);
        }
        else if (gameObject.tag == "DOORONE" && canOpenDoor == false)
        {
            interactAction.SetActive(false);
            Debug.Log("doorcheck no action " + canOpenDoor);
        }
        else
        {
            interactAction.SetActive(true);
            Debug.Log("what croce two " + gameObject.transform.rotation.x);
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
                EnterSubscene();
                break;
            case "LETTER":
                ShowLetter();
                break;
            case "CANDLE":
                SwitchLight();
                break;
            case "CROCE":
                TurnCroce();
                break;
            case "DOORONE":
                OpenDoorOne();
                
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
        nanny.SetActive(false);

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
        nanny.SetActive(true);
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
        letterView.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
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
        doorParent.transform.Rotate(0f, 270f, 0f, Space.World);

        //} else
        //{
        //    Debug.Log("doorcheck cannot open");
        //}
    }
    
}
