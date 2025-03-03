using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject letterView;
    private Camera currentActiveCamera; // To track which camera was active
    [SerializeField] Camera mainCamera; 
    [SerializeField] Camera[] subCameras;

    private bool isPaused = false;
    private bool wasInSubscene = false;
    private bool wasViewingLetter = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        wasInSubscene = !mainCamera.enabled; 
        wasViewingLetter = letterView.activeSelf;
        //Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    void Update()
    {

        if (Input.GetButtonDown("Cancel"))
        {
            if (isPaused)
            {
                Resume();  
            }
            else if (!letterView.activeSelf)
            {
                Pause();  
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (wasViewingLetter)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (wasInSubscene)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

    }

  

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }
}
