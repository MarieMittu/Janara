using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

    public float nightDuration = 60f;
    public float startNightDuration;
    float secondTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        secondTimer = secondTimer + Time.deltaTime;
        if (secondTimer >= 1f)
        {
            nightDuration--;
            secondTimer = secondTimer - 1f;

            
        }



        if (nightDuration <= 0)
        {

            SceneManager.LoadScene("GameOverScene"); // change to normal
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TIMER"))
        {
            //start timer night
            startNightDuration = nightDuration;
        }
    }


}
