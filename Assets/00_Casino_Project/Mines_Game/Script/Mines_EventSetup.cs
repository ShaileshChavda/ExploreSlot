using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mines_EventSetup : MonoBehaviour
{
    public static Mines_EventSetup Inst;
    public static event Action<string> _MS_BetSelect;
    public static event Action<string> _MS_CHAAL;

    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    public static void SelectedBET_MS(string nameselected)
    {
        if (_MS_BetSelect != null)
            _MS_BetSelect(nameselected);
    }

    public static void BET_CHAAL(string ID)
    {
        if (_MS_CHAAL != null)
            _MS_CHAAL(ID);
    }


    public void CLEAR_EVENT_DATA()
    {
        _MS_BetSelect = null;
        _MS_CHAAL = null;
    }
}
