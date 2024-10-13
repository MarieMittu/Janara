using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : Interactable
{

    private void Start()
    {
        interactAction.SetActive(false);
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
        }
    }

    public override void OnLoseFocus()
    {
        Debug.Log("STOPPED LOOKING AT " + gameObject.name);
        interactAction.SetActive(false);
    }
}
