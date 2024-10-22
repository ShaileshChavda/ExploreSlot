namespace CarRoulette_Game
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using UnityEngine;

    public class CarRoulette_EventManager : MonoBehaviour
    {
        public static CarRoulette_EventManager Inst;
        public static event Action<string> _USER_CHAL;
        public static event Action<string> _USER_LEAVE;
        public static event Action<JSONObject> _USER_SEAT;
        public static event Action _COIN_KILL_PREFAB;
        public static event Action _COIN_WIN_MOVE_PREFAB;

        public static event Action<string> _SELECTED_BET;
        public static event Action<string> _SELECTED_BET_SEND;

        public List<Sprite> _chipSptList;

        // Start is called before the first frame update
        void Awake()
        {
            Inst = this;
        }

        public static string RemoveSpecialCharacters(string input)
        {
            // Define a regular expression pattern for special characters
            string pattern = "[^a-zA-Z0-9 ]";

            // Use Regex.Replace to remove special characters
            string result = Regex.Replace(input, pattern, "");

            return result;
        }

        public static void SELECTED_BET(string nameselected)
        {
            if (_SELECTED_BET != null)
                _SELECTED_BET(nameselected);
        }

        public static void SELECTED_BET_SEND(string nameselected)
        {
            if (_SELECTED_BET_SEND != null)
                _SELECTED_BET_SEND(nameselected);
        }

        public static void BET_CHAAL(string ID)
        {
            if (_USER_CHAL != null)
                _USER_CHAL(ID);
        }

        public static void PFB_COIN_KILL()
        {
            if (_COIN_KILL_PREFAB != null)
                _COIN_KILL_PREFAB();
        }

        public static void PFB_COIN_DT_WIN_MOVE()
        {
            if (_COIN_WIN_MOVE_PREFAB != null)
                _COIN_WIN_MOVE_PREFAB();
        }
        public static void USER_LEAVE(string ID)
        {
            if (_USER_LEAVE != null)
                _USER_LEAVE(ID);
        }

        public static void USER_SEAT(JSONObject data)
        {
            if (_USER_SEAT != null)
                _USER_SEAT(data);
        }

        public void CLEAR_EVENT_DATA()
        {
            _SELECTED_BET = null;
            _SELECTED_BET_SEND = null;
            _USER_CHAL = null;
            _COIN_KILL_PREFAB = null;
            _COIN_WIN_MOVE_PREFAB = null;
            _USER_LEAVE = null;
            _USER_SEAT = null;
        }
    }
}
