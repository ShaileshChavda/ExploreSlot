using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HR_EventSetup : MonoBehaviour
{
    public static HR_EventSetup Inst;
    public static event Action<string> _DT_BetSelect;
    public static event Action<string> _DT_CHAAL;
    public static event Action<string> _DT_LEAVE;
    public static event Action<JSONObject> _DT_SEAT;
    public static event Action _DT_PFB_COIN_KILL;
    public static event Action _DT_PFB_COIN_WIN_MOVE;

    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    public static void SelectedBET_DT(string nameselected)
    {
        if (_DT_BetSelect != null)
            _DT_BetSelect(nameselected);
    }

    public static void BET_CHAAL(string ID)
    {
        if (_DT_CHAAL != null)
            _DT_CHAAL(ID);
    }

    public static void PFB_COIN_DT()
    {
        if (_DT_PFB_COIN_KILL != null)
            _DT_PFB_COIN_KILL();
    }

    public static void PFB_COIN_DT_WIN_MOVE()
    {
        if (_DT_PFB_COIN_WIN_MOVE != null)
            _DT_PFB_COIN_WIN_MOVE();
    }

    public static void USER_LEAVE(string ID)
    {
        if (_DT_LEAVE != null)
            _DT_LEAVE(ID);
    }

    public static void USER_SEAT(JSONObject data)
    {
        if (_DT_SEAT != null)
            _DT_SEAT(data);
    }

    public void CLEAR_EVENT_DATA()
    {
        _DT_BetSelect = null;
        _DT_CHAAL = null;
        _DT_PFB_COIN_KILL = null;
        _DT_PFB_COIN_WIN_MOVE = null;
        _DT_LEAVE = null;
        _DT_SEAT = null;
    }
}
