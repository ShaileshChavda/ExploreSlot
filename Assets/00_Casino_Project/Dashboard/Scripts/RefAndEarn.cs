using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RefAndEarn : MonoBehaviour
{
    public static RefAndEarn Inst;
    [SerializeField] Transform Button_Selection;
    [SerializeField] List<Transform> _Button_List;

    [Header("ALL SCREEN")]
    [SerializeField] GameObject Rules_SC;
    [SerializeField] GameObject Refaral_SC;
    [SerializeField] GameObject Rank_SC;
    [SerializeField] GameObject MyBonus_SC;
    [SerializeField] GameObject BonusRecord_SC;
    [SerializeField] GameObject WeeklyBonu_SC;
    public PFB_REF_RANK _PFB_RANK;
    public RectTransform DataParent;
    public List<GameObject> CellList;

    [Header("====== REFERALS =======")]
    [SerializeField] Text TxtReferal_Bonus;
    [SerializeField] Text TxtYesterday_Bonus;
    [SerializeField]public Text TxtCurrent_Bonus;
    [SerializeField] IMGLoader_Banner Rules_IMG;
    public PFB_Referrals _PFB_RAFFREALS;
    public RectTransform DataParent2;
    [SerializeField] Dropdown DropDown_BonusMonth;
    public Button _Button_Claim_REF;

    [Header("====== My Bonus =======")]
    public PFB_MyBonusTime _PFB_MyBonusTime;
    public RectTransform DataParent3;

    [Header("====== Bonus Record =======")]
    public PFB_Bonus_Record _PFB_BonusRecord;
    public RectTransform DataParent4;

    [Header("====== Weekly Extra Bonus Record =======")]
    public PFB_Weekly_Extrabonus _PFB_WEEKLY_EXTRABONUS;
    public RectTransform DataParent5;
    public TextMeshProUGUI Txt_LastweekBonus;
    public TextMeshProUGUI Txt_ExtraBonusWeek;
    public Button _Button_Claim;

    [Header("REF_AND_EARN LNG")]
    [SerializeField] Text TxtHeader;
    [SerializeField] Text Txt_Rules_L;
    [SerializeField] Text Txt_REF_L;
    [SerializeField] Text Txt_RANK_L;
    [SerializeField] Text Txt_MYBONUS_L;
    [SerializeField] Text Txt_WTS_BTN, Txt_FB_BTN, Txt_Share_BTN;

    [Header("REF_AND_EARN LNG Rules")]
    [SerializeField] Text TxtH_Referals;
    [SerializeField] Text TxtH_YestardayBonus;
    [SerializeField] Text TxtH_Current_Bonus;

    [Header("REF_AND_EARN LNG Referals")]
    [SerializeField] Text TxtH_ID;
    [SerializeField] Text TxtH_Name;
    [SerializeField] Text TxtH_TodayBonus;
    [SerializeField] Text Txt_TotalBonus;

    [Header("REF_AND_EARN LNG Rank")]
    [SerializeField] Text TxtH_Rank;
    [SerializeField] Text TxtH_VIP_LEVEL;
    [SerializeField] Text TxtH_Prize;

    [Header("REF_AND_EARN LNG MyBonus")]
    [SerializeField] Text TxtH_Time;

    [Header("REF_AND_EARN LNG Bonus Record")]
    [SerializeField] Text TxtH_WeekTime;
    [SerializeField] Text TxtH_CollectionTime;
    [SerializeField] Text TxtH_BonusTime;

    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void OPEN_RefAndEarn_SC()
    {
        SoundManager.Inst.PlaySFX(0);
        _Button_Action(0);
        LNG_SETUP();
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void CLOSE_RefAndEarn_SC()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
    public void CLAIM_BONUS()
    {
        if (TxtCurrent_Bonus.text != "" && TxtCurrent_Bonus.text != "0")
        {
            SoundManager.Inst.PlaySFX(0);
            SocketHandler.Inst.SendData(SocketEventManager.Inst.REFER_BONUS_CLAIM());
        }
    }
    public void _Button_Action(int ButtonNo)
    {
        SoundManager.Inst.PlaySFX(0);
        Close_Screen();
        switch (ButtonNo)
        {
            case 0:
                Button_Selection.localPosition = _Button_List[0].localPosition;
                Rules_SC.transform.localScale = Vector3.one;
                SocketHandler.Inst.SendData(SocketEventManager.Inst.REFER_RULE_INFO());
                break;
            case 1:
                Button_Selection.localPosition = _Button_List[1].localPosition;
                Refaral_SC.transform.localScale = Vector3.one;
                SocketHandler.Inst.SendData(SocketEventManager.Inst.REFER_REFERREL_LISTS(1));
                break;
            case 2:
                Button_Selection.localPosition = _Button_List[2].localPosition;
                Rank_SC.transform.localScale = Vector3.one;
                SocketHandler.Inst.SendData(SocketEventManager.Inst.RANK_LISTS());
                break;
            case 3:
                Button_Selection.localPosition = _Button_List[3].localPosition;
                MyBonus_SC.transform.localScale = Vector3.one;

                if (DropDown_BonusMonth.value == 0)
                    SocketHandler.Inst.SendData(SocketEventManager.Inst.GET_BONUS_ERN_LIST("last_7_days", 1));
                else
                    SocketHandler.Inst.SendData(SocketEventManager.Inst.GET_BONUS_ERN_LIST("last_month", 1));
                break;
            case 4:
                Button_Selection.localPosition = _Button_List[4].localPosition;
                BonusRecord_SC.transform.localScale = Vector3.one;
                SocketHandler.Inst.SendData(SocketEventManager.Inst.GET_BONUS_RECORD(1));
                break;
            case 5:
                Button_Selection.localPosition = _Button_List[5].localPosition;
                WeeklyBonu_SC.transform.localScale = Vector3.one;
                SocketHandler.Inst.SendData(SocketEventManager.Inst.WEEKLY_EXTRA_BONUS_INFO());
                break;
        }
    }
    public void BTN_EXTRA_WEEKLY_CLAIM()
    {
        SocketHandler.Inst.SendData(SocketEventManager.Inst.CLAIM_WEEKLY_EXTRA_BONUS());
    }
    public void OnDropdownChanged(Dropdown dropdown)
    {
        if(dropdown.value==0)
            SocketHandler.Inst.SendData(SocketEventManager.Inst.GET_BONUS_ERN_LIST("last_7_days", 1));
        else
            SocketHandler.Inst.SendData(SocketEventManager.Inst.GET_BONUS_ERN_LIST("last_month", 1));
    }

    void Close_Screen()
    {
        Rules_SC.transform.localScale=Vector3.zero;
        Refaral_SC.transform.localScale = Vector3.zero;
        Rank_SC.transform.localScale = Vector3.zero;
        MyBonus_SC.transform.localScale = Vector3.zero;
        BonusRecord_SC.transform.localScale = Vector3.zero;
        WeeklyBonu_SC.transform.localScale = Vector3.zero;
    }
    public void SET_REFER_RULES_DATA(JSONObject data)
    {
        TxtReferal_Bonus.text = data.GetField("user_counter").ToString().Trim(Config.Inst.trim_char_arry);
        TxtYesterday_Bonus.text = data.GetField("yesterday_bonus").ToString().Trim(Config.Inst.trim_char_arry);
        TxtCurrent_Bonus.text = data.GetField("bonus").ToString().Trim(Config.Inst.trim_char_arry);
        string url = data.GetField("rule_url").ToString().Trim(Config.Inst.trim_char_arry);
        int claim = int.Parse(data.GetField("bonus").ToString().Trim(Config.Inst.trim_char_arry));
        if (claim<=0)
            _Button_Claim.interactable = false;
        else
            _Button_Claim.interactable = true;

        if (url != "")
            Rules_IMG.LoadIMG(url, false, false);

    }
    public void SET_REFERALS_DATA(JSONObject data)
    {
            DataParent2.parent.parent.GetComponent<ScrollRect>().enabled = false;
            Clear_OLD_RANK();
            for (int i = 0; i < data.GetField("referrel_info_lists").Count; i++)
            {
                PFB_Referrals cell = Instantiate(_PFB_RAFFREALS) as PFB_Referrals;
                CellList.Add(cell.gameObject);
                cell.transform.SetParent(DataParent2, false);
                cell.SET_DATA(data.GetField("referrel_info_lists")[i]);
            }
            DataParent2.anchoredPosition = new Vector2(DataParent2.GetComponent<RectTransform>().anchoredPosition.x, 0f);
            DataParent2.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }

    public void SET_RANK_LIST(JSONObject data)
    {
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_RANK();
        for (int i = 0; i < data.GetField("rank_lists").Count; i++)
        {
            PFB_REF_RANK cell = Instantiate(_PFB_RANK) as PFB_REF_RANK;
            CellList.Add(cell.gameObject);
            cell.transform.SetParent(DataParent, false);
            cell.SET_RANK_DATA(data.GetField("rank_lists")[i]);
        }
        DataParent.anchoredPosition = new Vector2(DataParent.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }

    public void SET_BONUS_EARN_LIST(JSONObject data)
    {
        DataParent3.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_RANK();
        for (int i = 0; i < data.GetField("referrel_info_lists").Count; i++)
        {
            PFB_MyBonusTime cell = Instantiate(_PFB_MyBonusTime) as PFB_MyBonusTime;
            CellList.Add(cell.gameObject);
            cell.transform.SetParent(DataParent3, false);
            cell.SET_DATA(data.GetField("referrel_info_lists")[i]);
        }
        DataParent3.anchoredPosition = new Vector2(DataParent3.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent3.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }

    public void SET_BONUS_RECORD_LIST(JSONObject data)
    {
        DataParent4.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_RANK();
        for (int i = 0; i < data.GetField("bonus_records").Count; i++)
        {
            PFB_Bonus_Record cell = Instantiate(_PFB_BonusRecord) as PFB_Bonus_Record;
            CellList.Add(cell.gameObject);
            cell.transform.SetParent(DataParent4, false);
            cell.SET_DATA(data.GetField("bonus_records")[i]);
        }
        DataParent4.anchoredPosition = new Vector2(DataParent4.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent4.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }

    public void SET_WEEKLY_EXTRABONUS_DATA(JSONObject data)
    {
        bool claim = bool.Parse(data.GetField("is_claim").ToString().Trim(Config.Inst.trim_char_arry));
        if (claim)
            _Button_Claim.interactable = true;
        else
            _Button_Claim.interactable = false;

        Txt_LastweekBonus.text= data.GetField("total_bonus").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_ExtraBonusWeek.text= data.GetField("bonus").ToString().Trim(Config.Inst.trim_char_arry);
        DataParent5.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_RANK();
        for (int i = 0; i < data.GetField("config").Count; i++)
        {
            PFB_Weekly_Extrabonus cell = Instantiate(_PFB_WEEKLY_EXTRABONUS) as PFB_Weekly_Extrabonus;
            CellList.Add(cell.gameObject);
            cell.transform.SetParent(DataParent5, false);
            cell.SET_DATA(data.GetField("config")[i]);
        }
        DataParent5.anchoredPosition = new Vector2(DataParent5.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent5.parent.parent.GetComponent<ScrollRect>().enabled = true;
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
    void LNG_SETUP()
    {
        switch (PlayerPrefs.GetInt("LNG"))
        {
            case 0:
                //english
                TxtHeader.text = "Refer & Win";
                Txt_Rules_L.text = "Rule";
                Txt_REF_L.text = "Referrals";
                Txt_RANK_L.text = "Rank";
                Txt_MYBONUS_L.text = "My Bonus";
                Txt_WTS_BTN.text = "WhatsApp";
                Txt_FB_BTN.text = "Facebook";
                Txt_Share_BTN.text = "Share";

                TxtH_ID.text = "ID";
                TxtH_Name.text = "Name";
                TxtH_TodayBonus.text = "Today's Bonus";
                Txt_TotalBonus.text = "Total Bonus";

                TxtH_Rank.text = "Rank";
                TxtH_VIP_LEVEL.text = "GameID";
                TxtH_Prize.text = "Prize";

                TxtH_Time.text = "Time";

                TxtH_WeekTime.text = "Week Time";
                TxtH_CollectionTime.text = "Collection time";
                TxtH_BonusTime.text = "Bonus";
                break;
            case 1:
                //Nepali
                TxtHeader.text = "सन्दर्भ र जीत";
                Txt_Rules_L.text = "नियम";
                Txt_REF_L.text = "रेफरलहरू";
                Txt_RANK_L.text = "श्रेणी";
                Txt_MYBONUS_L.text = "मेरो बोनस";
                Txt_WTS_BTN.text = "व्हाट्सएप";
                Txt_FB_BTN.text = "फेसबुक";
                Txt_Share_BTN.text = "लिङ्क प्रतिलिपि गर्नुहोस्";

                TxtH_ID.text = "आईडी";
                TxtH_Name.text = "नाम";
                TxtH_TodayBonus.text = "आजको बोनस";
                Txt_TotalBonus.text = "कुल बोनस";

                TxtH_Rank.text = "श्रेणी";
                TxtH_VIP_LEVEL.text = "गेमआईडी";
                TxtH_Prize.text = "पुरस्कार";

                TxtH_Time.text = "समय";

                TxtH_WeekTime.text = "हप्ताको समय";
                TxtH_CollectionTime.text = "सङ्कलन समय";
                TxtH_BonusTime.text = "बोनस";
                break;
            case 2:
                //urdu
                TxtHeader.text = "رجوع کریں اور جیتیں۔";
                Txt_Rules_L.text = "قاعدہ";
                Txt_REF_L.text = "حوالہ جات";
                Txt_RANK_L.text = "رینک";
                Txt_MYBONUS_L.text = "میرا بونس";
                Txt_WTS_BTN.text = "واٹس ایپ";
                Txt_FB_BTN.text = "فیس بک";
                Txt_Share_BTN.text = "لنک کاپی کریں۔";

                TxtH_ID.text = "آئی ڈی";
                TxtH_Name.text = "نام";
                TxtH_TodayBonus.text = "آج کا بونس";
                Txt_TotalBonus.text = "کل بونس";

                TxtH_Rank.text = "رینک";
                TxtH_VIP_LEVEL.text = "گیم آئی ڈی";
                TxtH_Prize.text = "انعام";

                TxtH_Time.text = "وقت";

                TxtH_WeekTime.text = "ہفتہ کا وقت";
                TxtH_CollectionTime.text = "جمع کرنے کا وقت";
                TxtH_BonusTime.text = "اضافی انعام";
                break;
            case 3:
                //bangali 
                TxtHeader.text = "রেফার করুন এবং জয় করুন";
                Txt_Rules_L.text = "নিয়ম";
                Txt_REF_L.text = "প্রচার";
                Txt_RANK_L.text = "পদমর্যাদা";
                Txt_MYBONUS_L.text = "আমার বোনাস";
                Txt_WTS_BTN.text = "হোয়াটসঅ্যাপ";
                Txt_FB_BTN.text = "ফেসবুক";
                Txt_Share_BTN.text = "লিংক কপি করুন";

                TxtH_ID.text = "আইডি";
                TxtH_Name.text = "নাম";
                TxtH_TodayBonus.text = "আজকের বোনাস";
                Txt_TotalBonus.text = "মোট বোনাস";

                TxtH_Rank.text = "পদমর্যাদা";
                TxtH_VIP_LEVEL.text = "খেলার সনাক্তকরণ নম্বর";
                TxtH_Prize.text = "পুরস্কার";

                TxtH_Time.text = "সময়";

                TxtH_WeekTime.text = "সপ্তাহের সময়";
                TxtH_CollectionTime.text = "সংগ্রহের সময়";
                TxtH_BonusTime.text = "বোনাস";
                break;
            case 4:
                //Marathi
                TxtHeader.text = "संदर्भ घ्या आिण जिंका";
                Txt_Rules_L.text = "नियम";
                Txt_REF_L.text = "संदर्भ";
                Txt_RANK_L.text = "रँक";
                Txt_MYBONUS_L.text = "माझा बोनस";
                Txt_WTS_BTN.text = "व्हॉट्सअ‍ॅप";
                Txt_FB_BTN.text = "फेसबुक";
                Txt_Share_BTN.text = "लिंक कॉपी करा";

                TxtH_ID.text = "आयडी";
                TxtH_Name.text = "नाव";
                TxtH_TodayBonus.text = "आजचा बोनस";
                Txt_TotalBonus.text = "एकूण बोनस";

                TxtH_Rank.text = "रँक";
                TxtH_VIP_LEVEL.text = "गेमआयडी";
                TxtH_Prize.text = "बक्षीस";

                TxtH_Time.text = "वेळ";

                TxtH_WeekTime.text = "आठवड्याची वेळ";
                TxtH_CollectionTime.text = "संकलन वेळ";
                TxtH_BonusTime.text = "बोनस";
                break;
            case 5:
                //telugu
                TxtHeader.text = "చూడండి & గెలవండి";
                Txt_Rules_L.text = "నియమం";
                Txt_REF_L.text = "రెఫరల్స్";
                Txt_RANK_L.text = "ర్యాంక్";
                Txt_MYBONUS_L.text = "నా బోనస్";
                Txt_WTS_BTN.text = "వాట్సాప్";
                Txt_FB_BTN.text = "ఫేస్బుక్";
                Txt_Share_BTN.text = "లింక్ను కాపీ చేయండి";

                TxtH_ID.text = "గుర్తింపు";
                TxtH_Name.text = "పేరు";
                TxtH_TodayBonus.text = "నేటి బోనస్";
                Txt_TotalBonus.text = "మొత్తం బోనస్";

                TxtH_Rank.text = "ర్యాంక్";
                TxtH_VIP_LEVEL.text = "గేమ్‌ID";
                TxtH_Prize.text = "బహుమతి";

                TxtH_Time.text = "సమయం";

                TxtH_WeekTime.text = "వారం సమయం";
                TxtH_CollectionTime.text = "సేకరణ సమయం";
                TxtH_BonusTime.text = "అదనపు";
                break;
            default:
                break;
        }
    }
}
