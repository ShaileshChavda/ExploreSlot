using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TP_TableInfo : MonoBehaviour
{
    public static TP_TableInfo Inst;
    [SerializeField]public Text TxtBootAmount, TxtMaxBlinds, TxtChaalLimit, TxtPotLimit;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void OPEN_TABLE_INFO()
    {
        PreeLoader.Inst.Show();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_LeaveTable());
    }

    //-------------- Information Sceeen -------------------
    public void SET_TABLE_INFO_DATA(JSONObject data)
    {
        TxtBootAmount.text = data.GetField("boot").ToString().Trim(Config.Inst.trim_char_arry);
        TxtMaxBlinds.text = data.GetField("max_blind").ToString().Trim(Config.Inst.trim_char_arry);
        TxtChaalLimit.text = data.GetField("chal_limit").ToString().Trim(Config.Inst.trim_char_arry);
        TxtPotLimit.text = data.GetField("pot_limit").ToString().Trim(Config.Inst.trim_char_arry);
        GS.Inst.iTwin_Open(this.gameObject);
    }
    //-------------- Information Sceeen -------------------


    public void BTN_OK_INFO()
    {
        GS.Inst.iTwin_Close(this.gameObject,0.3f);
    }
}
