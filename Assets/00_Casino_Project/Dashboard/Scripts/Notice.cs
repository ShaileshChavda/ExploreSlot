using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{
    public static Notice Inst;
    public List<Sprite> Head_BG_List;
    [SerializeField] Image Head_IMG_BG;
    [SerializeField] Transform Button_Selection, Button_Selection2;
    [SerializeField] GameObject Hots_Box,Notice_Box;
    [SerializeField] List<Transform> Hots_Button_List, Notice_Button_List;
    [SerializeField] List<Sprite> Host_IMG_SP_List;
    [SerializeField] List<IMGLoader_Banner> Box_IMG_HOT;
    [SerializeField] List<IMGLoader_Banner> Box_IMG_NOTICE;
    [SerializeField] List<GameObject> OBJ_IMG_NOTICE_HOT;
    bool Notice_load, Hot_Load;
    public RectTransform VIP_EVENT_RectScroll, VIP_LEVEL_RectScroll;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_HOT_AND_NOTICE_DATA(JSONObject data)
    {
        string h = data.GetField("header").ToString().Trim(Config.Inst.trim_char_arry);
        if (h.Equals("hot"))
        {
            if (!Hot_Load)
            {
                Box_IMG_HOT[0].LoadIMG(data.GetField("panel").GetField("Official channel").ToString().Trim(Config.Inst.trim_char_arry), false, true);
                Box_IMG_HOT[1].LoadIMG(data.GetField("panel").GetField("VIP event bonus").ToString().Trim(Config.Inst.trim_char_arry), false, true);
                Box_IMG_HOT[2].LoadIMG(data.GetField("panel").GetField("National agency").ToString().Trim(Config.Inst.trim_char_arry), false, true);
                Box_IMG_HOT[3].LoadIMG(data.GetField("panel").GetField("First deposit Bonus").ToString().Trim(Config.Inst.trim_char_arry), false, true);
                Box_IMG_HOT[4].LoadIMG(data.GetField("panel").GetField("UTR (12 Digits)").ToString().Trim(Config.Inst.trim_char_arry), false, true);
                Box_IMG_HOT[5].LoadIMG(data.GetField("panel").GetField("VIP level activites").ToString().Trim(Config.Inst.trim_char_arry), false, true);
                Hot_Load = true;
            }
        }
        else
        {
            if (!Notice_load)
            {
                Box_IMG_NOTICE[0].LoadIMG(data.GetField("panel").GetField("Agency Guide").ToString().Trim(Config.Inst.trim_char_arry), false, true);
                Box_IMG_NOTICE[1].LoadIMG(data.GetField("panel").GetField("IOS Download").ToString().Trim(Config.Inst.trim_char_arry), false, true);
                Box_IMG_NOTICE[2].LoadIMG(data.GetField("panel").GetField("Q&A need to know").ToString().Trim(Config.Inst.trim_char_arry), false, true);
                Box_IMG_NOTICE[3].LoadIMG(data.GetField("panel").GetField("Hindi Q&A").ToString().Trim(Config.Inst.trim_char_arry), false, true);
                Notice_load = true;
            }
        }
    }

    public void OPEN_NOTICE_SC()
    {
        SoundManager.Inst.PlaySFX(0);
        Head_IMG_BG.sprite = Head_BG_List[0];
        OPEN_SC_BOX(0);
        Hots_Box.SetActive(true);
        Notice_Box.SetActive(false);
        Hots_Button_Action(0);
        GS.Inst.iTwin_Open(this.gameObject);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.NOTICE_LISTS("","hot"));
    }

    public void OPEN_NOTICE_SC_START()
    {
        Head_IMG_BG.sprite = Head_BG_List[0];
        OPEN_SC_BOX(3);
        Hots_Box.SetActive(true);
        Notice_Box.SetActive(false);
        Hots_Button_Action(3);
        GS.Inst.iTwin_Open(this.gameObject);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.NOTICE_LISTS("", "hot"));
    }

    public void CLOSE_NOTICE_SC()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }

    public void OPEN_Hots()
    {
        SoundManager.Inst.PlaySFX(0);
        Head_IMG_BG.sprite = Head_BG_List[0];
        OPEN_SC_BOX(0);
        Hots_Box.SetActive(true);
        Notice_Box.SetActive(false);
        Hots_Button_Action(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.NOTICE_LISTS("", "hot"));
    }

    public void OPEN_Nitice()
    {
        SoundManager.Inst.PlaySFX(0);
        Head_IMG_BG.sprite = Head_BG_List[1];
        OPEN_SC_BOX(6);
        Notice_Box.SetActive(true);
        Hots_Box.SetActive(false);
        Notice_Button_Action(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.NOTICE_LISTS("","notice"));
    }

    public void Hots_Button_Action(int ButtonNo)
    {
        SoundManager.Inst.PlaySFX(0);
        VIP_EVENT_RectScroll.parent.parent.GetComponent<ScrollRect>().enabled = false;
        VIP_LEVEL_RectScroll.parent.parent.GetComponent<ScrollRect>().enabled = false;
        switch (ButtonNo)
        {
            case 0:
                Button_Selection.localPosition=Hots_Button_List[0].localPosition;
                OPEN_SC_BOX(0);
                break;
            case 1:
                Button_Selection.localPosition = Hots_Button_List[1].localPosition;
                OPEN_SC_BOX(1);
                break;
            case 2:
                Button_Selection.localPosition = Hots_Button_List[2].localPosition;
                OPEN_SC_BOX(2);
                break;
            case 3:
                Button_Selection.localPosition = Hots_Button_List[3].localPosition;
                OPEN_SC_BOX(3);
                break;
            case 4:
                Button_Selection.localPosition = Hots_Button_List[4].localPosition;
                OPEN_SC_BOX(4);
                break;
            case 5:
                Button_Selection.localPosition = Hots_Button_List[5].localPosition;
                OPEN_SC_BOX(5);
                break;
        }
    }
    public void Notice_Button_Action(int ButtonNo)
    {
        SoundManager.Inst.PlaySFX(0);
        switch (ButtonNo)
        {
            case 0:
                Button_Selection2.localPosition = Notice_Button_List[0].localPosition;
                OPEN_SC_BOX(6);
                break;
            case 1:
                Button_Selection2.localPosition = Notice_Button_List[1].localPosition;
                OPEN_SC_BOX(7);
                break;
            case 2:
                Button_Selection2.localPosition = Notice_Button_List[2].localPosition;
                OPEN_SC_BOX(8);
                break;
            case 3:
                Button_Selection2.localPosition = Notice_Button_List[3].localPosition;
                OPEN_SC_BOX(9);
                break;
        }
    }

    public void OPEN_SC_BOX(int index)
    {
        for (int i = 0; i < OBJ_IMG_NOTICE_HOT.Count; i++)
        {
            if (index.Equals(i))
                OBJ_IMG_NOTICE_HOT[i].transform.localScale = Vector3.one;
            else
                OBJ_IMG_NOTICE_HOT[i].transform.localScale = Vector3.zero;
        }

        VIP_EVENT_RectScroll.anchoredPosition = new Vector2(VIP_EVENT_RectScroll.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        VIP_EVENT_RectScroll.parent.parent.GetComponent<ScrollRect>().enabled = true;
        VIP_LEVEL_RectScroll.anchoredPosition = new Vector2(VIP_EVENT_RectScroll.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        VIP_LEVEL_RectScroll.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }
}
