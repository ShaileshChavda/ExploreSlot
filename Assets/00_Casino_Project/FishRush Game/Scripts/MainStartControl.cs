using UnityEngine;
using System.Collections;

public class MainStartControl : MonoBehaviour
{

    void Start()
    {
        AudioControl.Instance.Offbg();
        AudioControl.Instance.openbg();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
