using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TP_GameManager : MonoBehaviour
{
    public static TP_GameManager Inst;
    [SerializeField] internal List<TP_Player> playerList;
    internal bool IsMyTurn = false;
    public Text TxtGameID;

    //---------Sound ON OFF --------------
    [SerializeField] Image IMG_SOUND;
    [SerializeField] Sprite Sound_ON_Sprite, Sound_OF_Sprite;
    //---------Sound ON OFF --------------

    //---------Round Timer ---------------
    [SerializeField] Text TxtRoundTimer;
    [SerializeField] GameObject RoundTimer_OBJ;
    public int _RoundTimer;
    public GameObject MatchMaking_ANIM;
    //---------Round Timer ---------------

    //---------Card Deal ---------------
    public GameObject CardDeal_StartPOS;
    public GameObject Chaal_Coin_Perent,Chaal_AnimMove_Object, Tip_AnimMove_Object;
    public GameObject CardBack_Prefhab,Chaal_Coin_PFB;
    //---------Card Deal ---------------

    public Sprite Back_Card_Sprite;
    public bool Is_Increment_Chaal = false;

    //---------- Buttons ---------------
    public Button Btn_Chaal, Btn_Card_See, Btn_Show, Btn_SideShow, Btn_Pack,Btn_plus,Btn_Minus,Btn_Tips;
    public Button OBJ_Switch_Table;
    //---------- Buttons ---------------

    //---------- Win Card massage -------------
    [SerializeField]public GameObject Activity_Status_Message, Btn_PrivateCode_Share;
    //---------- Win Card massage -------------

    //---------- Boot value center manager ------------
    public Text TxtTotal_Boot,Txt_Chal_Amount;
    public float Chaal_Limit_Amount;
    //---------- Boot value center manager ------------

    //---------- Button List show and hide--------------
    [SerializeField] GameObject ButtonPenal, Start_ButtonPenal_POS, End_ButtonPenal_POS;
    //---------- Button List show and hide--------------

    int plus = 0;
    string Last_ChalAmount = "0.1";
    string Hukum_Card = "";

    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        //---------- Sound Setup --------------
        if (!PlayerPrefs.HasKey("sound"))
            PlayerPrefs.SetInt("sound", 1);
        Saved_Last_Setting();
        //---------- Sound Setup --------------

        SetMySeatIndex(GS.Inst.FullTableInfoData);
        SetPlayerSeatIndex(GS.Inst._userData.MySeatIndex);

        if (!GS.Inst.Rejoin)
            SET_MY_PLAYER_INFO();
        else
            Rejoin_Table_Set();
    }

    public void Saved_Last_Setting()
    {
        if (PlayerPrefs.GetInt("sound").Equals(0))
        {
            IMG_SOUND.sprite = Sound_OF_Sprite;
            PlayerPrefs.SetInt("sound", 0);
            TP_SoundManager.Inst.SFXAudio.mute = true;
        }
        else
        {
            IMG_SOUND.sprite = Sound_ON_Sprite;
            PlayerPrefs.SetInt("sound", 1);
            TP_SoundManager.Inst.SFXAudio.mute = false;
        }
    }

    //-------------Set All player info -------------
    public void SET_MY_PLAYER_INFO()
    {
        string GameState = GS.Inst.FullTableInfoData.GetField("game_state").ToString().Trim(Config.Inst.trim_char_arry);
        GS.Inst.PrivateTable = bool.Parse(GS.Inst.FullTableInfoData.GetField("is_private").ToString().Trim(Config.Inst.trim_char_arry));

        TxtGameID.text = "Game ID : " + GS.Inst.FullTableInfoData.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
        Hukum_Card = GS.Inst.FullTableInfoData.GetField("hukum").ToString().Trim(Config.Inst.trim_char_arry);
        TxtTotal_Boot.text = GS.Inst.FullTableInfoData.GetField("pot_value").ToString().Trim(Config.Inst.trim_char_arry);
        Chaal_Limit_Amount = float.Parse(GS.Inst.FullTableInfoData.GetField("chal_limit").ToString().Trim(Config.Inst.trim_char_arry));
        TP_RoomCodeShare.Inst.Table_PointValue = GS.Inst.FullTableInfoData.GetField("boot").ToString().Trim(Config.Inst.trim_char_arry);
        TP_RoomCodeShare.Inst.Table_MinEntry = GS.Inst.FullTableInfoData.GetField("min_entry").ToString().Trim(Config.Inst.trim_char_arry);
        if (GS.Inst.FullTableInfoData.GetField("dealer_seat_index").ToString().Trim(Config.Inst.trim_char_arry) != "-1")
        {
            int Deal_SeatIndex = int.Parse(GS.Inst.FullTableInfoData.GetField("dealer_seat_index").ToString().Trim(Config.Inst.trim_char_arry));
            TP_Player pd = GetPlayer_UingSetIndex(Deal_SeatIndex);
            pd.DealerIcon.transform.localScale = Vector3.one;
        }

        for (int i = 0; i < GS.Inst.FullTableInfoData.GetField("player_info").Count; i++)
        {
            if (GS.Inst.FullTableInfoData.GetField("player_info")[i].HasField("seat_index"))
            {
                TP_Player p = GetPlayer_UingSetIndex(int.Parse(GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry)));
                p.SET_MY_INFO(GS.Inst.FullTableInfoData.GetField("player_info")[i]);

                if (p.is_watch_MODE && p.SeatIndex.Equals(GS.Inst._userData.MySeatIndex))
                {
                    for (int j = 0; j < GS.Inst.FullTableInfoData.GetField("player_info").Count; j++)
                    {
                        if (GS.Inst.FullTableInfoData.GetField("player_info")[j].HasField("seat_index") && GS.Inst.FullTableInfoData.GetField("player_info")[j].GetField("cards").Count>0)
                        {
                            TP_Player p2 = GetPlayer_UingSetIndex(int.Parse(GS.Inst.FullTableInfoData.GetField("player_info")[j].GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry)));
                            p2.Show_PlayerCard_Back();
                        }
                    }

                    switch (GameState)
                    {
                        case "GameStartTimer":
                            int Roundtimer = int.Parse(GS.Inst.FullTableInfoData.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
                            Show_Round_Timer(Roundtimer);
                            break;
                        case "RoundStated":
                            //------------- Timer and Turn----------------
                            float Starttimer = float.Parse(GS.Inst.FullTableInfoData.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
                            int Turn_SeatIndex = int.Parse(GS.Inst.FullTableInfoData.GetField("turn_seat_index").ToString().Trim(Config.Inst.trim_char_arry));
                            TP_Player p_turn = GetPlayer_UingSetIndex(Turn_SeatIndex);
                            p_turn.MY_Timer_Start(Starttimer, 15f, true);
                            bool SideShow = false;
                            bool isShow = bool.Parse(GS.Inst.FullTableInfoData.GetField("turn_details").GetField("is_show").ToString().Trim(Config.Inst.trim_char_arry));
                            if (GS.Inst.FullTableInfoData.GetField("turn_details").HasField("is_side_details"))
                            {
                                SideShow = bool.Parse(GS.Inst.FullTableInfoData.GetField("turn_details").GetField("is_side_details").GetField("flag").ToString().Trim(Config.Inst.trim_char_arry));

                                if (GS.Inst.FullTableInfoData.GetField("side_show_details").HasField("request_index"))
                                    TP_SIdeShow_Hendler.Inst.HENDLE_SIDE_SHOW_REQ_REJOIN(GS.Inst.FullTableInfoData);
                            }
                            //------------- Timer and Turn----------------

                            //------------Player card management ------------------------
                            for (int j = 0; j < GS.Inst.FullTableInfoData.GetField("player_info").Count; j++)
                            {
                                if (GS.Inst.FullTableInfoData.GetField("player_info")[j].HasField("seat_index"))
                                {
                                    bool is_card_see = bool.Parse(GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("is_see").ToString().Trim(Config.Inst.trim_char_arry));
                                    string playStatus = GS.Inst.FullTableInfoData.GetField("player_info")[j].GetField("status").ToString().Trim(Config.Inst.trim_char_arry);
                                    int seatIndex = int.Parse(GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
                                    TP_Player p_card = GetPlayer_UingSetIndex(seatIndex);

                                    if (GS.Inst._userData.MySeatIndex.Equals(seatIndex)/* || playStatus == ""*/)
                                        p_card.Txt_UserChips.text = GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry);
                                    else
                                        p_card.Txt_UserChips.text = GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("last_chal_value").ToString().Trim(Config.Inst.trim_char_arry);

                                    string CardStatus = GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("play_status").ToString().Trim(Config.Inst.trim_char_arry);

                                    if (CardStatus != "")
                                    {
                                        if (CardStatus == "Chaal")
                                        {
                                            p_card.Card_Status.text = "Seen";
                                            p_card.Card_See_Eye.SetActive(true);
                                        }
                                        else if (CardStatus == "Pack")
                                            p_card.IM_PCKED();
                                        else
                                        {

                                            if (is_card_see)
                                            {
                                                p_card.Card_Status.text = "Seen";
                                                p_card.Card_See_Eye.SetActive(true);
                                            }
                                            else
                                                p_card.Card_Status.text = "Blind";
                                        }
                                    }

                                }
                            }
                            //------------Player card management ------------------------
                            break;
                        case "RoundWinState":
                            //------------Player card management ------------------------
                            for (int k = 0; k < GS.Inst.FullTableInfoData.GetField("player_info").Count; k++)
                            {
                                if (GS.Inst.FullTableInfoData.GetField("player_info")[k].HasField("seat_index"))
                                {
                                    string status = GS.Inst.FullTableInfoData.GetField("player_info")[k].GetField("play_status").ToString().Trim(Config.Inst.trim_char_arry);
                                    bool is_card_see = bool.Parse(GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("is_see").ToString().Trim(Config.Inst.trim_char_arry));
                                    int seatIndex = int.Parse(GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
                                    TP_Player p_card = GetPlayer_UingSetIndex(seatIndex);
                                    p_card.Card_Status.text = status;
                                    if (status == "Pack")
                                        p_card.IM_PCKED();

                                    if (p_card != null && p_card._SeatStatus != SeatStatus.Empty && p_card.Card_Status.text != "")
                                    {
                                        if (!is_card_see)
                                            p_card.Show_PlayerCard_Back();
                                        else
                                        {
                                            if (status != "Pack")
                                                p_card.SET_Card_SEE(GS.Inst.FullTableInfoData.GetField("player_info")[i]);
                                            else
                                                p_card.Show_PlayerCard_Back();
                                        }
                                        if (status.Equals("Winner"))
                                        {
                                            p_card.IM_Win();
                                            //Win_Card_Status_Message.transform.GetChild(0).GetComponent<Text>().text = data.GetField("user_info")[i].GetField("card_status").ToString().Trim(Config.Inst.trim_char_arry);
                                            //GS.Inst.iTwin_Open(Win_Card_Status_Message);
                                        }
                                    }

                                }
                            }
                            break;
                        case "RoundEndState":
                            break;
                    }
                }
            }
        }
       
        if (GS.Inst.PrivateTable)
        {
            string roomCode = GS.Inst.FullTableInfoData.GetField("private_game_id").ToString().Trim(Config.Inst.trim_char_arry);
            TP_RoomCodeShare.Inst.SET_ROOM_CODE(roomCode);
            TP_RoomCodeShare.Inst.OPEN_ROOM_CODE_SCREEN();
            OBJ_Switch_Table.interactable = false;
            Btn_PrivateCode_Share.transform.localScale = Vector3.one;
        }
        else
        {
            OBJ_Switch_Table.interactable = true;
            Btn_PrivateCode_Share.transform.localScale = Vector3.zero;
        }
        
        if (GameState.Equals("GameStartTimer"))
        {
            int Roundtimer = int.Parse(GS.Inst.FullTableInfoData.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
            Show_Round_Timer(Roundtimer);
        }
    }

    //-----------Set single player Info ---------------
    public void SET_NEW_USER_JOIN_INFO(JSONObject data)
    {
        GS.Inst.iTwin_Close(Activity_Status_Message, 0.3f);
        TP_Player p = GetPlayer_UingSetIndex(int.Parse(data.GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry)));
        p.SET_MY_INFO(data);
    }

    //Start New Round Timer 
    public void Show_Round_Timer(int Tot_Time)
    {
        if (GS.Inst.PrivateTable)
            TP_RoomCodeShare.Inst.CLOSE_ROOM_CODE_SCREEN();

        TxtTotal_Boot.text = "0";
        All_User_WinAnim_Hide();
        CLOSE_USER_WAIT_MSG();
        GS.Inst.iTwin_Close(Activity_Status_Message, 0.3f);
        All_Button_Active(false);
        All_User_Card_Hide();
        Stop_All_Player_Timer();
        _RoundTimer = Tot_Time;
        MatchMaking_ANIM.SetActive(true);
       // InvokeRepeating("Start_Round_Timer", 0.1f, 1f);
    }
    internal void Start_Round_Timer()
    {
        TP_SoundManager.Inst.PlaySFX_Others(5);
        GS.Inst.iTwin_Open(RoundTimer_OBJ);
        TxtRoundTimer.text="New Game Start In" + " " + _RoundTimer + " second(s)";
        _RoundTimer--;

        if (_RoundTimer <= 0)
        {
            _RoundTimer = 0;
            GS.Inst.iTwin_Close(RoundTimer_OBJ, 0.3f);
            CancelInvoke("Start_Round_Timer");
        }

    }

    //Set Player SeatIndex 5 player
    internal void SetPlayerSeatIndex(int id)
    {
        for (var i = 0; i < playerList.Count; i++)
        {
            var val = id + i;
            if (val >= playerList.Count)
            {
                val = val - playerList.Count;
            }
            playerList[i].SeatIndex = val;
        }
    }

    //Get player using seatindex
    internal TP_Player GetPlayer_UingSetIndex(int index)
    {
        TP_Player p = new TP_Player();
        for (var i = 0; i < playerList.Count; i++)
        {
            if (index == playerList[i].SeatIndex)
            {
                p = playerList[i];

            }
        }
        return p;
    }

    //Get player using playerID
    internal TP_Player GetPlayer_UsingID(string id)
    {
        TP_Player p = new TP_Player();
        for (var i = 0; i < playerList.Count; i++)
        {
            if (id == playerList[i].PlayerID)
            {
                p = playerList[i];
            }
        }
        return p;
    }

    //------------ Set My SeatIndex --------------
    internal void SetMySeatIndex(JSONObject data)
    {
        JSONObject allUser = new JSONObject(JSONObject.Type.ARRAY);
        allUser = data.GetField("player_info");
        for (int i = 0; i < allUser.Count; i++)
        {
            if (allUser[i].Count > 0)
            {
                string id = allUser[i].GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
                if (id.Equals(GS.Inst._userData.Id))
                    GS.Inst._userData.MySeatIndex = int.Parse(allUser[i].GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
            }

        }
    }


    //------------- Card Deal Animation -----------------
    public void Card_Deal_Now(JSONObject data)
    {
        if (data.GetField("cardDealIndexs").Count > 1)
        {
            TP_SoundManager.Inst.PlaySFX(6);
            CLOSE_USER_WAIT_MSG();
            StartCoroutine(Card_Deal_Anim(data));
        }
    }
    IEnumerator Card_Deal_Anim(JSONObject data)
    {
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < data.GetField("cardDealIndexs").Count; i++)
                {
                    int seatIndex = int.Parse(data.GetField("cardDealIndexs")[i].ToString().Trim(Config.Inst.trim_char_arry));
                    TP_Player p = GetPlayer_UingSetIndex(seatIndex);
                    p.ShowCard_DEAL_Animation();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < data.GetField("cardDealIndexs").Count; i++)
            {
                int seatIndex = int.Parse(data.GetField("cardDealIndexs")[i].ToString().Trim(Config.Inst.trim_char_arry));
                TP_Player p = GetPlayer_UingSetIndex(seatIndex);
                p.Show_PlayerCard_Back();

                if (seatIndex.Equals(GS.Inst._userData.MySeatIndex))
                {
                    p.Card_Status.text = "See";
                }
            }
            All_Button_Active(true);
    }
    //------------- Card Deal Animation -----------------


    //------------- User Tip -----------------
    public void User_Tip(JSONObject data)
    {
        int seatIndex = int.Parse(data.GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
        string amount = data.GetField("tip_amount").ToString().Trim(Config.Inst.trim_char_arry);
        TP_Player p = GetPlayer_UingSetIndex(seatIndex);
        p.Tip_Animation(amount);
    }
    //------------- User Tip -----------------

    //------------- User Turn Start -----------------
    public void User_Turn_Start(JSONObject data)
    {
        Stop_All_Player_Timer();
        Is_Increment_Chaal = false;
        TP_SIdeShow_Hendler.Inst.ClOSE_SIDE_SHOW_BOX();
        int seatIndex = int.Parse(data.GetField("next_turn").ToString().Trim(Config.Inst.trim_char_arry));
        bool is_Show = bool.Parse(data.GetField("is_show").ToString().Trim(Config.Inst.trim_char_arry));
        string Chalvalue = data.GetField("chal_value").ToString().Trim(Config.Inst.trim_char_arry);
        float Starttimer = float.Parse(data.GetField("turn_timer").ToString().Trim(Config.Inst.trim_char_arry));
        TP_Player p = GetPlayer_UingSetIndex(seatIndex);
        p.MY_Timer_Start(Starttimer, 15f, false);
        Txt_Chal_Amount.text = Chalvalue;
        bool SideShow = false;
        if (data.HasField("is_side_details"))
            SideShow = bool.Parse(data.GetField("is_side_details").GetField("flag").ToString().Trim(Config.Inst.trim_char_arry));

        if (GS.Inst._userData.MySeatIndex.Equals(seatIndex))
        {
            TP_SoundManager.Inst.PlaySFX(1);
            TP_Slide_Manu.Inst.CLOSE_Slide_Manu();
            TP_SwitchTable_Popup.Inst.transform.localScale = Vector3.zero;
            Btn_Chaal.interactable = true;
            Btn_plus.interactable = true;
            Btn_Minus.interactable = false;
            IsMyTurn = true;
            plus = 0;

            if (p.Card_Status.text != "")
            {
                if (p.Card_Status.text == "Chaal")
                {
                    if (float.Parse(Chalvalue) == Chaal_Limit_Amount)
                        Btn_plus.interactable = false;
                }
                else
                {
                    if (float.Parse(Chalvalue)*2 == Chaal_Limit_Amount)
                        Btn_plus.interactable = false;
                }
            }

            if (is_Show)
            {
                //p.Card_Status.text = "Seen";
                Btn_Show.interactable = true;
                Btn_Show.gameObject.transform.localScale = Vector3.one;
            }
            else
            {
                //p.Card_Status.text = "See";
                Btn_Show.interactable = false;
            }

            iTween.MoveTo(ButtonPenal.gameObject, iTween.Hash("position", Start_ButtonPenal_POS.transform.position, "time", 0.3f, "easetype", iTween.EaseType.linear));

            if (data.HasField("is_side_details"))
            {
                if (SideShow)
                {
                    Btn_Show.gameObject.transform.localScale = Vector3.zero;
                    Btn_SideShow.gameObject.transform.localScale = Vector3.one;
                    Btn_Show.interactable = false;
                }
                else
                {
                    Btn_Show.gameObject.transform.localScale = Vector3.one;
                    Btn_SideShow.gameObject.transform.localScale = Vector3.zero;
                }
            }
        }
        else
        {
            iTween.MoveTo(ButtonPenal.gameObject, iTween.Hash("position", End_ButtonPenal_POS.transform.position, "time", 0.3f, "easetype", iTween.EaseType.linear));
            IsMyTurn = false;
            Btn_Chaal.interactable = false;

            //if (is_Show || SideShow)
            //    p.Card_Status.text = "Seen";
            //else
            //    p.Card_Status.text = "Blind";
        }
    }
    public void Hide_Footer_Button()
    {
        iTween.MoveTo(ButtonPenal.gameObject, iTween.Hash("position", End_ButtonPenal_POS.transform.position, "time", 0.3f, "easetype", iTween.EaseType.linear));
    }
    public void Stop_All_Player_Timer()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].Timer_Filler.reset_turn_timer();
        }
    }
    //------------- User Turn Start -----------------

    //------------- User Card packed -----------------
    public void User_CardPack(JSONObject data)
    {
        int seatIndex = int.Parse(data.GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
        TP_Player p = GetPlayer_UingSetIndex(seatIndex);
        p.IM_PCKED();

        if(seatIndex.Equals(GS.Inst._userData.MySeatIndex))
            Btn_Card_See.interactable = false;
    }
    //------------- User Card packed -----------------

    //------------- User Chaal -----------------
    public void User_Chaal(JSONObject data)
    {
        int seatIndex = int.Parse(data.GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
        string amount = data.GetField("chal_value").ToString().Trim(Config.Inst.trim_char_arry);
        string PotValue = data.GetField("pot_value").ToString().Trim(Config.Inst.trim_char_arry);
        string Chalvalue = data.GetField("chal_value").ToString().Trim(Config.Inst.trim_char_arry);
        TP_Player p = GetPlayer_UingSetIndex(seatIndex);
        p.Chaal_Animation(amount, seatIndex);
        //p.Card_Status.text = "Chaal";
        TxtTotal_Boot.text = PotValue;
        Txt_Chal_Amount.text = Chalvalue;
    }
    //------------- User Chaal -----------------


    //------------- User Card See -----------------
    public void User_Card_See(JSONObject data)
    {
        int seatIndex = int.Parse(data.GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
        bool is_Show = bool.Parse(data.GetField("is_show").ToString().Trim(Config.Inst.trim_char_arry));
        TP_Player p = GetPlayer_UingSetIndex(seatIndex);
        p.Card_Status.text = "Seen";
        p.Card_See_Eye.SetActive(true);
        if (GS.Inst._userData.MySeatIndex.Equals(seatIndex))
        {
            TP_SoundManager.Inst.PlaySFX(8);
            Btn_Card_See.interactable = false;
            p.Card_Txt_Status_On_button.text = "Chaal";
            if (is_Show)
                Btn_Show.interactable = true;
            else
                Btn_Show.interactable = false;
        }
    }
    public void User_Card_See_DATA(JSONObject data)
    {
        TP_Player p = GetPlayer_UingSetIndex(GS.Inst._userData.MySeatIndex);
        p.SET_Card_SEE(data);
        p.Card_Status.text = "Seen";
        Txt_Chal_Amount.text = data.GetField("chalvalue").ToString().Trim(Config.Inst.trim_char_arry); ;
    }
    public void All_User_Card_Hide()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].Hide_PlayerCard_Back();
        }
    }
    //------------- User Card See -----------------

    //------------- All Buttons active true/false ---------------
    public void All_Button_Active(bool action)
    {
        //Btn_Chaal.interactable = false;
        Btn_Card_See.interactable = action;
        Btn_Show.interactable = false;
        Btn_Pack.interactable = action;
        Btn_plus.interactable = action;
        //Btn_Minus.interactable = action;
        Btn_Tips.interactable = action;
    }
    //------------- All Buttons active true/false ---------------

    //------------- Table leave ------------------------
    public void Table_Leave(JSONObject data)
    {
        if (SceneManager.GetActiveScene().name != "Dashboard")
        {
            int seatIndex = int.Parse(data.GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
            string reson = data.GetField("reason").ToString().Trim(Config.Inst.trim_char_arry);
            if (GS.Inst._userData.MySeatIndex.Equals(seatIndex))
            {
                if (reson != "switch_table")
                {
                    //CommenMSG.Inst.MSG("TABLE SWITCH ERROR",data.GetField("reason").ToString().Trim(Config.Inst.trim_char_arry));
                    SceneManager.LoadScene("Dashboard");
                }
                else
                {
                    PreeLoader.Inst.Show();
                }
            }
            else
            {
                TP_Player p = GetPlayer_UingSetIndex(seatIndex);
                p.IM_LEAVE();
            }
        }
    }
    //------------- Table leave ------------------------

    //------------- Wallete Update ------------------------
    public void Wallete_Update(JSONObject data)
    {
        int seatIndex = int.Parse(data.GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
        
        if (SceneManager.GetActiveScene().name == "Dashboard")
        {
            GS.Inst._userData.Chips = double.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
            //MainDashBoard.Inst.SetDashboardData();
        }
        if (SceneManager.GetActiveScene().name == "TeenPatti")
        {
            //if (GS.Inst._userData.MySeatIndex.Equals(seatIndex))
                //GS.Inst._userData.Chips = double.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));

            TP_Player p = GetPlayer_UingSetIndex(seatIndex);
            if (GS.Inst._userData.MySeatIndex.Equals(seatIndex))
                p.Txt_UserChips.text=data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry);

            //TP_Player p2 = GetPlayer_UingSetIndex(GS.Inst._userData.MySeatIndex);
            //p2.Txt_UserChips.text = GS.Inst._userData.Chips.ToString();
        }
    }
    //------------- Wallete Update ------------------------


    //------------- Winning -----------------
    public void Winning_DATA_SET(JSONObject data)
    {
        Stop_All_Player_Timer();
        bool cardPack = false;
        iTween.MoveTo(ButtonPenal.gameObject, iTween.Hash("position", End_ButtonPenal_POS.transform.position, "time", 0.3f, "easetype", iTween.EaseType.linear));

        if (data.HasField("is_card_pack_last_user"))
            cardPack = bool.Parse(data.GetField("is_card_pack_last_user").ToString().Trim(Config.Inst.trim_char_arry));

        for (int i = 0; i < data.GetField("user_info").Count; i++)
        {
            string status = data.GetField("user_info")[i].GetField("play_status").ToString().Trim(Config.Inst.trim_char_arry);
            int seatIndex = int.Parse(data.GetField("user_info")[i].GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
            TP_Player p = GetPlayer_UingSetIndex(seatIndex);
            //p.Txt_UserChips.text = data.GetField("user_info")[i].GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry);
            if (p != null && p._SeatStatus != SeatStatus.Empty && !cardPack)
            {
                p.SET_Card_SEE(data.GetField("user_info")[i]);
                p.Card_Status.gameObject.transform.parent.localScale = Vector3.zero;
            }
            if (status.Equals("win"))
            {
                if (seatIndex.Equals(GS.Inst._userData.MySeatIndex))
                {
                    TP_SoundManager.Inst.PlaySFX(4);
                    p.SET_Card_SEE(data.GetField("user_info")[i]);
                }
                p.Card_Status.gameObject.transform.parent.localScale = Vector3.zero;
                p.IM_Win();
                if (!cardPack)
                {
                    Activity_Status_Message.transform.GetChild(0).GetComponent<Text>().text = data.GetField("user_info")[i].GetField("card_status").ToString().Trim(Config.Inst.trim_char_arry);
                    GS.Inst.iTwin_Open(Activity_Status_Message);
                }
            }
        }

        if (data.GetField("user_info").Count == 2)
        {
            for (int i = 0; i < data.GetField("user_info").Count; i++)
            {
                string status = data.GetField("user_info")[i].GetField("play_status").ToString().Trim(Config.Inst.trim_char_arry);
                if (status.Equals("leave_table"))
                {
                    Invoke("Stop_Leave_Win_Anim",2f);
                }
            }
        }

    }
    void Stop_Leave_Win_Anim()
    {
        TP_Player p = GetPlayer_UingSetIndex(GS.Inst._userData.MySeatIndex);
        p.Stop_Win_Anim();
        TxtTotal_Boot.text = "0";
        p.Hide_PlayerCard_Back();
    }
    public void All_User_WinAnim_Hide()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].Stop_Win_Anim();
        }
    }
    //------------- Winning -----------------

    public void Activity_Message(string msg)
    {
        CancelInvoke(nameof(close_activityMsg));
        Activity_Status_Message.transform.GetChild(0).GetComponent<Text>().text = msg;
        GS.Inst.iTwin_Open(Activity_Status_Message);
        Invoke(nameof(close_activityMsg), 1.5f);
    }
    void close_activityMsg()
    {
        GS.Inst.iTwin_Close(Activity_Status_Message,0.3f);
    }
    //------------- Boot collection ---------------
    public void Boot_Collection(JSONObject data)
    {
        TP_SoundManager.Inst.PlaySFX(3);
        All_User_WinAnim_Hide();
        TxtGameID.text = "Game ID : " + data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
        float betVal = float.Parse(data.GetField("bet").ToString().Trim(Config.Inst.trim_char_arry));

        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].Boot_Collection_Animation(betVal);
            playerList[i].DealerIcon.transform.localScale = Vector3.zero;
        }
        int Deal_SeatIndex = int.Parse(data.GetField("dealer_seat_index").ToString().Trim(Config.Inst.trim_char_arry));
        TP_Player p = GetPlayer_UingSetIndex(Deal_SeatIndex);
        p.DealerIcon.transform.localScale = Vector3.one;
    }
    public void TotalBoot_Plus(float bootAmount)
    {
        TxtTotal_Boot.text = (float.Parse(TxtTotal_Boot.text) + bootAmount).ToString();
    }
    //------------- Boot collection ---------------


    //------------- Rejoin again old table ---------------
    public void Rejoin_Table_Set()
    {
        //------- Player and Info -------------
        SET_MY_PLAYER_INFO();
        //------- Player and Info -------------

        //-------- Table extra data -------------
        string GameState = GS.Inst.FullTableInfoData.GetField("game_state").ToString().Trim(Config.Inst.trim_char_arry);
        GS.Inst.PrivateTable = bool.Parse(GS.Inst.FullTableInfoData.GetField("is_private").ToString().Trim(Config.Inst.trim_char_arry));
        GS.Inst.ActivePlayer = int.Parse(GS.Inst.FullTableInfoData.GetField("active_player").ToString().Trim(Config.Inst.trim_char_arry));

        TxtGameID.text = "Game ID : " + GS.Inst.FullTableInfoData.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
        Hukum_Card = GS.Inst.FullTableInfoData.GetField("hukum").ToString().Trim(Config.Inst.trim_char_arry);
        TxtTotal_Boot.text = GS.Inst.FullTableInfoData.GetField("pot_value").ToString().Trim(Config.Inst.trim_char_arry);
        Chaal_Limit_Amount = float.Parse(GS.Inst.FullTableInfoData.GetField("chal_limit").ToString().Trim(Config.Inst.trim_char_arry));

        TP_RoomCodeShare.Inst.Table_PointValue = GS.Inst.FullTableInfoData.GetField("boot").ToString().Trim(Config.Inst.trim_char_arry);
        TP_RoomCodeShare.Inst.Table_MinEntry = GS.Inst.FullTableInfoData.GetField("min_entry").ToString().Trim(Config.Inst.trim_char_arry);
        if (GS.Inst.FullTableInfoData.GetField("dealer_seat_index").ToString().Trim(Config.Inst.trim_char_arry) != "-1")
        {
            int Deal_SeatIndex = int.Parse(GS.Inst.FullTableInfoData.GetField("dealer_seat_index").ToString().Trim(Config.Inst.trim_char_arry));
            TP_Player pd = GetPlayer_UingSetIndex(Deal_SeatIndex);
            pd.DealerIcon.transform.localScale = Vector3.one;
        }
        //-------- Table extra data -------------

        switch (GameState)
        {
            case "GameStartTimer":
                int Roundtimer = int.Parse(GS.Inst.FullTableInfoData.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
                Show_Round_Timer(Roundtimer);
                break;
            case "RoundStated":
                //------------- Timer and Turn----------------
                float Starttimer = float.Parse(GS.Inst.FullTableInfoData.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
                int Turn_SeatIndex = int.Parse(GS.Inst.FullTableInfoData.GetField("turn_seat_index").ToString().Trim(Config.Inst.trim_char_arry));
                TP_Player p = GetPlayer_UingSetIndex(Turn_SeatIndex);
                p.MY_Timer_Start(Starttimer, 15f, true);
                bool SideShow = false;
                bool isShow = bool.Parse(GS.Inst.FullTableInfoData.GetField("turn_details").GetField("is_show").ToString().Trim(Config.Inst.trim_char_arry));
                if (GS.Inst.FullTableInfoData.GetField("turn_details").HasField("is_side_details"))
                {
                    SideShow = bool.Parse(GS.Inst.FullTableInfoData.GetField("turn_details").GetField("is_side_details").GetField("flag").ToString().Trim(Config.Inst.trim_char_arry));

                    if (GS.Inst.FullTableInfoData.GetField("side_show_details").HasField("request_index"))
                        TP_SIdeShow_Hendler.Inst.HENDLE_SIDE_SHOW_REQ_REJOIN(GS.Inst.FullTableInfoData);
                }

                if (GS.Inst._userData.MySeatIndex.Equals(Turn_SeatIndex))
                {
                    Txt_Chal_Amount.text = GS.Inst.FullTableInfoData.GetField("turn_details").GetField("chal_value").ToString().Trim(Config.Inst.trim_char_arry);
                    Btn_Chaal.interactable = true;
                    Btn_plus.interactable = true;
                    Btn_Minus.interactable = false;
                    IsMyTurn = true;
                    plus = 0;
                    iTween.MoveTo(ButtonPenal.gameObject, iTween.Hash("position", Start_ButtonPenal_POS.transform.position, "time", 0.3f, "easetype", iTween.EaseType.linear));

                    if (GS.Inst.FullTableInfoData.GetField("turn_details").HasField("is_side_details"))
                    {
                        if (SideShow)
                        {
                            Btn_Show.gameObject.transform.localScale = Vector3.zero;
                            Btn_SideShow.gameObject.transform.localScale = Vector3.one;
                        }
                        else
                        {

                            Btn_Show.gameObject.transform.localScale = Vector3.one;
                            Btn_SideShow.gameObject.transform.localScale = Vector3.zero;
                        }
                    }
                }
                else
                {
                    iTween.MoveTo(ButtonPenal.gameObject, iTween.Hash("position", End_ButtonPenal_POS.transform.position, "time", 0.3f, "easetype", iTween.EaseType.linear));
                    IsMyTurn = false;
                    Btn_Chaal.interactable = false;
                }
                //------------- Timer and Turn----------------

                //------------Player card management ------------------------
                for (int i = 0; i < GS.Inst.FullTableInfoData.GetField("player_info").Count; i++)
                {
                    if (GS.Inst.FullTableInfoData.GetField("player_info")[i].HasField("seat_index"))
                    {
                        bool is_card_see = bool.Parse(GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("is_see").ToString().Trim(Config.Inst.trim_char_arry));
                        string playStatus = GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("status").ToString().Trim(Config.Inst.trim_char_arry);
                        int seatIndex = int.Parse(GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
                        TP_Player p_card = GetPlayer_UingSetIndex(seatIndex);

                        if (GS.Inst._userData.MySeatIndex.Equals(Turn_SeatIndex) && GS.Inst._userData.MySeatIndex.Equals(seatIndex)/* || playStatus == ""*/)
                            p_card.Txt_UserChips.text = GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry);
                        else
                        {
                            if (GS.Inst._userData.MySeatIndex.Equals(seatIndex)/* || playStatus == ""*/)
                                p_card.Txt_UserChips.text = GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry);
                            else
                                p_card.Txt_UserChips.text = GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("last_chal_value").ToString().Trim(Config.Inst.trim_char_arry);

                            //if(float.Parse(GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("last_chal_value").ToString().Trim(Config.Inst.trim_char_arry))>0)
                            // p_card.Txt_UserChips.text = GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("last_chal_value").ToString().Trim(Config.Inst.trim_char_arry);
                            // else
                            //p_card.Txt_UserChips.text = GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry);
                        }
                        string CardStatus = GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("play_status").ToString().Trim(Config.Inst.trim_char_arry);

                        if (CardStatus != "")
                        {
                            if (CardStatus == "Chaal")
                            {
                                p_card.Card_Status.text = "Seen";
                                p_card.Card_See_Eye.SetActive(true);
                            }
                            else if (CardStatus == "Pack")
                            {
                                if (GS.Inst._userData.MySeatIndex.Equals(seatIndex))
                                    Btn_Card_See.interactable = false;

                                p_card.IM_PCKED();
                            }
                            else
                            {
                                if (GS.Inst._userData.MySeatIndex.Equals(seatIndex))
                                {
                                    if (is_card_see)
                                    {
                                        p_card.Card_Status.text = "Seen";
                                        p_card.Card_See_Eye.SetActive(true);
                                    }
                                    else
                                        p_card.Card_Status.text = "See";
                                }
                                else
                                {
                                    if (is_card_see)
                                    {
                                        p_card.Card_Status.text = "Seen";
                                        p_card.Card_See_Eye.SetActive(true);
                                    }
                                    else
                                        p_card.Card_Status.text = "Blind";
                                }
                            }
                        }

                        if (!is_card_see && playStatus!="")
                            p_card.Show_PlayerCard_Back();
                        else
                        {
                            if (playStatus != "")
                            {
                                if (GS.Inst._userData.MySeatIndex.Equals(seatIndex))
                                {
                                    Btn_Card_See.interactable = false;
                                    p_card.RJ_SET_Card_SEE(GS.Inst.FullTableInfoData.GetField("player_info")[i]);
                                }
                                else
                                    p_card.Show_PlayerCard_Back();
                            }
                        }
                        if (GS.Inst._userData.MySeatIndex.Equals(Turn_SeatIndex) && GS.Inst._userData.MySeatIndex.Equals(seatIndex))
                        {

                            if (isShow)
                                Btn_Show.interactable = true;
                            else
                                Btn_Show.interactable = false;
                        }
                    }
                }
                //------------Player card management ------------------------
                break;
            case "RoundWinState":
                //------------Player card management ------------------------
                for (int i = 0; i < GS.Inst.FullTableInfoData.GetField("player_info").Count; i++)
                {
                    if (GS.Inst.FullTableInfoData.GetField("player_info")[i].HasField("seat_index"))
                    {
                        string status = GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("play_status").ToString().Trim(Config.Inst.trim_char_arry);
                        bool is_card_see = bool.Parse(GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("is_see").ToString().Trim(Config.Inst.trim_char_arry));
                        int seatIndex = int.Parse(GS.Inst.FullTableInfoData.GetField("player_info")[i].GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
                        TP_Player p_card = GetPlayer_UingSetIndex(seatIndex);
                        p_card.Card_Status.text = status;
                        if (status == "Pack")
                            p_card.IM_PCKED();

                        if (p_card != null && p_card._SeatStatus != SeatStatus.Empty && p_card.Card_Status.text != "")
                        {
                            if (!is_card_see)
                                p_card.Show_PlayerCard_Back();
                            else
                            {
                                if(status!= "Pack")
                                    p_card.SET_Card_SEE(GS.Inst.FullTableInfoData.GetField("player_info")[i]);
                                else
                                    p_card.Show_PlayerCard_Back();
                            }
                            if (status.Equals("Winner"))
                            {
                                p_card.IM_Win();
                                //Win_Card_Status_Message.transform.GetChild(0).GetComponent<Text>().text = data.GetField("user_info")[i].GetField("card_status").ToString().Trim(Config.Inst.trim_char_arry);
                                //GS.Inst.iTwin_Open(Win_Card_Status_Message);
                            }
                        }

                    }
                }
                break;
            case "RoundEndState":
                break;
            case "":
                if (GS.Inst.PrivateTable)
                {
                    string roomCode = GS.Inst.FullTableInfoData.GetField("private_game_id").ToString().Trim(Config.Inst.trim_char_arry);
                    TP_RoomCodeShare.Inst.SET_ROOM_CODE(roomCode);
                    TP_RoomCodeShare.Inst.OPEN_ROOM_CODE_SCREEN();
                    OBJ_Switch_Table.interactable = false;
                    Btn_PrivateCode_Share.transform.localScale = Vector3.one;
                }
                else
                {
                    OBJ_Switch_Table.interactable = true;
                    Btn_PrivateCode_Share.transform.localScale = Vector3.zero;
                }
                break;
        }
        GS.Inst.Rejoin = false;
    }
    //------------- Rejoin again old table ---------------


    //-------------- Opponent waiting screen-------------------
    public void SET_USER_WAIT_MSG(JSONObject data)
    {
        Stop_Leave_Win_Anim();
        Activity_Status_Message.transform.GetChild(0).GetComponent<Text>().text = data.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry);
        GS.Inst.iTwin_Open(Activity_Status_Message);
    }
    public void CLOSE_USER_WAIT_MSG()
    {
        if (SocketEventReceiver.Inst._WaitMSG_Couratine != null)
        {
            StopCoroutine(SocketEventReceiver.Inst._WaitMSG_Couratine);
            SocketEventReceiver.Inst._WaitMSG_Couratine = null;
        }
        Activity_Status_Message.transform.localScale = Vector3.zero;
    }
    //-------------- Information Sceeen -------------------


    public void BTN_CHAAL()
    {
        //TP_SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_Chal(Is_Increment_Chaal));
    }

    public void BTN_SHOW()
    {
        //TP_SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_Show(false));
    }

    public void BTN_CARD_SEE()
    {
        //TP_SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_ViewCard());
    }

    public void BTN_PACK()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_CardPack());
    }

    public void BTN_SIDE_SHOW()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_SideShowRequest(Is_Increment_Chaal));
    }

    public void BTN_SIDE_REQUEST_ACCEPT()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        TP_SIdeShow_Hendler.Inst.ClOSE_SIDE_SHOW_BOX();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_SideShowRequestAccept());
    }

    public void BTN_SIDE_REQUEST_REJECT()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        TP_SIdeShow_Hendler.Inst.ClOSE_SIDE_SHOW_BOX();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_SideShowRequestReject());
    }

    public void BTN_PLUS_MINUS_CHAL(string p_m)
    {
        TP_SoundManager.Inst.PlaySFX(0);
        if (IsMyTurn)
        {
            if (p_m.Equals("p")&& plus.Equals(0))
            {
                if (float.Parse(Txt_Chal_Amount.text) < Chaal_Limit_Amount)
                {
                    plus = 1;
                    Btn_plus.interactable = false;
                    Btn_Minus.interactable = true;
                    Is_Increment_Chaal = true;
                    Last_ChalAmount = Txt_Chal_Amount.text;
                    Txt_Chal_Amount.text = (float.Parse(Txt_Chal_Amount.text) + float.Parse(Txt_Chal_Amount.text)).ToString();
                }
            }
            else
            {
                plus = 0;
                Btn_plus.interactable = true;
                Btn_Minus.interactable = false;
                Is_Increment_Chaal = false;
                Txt_Chal_Amount.text = (float.Parse(Txt_Chal_Amount.text) - float.Parse(Last_ChalAmount)).ToString();
            }
        }
    }

    public void BTN_SOUND_ON_OFF()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        if (PlayerPrefs.GetInt("sound").Equals(1))
        {
            IMG_SOUND.sprite = Sound_OF_Sprite;
            PlayerPrefs.SetInt("sound", 0);
            TP_SoundManager.Inst.SFXAudio.mute = true;
        }
        else
        {
            IMG_SOUND.sprite = Sound_ON_Sprite;
            PlayerPrefs.SetInt("sound", 1);
            TP_SoundManager.Inst.SFXAudio.mute = false;
        }
    }

    public void BTN_TABLE_INFO()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_TableShortInfo());
    }

    public void BTN_Switch_Table()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(TP_SwitchTable_Popup.Inst.gameObject);
    }

    public void BTN_EXIT()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(TP_Exit.Inst.transform.gameObject);
    }
}
