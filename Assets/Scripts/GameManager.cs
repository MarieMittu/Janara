using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance to access GameManager globally
    public int currentLevel = 1; // Default level is 1
    public int maxLevels = 3;


   

    private void Awake()
    {
        // Ensure there's only one instance of GameManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy the GameManager between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLevel(int level)
    {
        if (level > 0 && level <= maxLevels)
        {
            currentLevel = level;
        }
        else
        {
        }
    }

    public void NextLevel()
    {
        if (currentLevel < maxLevels)
        {
            currentLevel++;
        }
        else
        {
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
   
    }

    // Update is called once per frame
    void Update()
    {
        
            
    }
    

}
