using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot_LuckyPlayer : MonoBehaviour
{
    public static Slot_LuckyPlayer Inst;
    [SerializeField] Image Img_New_History;
    [SerializeField] Sprite SP_New, SP_History;
    [SerializeField] TextMeshProUGUI lbl_New, lbl_History;
    public Slot_PFB_BigWin_List _PFB_BigWin_List;
    public RectTransform DataParent;
    public List<GameObject> CellList;
    public List<Sprite> Crown_SP_List;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_LIST_DATA(JSONObject data)
    {
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_DATA();
      
        for (int i = 0; i < data.GetField("user_list").Count; i++)
        {
            Slot_PFB_BigWin_List cell = Instantiate(_PFB_BigWin_List, DataParent) as Slot_PFB_BigWin_List;
            CellList.Add(cell.gameObject);
            cell.SET_DATA(data.GetField("user_list")[i],i);
        }
        DataParent.anchoredPosition = new Vector2(DataParent.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }
    public void Clear_OLD_DATA()
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
    public void BTN_New_Player()
    {
        Img_New_History.sprite = SP_New;
        lbl_New.color = Color.white;
        lbl_History.color = Color.yellow;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SLOT_NEW_LUCKY_USERS());
    }
    public void BTN_History_Player()
    {
        Img_New_History.sprite = SP_History;
        lbl_New.color = Color.yellow;
        lbl_History.color = Color.white;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SLOT_LUCKY_USERS_HISTORY());
    }
    public void OPEN_History()
    {
        SoundManager.Inst.PlaySFX(0);
        BTN_New_Player();
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void CLOSE_History()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
}
