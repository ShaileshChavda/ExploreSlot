using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crash_Hist_Annal : MonoBehaviour
{
    public static Crash_Hist_Annal Inst;
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
        int fixedCount;
        if (data.GetField("result").Count > 5)
        {
            fixedCount = 5;
        }
        else
        {
            fixedCount = data.GetField("result").Count;
        }
        for (int i = 0; i < fixedCount; i++) // 5 vadhare hoe aene display nathi karta
        {           
            string result = data.GetField("result")[i].ToString().Trim(Config.Inst.trim_char_arry);
            string[] split_XCard = result.Split('|');
            float crashAt = float.Parse(split_XCard[0]);
            if (crashAt >= 2)
            {
                Boxes_List[i].sprite = Crash_HistoryManager.Inst.red_Hist_Sprite;
            }
            else
            {
                Boxes_List[i].sprite = Crash_HistoryManager.Inst.green_Hist_Sprite;
            }
        }
    }
    public void Reset_ANNAL_Box()
    {
        for (int i = 0; i < Boxes_List.Count; i++)
        {            
            Boxes_List[i].sprite = Crash_HistoryManager.Inst.normalBoxSprite;           
        }
    }
}
