using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Crash_HistoryManager : MonoBehaviour
{
    public static Crash_HistoryManager Inst;
    public Sprite normalBoxSprite,red_Hist_Sprite, green_Hist_Sprite;
    [SerializeField] public Color histColorGreen, histColorGray;
    public List<GameObject> HIST_LIST;
   
    void Start()
    {
        Inst = this;
    }

    public void SET_HISTO(JSONObject data)
    {        
        for (int i = 0; i < data.GetField("last_win_cards").Count; i++)
        {
            SET_HIST_CARD_DATA(i, data.GetField("last_win_cards")[i].ToString().Trim(Config.Inst.trim_char_arry));
        }
    }
    public void SET_HIST_CARD_DATA(int index,string cardName)
    {
        string[] split_XCard = cardName.Split('|');
        float crashAt = float.Parse(split_XCard[0]);

        HIST_LIST[index].GetComponentInChildren<TextMeshProUGUI>().text = crashAt.ToString();

        if (crashAt >= 2.0f)
        {
            HIST_LIST[index].GetComponent<Image>().color = histColorGreen;
            HIST_LIST[index].transform.Find("green").gameObject.SetActive(false);
            HIST_LIST[index].transform.Find("red").gameObject.SetActive(true);
        }
        else
        {
            HIST_LIST[index].GetComponent<Image>().color = histColorGray;
            HIST_LIST[index].transform.Find("green").gameObject.SetActive(true);
            HIST_LIST[index].transform.Find("red").gameObject.SetActive(false);

        }


    }
}
