using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spinner_Records : MonoBehaviour
{
    public static Spinner_Records Inst;
    public List<Sprite> BG_List;
    [SerializeField] Image IMG_BG;
    public PFB_SPIN_BIG_WIN _PFB_BIG_RECORD;
    public PFB_LIVE_SPIN_RECORD _PFB_LIVE_SPIN_RECORD;
    public RectTransform DataParent, DataParent2;
    public List<GameObject> CellList, CellList2;
    [SerializeField] Scrollbar Scroll_Live_Record;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;

    }
    public void SET_RECORD_DATA(JSONObject data)
    {
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_RECORD();
        for (int i = 0; i < data.GetField("record_lists").Count; i++)
        {
            PFB_SPIN_BIG_WIN cell = Instantiate(_PFB_BIG_RECORD, DataParent) as PFB_SPIN_BIG_WIN;
            CellList.Add(cell.gameObject);
            cell.SET_RECORD_DATA(data.GetField("record_lists")[i]);
        }
        DataParent.anchoredPosition = new Vector2(DataParent.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }

    public void SET_CURRENT_WIN_RECORD_DATA(JSONObject data)
    {
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        PFB_LIVE_SPIN_RECORD cell = Instantiate(_PFB_LIVE_SPIN_RECORD, DataParent2) as PFB_LIVE_SPIN_RECORD;
        CellList2.Add(cell.gameObject);
        cell.SET_RECORD_DATA(data);
        DataParent.anchoredPosition = new Vector2(DataParent.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = true;
        Scroll_Live_Record.value = 0;
    }

    public void Clear_OLD_RECORD()
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

    public void Clear_OLD_LIVE_RECORD()
    {
        if (CellList2.Count > 0)
        {
            for (int i = 0; i < CellList2.Count; i++)
            {
                if (CellList2[i] != null)
                    Destroy(CellList2[i]);
            }
            CellList2.Clear();
        }
    }

    public void OPEN_BIG_RECORD()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SPINNER_BIG_WINS());
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void CLOSE_RECORD()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SPINNER_RECORD_CLOSE());
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
        //Clear_OLD_LIVE_RECORD();
    }
    public void BTN_BIG_RECORD()
    {
        SoundManager.Inst.PlaySFX(0);
        IMG_BG.sprite = BG_List[0];
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SPINNER_BIG_WINS());
    }
    public void BTN_RECORD()
    {
        SoundManager.Inst.PlaySFX(0);
        IMG_BG.sprite = BG_List[1];
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SPINNER_RECORDS());
    }
}
