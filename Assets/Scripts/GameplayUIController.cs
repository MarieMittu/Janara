using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUIController : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void ContinueNext()
    {
        GameManager.instance.NextLevel();
        SceneManager.LoadScene("MainScene");
    }

    public void FinalVictory()
    {
        SceneManager.LoadScene("VictoryScene");
    }

    public void FinalDeath()
    {
        SceneManager.LoadScene("DeathScene");
    }
}
