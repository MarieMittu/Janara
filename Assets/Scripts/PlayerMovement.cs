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
    public AudioClip footstepSound; // Single footstep sound clip
    private AudioSource audioSource;


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
    public bool doorLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = footstepSound; // Set the footstep sound clip
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.15f;
    }


    private void HandleInteractionCheck()
    {
        //if (GameManager.instance.currentLevel == 1)
        //{


        //    Debug.Log("INTERACTION start check " + interactionDistance);
        //    GameManager gm = FindObjectOfType<GameManager>();
        //    if (gm != null && gm.witchReflects == true)
        //    {
        //        interactionDistance = 10f;
        //    }
        //    else if (gm != null && gm.witchReflects == false)
        //    {
        //        interactionDistance = 3f;
        //    }
        //}
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

            if (move.magnitude > 0)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play(); // Start playing the footstep sound
                    Debug.Log("audioSource.Play");
                }
               
            }
            else 
            {
                if (audioSource.isPlaying) audioSource.Stop(); // Stop the footstep sound
            }
        }
      


        if (canInteract)
        {
            Debug.Log("INTERCACTION come on");
            HandleInteractionCheck();
            HandleInteractionInput();
        }

        if (GameManager.instance.currentLevel == 1)
        {


            if (currentKeyholeIndex >= keyholes.Length && isFilling)
            {
                isFilling = false;
                currentKeyholeIndex = keyholes.Length - 1;

            }

            if (currentKeyholeIndex >= keyholes.Length - 1)
            {
                //door locked
                Debug.Log("LOCKLOCK " + doorLocked + " " + currentKeyholeIndex + " " + " " + keyholes.Length);
                doorLocked = true;
            }
            else
            {
                doorLocked = false;
                Debug.Log("NO LOCKLOCK " + doorLocked + " " + currentKeyholeIndex + " " + " " + keyholes.Length);
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
        if (GameManager.instance.currentLevel == 1)
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
    }

    private IEnumerator UnfillCurrentKeyhole()
    {
        if (GameManager.instance.currentLevel == 1)
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
}
