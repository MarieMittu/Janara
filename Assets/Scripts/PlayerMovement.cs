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

    private void HandleInteractionCheck()
    {
        Debug.Log("INTERCACTION start check " + interactionDistance);
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            Debug.Log("INTERCACTION 2 " + interactionDistance);
            if (hit.collider.gameObject.layer == 6 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                Debug.Log("INTERCACTION 3 " + interactionDistance);
                hit.collider.TryGetComponent(out currentInteractable);
                if(currentInteractable)
                {
                    Debug.Log("INTERCACTION 4 " + interactionDistance);
                    currentInteractable.OnFocus();
                }
            }
        } else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
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
            HandleInteractionCheck();
            HandleInteractionInput();
        }
    }


}
