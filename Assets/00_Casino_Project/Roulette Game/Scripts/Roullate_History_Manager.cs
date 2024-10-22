using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roullate_History_Manager : MonoBehaviour
{
    public static Roullate_History_Manager Inst;
    public List<Roullate_Hist_Card> ROULLATE_HIST_LIST;
    public List<Sprite> Ball_Sprite;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_HISTO(JSONObject data)
    {
        for (int i = 0; i < ROULLATE_HIST_LIST.Count; i++)
        {
            ROULLATE_HIST_LIST[i].SET_HIST(data.GetField("last_win_cards")[i].ToString().Trim(Config.Inst.trim_char_arry));
        }
    }
}
