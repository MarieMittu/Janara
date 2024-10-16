using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleFive : MonoBehaviour
{
    public float timeRemaining = 10;
    bool isGuarding = false;
    [SerializeField] GameObject baby;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGuarding && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            //Debug.Log("GUARDA here" + timeRemaining);
        }

        if (timeRemaining < 1)
        {
            Destroy(baby);
            Debug.Log("GUARDA " + timeRemaining);
            //game over scene
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GUARDA"))
        {
            //stop and reset countdown
            Debug.Log("GUARDA okay");
            isGuarding = true;
            timeRemaining = 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GUARDA"))
        {
            //start countdown
            Debug.Log("GUARDA madonnna");
            isGuarding = false;
        }
    }
}
