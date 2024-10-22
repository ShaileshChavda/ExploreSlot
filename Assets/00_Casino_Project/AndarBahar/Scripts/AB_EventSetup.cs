using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AB_EventSetup : MonoBehaviour
{
    public static AB_EventSetup Inst;
    public static event Action<string> _AB_BetSelect;
    public static event Action<string> _AB_CHAAL;
    public static event Action<string> _AB_LEAVE;
    public static event Action<JSONObject> _AB_SEAT;
    public static event Action _AB_PFB_COIN_KILL;
    public static event Action _AB_PFB_COIN_WIN_MOVE;


    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    public static void SelectedBET_DT(string nameselected)
    {
        if (_AB_BetSelect != null)
            _AB_BetSelect(nameselected);
    }

    public static void BET_CHAAL(string ID)
    {
        if (_AB_CHAAL != null)
            _AB_CHAAL(ID);
    }

    public static void PFB_COIN_DT()
    {
        if (_AB_PFB_COIN_KILL != null)
            _AB_PFB_COIN_KILL();
    }

    public static void PFB_COIN_DT_WIN_MOVE()
    {
        if (_AB_PFB_COIN_WIN_MOVE != null)
            _AB_PFB_COIN_WIN_MOVE();
    }

    public static void USER_LEAVE(string ID)
    {
        if (_AB_LEAVE != null)
            _AB_LEAVE(ID);
    }

    public static void USER_SEAT(JSONObject data)
    {
        if (_AB_SEAT != null)
            _AB_SEAT(data);
    }

    public void CLEAR_EVENT_DATA()
    {
        _AB_BetSelect = null;
        _AB_CHAAL = null;
        _AB_PFB_COIN_KILL = null;
        _AB_PFB_COIN_WIN_MOVE = null;
        _AB_LEAVE = null;
        _AB_SEAT = null;
    }
}
