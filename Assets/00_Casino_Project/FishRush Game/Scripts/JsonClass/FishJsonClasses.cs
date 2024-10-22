using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishJsonClasses : MonoBehaviour
{
    
}

[System.Serializable]
public struct BetInfo
{
    public string _id;
    public string name;
    public int max_seat;
    public int point_value;
    public int min_entry;
    public int active_player;
    public List<int> bet_slots;
    public bool is_free;
    public bool is_active;
    public bool is_remove;
    public int index;

    public BetInfo(string id, string n, int max, int point, int min, int active, List<int> slots, bool free, bool activeStatus, bool remove, int idx)
    {
        _id = id;
        name = n;
        max_seat = max;
        point_value = point;
        min_entry = min;
        active_player = active;
        bet_slots = slots;
        is_free = free;
        is_active = activeStatus;
        is_remove = remove;
        index = idx;
    }
}


[System.Serializable]
public struct FishGameInfo
{
    public string _id;
    public string bet_id;
    public string game_type;
    public BetInfo bet_info;
    public double total_wallet;
    public int vip_level;
    public string watch_msg;
    public UserInfo user_info;

    public FishGameInfo(string id, string betId, string type, BetInfo bet, double total, int vip, string watchMsg, UserInfo userInfo)
    {
        _id = id;
        bet_id = betId;
        game_type = type;
        bet_info = bet;
        total_wallet = total;
        vip_level = vip;
        watch_msg = watchMsg;
        user_info = userInfo;
    }
}

[System.Serializable]
public struct UserInfo
{
    public string _id;
    public string user_name;
    public string profile_url;
    public double wallet;

    public UserInfo(string id, string username, string profileUrl, double walletAmount)
    {
        _id = id;
        user_name = username;
        profile_url = profileUrl;
        wallet = walletAmount;
    }
}

[System.Serializable]
public struct FishKillInfo
{
    public bool kill;
    public int win_amount;
    public int fish_key;
    public string player_key;
    public int fish_id;
}
