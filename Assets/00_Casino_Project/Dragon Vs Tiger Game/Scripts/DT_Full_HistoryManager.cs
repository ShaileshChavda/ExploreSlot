using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DT_Full_HistoryManager : MonoBehaviour
{
    public static DT_Full_HistoryManager Inst;
    public DT_PFB_HIST_FIGHT _PFB_FIGHT;
    public RectTransform DataParent;
    public List<GameObject> CellList;
    [SerializeField] Text Dragon_Percentage, Tiger_Percentage;
    [SerializeField] Text Total_Dragon, Total_Tiger,Total_Tie,Total_Count;
    [SerializeField] List<Image> Uper_20_Card_Hist_List;
    [SerializeField] Image Percentage_Filler;
    [SerializeField] List<DT_Hist_Annal> Annal_Box_List;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_DATA(JSONObject data)
    {
        Debug.Log("naresh Full History"+data.ToString());
        for (int i = 0; i < Annal_Box_List.Count; i++)
        {
            Annal_Box_List[i].RESET_ANNAL_BOX();
        }       

        Dragon_Percentage.text=data.GetField("summanry").GetField("dragon_per").ToString().Trim(Config.Inst.trim_char_arry)+"%";
        Tiger_Percentage.text=data.GetField("summanry").GetField("tiger_per").ToString().Trim(Config.Inst.trim_char_arry) + "%";

        Total_Dragon.text=data.GetField("result_sums")[0].GetField("dragon").ToString().Trim(Config.Inst.trim_char_arry);   
        Total_Tie.text=data.GetField("result_sums")[0].GetField("tie").ToString().Trim(Config.Inst.trim_char_arry);       
        Total_Tiger.text=data.GetField("result_sums")[0].GetField("tiger").ToString().Trim(Config.Inst.trim_char_arry);      
        Total_Count.text=data.GetField("result_sums")[0].GetField("total").ToString().Trim(Config.Inst.trim_char_arry);       

        float dragonPer = float.Parse(data.GetField("summanry").GetField("dragon_per").ToString());
        Percentage_Filler.fillAmount = dragonPer / 100;
        Debug.Log("naresh Percentage_Filler  " + Percentage_Filler.fillAmount);

        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_Fight();
        for (int i = data.GetField("figth").Count; i > 0 ; i--)
        {           
            DT_PFB_HIST_FIGHT cell = Instantiate(_PFB_FIGHT, DataParent) as DT_PFB_HIST_FIGHT;
            CellList.Add(cell.gameObject);
            cell.SET_DATA(data.GetField("figth")[i-1].ToString().Trim(Config.Inst.trim_char_arry));
        }
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = true;
        for (int i = 0; i < 20/*data.GetField("figth").Count*/; i++)
        {
            SET_HIST_CARD_DATA(i, data.GetField("figth")[i].ToString().Trim(Config.Inst.trim_char_arry));
        }
        for (int i = 0; i < data.GetField("annal").Count; i++)
        {
            Annal_Box_List[i].SET_ANNAL_Box(data.GetField("annal")[i]);
        }
    }

    public void SET_HIST_CARD_DATA(int index,string CardName)
    {
        string[] split_XCard = CardName.Split('|');

        if (split_XCard[0].Equals("dragon"))
            Uper_20_Card_Hist_List[index].sprite = DT_HistoryManager.Inst.D_Hist_Sprite;
        else if (split_XCard[0].Equals("tiger"))
            Uper_20_Card_Hist_List[index].sprite = DT_HistoryManager.Inst.T_Hist_Sprite;
        else
            Uper_20_Card_Hist_List[index].sprite = DT_HistoryManager.Inst.TIE_Hist_Sprite;
    }

    public void Open_Full_History()
    {
        GS.Inst.iTwin_Open(this.gameObject);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.DRAGON_TIGER_RESULT_HISTORY());
    }
    public void Close_Full_History()
    {
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }

    public void Clear_OLD_Fight()
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
