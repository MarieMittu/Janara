using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{

    public AudioSource audioSource; // The AudioSource attached to the empty GameObject
    public float maxVolume = 0.26f; // The maximum volume level
    public float minVolume = 0f; // The minimum volume level (mute)
    public float timeAtMaxVolume = 30f; // Time to stay at max volume
    public float timeAtMinVolume = 30f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(VolumeCycle());
    }

    private IEnumerator VolumeCycle()
    {
        // Wait for the initial delay of 60 seconds
        yield return new WaitForSeconds(60f);

        // Loop to continuously switch between volumes
        while (true)
        {
            // Play audio with max volume
            audioSource.volume = maxVolume;
            if (!audioSource.isPlaying) audioSource.Play(); // Ensure the audio is playing

            // Wait for the time at max volume
            yield return new WaitForSeconds(timeAtMaxVolume);

            // Mute the audio (set volume to min)
            audioSource.volume = minVolume;

            // Wait for the time at min volume
            yield return new WaitForSeconds(timeAtMinVolume);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
