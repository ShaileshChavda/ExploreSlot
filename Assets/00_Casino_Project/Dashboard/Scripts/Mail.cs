using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mail : MonoBehaviour
{
    public static Mail Inst;
    public PFB_MAIL _Pfb_Mail;
    public GameObject GreenSelection;
    public RectTransform DataParent;
    public List<GameObject> CellList;
    [SerializeField] GameObject IMG_SYSTEM, IMG_PERSONAL, IMG_CHANNEL;

    [Header("MAIL POPUP")]
    [SerializeField] GameObject OBJ_Mail_Popup;
    [SerializeField] Text Txt_Popup_Header;
    [SerializeField] TextMeshProUGUI Txt_Popup_FullMSG;
    [SerializeField] Text Txt_Popup_Bonus;
    [SerializeField] GameObject BTN_Mail_POPUP_CLAIM;
    string Popup_Claim_ID;

    [Header("MAIL LNG")]
    [SerializeField] Text TxtHeader;
    [SerializeField] Text Txt_Left_SystemMail;
    [SerializeField] Text Txt_Persional_Mail;
    [SerializeField] Text Txt_Chennal_Mail;

    public GameObject Mail_DOT;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.NEW_MAILS());
    }
    public void OPEN_MAIL()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(this.gameObject);
        LNG_SETUP();
        GreenSelection.transform.localPosition = IMG_SYSTEM.transform.localPosition;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.MAIL_LISTS("system"));
    }
    public void CLOSE_MAIL()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.NEW_MAILS());
    }

    public void OPEN_SYSTEM_MAIL()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.MAIL_LISTS("system"));
        GreenSelection.transform.position = IMG_SYSTEM.transform.position;
    }
    public void OPEN_PERSIONAL_MAIL()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.MAIL_LISTS("personal"));
        GreenSelection.transform.position = IMG_PERSONAL.transform.position;
    }

    public void OPEN_CHANNEL_MAIL()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.MAIL_LISTS("Channel"));
        GreenSelection.transform.position = IMG_CHANNEL.transform.position;
    }

    public void SET_MAIL_LIST(JSONObject data)
    {
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_RANK();
        for (int i = 0; i < data.GetField("mail_lists").Count; i++)
        {
            PFB_MAIL cell = Instantiate(_Pfb_Mail) as PFB_MAIL;
            CellList.Add(cell.gameObject);
            cell.transform.SetParent(DataParent, false);
            cell.SET_DATA(data.GetField("mail_lists")[i]);
        }
        DataParent.anchoredPosition = new Vector2(DataParent.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }

    public void Clear_OLD_RANK()
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

    public void SET_POPUP_DATA(JSONObject data)
    {
        Txt_Popup_Header.text= data.GetField("title").ToString().Trim(Config.Inst.trim_char_arry);
        string funmsg = data.GetField("description").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Popup_FullMSG.text= funmsg.Replace("|","\n");
        bool isBonus = bool.Parse(data.GetField("is_bonus").ToString().Trim(Config.Inst.trim_char_arry));
        bool isClaim = bool.Parse(data.GetField("is_claim").ToString().Trim(Config.Inst.trim_char_arry));

        if (isBonus && !isClaim)
        {
            BTN_Mail_POPUP_CLAIM.SetActive(true);
            Txt_Popup_Bonus.text = data.GetField("bonus").ToString().Trim(Config.Inst.trim_char_arry);
        }
        else
        {
            Txt_Popup_Bonus.transform.parent.transform.localScale = Vector3.zero;
            Txt_Popup_Bonus.text = "0";
            BTN_Mail_POPUP_CLAIM.SetActive(false);
        }
    }

    public void MAIL_DOT_ACTION(bool action)
    {
        if (action)
            Mail_DOT.SetActive(true);
        else
            Mail_DOT.SetActive(false);
    }

    public void OPEN_MAIL_POPUP(string id)
    {
        SoundManager.Inst.PlaySFX(0);
        Popup_Claim_ID = id;
        GS.Inst.iTwin_Open(OBJ_Mail_Popup);
    }
    public void CLOSE_MAIL_POPUP()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(OBJ_Mail_Popup, 0.3f);
    }

    public void CLAIM_POPUP_MAIL()
    {
        SoundManager.Inst.PlaySFX(0);
        CLOSE_MAIL_POPUP();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.MAIL_BONUS_CLAIM(Popup_Claim_ID));
    }

    void LNG_SETUP()
    {
        switch (PlayerPrefs.GetInt("LNG"))
        {
            case 0:
                //english
                TxtHeader.text = "Mail";
                Txt_Left_SystemMail.text = "System mail";
                Txt_Persional_Mail.text = "Personal mail";
                Txt_Chennal_Mail.text = "Channel mail";
                break;
            case 1:
                //Nepali
                TxtHeader.text = "मेल";
                Txt_Left_SystemMail.text = "प्रणाली मेल";
                Txt_Persional_Mail.text = "व्यक्तिगत मेल";
                Txt_Chennal_Mail.text = "च्यानल मेल";
                break;
            case 2:
                //urdu
                TxtHeader.text = "میل";
                Txt_Left_SystemMail.text = "سسٹم میل";
                Txt_Persional_Mail.text = "ذاتی میل";
                Txt_Chennal_Mail.text = "چینل میل";
                break;
            case 3:
                //bangali
                TxtHeader.text = "মেইল";
                Txt_Left_SystemMail.text = "সিস্টেম মেল";
                Txt_Persional_Mail.text = "ব্যক্তিগত মেইল";
                Txt_Chennal_Mail.text = "চ্যানেল মেইল";
                break;
            case 4:
                //Marathi
                TxtHeader.text = "मेल";
                Txt_Left_SystemMail.text = "सिस्टम मेल";
                Txt_Persional_Mail.text = "वैयक्तिक मेल";
                Txt_Chennal_Mail.text = "चॅनल मेल";
                break;
            case 5:
                //telugu
                TxtHeader.text = "మెయిల్";
                Txt_Left_SystemMail.text = "సిస్టమ్ మెయిల్";
                Txt_Persional_Mail.text = "వ్యక్తిగత మెయిల్";
                Txt_Chennal_Mail.text = "ఛానెల్ మెయిల్";
                break;
            default:
                break;
        }
    }
}
