using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Rule 2
    public Light[] candleLights;
    public float flickerDuration = 0.2f; // How long each flicker lasts
    public float timeBetweenFlickers = 0.1f; // Time between the two flickers
    public float startDelay = 60f; // Initial delay of 1 minute before starting flickers
    public float intervalBetweenFlickerSets = 45f; // Time between each flicker set

    float[] defaultIntensities;
    float timer;

    // Rule 6
    public GameObject croce;
    private Coroutine rotationCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        defaultIntensities = new float[candleLights.Length];
        for (int i = 0; i < candleLights.Length; i++)
        {
            defaultIntensities[i] = candleLights[i].intensity; // Store the default intensity for each light
        }
        StartCoroutine(FlickerRoutine());

        RotationCycle();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FlickerRoutine()
    {
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

            // Wait for 45 seconds before the next flicker set
            yield return new WaitForSeconds(intervalBetweenFlickerSets);
        }
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
            float randomInterval = UnityEngine.Random.Range(0f, 120f);

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
