using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : Interactable
{
    

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
        interactAction.SetActive(true);
    }

    public override void OnInteract()
    {
        
        interactAction.SetActive(false);

        switch (gameObject.tag)
        {
            case "CUBE":
                Debug.Log("INTERACTED WITH " + gameObject.name);
                break;
            case "BED":
                Debug.Log("CHECKED THE BABY UAAAA");
                break;
            case "TOYS":    
                EnterSubscene();
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

        if (exitBtn != null)
        {
            var button = exitBtn.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                button.onClick.AddListener(ExitSubscene);
            }
        }
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

    
}
