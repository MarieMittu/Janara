using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    private Camera currentActiveCamera; // To track which camera was active
    [SerializeField] Camera mainCamera; // Assign your main camera in the inspector
    [SerializeField] Camera[] subCameras;

    private bool isPaused = false;
    private bool hideCursorOnResume;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Initially lock the cursor
        Cursor.visible = false; // Hide the cursor in the main camera
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Determine the camera in use and adjust cursor behavior for when we resume
        hideCursorOnResume = mainCamera.gameObject.activeSelf;

        Debug.Log("Game paused. Cursor visible. Active camera: " +
                  (hideCursorOnResume ? "Main Camera" : "Subcamera"));
    }

    void Update()
    {

        if (Input.GetButtonDown("Cancel"))
        {
            if (isPaused)
            {
                Resume();  // Resume game
            }
            else
            {
                Pause();   // Pause game
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Restore cursor lock state based on whether the main camera is active
        if (hideCursorOnResume)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // Lock cursor for main gameplay
            Debug.Log("Resumed on Main Camera. Cursor hidden and locked.");
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;   // Keep cursor visible for subcameras
            Debug.Log("Resumed on Subcamera. Cursor visible.");
        }

    }

  

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }
}
