using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_BETLIST : MonoBehaviour
{
    public static PFB_BETLIST Inst;
    [SerializeField] Text Title, Txt_MaxSeat, TxtPointValue, Txt_MinEntry,Txt_ActivePlayers;
    string Bet_ID;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_BET_DATA_LIST(JSONObject data)
    {
        Title.text = "TeenPatti";
        Bet_ID = data.GetField("boot_id").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_MaxSeat.text = data.GetField("max_seat").ToString().Trim(Config.Inst.trim_char_arry);
        TxtPointValue.text = data.GetField("boot").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_MinEntry.text = data.GetField("min_entry").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_ActivePlayers.text = data.GetField("active_player").ToString().Trim(Config.Inst.trim_char_arry);
    }

    public void BTN_PLAY_NOW()
    {
        PreeLoader.Inst.Show();
        if (!GS.Inst.PrivateTable)
            SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_JoinTable(Bet_ID));
        else
            SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_PrivateTable(Bet_ID));
    }
}
