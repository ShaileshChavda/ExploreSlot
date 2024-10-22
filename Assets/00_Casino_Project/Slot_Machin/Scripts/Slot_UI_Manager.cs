using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot_UI_Manager : MonoBehaviour
{
    public static Slot_UI_Manager Inst;
    [Header("UserDATA")]
    public IMGLoader User_Pic;
    public TextMeshProUGUI Txt_UserName;
    public TextMeshProUGUI Txt_LuckyPlayerName;
    public TextMeshProUGUI TxtChips;
    public TextMeshProUGUI TxtGameID;
    public Image Vip_Ring;
    public TextMeshProUGUI Txt_Jackpot;
    public Animator Win_Plus_Minus_Anim;
    public Text TxtPlusMinus;


    [Header("Footer")]
    public TextMeshProUGUI Txt_Line;
    public TextMeshProUGUI Txt_Bet;
    [SerializeField] TextMeshProUGUI Txt_Total_Bet;
    public TextMeshProUGUI Txt_Total_Won;
    [SerializeField] List<string> BetList;
    [SerializeField] GameObject[] Line_OBJ;
    int Line_PL = 9;
    int Bet_Index = 0;

    [Header("Item")]
    public List<Sprite> All_Item_Sprite;
    public List<Sprite> All_Item_Sprite_Glow;

    [Header("<<< Blur Images >>>")]
    public List<Sprite> List_Blur_Sprite;
    public List<Image> List_Item1_Images;
    public List<Image> List_Item2_Images;
    public List<Image> List_Item3_Images;
    public List<Image> List_Item4_Images;
    public List<Image> List_Item5_Images;


    [Header("<<< Glow Images >>>")]
    [SerializeField] List<Image> Line1_Glow;
    [SerializeField] List<Image> Line2_Glow;
    [SerializeField] List<Image> Line3_Glow;
    [SerializeField] List<Image> Line4_Glow;
    [SerializeField] List<Image> Line5_Glow;
    [SerializeField] List<Image> Line6_Glow;
    [SerializeField] List<Image> Line7_Glow;
    [SerializeField] List<Image> Line8_Glow;
    [SerializeField] List<Image> Line9_Glow;
    [SerializeField] List<List<Image>> Line_Glow_LIST;

    private void Awake()
    {
        Inst = this;
        SET_GLOW_ANIMATION_DATA();
    }
    // Start is called before the first frame update
    void Start()
    {
        Line(8);
        PreeLoader.Inst.Stop_Loader();
    }
    public void Update_JACKPOT(JSONObject data)
    {
        Txt_Jackpot.text = data.GetField("collected_jackpot_amount").ToString().Trim(Config.Inst.trim_char_arry);
    }

    public void Show_Win_Line(JSONObject data)
    {
        if (data.GetField("win_lines_infos").Count > 0)
        {
            for (int j = 0; j < Line_Glow_LIST.Count; j++)
            {
                for (int i = 0; i < Line_Glow_LIST[j].Count; i++)
                {
                    Line_Glow_LIST[j][i].color = new Color32(255, 255, 255, 120);
                }
            }

            for (int j = 0; j < data.GetField("win_lines_infos").Count; j++)
            {
                int line = int.Parse(data.GetField("win_lines_infos")[j].GetField("line").ToString().Trim(Config.Inst.trim_char_arry));
                int count = int.Parse(data.GetField("win_lines_infos")[j].GetField("count").ToString().Trim(Config.Inst.trim_char_arry));
                string item = data.GetField("win_lines_infos")[j].GetField("item").ToString().Trim(Config.Inst.trim_char_arry);
                for (int i = 0; i < Line_OBJ.Length; i++)
                {
                    if ((line - 1).Equals(i))
                        Line_OBJ[i].SetActive(true);
                }
                START_BOX_ANIM(line-1,count, item);
            }
        }
    }

    void START_BOX_ANIM(int line,int count,string item)
    {
        for (int j = 0; j < All_Item_Sprite_Glow.Count; j++)
        {
            if (item.Equals(All_Item_Sprite_Glow[j].name))
            {
                for (int i = 0; i < Find_Actual_Fruit_Glow_Index(count); i++)
                {
                    if (Line_Glow_LIST[line][i].gameObject.activeSelf)
                    {
                        Line_Glow_LIST[line][i].sprite = All_Item_Sprite_Glow[j];
                        Line_Glow_LIST[line][i].GetComponent<Animator>().Play("Admin_Scale", 0);
                        Line_Glow_LIST[line][i].color = new Color32(255, 255, 255, 255);
                    }
                }
            }
        }
    }

    int Find_Actual_Fruit_Glow_Index(int count)
    {
        int ct = count;
        switch (count)
        {
            case 1:
                ct = 4;
                break;
            case 2:
                ct = 8;
                break;
            case 3:
                ct = 12;
                break;
            case 4:
                ct = 16;
                break;
            case 5:
                ct = 20;
                break;
        }
        return ct;
    }

    public void SET_BET_LIST_DATA(JSONObject data)
    {
        BetList.Clear();
        for (int i = 0; i < data.Count; i++)
        {
            BetList.Insert(i,data[i].ToString().Trim(Config.Inst.trim_char_arry));
        }
    }

    public void SET_USER_DATA(JSONObject data)
    {
        Txt_UserName.text = data.GetField("user_info").GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        TxtChips.text = double.Parse(data.GetField("user_info").GetField("wallet").ToString().Trim(Config.Inst.trim_char_arry)).ToString("n2");
        User_Pic.LoadIMG(data.GetField("user_info").GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry),false,false);
    }

    //-------------Line Pluse minus Button -----------------
    public void Btn_Line_Plus_Minus(string pl)
    {
        if (Slot_Manager.Inst.CLICK_ACTION && Slot_FreeSpin.Inst.transform.localScale.x <= 0 && Slot_BigWin.Inst.transform.localScale.x <= 0 && Shop.Inst.transform.localScale.x <= 0 && Slot_LuckyPlayer.Inst.transform.localScale.x <= 0)
        {
            Slot_SoundManager.Inst.PlaySFX(0);
            if (pl.Equals("p"))
            {
                if (Line_PL < 9)
                {
                    Line_PL++;
                    Line(Line_PL);
                    Txt_Line.text = Line_PL.ToString();
                }
            }
            else
            {
                if (Line_PL > 1)
                {
                    Line_PL--;
                    Line(Line_PL);
                    Txt_Line.text = Line_PL.ToString();
                }
            }
            Calculate_Total_Bet();
        }
    }
    //-------------Line Pluse minus Button -----------------


    //-------------Bet Pluse minus Button -----------------
    public void Btn_Bet_Plus_Minus(string pl)
    {
        if (Slot_Manager.Inst.CLICK_ACTION && Slot_FreeSpin.Inst.transform.localScale.x <= 0 && Slot_BigWin.Inst.transform.localScale.x <= 0 && Shop.Inst.transform.localScale.x <= 0 && Slot_LuckyPlayer.Inst.transform.localScale.x <= 0)
        {
            Slot_SoundManager.Inst.PlaySFX(0);
            if (pl.Equals("p"))
            {
                if (Bet_Index < BetList.Count - 1)
                {
                    Bet_Index++;
                    Txt_Bet.text = BetList[Bet_Index];
                }
            }
            else
            {
                if (Bet_Index >= 1)
                {
                    Bet_Index--;
                    Txt_Bet.text = BetList[Bet_Index];
                }
            }
            Calculate_Total_Bet();
        }
    }
    //-------------Bet Pluse minus Button -----------------

    //------------- Line Action ---------------------------
    void Line(int lineNO)
    {
        for (int i = 0; i < Line_OBJ.Length; i++)
        {
            if(i<=lineNO-1)
                Line_OBJ[i].SetActive(true);
            else
                Line_OBJ[i].SetActive(false);
        }
    }
    public void Hide_All_Lines()
    {
        for (int i = 0; i < Line_OBJ.Length; i++)
        {
           Line_OBJ[i].SetActive(false);
        }
        for (int j = 0; j < Line_Glow_LIST.Count; j++)
        {
            for (int i = 0; i < Line_Glow_LIST[j].Count; i++)
            {
                Line_Glow_LIST[j][i].color = new Color32(255, 255, 255, 255);
            }
        }
    }
    //------------- Line Action ---------------------------

   //-------- Calculate total bet -------------------
    void Calculate_Total_Bet()
    {
        Txt_Total_Bet.text = (float.Parse(BetList[Bet_Index]) * Line_PL).ToString();
    }
    //-------- Calculate total bet -------------------

    public void BTN_MAX()
    {
        if (Slot_Manager.Inst.CLICK_ACTION && Slot_FreeSpin.Inst.transform.localScale.x <= 0 && Slot_BigWin.Inst.transform.localScale.x <= 0 && Shop.Inst.transform.localScale.x <= 0 && Slot_LuckyPlayer.Inst.transform.localScale.x <= 0)
        {
            Slot_SoundManager.Inst.PlaySFX(0);
            Line_PL = 9;
            Txt_Line.text = Line_PL.ToString();
            Bet_Index = 7;
            Txt_Bet.text = BetList[Bet_Index];
            Calculate_Total_Bet();
        }
    }

    public void Change_Blur_Image()
    {
        for (int i = 0; i < List_Item1_Images.Count; i++)
        {
            if(List_Item1_Images[i].gameObject.activeSelf)
                List_Item1_Images[i].sprite = List_Blur_Sprite[i];
            if (List_Item2_Images[i].gameObject.activeSelf)
                List_Item2_Images[i].sprite = List_Blur_Sprite[i];
            if (List_Item3_Images[i].gameObject.activeSelf)
                List_Item3_Images[i].sprite = List_Blur_Sprite[i];
            if (List_Item4_Images[i].gameObject.activeSelf)
                List_Item4_Images[i].sprite = List_Blur_Sprite[i];
            if (List_Item5_Images[i].gameObject.activeSelf)
                List_Item5_Images[i].sprite = List_Blur_Sprite[i];
        }
    }

    void SET_GLOW_ANIMATION_DATA()
    {
        Line_Glow_LIST = new List<List<Image>>();
        Line_Glow_LIST.Insert(0,Line1_Glow);
        Line_Glow_LIST.Insert(1,Line2_Glow);
        Line_Glow_LIST.Insert(2,Line3_Glow);
        Line_Glow_LIST.Insert(3,Line4_Glow);
        Line_Glow_LIST.Insert(4,Line5_Glow);
        Line_Glow_LIST.Insert(5,Line6_Glow);
        Line_Glow_LIST.Insert(6,Line7_Glow);
        Line_Glow_LIST.Insert(7,Line8_Glow);
        Line_Glow_LIST.Insert(8,Line9_Glow);
    }   
}
