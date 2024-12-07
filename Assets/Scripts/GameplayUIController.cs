using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUIController : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void Update()
    {
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = true;
        Debug.Log("SURVIVE CURSOR" + Cursor.visible);
    }

    public void RestartGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene("MainScene");
    }

    public void HomeButton()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene("StartScene");
    }

    public void ContinueNext()
    {
        GameManager.instance.NextLevel();
        SceneManager.LoadScene("MainScene");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void FinalVictory()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene("VictoryScene");
    }

    public void FinalDeath()
    {
        SceneManager.LoadScene("DeathScene");
    }
}
