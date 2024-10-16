using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Rule 2
    public Light candleLight;
    public float maxInterval = 1;
    public float maxFlicker = 0.2f;

    float defaultIntensity;
    bool isOn;
    float timer;
    float delay;

    // Rule 6
    public GameObject croce;
    private Coroutine rotationCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        defaultIntensity = candleLight.intensity;

        RotationCycle();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
        {
            FlickerCandle();
        }

      

    }

    void FlickerCandle()
    {
        isOn = !isOn;

        if (isOn)
        {
            candleLight.intensity = defaultIntensity;
            delay = Random.Range(0, maxInterval);
        }
        else
        {
            candleLight.intensity = Random.Range(0.6f, defaultIntensity);
            delay = Random.Range(0, maxFlicker);
        }

        timer = 0;
    }

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
            Debug.Log("crrrr before " + currentXRotation);
            if (Mathf.Abs(currentXRotation - 0f) < 0.1f || Mathf.Abs(currentXRotation - 360f) < 0.1f) 
            {
                croce.transform.Rotate(180f, 0f, 0f, Space.World);
                Debug.Log("crrrr after " + croce.transform.eulerAngles.x);
            }
        }
    }
}
