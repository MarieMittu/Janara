using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Events.UnityAction;
using System;

public class TestInteractable : Interactable
{
    [SerializeField] GameObject letterView;
    [SerializeField] Light candleLight;

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
        } else
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
                EnterSubscene();
                break;
            case "LETTER":
                ShowLetter();
                break;
            case "CANDLE":
                SwitchLight();
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
    
}
