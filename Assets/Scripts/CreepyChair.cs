using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepyChair : MonoBehaviour
{

    public float minRotation = -7f;
    public float maxRotation = 7f;
    public float rotationSpeed = 30f; 

    private bool isRotating = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RotateObjectRandomly());
    }

    private IEnumerator RotateObjectRandomly()
    {
        while (true)
        {
            float randomInterval = Random.Range(30f, 120f); // Adjust as needed
            yield return new WaitForSeconds(randomInterval);

            yield return StartCoroutine(RotateBackAndForth());

            yield return StartCoroutine(ReturnToZero());
        }
    }

    private IEnumerator RotateBackAndForth()
    {
        float randomDuration = Random.Range(5f, 10f); 
        float elapsedTime = 0f;

        while (elapsedTime < randomDuration)
        {
            float angle = Mathf.Lerp(minRotation, maxRotation, Mathf.PingPong(Time.time * rotationSpeed, 1));

            transform.eulerAngles = new Vector3(angle, transform.eulerAngles.y, transform.eulerAngles.z);

            elapsedTime += Time.deltaTime;
            yield return null; 
        }
    }

    private IEnumerator ReturnToZero()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0f, transform.eulerAngles.y, transform.eulerAngles.z);
        float elapsedTime = 0f;
        float duration = 1f; 

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
    }

}
