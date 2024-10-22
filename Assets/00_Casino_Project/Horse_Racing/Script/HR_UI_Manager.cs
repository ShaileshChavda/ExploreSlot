using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HR_UI_Manager : MonoBehaviour
{
    public static HR_UI_Manager Inst;
    public Text TxtGameID;
    public TextMeshProUGUI Txt_Jackpot;
    public float Total_JackPot;
    [Header("BetBox")]
    public List<TextMeshProUGUI> List_Txt_User_Total_Bet;
    public List<TextMeshProUGUI> List_Txt_Other_User_Total_Bet;
    public List<TextMeshProUGUI> List_Txt_Total_X;
    public List<TextMeshProUGUI> List_Txt_X_Ground;
    public List<GameObject> List_Win_Highlight_Anim;

    public List<Sprite> Horse_with_No_Box_List;

    public GameObject Jackpot_SC,Horse_X_Box_Ground, CollectJackpot_Mini_Box;
    [SerializeField] TextMeshProUGUI Txt_Jackpot_Win_Amount,Txt_CollectJackpot_Mini;
    [SerializeField] TextMeshProUGUI Txt_WinX_Bet_Box;
    public GameObject Pfb_jackpot_coin;

    [Header("Jackpot Winner")]
    public TextMeshProUGUI Txt_Jackpot_WinnerName;
    public TextMeshProUGUI Txt_Jackpot_WinnerChips;
    public IMGLoader _Jackpot_winner_pic;
    public TextMeshProUGUI Txt_Jackpot_My_Name;
    public TextMeshProUGUI Txt_Jackpot_My_Chips;
    public IMGLoader _Jackpot_My_pic;

    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Update_JACKPOT(JSONObject data)
    {
        Total_JackPot = float.Parse(data.GetField("collected_jackpot_amount").ToString().Trim(Config.Inst.trim_char_arry));
        Txt_Jackpot.text = "JACKPOT : " + int.Parse(data.GetField("collected_jackpot_amount").ToString().Trim(Config.Inst.trim_char_arry)).ToString("N0"); 
        HR_Jackpot_List_Hendler.Inst.Txt_Total_jackpotAmount.text = "JACKPOT : " + data.GetField("collected_jackpot_amount").ToString().Trim(Config.Inst.trim_char_arry);
    }
    public void Open_Jackpot_Popup(JSONObject data)
    {
        Txt_Jackpot_Win_Amount.text = "₹" + data.GetField("curr_jackport_amount").ToString().Trim(Config.Inst.trim_char_arry);
        bool my_user_found = false;
        GS.Inst.iTwin_Open(Jackpot_SC);
        for (int i = 0; i < data.GetField("jackport_users").Count; i++)
        {
            if (data.GetField("jackport_users")[i].GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry).Equals(GS.Inst._userData.Id))
            {
                my_user_found = true;
                Txt_Jackpot_My_Chips.text = data.GetField("jackport_users")[i].GetField("win_amount").ToString().Trim(Config.Inst.trim_char_arry);
                Txt_Jackpot_WinnerChips.text = data.GetField("jackport_users")[i].GetField("win_amount").ToString().Trim(Config.Inst.trim_char_arry);
            }
        }

        if (!my_user_found)
        {
            Txt_Jackpot_My_Name.text = GS.Inst._userData.Name;
            Txt_Jackpot_My_Chips.text = "0";
            _Jackpot_My_pic.LoadIMG(GS.Inst._userData.PicUrl, false, false);

            Txt_Jackpot_WinnerName.text = data.GetField("jackport_users")[0].GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
            Txt_Jackpot_WinnerChips.text = data.GetField("jackport_users")[0].GetField("win_amount").ToString().Trim(Config.Inst.trim_char_arry);
            _Jackpot_winner_pic.LoadIMG(data.GetField("jackport_users")[0].GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false, false);
        }
        else
        {
            Txt_Jackpot_WinnerName.text = GS.Inst._userData.Name;
            _Jackpot_winner_pic.LoadIMG(GS.Inst._userData.PicUrl, false, false);

            Txt_Jackpot_My_Name.text = GS.Inst._userData.Name;
            _Jackpot_My_pic.LoadIMG(GS.Inst._userData.PicUrl, false, false);
        }
    }
    public void Close_Jackpot_Popup()
    {
        GS.Inst.iTwin_Close(Jackpot_SC,0.3f);
    }
    public void Exit_Game()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        HR_Exit_Popup.Inst.Open_Popup();
    }

    public void BTN_GROUP_LIST()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        DT_Online_User_Manager.Inst.BTN_OPEN();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.HORSE_RACING_JOINED_USER_LISTS());
    }
    public void UPDATE_USER_BET(JSONObject data)
    {
        int side = int.Parse(data.GetField("sides").ToString().Trim(Config.Inst.trim_char_arry));
        if(List_Txt_User_Total_Bet[side - 1].text!="0")
            List_Txt_User_Total_Bet[side - 1].text = (int.Parse(List_Txt_User_Total_Bet[side - 1].text) + int.Parse(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry))).ToString();
        else
            List_Txt_User_Total_Bet[side - 1].text = data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry);
    }
    public void SET_USER_PLAYED_TOTAL_CHIPS(JSONObject data)
    {
        if (data.Count > 0)
        {
            for (int i = 0; i < 6; i++)
            {
                if (data.HasField((i + 1).ToString()))
                {
                    List_Txt_User_Total_Bet[i].text = data.GetField((i + 1).ToString()).ToString().Trim(Config.Inst.trim_char_arry);
                    if (int.Parse(data.GetField((i + 1).ToString()).ToString().Trim(Config.Inst.trim_char_arry)) != 0)
                        HR_Manager.Inst.Total_BetBox_Click_List.Add(i + 1);
                }
            }
        }
    }
    public void SET_OTHER_PLAYED_TOTAL_CHIPS(JSONObject data)
    {
        if (data.Count > 0)
        {
            for (int i = 0; i < 6; i++)
            {
                List_Txt_Other_User_Total_Bet[i].text = data.GetField((i + 1).ToString()).ToString().Trim(Config.Inst.trim_char_arry);
            }
        }
    }
    public void SET_TOTAL_X_CHIPS(JSONObject data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            List_Txt_Total_X[i].text= data[i].ToString().Trim(Config.Inst.trim_char_arry) + "x";
            List_Txt_X_Ground[i].text= data[i].ToString().Trim(Config.Inst.trim_char_arry) + "x";
        }
    }

    public void RESET_UI_NEXT_ROUDN()
    {
        for (int i = 0; i < List_Txt_User_Total_Bet.Count; i++)
        {
            List_Txt_User_Total_Bet[i].text = "0";
            List_Txt_Other_User_Total_Bet[i].text = "0";
        }
    }

    public void X_Ground_Box(bool action)
    {
        if (action)
            Horse_X_Box_Ground.transform.localScale = Vector3.one;
        else
            Horse_X_Box_Ground.transform.localScale = Vector3.zero;
    }

    public void WIN_X_BET_Box(bool action,string Text,GameObject target)
    {
        if (action)
        {
            Txt_WinX_Bet_Box.text ="WIN x"+Text;
            Txt_WinX_Bet_Box.transform.position = target.transform.position;
            Txt_WinX_Bet_Box.transform.localScale = Vector3.one;
        }
        else
        {
            Txt_WinX_Bet_Box.transform.localScale = Vector3.zero;
        }
    }

    public void Win_Highlight(int Index,string x)
    {
        for (int i = 0; i < List_Win_Highlight_Anim.Count; i++)
        {
            if (i == Index)
            {
                List_Win_Highlight_Anim[i].SetActive(true);
                WIN_X_BET_Box(true, x, List_Win_Highlight_Anim[i]);
            }
            else
                List_Win_Highlight_Anim[i].SetActive(false);
        }

        if(Index==7)
            WIN_X_BET_Box(false, x, null);
    }

    public IEnumerator JackPot_CountBox_ON(int JackpotTargetValue)
    {
        CollectJackpot_Mini_Box.transform.localScale = Vector3.one;
        float jackpot_amount = Total_JackPot;
        float targetValue = JackpotTargetValue;
        int targetValue2 = JackpotTargetValue;
        float currentValue = 1;
        int i = 50;
        var rate = Mathf.Abs(targetValue - currentValue) / 2;
        while (currentValue != targetValue)
        {
            currentValue = Mathf.MoveTowards(currentValue, targetValue, rate * Time.deltaTime);
            Txt_CollectJackpot_Mini.text = ((int)currentValue).ToString("N0");
            Txt_Jackpot.text = "JACKPOT : " + (jackpot_amount - (int)currentValue).ToString("N0");
            if (i > 1)
            {
                GameObject _Coin = Instantiate(Pfb_jackpot_coin, GameObject.Find("BG_MAIN").transform) as GameObject;
                _Coin.transform.position = Txt_Jackpot.transform.position;
                _Coin.transform.GetComponent<HR_PFB_JackpotCoin>().Move_Anim(Txt_CollectJackpot_Mini.transform.position);
                i--;
            }
            yield return null;
        }
    }

    public void JackPot_CountBox_OFF()
    {
        CollectJackpot_Mini_Box.transform.localScale = Vector3.zero;
        Txt_CollectJackpot_Mini.text = "₹0";
    }
}
