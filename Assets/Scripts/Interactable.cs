using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public virtual void Awake()
    {
        gameObject.layer = 6;
    }

    public GameObject interactAction;
    public Camera subCamera;
    public Camera playerCamera;
    public GameObject eye;
    public GameObject exitBtn;
    public GameObject nanny;

    public abstract void OnInteract();
    public abstract void OnFocus();
    public abstract void OnLoseFocus();
}
