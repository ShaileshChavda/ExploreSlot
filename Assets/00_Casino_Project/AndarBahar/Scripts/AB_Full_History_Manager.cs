using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AB_Full_History_Manager : MonoBehaviour
{
    public static AB_Full_History_Manager Inst;
    public RectTransform DataParent;
    public List<GameObject> CellList;
    public TextMeshProUGUI Andar_Percentage, Bahar_Percentage, Andar_History_Percentage, Bahar_History_Percentage,Txt_Joker_text;
    [SerializeField] GameObject PFB_Full_History_DOT;
    public List<TextMeshProUGUI> List_8_History;
    [SerializeField] Sprite _DOT_BLUE, _DOT_RED;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_DATA(JSONObject data)
    {
        Clear_OLD_HIST();
        Andar_Percentage.text = data.GetField("andar").ToString().Trim(Config.Inst.trim_char_arry) + "%";
        Bahar_Percentage.text = data.GetField("bahar").ToString().Trim(Config.Inst.trim_char_arry) + "%";

        Andar_History_Percentage.text = data.GetField("blud_pro").ToString().Trim(Config.Inst.trim_char_arry) + "%";
        Bahar_History_Percentage.text = data.GetField("red_pro").ToString().Trim(Config.Inst.trim_char_arry) + "%";

        for (int i = 0; i < data.GetField("last_win_cards").Count; i++)
        {
            SET_HIST_CARD_DATA(i, data.GetField("last_win_cards")[i].ToString().Trim(Config.Inst.trim_char_arry));
        }
        for (int i = 0; i < data.GetField("history").Count; i++)
        {
            string Dot_Name = data.GetField("history")[i].ToString().Trim(Config.Inst.trim_char_arry);
            GameObject cell = Instantiate(PFB_Full_History_DOT, DataParent) as GameObject;
            CellList.Add(cell);
            if (Dot_Name.Equals("blue"))
                cell.transform.GetComponent<Image>().sprite = _DOT_BLUE;
            else
                cell.transform.GetComponent<Image>().sprite = _DOT_RED;
        }
    }

    public void RESET_Probability() {
        Andar_Percentage.text = "50%";
        Bahar_Percentage.text = "50%";
    }

    public void SET_HIST_CARD_DATA(int index, string CardName)
    {
        string[] split_XCard = CardName.Split('|');
        string[] split_NO = split_XCard[1].Split('-');

        if (split_XCard[0].Equals("andar"))
        {
            List_8_History[index].color = new Color32(0, 162, 255, 255);

            if (split_NO[1].Equals("11"))
                List_8_History[index].text = "J";
            else if (split_NO[1].Equals("12"))
                List_8_History[index].text = "Q";
            else if (split_NO[1].Equals("13"))
                List_8_History[index].text = "K";
            else if (split_NO[1].Equals("1"))
                List_8_History[index].text = "A";
            else
                List_8_History[index].text = split_NO[1];
        }
        else
        {
            List_8_History[index].color = new Color32(255, 95, 163, 255);

            if (split_NO[1].Equals("11"))
                List_8_History[index].text = "J";
            else if (split_NO[1].Equals("12"))
                List_8_History[index].text = "Q";
            else if (split_NO[1].Equals("13"))
                List_8_History[index].text = "K";
            else if (split_NO[1].Equals("1"))
                List_8_History[index].text = "A";
            else
                List_8_History[index].text = split_NO[1];
        }
    }

    public void Clear_OLD_HIST()
    {
        if (CellList.Count > 0)
        {
            for (int i = 0; i < CellList.Count; i++)
            {
                if (CellList[i] != null)
                    Destroy(CellList[i]);
            }
            CellList.Clear();
        }
    }
}
