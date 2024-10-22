namespace ZooRoulette_Game
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ZooRoulette_DataConfig
    {
        const string BETDATA_KEY = "betdata";
        const string REBETDATA_KEY = "rebetdata";

        //***** XML BET DETAILS *****\\
        public static UserBetDataClass LoadUserBetData()
        {
            UserBetDataClass val = null;
            string jsonstring = PlayerPrefs.GetString(BETDATA_KEY);

            // Debug.Log("LOAD DATA"+ jsonstring);
            if (string.IsNullOrEmpty(jsonstring))
            {
                val = new UserBetDataClass(0, new List<int>(), new List<BetDataClass>());
            }
            else
            {
                val = JsonUtility.FromJson<UserBetDataClass>(jsonstring);
            }
            return val;
        }
        public static void SaveUserBetData(UserBetDataClass data)
        {
            string jsonstring = JsonUtility.ToJson(data);
            // Debug.Log("SAVE DATA" + jsonstring);
            PlayerPrefs.SetString(BETDATA_KEY, jsonstring);
            PlayerPrefs.Save();
        }
    }
}
