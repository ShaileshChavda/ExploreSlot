using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Record : MonoBehaviour
{
    public static Shop_Record Inst;
    public List<Sprite> BG_List;
    [SerializeField] Image IMG_BG;
    public PFB_Record _PFB_RECORD;
    public RectTransform DataParent;
    public List<GameObject> CellList;
    [SerializeField] Text TxtPageNo;
    int TotalPage;
    int currentPage;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_RECORD_DATA(JSONObject data)
    {
        TotalPage=int.Parse(data.GetField("total_page").ToString().Trim(Config.Inst.trim_char_arry));
        currentPage = int.Parse(data.GetField("page").ToString().Trim(Config.Inst.trim_char_arry));
        string filter = data.GetField("filter").ToString().Trim(Config.Inst.trim_char_arry);
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_SHOP();
        for (int i = 0; i < data.GetField("record_lists").Count; i++)
        {
            PFB_Record cell = Instantiate(_PFB_RECORD, DataParent) as PFB_Record;
            CellList.Add(cell.gameObject);
            cell.SET_RECORD_DATA(data.GetField("record_lists")[i],i+1, filter);
        }
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }
    public void Clear_OLD_SHOP()
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
    public void OPEN_RECORD()
    {
        BTN_Payment();
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void OPEN_WITHDRAW_RECORD()
    {
        BTN_Withdraw();
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void CLOSE_RECORD()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
    public void BTN_Payment()
    {
        SoundManager.Inst.PlaySFX(0);
        IMG_BG.sprite = BG_List[0];
        SocketHandler.Inst.SendData(SocketEventManager.Inst.RECORDS("deposit", 1));
    }
    public void BTN_Withdraw()
    {
        SoundManager.Inst.PlaySFX(0);
        IMG_BG.sprite = BG_List[1];
        SocketHandler.Inst.SendData(SocketEventManager.Inst.RECORDS("withdraw", 1));
    }
    public void BTN_Other()
    {
        SoundManager.Inst.PlaySFX(0);
        IMG_BG.sprite = BG_List[2];
        SocketHandler.Inst.SendData(SocketEventManager.Inst.RECORDS("other", 1));
    }

    public void BTN_PreviouseAndNext(string p_n)
    {
        SoundManager.Inst.PlaySFX(0);
        if (p_n.Equals("n"))
        {
            if (currentPage < TotalPage)
            {
                currentPage++;
                TxtPageNo.text = currentPage.ToString();
                SocketHandler.Inst.SendData(SocketEventManager.Inst.RECORDS(IMG_BG.sprite.name, currentPage));
            }
        }
        else
        {
            if (currentPage > 1)
            {
                currentPage--;
                TxtPageNo.text = currentPage.ToString();
                SocketHandler.Inst.SendData(SocketEventManager.Inst.RECORDS(IMG_BG.sprite.name, currentPage));
            }
        }
    }

}
