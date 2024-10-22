using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Net.Sockets;

public class Maintenance : MonoBehaviour
{
    public static Maintenance Inst;

    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Start_MM_MODE()
    {
        transform.localScale = Vector3.one;
    }

    public void BTN_EXIT()
    {
        PreeLoader.Inst.Show();
        Application.Quit();
    }
}
