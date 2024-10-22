using UnityEngine;
using System;

public class splashControl : MonoBehaviour
{


    void Start()
    {
      
        Invoke("Play", 2);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void Play()
    {
        Application.LoadLevel("Home");
    }

  
}
