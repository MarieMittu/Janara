using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{

    public float nightDuration = 60f;
    public float startNightDuration;
    float secondTimer = 0f;

    public Image nightTimer;
    private bool timerStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        if (nightTimer != null)
        {
            float initialFillAmount = 0.67f; // Set your initial fill amount
            nightTimer.fillAmount = initialFillAmount; // Visual representation
            startNightDuration = nightDuration / initialFillAmount;
            UnityEngine.Debug.Log("TIMERRR Initial Fill Amount: " + nightTimer.fillAmount);
            UnityEngine.Debug.Log("TIMERRR Start Night Duration: " + startNightDuration);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (timerStarted)
        {
            secondTimer = secondTimer + Time.deltaTime;
            if (secondTimer >= 1f)
            {
                nightDuration--;
                secondTimer = 0f;

                if (nightTimer != null)
                {
                    nightTimer.fillAmount = Mathf.Clamp(nightDuration / startNightDuration, 0f, 1f);
                    UnityEngine.Debug.Log("TIMERRR Updated Fill Amount: " + nightTimer.fillAmount);
                }
            }



            if (nightDuration <= 0)
            {

                SceneManager.LoadScene("GameOverScene"); // change to normal
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TIMER"))
        {
            //start timer night
            UnityEngine.Debug.Log("TIMERRR GAME BEGIN");
            timerStarted = true;
            Destroy(other.gameObject);

            
        }
    }


}
