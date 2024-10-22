using UnityEngine;
using System.Collections;

public class MainPlayControl : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("Home");
        }

    }
}
