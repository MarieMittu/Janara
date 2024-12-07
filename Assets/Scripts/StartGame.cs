using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{

    public float nightDuration = 160f;
    public float startNightDuration;
    float secondTimer = 0f;

    public Image nightTimer;
    private bool timerStarted = false;
    private float timePassed = 0;

    public GameObject clock;
    public GameObject clockArrow;
    public GameObject witchHolder;
    public GameObject chick;
    public GameObject finalWitch;


    // Rule 2
    public Light[] candleLights;
    public float flickerDuration = 0.2f; // How long each flicker lasts
    public float timeBetweenFlickers = 0.1f; // Time between the two flickers
    public float startDelay = 60f; // Initial delay of 1 minute before starting flickers
    public float intervalBetweenFlickerSets = 35f; // Time between each flicker set

    float[] defaultIntensities;
    float timer;

    public bool witchReflects = false;

    //Rule 4
    public float intervalBetweenFootsteps = 65f;
    public GameObject footsteps;
    private bool areSafe = false;
    private bool hearFootsteps = false;
    public GameObject witchHolder2;
    public GameObject witchHolder3;


    // Rule 6
    public GameObject croce;
    private Coroutine rotationCoroutine;
    private Coroutine timerCoroutine;
    public GameObject soundCroce;
    public GameObject witchCroce;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (nightTimer != null)
        {
            float initialFillAmount = 0.67f; // Set your initial fill amount
            nightTimer.fillAmount = initialFillAmount; // Visual representation
            startNightDuration = nightDuration / initialFillAmount;
            UnityEngine.Debug.Log("TIMERRR Initial Fill Amount: " + nightTimer.fillAmount);
            UnityEngine.Debug.Log("TIMERRR Start Night Duration: " + startNightDuration);
        }

        if (GameManager.instance.currentLevel == 1)
        {
            // Rule 2 
            defaultIntensities = new float[candleLights.Length];
            for (int i = 0; i < candleLights.Length; i++)
            {
                defaultIntensities[i] = candleLights[i].intensity; // Store the default intensity for each light
            }
            StartCoroutine(FlickerRoutine());
        } else if (GameManager.instance.currentLevel == 2)
        {
            //Rule 4
            UnityEngine.Debug.Log("FOOTSTEPS level 2");
            StartCoroutine(FootstepsRoutine());
        }
        else if (GameManager.instance.currentLevel == 3)
        {
            // Rule 6
            RotationCycle();
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.currentLevel == 2)
        {
            if (hearFootsteps && !areSafe)
            {
                witchHolder2.SetActive(true);
                
                Invoke("LoadGO", 3f);
            }
            UnityEngine.Debug.Log("NOTSAFE " + hearFootsteps + areSafe);
        }
        if (timerStarted)
        {
            timePassed += Time.deltaTime;
            secondTimer = secondTimer + Time.deltaTime;
            if (secondTimer >= 1f)
            {
                nightDuration--;
                secondTimer = 0f;

                if (nightTimer != null)
                {
                    nightTimer.fillAmount = Mathf.Clamp(nightDuration / startNightDuration, 0f, 1f);
                    UnityEngine.Debug.Log("TIMERRR Updated Fill Amount: " + nightTimer.fillAmount);

                    if (timePassed >= 40f)
                    {
                        UnityEngine.Debug.Log("MIDNIGHT time " + timePassed);


                        timePassed = 0;
                        UnityEngine.Debug.Log("MIDNIGHT clock " + clockArrow.GetComponent<AudioSource>());
                                //clockArrow.SetActive(true);
                        clockArrow.GetComponent<AudioSource>().Play();
                        Invoke("RemoveArrow", 15f);

                        if (GameManager.instance.currentLevel == 1)
                        {
                            UnityEngine.Debug.Log("MIDNIGHT current level");
                            PlayerMovement pm = FindObjectOfType<PlayerMovement>();
                            if (pm != null && pm.doorLocked == false)
                            {
                                //game over
                                UnityEngine.Debug.Log("MIDNIGHT game over " + pm.doorLocked);
                                witchHolder.SetActive(true);
                                Invoke("LoadGO", 3f);

                            }
                        } else
                        {
                            UnityEngine.Debug.Log("MIDNIGHT otther level" + GameManager.instance.currentLevel);
                        }
                    }
                        

                        
                 }
            }
        }

            //1 abd 2 nights
            if (nightDuration <= 0 && (GameManager.instance.currentLevel == 2 || GameManager.instance.currentLevel == 1))
            {
                chick.SetActive(true);
                Invoke("LoadSurvive", 4f);

            } else if (nightDuration <= 0 && (GameManager.instance.currentLevel == 3))
            {
                LoadGO();
            } else if (nightDuration <= 10 && GameManager.instance.currentLevel == 3)
            {
                finalWitch.SetActive(true);
            }
        
    }

    void LoadSurvive()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene("SurvivedScene");
    }

    private void RemoveArrow()
    {
        Destroy(clockArrow);
    }

    void LoadGO()
    {
        Cursor.lockState = CursorLockMode.Confined;
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

    // Rule 2
    IEnumerator FlickerRoutine()
    {
        //if (currentLevel == 1)
        //{


        // Wait for the initial delay before starting (1 minute)
        yield return new WaitForSeconds(startDelay);

        while (true) // Infinite loop to keep repeating the flicker every 45 seconds
        {
            // Flicker 1: Candle flickers down
            FlickerOnce();
            yield return new WaitForSeconds(flickerDuration);

            ResetToDefaultIntensity();

            // Small pause between flickers
            yield return new WaitForSeconds(timeBetweenFlickers);

            // Flicker 2: Candle flickers down again
            FlickerOnce();
            yield return new WaitForSeconds(flickerDuration);

            // Return the candle to the default intensity
            ResetToDefaultIntensity();

            witchReflects = true;
            yield return new WaitForSeconds(10);
            witchReflects = false;

            // Wait for 45 seconds before the next flicker set
            yield return new WaitForSeconds(intervalBetweenFlickerSets);
        }
        //}
    }

    void FlickerOnce()
    {
        // Simulate a flicker by reducing the intensity
        foreach (Light candleLight in candleLights)
        {
            candleLight.intensity = 0.1f;
        }
    }

    void ResetToDefaultIntensity()
    {
        // Reset all lights to their default intensities
        for (int i = 0; i < candleLights.Length; i++)
        {
            candleLights[i].intensity = defaultIntensities[i];
        }
    }

    //Rule 4

    IEnumerator FootstepsRoutine()
    {
       


        yield return new WaitForSeconds(startDelay + 15f);

        while (true)
        {
            UnityEngine.Debug.Log("FOOTSTEPS");
            //footsteps.GetComponent<AudioSource>().Play();
            footsteps.GetComponent<AudioSource>().PlayOneShot(footsteps.GetComponent<AudioSource>().clip);

            
            yield return new WaitForSeconds(37f);
            hearFootsteps = true;
            foreach (Light candle in candleLights)
            {
                if (candle.intensity > 0)
                {
                    areSafe = false;
                }
                else
                {
                    areSafe = true;
                }
            }
            
            yield return new WaitForSeconds(2f);
            hearFootsteps = false;
            // Wait for 45 seconds before the next flicker set
            yield return new WaitForSeconds(intervalBetweenFootsteps);
        }
        
    }

    // Rule 6
    public void RotationCycle()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine); // Stop any existing coroutine if needed
        }
        rotationCoroutine = StartCoroutine(ChangeCroceRotation());
    }

    private IEnumerator ChangeCroceRotation()
    {
       


        while (true)
        {
            float randomInterval = Random.Range(0f, 120f);

            yield return new WaitForSeconds(randomInterval);

            float currentXRotation = croce.transform.eulerAngles.x;
            if (Mathf.Abs(currentXRotation - 0f) < 0.1f || Mathf.Abs(currentXRotation - 360f) < 0.1f)
            {
                croce.transform.Rotate(180f, 0f, 0f, Space.World);
                soundCroce.SetActive(true);

                if (timerCoroutine != null)
                {
                    StopCoroutine(timerCoroutine); // Stop any previous timer coroutine
                }
                timerCoroutine = StartCoroutine(StartTimer());
            }
        }
        
    }

    private IEnumerator StartTimer()
    {
        float timer = 10f; // Set the timer for 10 seconds
        float initialRotationX = croce.transform.eulerAngles.x; // Store the initial x-rotation

        while (timer > 0)
        {
            // Check if the rotation has changed externally
            if (Mathf.Abs(croce.transform.eulerAngles.x - initialRotationX) > 0.1f)
            {
                yield break; // Exit the timer coroutine and reset the timer
            }

            timer -= Time.deltaTime; // Countdown the timer
            yield return null; // Wait for the next frame
        }

        // If the timer completes without external rotation change, load a new scene
        //game over scene
        witchCroce.SetActive(true);
        Invoke("LoadGO", 3f);
    }
}
