using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DT_Hist_Annal : MonoBehaviour
{
    public static DT_Hist_Annal Inst;
    public List<Image> Boxes_List;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Boxes_List[i] = transform.GetChild(i).GetComponent<Image>();
        }
    }

    public void SET_ANNAL_Box(JSONObject data)
    {
        for (int i = 0; i < data.GetField("result").Count; i++)
        {
            string result = data.GetField("result")[i].ToString().Trim(Config.Inst.trim_char_arry);
            if (result.Equals("dragon"))
                Boxes_List[i].sprite = DT_HistoryManager.Inst.D_Hist_Sprite;
            else if (result.Equals("tiger"))
                Boxes_List[i].sprite = DT_HistoryManager.Inst.T_Hist_Sprite;
            else
                Boxes_List[i].sprite = DT_HistoryManager.Inst.TIE_Hist_Sprite;
        }
    }

    public void RESET_ANNAL_BOX()
    {
        for (int i = 0; i < Boxes_List.Count; i++)
        {
            Boxes_List[i].sprite = DT_HistoryManager.Inst.Defoult_Hist_Sprite;
        }
    }
}
