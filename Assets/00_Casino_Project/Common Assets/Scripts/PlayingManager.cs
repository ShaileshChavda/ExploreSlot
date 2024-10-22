using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableData
{
    public string TableId;
    public TableType _TableType;
    public string TableName;
    public int MinimumPlayerOnTable;
    public int MaximumSeatOnTable;
    public int AvailablePlayer;
    public float BootValue;
    public string Winning_Prize;
    public float RaiseMaxValue;
    public float RaiseNextValue;
    public float RaiseIncrementValue;
    public int PoolRummyPoint;
    public string NumberOfDeals;
    public string PlayingTableHeaderId;
    public string PlayingTableDeck;
    public string PlayingTableRound;
    public double PoolRummyPrizeValue;
    public string TableStatus;


    public TableData()
    {
        TableId = "";
        TableName = "";
        MinimumPlayerOnTable = 0;
        MaximumSeatOnTable = 0;
        AvailablePlayer = 0;
        BootValue = 0;
        Winning_Prize = "";
        RaiseMaxValue = 0.0f;
        RaiseNextValue = 0.0f;
        RaiseIncrementValue = 0.0f;
        PoolRummyPoint = 0;
        NumberOfDeals = "";
        _TableType = TableType.None;
        PlayingTableHeaderId = "";
        PlayingTableDeck = "";
        PlayingTableRound = "";
        PoolRummyPrizeValue = 0;
        TableStatus = "";
    }

}

public class PlayerData
{
    public int seatIndex;
    public string userId;
    public string userName;
    public string profilePicture;
    public string userStatus;
    public double userChips;
    public double userCash;
    public double userPlayingCash;
    public double userPoint;
    public int userSecondaryTimer;

    public PlayerData()
    {
        seatIndex = -1;
        userId = "";
        userName = "";
        profilePicture = "";
        userChips = 0;
        userCash = 0;
        userSecondaryTimer = 0;
        userStatus = "";
        userPoint = 0;
    }
}

public enum SeatStatus
{
    Empty,
    Seated,
    Allowed
}

public enum TableType
{
    Classic,
    Deal,
    Pool,
    Raise,
    None,
}

public class PlayingManager : MonoBehaviour
{
    public static PlayingManager Inst;
    internal TableData _TableData = new TableData();
    internal PlayerData _MyData = new PlayerData();
    internal JSONObject MyTableData = new JSONObject();
    internal JSONObject AllUserOnTableData = new JSONObject(JSONObject.Type.ARRAY);
    internal JSONObject RemainingEventList = new JSONObject(JSONObject.Type.ARRAY);
    internal bool isDealTieBreaker = false;
    internal bool IsExitFromPlaying = false;
    internal JSONObject TableInfoJson = new JSONObject();
   

    void Awake()
    {
        Inst = this;
    }

    internal void SetMySeatIndex(JSONObject data)
    {
        JSONObject allUser = new JSONObject(JSONObject.Type.ARRAY);
        allUser = data.GetField("player_info");
        for (int i = 0; i < allUser.Count; i++)
        {

            if (allUser[i].Count > 0)
            {
                string id = allUser[i].GetField("user_info").GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
                if (id.Equals(GS.Inst._userData.Id))
                {
                    PlayingManager.Inst.MyTableData = allUser[i];
                    _MyData.seatIndex = int.Parse(allUser[i].GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
                    _MyData.userId = allUser[i].GetField("user_info").GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);

                }
            }

        }
        PlayingManager.Inst.AllUserOnTableData = data.GetField("player_info");
    }
}
