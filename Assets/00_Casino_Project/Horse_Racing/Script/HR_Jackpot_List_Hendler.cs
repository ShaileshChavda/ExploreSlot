using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HR_Jackpot_List_Hendler : MonoBehaviour
{
    public static HR_Jackpot_List_Hendler Inst;
    [SerializeField] HR_PFB_Jackpot_List pfb_Jackpot_list;
    public RectTransform DataParent;
    public List<GameObject> CellList;
    public TextMeshProUGUI Txt_Total_jackpotAmount;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_JACKPOT_DATA(JSONObject data)
    {
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_PFB();
        for (int i = 0; i < data.GetField("lists").Count; i++)
        {
            HR_PFB_Jackpot_List cell = Instantiate(pfb_Jackpot_list, DataParent) as HR_PFB_Jackpot_List;
            CellList.Add(cell.gameObject);
            cell.SET_LIST_DATA(data.GetField("lists")[i]);
        }
       DataParent.anchoredPosition = new Vector2(0, 0);
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

    public void Open_jackpot()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(this.gameObject);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.HORSE_RACING_JACKPORT_USER_LISTS());
    }

    public void Close_jackpot()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
}
