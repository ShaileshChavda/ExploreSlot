using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Roullate_EventSetup : MonoBehaviour
{
    public static Roullate_EventSetup Inst;
    public static event Action<string>  _Roullate_BetSelect;
    public static event Action<string>  _Roullate_BetSelect_SEND;
    public static event Action<string> _Roullate_CHAAL;
    public static event Action<string> _Roullate_LEAVE;
    public static event Action<JSONObject> _Roullate_SEAT;
    public static event Action _RESET_ALL_GLOW;
    public static event Action _ROULLATE_PFB_COIN_KILL;
    public static event Action _ROULLATE_PFB_COIN_WIN_MOVE;

    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    public static void SelectedBET_Roullate(string nameselected)
    {
        if (_Roullate_BetSelect != null)
            _Roullate_BetSelect(nameselected);
    }

    public static void SelectedBET_Roullate_SEND(string nameselected)
    {
        if (_Roullate_BetSelect_SEND != null)
            _Roullate_BetSelect_SEND(nameselected);
    }

    public static void BET_CHAAL(string nameselected)
    {
        if (_Roullate_CHAAL != null)
            _Roullate_CHAAL(nameselected);
    }

    public static void RESET_ALL_GLOW()
    {
        if (_RESET_ALL_GLOW != null)
            _RESET_ALL_GLOW();
    }
    public static void PFB_COIN_ROULLATE()
    {
        if (_ROULLATE_PFB_COIN_KILL != null)
            _ROULLATE_PFB_COIN_KILL();
    }
    public static void PFB_COIN_ROULLATE_WIN_MOVE()
    {
        if (_ROULLATE_PFB_COIN_WIN_MOVE != null)
            _ROULLATE_PFB_COIN_WIN_MOVE();
    }
    public static void USER_LEAVE(string ID)
    {
        if (_Roullate_LEAVE != null)
            _Roullate_LEAVE(ID);
    }

    public static void USER_SEAT(JSONObject data)
    {
        if (_Roullate_SEAT != null)
            _Roullate_SEAT(data);
    }

    public void CLEAR_EVENT_DATA()
    {
        _Roullate_BetSelect = null;
        _Roullate_BetSelect_SEND = null;
        _Roullate_CHAAL = null;
        _RESET_ALL_GLOW = null;
        _ROULLATE_PFB_COIN_KILL = null;
        _ROULLATE_PFB_COIN_WIN_MOVE = null;
        _Roullate_LEAVE = null;
        _Roullate_SEAT = null;
    }
}
