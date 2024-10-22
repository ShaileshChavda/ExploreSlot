using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_HistoryManager : MonoBehaviour
{
    public static HR_HistoryManager Inst;
    public List<HR_HistCard> HR_HIST_LIST;
    public List<Color>Number_Color_List;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public Color Get_Number_Color(int no)
    {
        return Number_Color_List[no - 1];
    }

    public void SET_HISTO(JSONObject data)
    {
        int j = 0;
        for (int i = data.GetField("last_win_cards").Count; i > 0; i--)
        {
            //Debug.Log("HIST :" + data.GetField("last_win_cards")[i-1].ToString().Trim(Config.Inst.trim_char_arry));
            HR_HIST_LIST[j].SET_HIST(data.GetField("last_win_cards")[i-1].ToString().Trim(Config.Inst.trim_char_arry));
            j++;
        }
        //for (int i = 0; i < HR_HIST_LIST.Count; i++)
        //{
        //    HR_HIST_LIST[i].SET_HIST(data.GetField("last_win_cards")[i].ToString().Trim(Config.Inst.trim_char_arry));
        //}
    }
}
