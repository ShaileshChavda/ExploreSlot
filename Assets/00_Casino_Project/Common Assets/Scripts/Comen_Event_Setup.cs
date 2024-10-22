using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Comen_Event_Setup : MonoBehaviour
{
    public static Comen_Event_Setup Inst;
    public static event Action<string> _Shop_UPI;
    public static event Action<int> _Shop_PFB;

    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    public static void Selected_Shop_UPI(string nameselected)
    {
        if (_Shop_UPI != null)
            _Shop_UPI(nameselected);
    }

    public static void Selected_Shop_PFB(int index)
    {
        if (_Shop_PFB != null)
            _Shop_PFB(index);
    }

    public void Clear_Shop_PFB()
    {
        _Shop_PFB = null;
    }
}
