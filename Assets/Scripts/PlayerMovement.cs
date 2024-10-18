using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public CharacterController controller;
    public float speed = 20f;
    Vector3 velocity;
    public float gravity = -9.81f;
    public bool canMove = true;


    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    private Interactable currentInteractable;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private KeyCode interactKey = KeyCode.Mouse0;

    public Image[] keyholes;
    public float fillSpeed = 1f; // Speed at which the image fills
    private int currentKeyholeIndex = 0; // Index of the current image being filled
    private bool spaceHeld = false; // Tracks if the spacebar is held down
    private bool fillingInProgress = false;
    private bool isFilling = true;


    // Start is called before the first frame update
    void Start()
    {

    }

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

    public void SetMovementState(bool state)
    {
        canMove = state;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

         
            Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
        if (canInteract)
        {
            Debug.Log("INTERCACTION come on");
            HandleInteractionCheck();
            HandleInteractionInput();
        }

        if (currentKeyholeIndex >= keyholes.Length && isFilling)
        {
            isFilling = false;
            currentKeyholeIndex = keyholes.Length - 1;
        }

        if (isFilling)
        {
            HandleFilling();
        }
        else
        {
            HandleUnfilling();
        }
    }

    private void HandleFilling()
    {
        if (Input.GetKey(KeyCode.Space) && !spaceHeld && !fillingInProgress)
        {
            spaceHeld = true; // Track that spacebar is pressed
            fillingInProgress = true; // Prevent further input until done
            StartCoroutine(FillCurrentKeyhole());
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            spaceHeld = false; // Spacebar has been released, can press again
        }
    }

    // Handle unfilling keyholes
    private void HandleUnfilling()
    {
        if (Input.GetKey(KeyCode.Space) && !spaceHeld && !fillingInProgress)
        {
            spaceHeld = true; // Track that spacebar is pressed
            fillingInProgress = true; // Prevent further input until done
            StartCoroutine(UnfillCurrentKeyhole());
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            spaceHeld = false; // Spacebar has been released, can press again
        }
    }

    private IEnumerator FillCurrentKeyhole()
    {
        Image currentKeyhole = keyholes[currentKeyholeIndex];

        while (currentKeyhole.fillAmount < 1f)
        {
            // Gradually fill the image
            currentKeyhole.fillAmount += fillSpeed * Time.deltaTime;
            yield return null;
        }

        // Move to the next image after the current one is fully filled
        currentKeyholeIndex++;
        fillingInProgress = false; // Allow the next image to be filled after spacebar is released
    }

    private IEnumerator UnfillCurrentKeyhole()
    {
        Image currentKeyhole = keyholes[currentKeyholeIndex];

        while (currentKeyhole.fillAmount > 0f)
        {
            currentKeyhole.fillAmount -= fillSpeed * Time.deltaTime;
            yield return null;
        }

        // Move to the previous image after the current one is fully unfilled
        currentKeyholeIndex--;
        fillingInProgress = false; // Allow the next image to be unfilled after spacebar is released

        // If we've unfilled all images, switch back to filling mode
        if (currentKeyholeIndex < 0)
        {
            isFilling = true; // Switch back to filling mode
            currentKeyholeIndex = 0; // Start from the first image again
        }
    }
}
