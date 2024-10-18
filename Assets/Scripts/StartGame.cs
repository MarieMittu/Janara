using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{

    public float nightDuration = 480f;
    public float startNightDuration;
    float secondTimer = 0f;

    public Image nightTimer;
    private bool timerStarted = false;

    public GameObject clock;
    public GameObject clockArrow;
    public GameObject witchHolder;
    public GameObject chick;

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

                    if (nightTimer.fillAmount <= 0.52 && nightTimer.fillAmount >= 0.49)
                    {
                        UnityEngine.Debug.Log("MIDNIGHT");
                        clockArrow.SetActive(true);
                        PlayerMovement pm = FindObjectOfType<PlayerMovement>();
                        if (pm != null && pm.doorLocked == false)
                        {
                            //game over
                            UnityEngine.Debug.Log("MIDNIGHT game over " + pm.doorLocked);
                            witchHolder.SetActive(true);
                            Invoke("LoadGO", 3f);

                        }
                        
                        Invoke("RemoveArrow", 15f);

                        
                    }
                }
            }

            

              

            if (nightDuration <= 0)
            {
                chick.SetActive(true);
                Invoke("LoadSurvive", 4f);
            }
        }
    }

    void LoadSurvive()
    {
        SceneManager.LoadScene("SurvivedScene");
    }

    private void RemoveArrow()
    {
        Destroy(clockArrow);
    }

    void LoadGO()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("GameOverScene");
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
