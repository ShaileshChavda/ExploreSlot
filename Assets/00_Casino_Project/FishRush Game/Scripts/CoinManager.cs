using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoinManager
{
    public static void AddCoin(int scene)
    {
        Debug.Log("ADD GOLD COIN");
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 100000);
    }
}
