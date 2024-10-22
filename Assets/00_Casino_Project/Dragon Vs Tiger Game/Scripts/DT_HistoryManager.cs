using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DT_HistoryManager : MonoBehaviour
{
    public static DT_HistoryManager Inst;
    public Sprite T_Hist_Sprite, D_Hist_Sprite, TIE_Hist_Sprite,Defoult_Hist_Sprite;
    public List<DT_HIST_CARD> DT_HIST_LIST;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_HISTO(JSONObject data)
    {
        Debug.Log("Naresh History: "+data.GetField("last_win_cards").Count);       
        for (int i = 0; i < data.GetField("last_win_cards").Count; i++)
        {
            DT_HIST_LIST[i].SET_HIST_CARD_DATA(data.GetField("last_win_cards")[i].ToString().Trim(Config.Inst.trim_char_arry));
        }
    }
}
