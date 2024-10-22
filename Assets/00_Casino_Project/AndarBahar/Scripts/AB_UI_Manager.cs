using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AB_UI_Manager : MonoBehaviour
{
    public static AB_UI_Manager Inst;
    public Text TxtGameID;
    public TextMeshProUGUI Txt_Total_Andar_PlayCoin,
        Txt_Total_Bahar_PlayCoin,
        Txt_Total_PlayCoin_1_5,
        Txt_Total_PlayCoin_6_10,
        Txt_Total_PlayCoin_11_15,
        Txt_Total_PlayCoin_16_25,
        Txt_Total_PlayCoin_26_30,
        Txt_Total_PlayCoin_31_35,
        Txt_Total_PlayCoin_36_40,
        Txt_Total_PlayCoin_41_48;

    public TextMeshProUGUI Other_Txt_Total_Andar_PlayCoin,
       Other_Txt_Total_Bahar_PlayCoin,
       Other_Txt_Total_PlayCoin_1_5,
       Other_Txt_Total_PlayCoin_6_10,
       Other_Txt_Total_PlayCoin_11_15,
       Other_Txt_Total_PlayCoin_16_25,
       Other_Txt_Total_PlayCoin_26_30,
       Other_Txt_Total_PlayCoin_31_35,
       Other_Txt_Total_PlayCoin_36_40,
       Other_Txt_Total_PlayCoin_41_48;

    public List<GameObject> Glow_Anim_List;
    public GameObject HistAndarGlow, HistBaharGlow;
    public GameObject AndarGlow, BaharGlow;
    public Animator Start_Bet_Anim, Stop_Bet_Anim;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Exit_Game()
    {
        AB_SoundManager.Inst.PlaySFX(0);
        AB_Exit_Popup.Inst.Open_Popup();
    }

    public void BTN_GROUP_LIST()
    {
        AB_SoundManager.Inst.PlaySFX(0);
        AB_Online_User_Manager.Inst.BTN_OPEN();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.ANDAR_BAHAR_JOINED_USER_LISTS());
    }
    public void NEW_ROUND_START_STOP(bool action, string screen)
    {
        if (screen.Equals("sb"))
            Start_Bet_Anim.Play("StartBet_Anim");
        else
            Stop_Bet_Anim.Play("StartBet_Anim");
    }

    public void SET_USER_BET_ONBOARD(JSONObject data)
    {
        AB_PlayerManager.Inst._User_TotalBet_Andar = double.Parse(data.GetField("current_bets").GetField("andar").ToString().Trim(Config.Inst.trim_char_arry));
        AB_PlayerManager.Inst._User_TotalBet_Bahar = double.Parse(data.GetField("current_bets").GetField("bahar").ToString().Trim(Config.Inst.trim_char_arry));
        AB_PlayerManager.Inst._User_TotalBet_1_5 = double.Parse(data.GetField("current_bets").GetField("1-5").ToString().Trim(Config.Inst.trim_char_arry));
        AB_PlayerManager.Inst._User_TotalBet_6_10 = double.Parse(data.GetField("current_bets").GetField("6-10").ToString().Trim(Config.Inst.trim_char_arry));
        AB_PlayerManager.Inst._User_TotalBet_11_15 = double.Parse(data.GetField("current_bets").GetField("11-15").ToString().Trim(Config.Inst.trim_char_arry));
        AB_PlayerManager.Inst._User_TotalBet_16_25 = double.Parse(data.GetField("current_bets").GetField("16-25").ToString().Trim(Config.Inst.trim_char_arry));
        AB_PlayerManager.Inst._User_TotalBet_26_30 = double.Parse(data.GetField("current_bets").GetField("26-30").ToString().Trim(Config.Inst.trim_char_arry));
        AB_PlayerManager.Inst._User_TotalBet_31_35 = double.Parse(data.GetField("current_bets").GetField("31-35").ToString().Trim(Config.Inst.trim_char_arry));
        AB_PlayerManager.Inst._User_TotalBet_36_40 = double.Parse(data.GetField("current_bets").GetField("36-40").ToString().Trim(Config.Inst.trim_char_arry));
        AB_PlayerManager.Inst._User_TotalBet_41_48 = double.Parse(data.GetField("current_bets").GetField("41-48").ToString().Trim(Config.Inst.trim_char_arry));


                if (AB_PlayerManager.Inst._User_TotalBet_Andar > 0)
                    Txt_Total_Andar_PlayCoin.transform.parent.localScale = Vector3.one;
                Txt_Total_Andar_PlayCoin.text = AB_PlayerManager.Inst._User_TotalBet_Andar.ToString();

                if (data.GetField("dis_total_bet_on_cards").HasField("andar"))
                {
                    if (int.Parse(data.GetField("dis_total_bet_on_cards").GetField("andar").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                        Other_Txt_Total_Andar_PlayCoin.transform.parent.localScale = Vector3.one;
                    Other_Txt_Total_Andar_PlayCoin.text = data.GetField("dis_total_bet_on_cards").GetField("andar").ToString().Trim(Config.Inst.trim_char_arry);
                }

                if (AB_PlayerManager.Inst._User_TotalBet_Bahar > 0)
                    Txt_Total_Bahar_PlayCoin.transform.parent.localScale = Vector3.one;
                Txt_Total_Bahar_PlayCoin.text = AB_PlayerManager.Inst._User_TotalBet_Bahar.ToString();

                if (data.GetField("dis_total_bet_on_cards").HasField("bahar"))
                {
                    if (int.Parse(data.GetField("dis_total_bet_on_cards").GetField("bahar").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                        Other_Txt_Total_Bahar_PlayCoin.transform.parent.localScale = Vector3.one;
                    Other_Txt_Total_Bahar_PlayCoin.text = data.GetField("dis_total_bet_on_cards").GetField("bahar").ToString().Trim(Config.Inst.trim_char_arry);
                }

                if (AB_PlayerManager.Inst._User_TotalBet_1_5 > 0)
                    Txt_Total_PlayCoin_1_5.transform.parent.localScale = Vector3.one;
                Txt_Total_PlayCoin_1_5.text = AB_PlayerManager.Inst._User_TotalBet_1_5.ToString();

                if (data.GetField("dis_total_bet_on_cards").HasField("1-5"))
                {
                    if (int.Parse(data.GetField("dis_total_bet_on_cards").GetField("1-5").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                        Other_Txt_Total_PlayCoin_1_5.transform.parent.localScale = Vector3.one;
                    Other_Txt_Total_PlayCoin_1_5.text = data.GetField("dis_total_bet_on_cards").GetField("1-5").ToString().Trim(Config.Inst.trim_char_arry);
                }

                if (AB_PlayerManager.Inst._User_TotalBet_6_10 > 0)
                    Txt_Total_PlayCoin_6_10.transform.parent.localScale = Vector3.one;
                Txt_Total_PlayCoin_6_10.text = AB_PlayerManager.Inst._User_TotalBet_6_10.ToString();

                if (data.GetField("dis_total_bet_on_cards").HasField("6-10"))
                {
                    if (int.Parse(data.GetField("dis_total_bet_on_cards").GetField("6-10").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                        Other_Txt_Total_PlayCoin_6_10.transform.parent.localScale = Vector3.one;
                    Other_Txt_Total_PlayCoin_6_10.text = data.GetField("dis_total_bet_on_cards").GetField("6-10").ToString().Trim(Config.Inst.trim_char_arry);
                }

                if (AB_PlayerManager.Inst._User_TotalBet_11_15 > 0)
                    Txt_Total_PlayCoin_11_15.transform.parent.localScale = Vector3.one;
                Txt_Total_PlayCoin_11_15.text = AB_PlayerManager.Inst._User_TotalBet_11_15.ToString();

                if (data.GetField("dis_total_bet_on_cards").HasField("11-15"))
                {
                    if (int.Parse(data.GetField("dis_total_bet_on_cards").GetField("11-15").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                        Other_Txt_Total_PlayCoin_11_15.transform.parent.localScale = Vector3.one;
                    Other_Txt_Total_PlayCoin_11_15.text = data.GetField("dis_total_bet_on_cards").GetField("11-15").ToString().Trim(Config.Inst.trim_char_arry);
                }

                if (AB_PlayerManager.Inst._User_TotalBet_16_25 > 0)
                    Txt_Total_PlayCoin_16_25.transform.parent.localScale = Vector3.one;
                Txt_Total_PlayCoin_16_25.text = AB_PlayerManager.Inst._User_TotalBet_16_25.ToString();

                if (data.GetField("dis_total_bet_on_cards").HasField("16-25"))
                {
                    if (int.Parse(data.GetField("dis_total_bet_on_cards").GetField("16-25").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                        Other_Txt_Total_PlayCoin_16_25.transform.parent.localScale = Vector3.one;
                    Other_Txt_Total_PlayCoin_16_25.text = data.GetField("dis_total_bet_on_cards").GetField("16-25").ToString().Trim(Config.Inst.trim_char_arry);
                }

                if (AB_PlayerManager.Inst._User_TotalBet_26_30 > 0)
                    Txt_Total_PlayCoin_26_30.transform.parent.localScale = Vector3.one;
                Txt_Total_PlayCoin_26_30.text = AB_PlayerManager.Inst._User_TotalBet_26_30.ToString();

                if (data.GetField("dis_total_bet_on_cards").HasField("26-30"))
                {
                    if (int.Parse(data.GetField("dis_total_bet_on_cards").GetField("26-30").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                        Other_Txt_Total_PlayCoin_26_30.transform.parent.localScale = Vector3.one;
                    Other_Txt_Total_PlayCoin_26_30.text = data.GetField("dis_total_bet_on_cards").GetField("26-30").ToString().Trim(Config.Inst.trim_char_arry);
                }

                if (AB_PlayerManager.Inst._User_TotalBet_31_35 > 0)
                    Txt_Total_PlayCoin_31_35.transform.parent.localScale = Vector3.one;
                Txt_Total_PlayCoin_31_35.text = AB_PlayerManager.Inst._User_TotalBet_31_35.ToString();

                if (data.GetField("dis_total_bet_on_cards").HasField("31-35"))
                {
                    if (int.Parse(data.GetField("dis_total_bet_on_cards").GetField("31-35").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                        Other_Txt_Total_PlayCoin_31_35.transform.parent.localScale = Vector3.one;
                    Other_Txt_Total_PlayCoin_31_35.text = data.GetField("dis_total_bet_on_cards").GetField("31-35").ToString().Trim(Config.Inst.trim_char_arry);
                }

                if (AB_PlayerManager.Inst._User_TotalBet_36_40 > 0)
                    Txt_Total_PlayCoin_36_40.transform.parent.localScale = Vector3.one;
                Txt_Total_PlayCoin_36_40.text = AB_PlayerManager.Inst._User_TotalBet_36_40.ToString();

                if (data.GetField("dis_total_bet_on_cards").HasField("36-40"))
                {
                    if (int.Parse(data.GetField("dis_total_bet_on_cards").GetField("36-40").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                        Other_Txt_Total_PlayCoin_36_40.transform.parent.localScale = Vector3.one;
                    Other_Txt_Total_PlayCoin_36_40.text = data.GetField("dis_total_bet_on_cards").GetField("36-40").ToString().Trim(Config.Inst.trim_char_arry);
                }

                if (AB_PlayerManager.Inst._User_TotalBet_41_48 > 0)
                    Txt_Total_PlayCoin_41_48.transform.parent.localScale = Vector3.one;
                Txt_Total_PlayCoin_41_48.text = AB_PlayerManager.Inst._User_TotalBet_41_48.ToString();

                if (data.GetField("dis_total_bet_on_cards").HasField("41-48"))
                {
                    if (int.Parse(data.GetField("dis_total_bet_on_cards").GetField("41-48").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                        Other_Txt_Total_PlayCoin_41_48.transform.parent.localScale = Vector3.one;
                    Other_Txt_Total_PlayCoin_41_48.text = data.GetField("dis_total_bet_on_cards").GetField("41-48").ToString().Trim(Config.Inst.trim_char_arry);
                }
        }
    
    public void SET_PLAYED_TOTAL_CHIPS(JSONObject data)
    {
        string side= data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry);
        switch (side)
        {
            case "andar":
                if(AB_PlayerManager.Inst._User_TotalBet_Andar>0)
                    Txt_Total_Andar_PlayCoin.transform.parent.localScale = Vector3.one;
                if(int.Parse(data.GetField("andar").ToString().Trim(Config.Inst.trim_char_arry))>0)
                    Other_Txt_Total_Andar_PlayCoin.transform.parent.localScale = Vector3.one;

                Txt_Total_Andar_PlayCoin.text = AB_PlayerManager.Inst._User_TotalBet_Andar.ToString();
                Other_Txt_Total_Andar_PlayCoin.text = data.GetField("andar").ToString().Trim(Config.Inst.trim_char_arry);
                break;
            case "bahar":
                if (AB_PlayerManager.Inst._User_TotalBet_Bahar > 0)
                    Txt_Total_Bahar_PlayCoin.transform.parent.localScale = Vector3.one;
                if (int.Parse(data.GetField("bahar").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                    Other_Txt_Total_Bahar_PlayCoin.transform.parent.localScale = Vector3.one;

                Txt_Total_Bahar_PlayCoin.text = AB_PlayerManager.Inst._User_TotalBet_Bahar.ToString();
                Other_Txt_Total_Bahar_PlayCoin.text = data.GetField("bahar").ToString().Trim(Config.Inst.trim_char_arry);
                break;
            case "1-5":
                if (AB_PlayerManager.Inst._User_TotalBet_1_5 > 0)
                    Txt_Total_PlayCoin_1_5.transform.parent.localScale = Vector3.one;
                if (int.Parse(data.GetField("1-5").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                    Other_Txt_Total_PlayCoin_1_5.transform.parent.localScale = Vector3.one;

                Txt_Total_PlayCoin_1_5.text = AB_PlayerManager.Inst._User_TotalBet_1_5.ToString();
                Other_Txt_Total_PlayCoin_1_5.text = data.GetField("1-5").ToString().Trim(Config.Inst.trim_char_arry);
                break;
            case "6-10":
                if (AB_PlayerManager.Inst._User_TotalBet_6_10 > 0)
                    Txt_Total_PlayCoin_6_10.transform.parent.localScale = Vector3.one;
                if (int.Parse(data.GetField("6-10").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                    Other_Txt_Total_PlayCoin_6_10.transform.parent.localScale = Vector3.one;

                Txt_Total_PlayCoin_6_10.text = AB_PlayerManager.Inst._User_TotalBet_6_10.ToString();
                Other_Txt_Total_PlayCoin_6_10.text = data.GetField("6-10").ToString().Trim(Config.Inst.trim_char_arry);
                break;
            case "11-15":
                if (AB_PlayerManager.Inst._User_TotalBet_11_15 > 0)
                    Txt_Total_PlayCoin_11_15.transform.parent.localScale = Vector3.one;
                if (int.Parse(data.GetField("11-15").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                    Other_Txt_Total_PlayCoin_11_15.transform.parent.localScale = Vector3.one;

                Txt_Total_PlayCoin_11_15.text = AB_PlayerManager.Inst._User_TotalBet_11_15.ToString();
                Other_Txt_Total_PlayCoin_11_15.text = data.GetField("11-15").ToString().Trim(Config.Inst.trim_char_arry);
                break;
            case "16-25":
                if (AB_PlayerManager.Inst._User_TotalBet_16_25 > 0)
                    Txt_Total_PlayCoin_16_25.transform.parent.localScale = Vector3.one;
                if (int.Parse(data.GetField("16-25").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                    Other_Txt_Total_PlayCoin_16_25.transform.parent.localScale = Vector3.one;

                Txt_Total_PlayCoin_16_25.text = AB_PlayerManager.Inst._User_TotalBet_16_25.ToString();
                Other_Txt_Total_PlayCoin_16_25.text = data.GetField("16-25").ToString().Trim(Config.Inst.trim_char_arry);
                break;
            case "26-30":
                if (AB_PlayerManager.Inst._User_TotalBet_26_30 > 0)
                    Txt_Total_PlayCoin_26_30.transform.parent.localScale = Vector3.one;
                if (int.Parse(data.GetField("26-30").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                    Other_Txt_Total_PlayCoin_26_30.transform.parent.localScale = Vector3.one;

                Txt_Total_PlayCoin_26_30.text = AB_PlayerManager.Inst._User_TotalBet_26_30.ToString();
                Other_Txt_Total_PlayCoin_26_30.text = data.GetField("26-30").ToString().Trim(Config.Inst.trim_char_arry);
                break;
            case "31-35":
                if (AB_PlayerManager.Inst._User_TotalBet_31_35 > 0)
                    Txt_Total_PlayCoin_31_35.transform.parent.localScale = Vector3.one;
                if (int.Parse(data.GetField("31-35").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                    Other_Txt_Total_PlayCoin_31_35.transform.parent.localScale = Vector3.one;

                Txt_Total_PlayCoin_31_35.text = AB_PlayerManager.Inst._User_TotalBet_31_35.ToString();
                Other_Txt_Total_PlayCoin_31_35.text = data.GetField("31-35").ToString().Trim(Config.Inst.trim_char_arry);
                break;
            case "36-40":
                if (AB_PlayerManager.Inst._User_TotalBet_36_40 > 0)
                    Txt_Total_PlayCoin_36_40.transform.parent.localScale = Vector3.one;
                if (int.Parse(data.GetField("36-40").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                    Other_Txt_Total_PlayCoin_36_40.transform.parent.localScale = Vector3.one;

                Txt_Total_PlayCoin_36_40.text = AB_PlayerManager.Inst._User_TotalBet_36_40.ToString();
                Other_Txt_Total_PlayCoin_36_40.text = data.GetField("36-40").ToString().Trim(Config.Inst.trim_char_arry);
                break;
            case "41-48":
                if (AB_PlayerManager.Inst._User_TotalBet_41_48 > 0)
                    Txt_Total_PlayCoin_41_48.transform.parent.localScale = Vector3.one;
                if (int.Parse(data.GetField("41-48").ToString().Trim(Config.Inst.trim_char_arry)) > 0)
                    Other_Txt_Total_PlayCoin_41_48.transform.parent.localScale = Vector3.one;

                Txt_Total_PlayCoin_41_48.text = AB_PlayerManager.Inst._User_TotalBet_41_48.ToString();
                Other_Txt_Total_PlayCoin_41_48.text = data.GetField("41-48").ToString().Trim(Config.Inst.trim_char_arry);
                break;
        }
    }

    public void Active_Glow_Anim(string winCard,string winBox)
    {
        if (winCard.Equals("andar"))
        {
            HistBaharGlow.SetActive(false);
            HistAndarGlow.SetActive(true);
            BaharGlow.SetActive(false);
            AndarGlow.SetActive(true);
        }
        else
        {
            HistAndarGlow.SetActive(false);
            HistBaharGlow.SetActive(true);
            AndarGlow.SetActive(false);
            BaharGlow.SetActive(true);
        }

        for (int i = 0; i < Glow_Anim_List.Count; i++)
        {
            if (winBox.Equals(Glow_Anim_List[i].name))
                Glow_Anim_List[i].SetActive(true);
            else
                Glow_Anim_List[i].SetActive(false);
        }
    }

    public void Stop_Glow_Anim()
    {
        HistBaharGlow.SetActive(false);
        HistAndarGlow.SetActive(false);
        BaharGlow.SetActive(false);
        AndarGlow.SetActive(false);
        for (int i = 0; i < Glow_Anim_List.Count; i++)
        {
            Glow_Anim_List[i].SetActive(false);
        }
    }

    public void RESET_UI_NEXT_ROUDN()
    {
        Txt_Total_Andar_PlayCoin.text = "";
        Txt_Total_Bahar_PlayCoin.text = "";
        Txt_Total_PlayCoin_1_5.text = "";
        Txt_Total_PlayCoin_6_10.text = "";
        Txt_Total_PlayCoin_11_15.text = "";
        Txt_Total_PlayCoin_16_25.text = "";
        Txt_Total_PlayCoin_26_30.text = "";
        Txt_Total_PlayCoin_31_35.text = "";
        Txt_Total_PlayCoin_36_40.text = "";
        Txt_Total_PlayCoin_41_48.text = "";

        Txt_Total_Andar_PlayCoin.transform.parent.localScale = Vector3.zero;
        Txt_Total_Bahar_PlayCoin.transform.parent.localScale = Vector3.zero;
        Txt_Total_PlayCoin_1_5.transform.parent.localScale = Vector3.zero;
        Txt_Total_PlayCoin_6_10.transform.parent.localScale = Vector3.zero;
        Txt_Total_PlayCoin_11_15.transform.parent.localScale = Vector3.zero;
        Txt_Total_PlayCoin_16_25.transform.parent.localScale = Vector3.zero;
        Txt_Total_PlayCoin_26_30.transform.parent.localScale = Vector3.zero;
        Txt_Total_PlayCoin_31_35.transform.parent.localScale = Vector3.zero;
        Txt_Total_PlayCoin_36_40.transform.parent.localScale = Vector3.zero;
        Txt_Total_PlayCoin_41_48.transform.parent.localScale = Vector3.zero;

        Other_Txt_Total_Andar_PlayCoin.transform.parent.localScale = Vector3.zero;
        Other_Txt_Total_Bahar_PlayCoin.transform.parent.localScale = Vector3.zero;
        Other_Txt_Total_PlayCoin_1_5.transform.parent.localScale = Vector3.zero;
        Other_Txt_Total_PlayCoin_6_10.transform.parent.localScale = Vector3.zero;
        Other_Txt_Total_PlayCoin_11_15.transform.parent.localScale = Vector3.zero;
        Other_Txt_Total_PlayCoin_16_25.transform.parent.localScale = Vector3.zero;
        Other_Txt_Total_PlayCoin_26_30.transform.parent.localScale = Vector3.zero;
        Other_Txt_Total_PlayCoin_31_35.transform.parent.localScale = Vector3.zero;
        Other_Txt_Total_PlayCoin_36_40.transform.parent.localScale = Vector3.zero;
        Other_Txt_Total_PlayCoin_41_48.transform.parent.localScale = Vector3.zero;

        Other_Txt_Total_Andar_PlayCoin.text = "";
        Other_Txt_Total_Bahar_PlayCoin.text = "";
        Other_Txt_Total_PlayCoin_1_5.text = "";
        Other_Txt_Total_PlayCoin_6_10.text = "";
        Other_Txt_Total_PlayCoin_11_15.text = "";
        Other_Txt_Total_PlayCoin_16_25.text = "";
        Other_Txt_Total_PlayCoin_26_30.text = "";
        Other_Txt_Total_PlayCoin_31_35.text = "";
        Other_Txt_Total_PlayCoin_36_40.text = "";
        Other_Txt_Total_PlayCoin_41_48.text = "";

        AB_PlayerManager.Inst._User_TotalBet_Andar = 0;
        AB_PlayerManager.Inst._User_TotalBet_Bahar = 0;
        AB_PlayerManager.Inst._User_TotalBet_1_5 = 0;
        AB_PlayerManager.Inst._User_TotalBet_6_10 = 0;
        AB_PlayerManager.Inst._User_TotalBet_11_15 = 0;
        AB_PlayerManager.Inst._User_TotalBet_16_25 = 0;
        AB_PlayerManager.Inst._User_TotalBet_26_30 = 0;
        AB_PlayerManager.Inst._User_TotalBet_31_35 = 0;
        AB_PlayerManager.Inst._User_TotalBet_36_40 = 0;
        AB_PlayerManager.Inst._User_TotalBet_41_48 = 0;

        Stop_Glow_Anim();
    }
}
