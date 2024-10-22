using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HR_Trend : MonoBehaviour
{
    public static HR_Trend Inst;
    [SerializeField] HR_PFB_Trend pfb_trend;
    public RectTransform DataParent;
    public List<GameObject> CellList;
    public List<TextMeshProUGUI> List_Horse_Percentage;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_TREND_DATA(JSONObject data)
    {
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_PFB();
        List_Horse_Percentage[0].text = data.GetField("fr_h").ToString().Trim(Config.Inst.trim_char_arry)+"%";
        List_Horse_Percentage[1].text = data.GetField("se_h").ToString().Trim(Config.Inst.trim_char_arry)+"%";
        List_Horse_Percentage[2].text = data.GetField("th_h").ToString().Trim(Config.Inst.trim_char_arry)+"%";
        List_Horse_Percentage[3].text = data.GetField("fo_h").ToString().Trim(Config.Inst.trim_char_arry)+"%";
        List_Horse_Percentage[4].text = data.GetField("fi_h").ToString().Trim(Config.Inst.trim_char_arry)+"%";
        List_Horse_Percentage[5].text = data.GetField("si_h").ToString().Trim(Config.Inst.trim_char_arry)+"%";
        for (int i = 0; i < data.GetField("results").Count; i++)
        {
            HR_PFB_Trend cell = Instantiate(pfb_trend, DataParent) as HR_PFB_Trend;
            CellList.Add(cell.gameObject);
            cell.SET_TREND_RESULT(data.GetField("results")[i]);
        }
        DataParent.anchoredPosition = new Vector2(0, DataParent.GetComponent<RectTransform>().anchoredPosition.y);
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }
    public void Clear_OLD_PFB()
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

    public void Open_Trend()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(this.gameObject);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.HORSE_RACING_TREND_REPORTS());
    }

    public void Close_Trend()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
}
