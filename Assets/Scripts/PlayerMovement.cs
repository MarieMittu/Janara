using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public CharacterController controller;
    public float speed = 20f;
    Vector3 velocity;
    public float gravity = -9.81f;

    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    private Interactable currentInteractable;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private KeyCode interactKey = KeyCode.Mouse0;


    // Start is called before the first frame update
    void Start()
    {

    }

    //private void HandleInteractionCheck()
    //{
    //    Debug.Log("INTERCACTION start check " + interactionDistance);

    //    Ray ray = playerCamera.ViewportPointToRay(interactionRayPoint);

    //    // Visualize the ray in the Scene view (it will be visible for 1 second)
    //    Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red, 1f);

    //    if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
    //    {
    //        Debug.Log("INTERCACTION 2 " + hit.collider.gameObject.name + "current is" + currentInteractable.name);
    //        if (hit.collider.gameObject.layer == 6 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
    //        {
    //            Debug.Log("INTERCACTION 3 " + interactionDistance);
    //            hit.collider.TryGetComponent(out currentInteractable);
    //            if(currentInteractable)
    //            {
    //                Debug.Log("INTERCACTION 4 " + interactionDistance);
    //                currentInteractable.OnFocus();
    //            }
    //        }
    //    } else if (currentInteractable)
    //    {
    //        Debug.Log("NO MORE INTERCACTION");
    //        currentInteractable.OnLoseFocus();
    //        currentInteractable = null;
    //    }
    //}

    private void HandleInteractionCheck()
    {
        Debug.Log("INTERACTION start check " + interactionDistance);

        // Create the ray directly from the camera's forward direction
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // center of the screen
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red, 2f); // Visible for 2 seconds

        // Perform a simple raycast with no conditions
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            hit.collider.TryGetComponent(out currentInteractable);
            if (currentInteractable)
            {
                Debug.Log("INTERACTION found interactable on: " + hit.collider.gameObject.name);

                currentInteractable.OnFocus();
            }
            else
            {
                Debug.LogWarning("The object hit does not have an Interactable component: " + hit.collider.gameObject.name);
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;

        }
        else
        {
            Debug.Log("No object hit by raycast.");
        }
    }



    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteractable.OnInteract();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (canInteract)
        {
            Debug.Log("INTERCACTION come on");
            HandleInteractionCheck();
            HandleInteractionInput();
        }
    }


}
