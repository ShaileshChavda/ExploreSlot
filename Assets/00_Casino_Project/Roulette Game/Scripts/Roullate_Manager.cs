using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roullate_Manager : MonoBehaviour
{
    public static Roullate_Manager Inst;
    public int Selected_Bet_Amount;
    public GameObject Selected_Bet_Ring, Selected_Bet_Tick;
    public GameObject PFB_COINS;
    public GameObject Real_User_Chal_Pos,MyUser_Chal_Pos;
    public List<GameObject> Total_Player_OBJ_List;
    public List<GameObject> TargetList;
    [SerializeField] Animator Chaal_Anim,My_User_ChalAnim,TableZoomAnim;
    [SerializeField] bool InfoSetup;
    public string GameID;
    public List<Roullate_NumberClicked> All_HightLight_Glow_OBJ;
    public AudioSource Coin_Audio_Source;
    public int Total_Bet_Pos_Count;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
        Total_Bet_Pos_Count = 0;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.ROULETTE_GAME_INFO());
    }

    public void ON_SEND_BET_Box_Click(string BoxNo)
    {
        if (!Roullate_Timer.Inst.Last3Sec)
        {
            Roullate_SoundManager.Inst.PlaySFX(38);
            //if (Total_Bet_Pos_Count < 16)
            //{
            if (BoxNo.Length > 2)
                Roullate_EventSetup.SelectedBET_Roullate_SEND(BoxNo);
            else
                Roullate_EventSetup.SelectedBET_Roullate_SEND(BoxNo + "_b");
            //}
            //else
            //{
            //    if(Roullate_UI_Manager.Inst.Wait_For_NewRound.transform.localScale.x<=0)
            //        Alert_MSG.Inst.MSG("Players cannot bet more then the maximum betting area");
            //}
        }
        else
        {
            Alert_MSG.Inst.MSG("Please wait while game round start!");
        }
    }

    public void RESET_ALL_BOX_GLOW()
    {
        Roullate_EventSetup.RESET_ALL_GLOW();
    }

    //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE ------------------------------------
    public void JOIN_OLD_GAME(JSONObject data)
    {
        InfoSetup = false;
        string GameState = data.GetField("game_state").ToString().Trim(Config.Inst.trim_char_arry);
        float time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
        GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
        string winCARD = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
        Roullate_UI_Manager.Inst.TxtGameID.text = GameID;
        Roullate_PlayerManager.Inst._User_TotalBet = double.Parse(data.GetField("total_bet").ToString().Trim(Config.Inst.trim_char_arry));
        Roullate_UI_Manager.Inst.Txt_TotalBet.text = Roullate_PlayerManager.Inst._User_TotalBet + "/" +data.GetField("disp_user_total_bet").ToString().Trim(Config.Inst.trim_char_arry);
        Roullate_PlayerManager.Inst.SET_PLAYER_DATA(data);
        Roullate_History_Manager.Inst.SET_HISTO(data);
        switch (GameState)
        {
            case "game_timer_start":
                Roullate_Timer.Inst.Txt_Timer_Status.text = "Start Betting";
                START_MAIN_TIMER(data,"M");
                break;
            case "start_spin":
                Roullate_Timer.Inst.Txt_Timer_Status.text = "Free Time";
                START_MAIN_TIMER(data, "N");
                if (time > 17)
                {
                    RouletteSpinWheel.Inst.Play(int.Parse(data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry)));
                }
                else
                {
                    Roullate_UI_Manager.Inst.Wait_Next_Round_POP(true);
                }
                break;
            case "winner_declare_done":
                Roullate_Timer.Inst.Txt_Timer_Status.text = "Open Time";
                Roullate_UI_Manager.Inst.Wait_Next_Round_POP(true);
                break;
        }
        RouletteSpinWheel.Inst.SET_FIRST_BALL(int.Parse(winCARD));
        if (data.GetField("card_details").Count > 0)
            REJOIN_CHIPS(data);
        GS.Inst.Rejoin = false;
    }
    //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE --------------------------------------


    //------------------------------ Rejoin Chips ---------------------------------------------------------
    public void REJOIN_CHIPS(JSONObject data)
    {
        for (int i = 0; i < data.GetField("card_details").Count; i++)
        {
            string side=data.GetField("card_details")[i].keys[0].ToString().Trim(Config.Inst.trim_char_arry);
            string Chaa_Amount = data.GetField("card_details")[i].GetField(side).ToString().Trim(Config.Inst.trim_char_arry);
            GameObject TG;
            if (side.Length > 2)
                TG = GameObject.Find(side);
            else
                TG = GameObject.Find(side + "_b");
            Vector3 target = getRandomPoint(TG);
            GameObject _Coin = Instantiate(PFB_COINS, TG.transform) as GameObject;
            _Coin.transform.GetComponent<Roullate_PFB_Coins>().SET_COIN(Chaa_Amount);
            _Coin.transform.position = MyUser_Chal_Pos.transform.position;
            _Coin.transform.localScale = Vector3.one;
            _Coin.transform.position = target;
        }
    }
    //------------------------------ Rejoin Chips ---------------------------------------------------------



    //------------------------------ Timer Start -----------------------------------------------------------
    public void START_MAIN_TIMER(JSONObject data,string TimerType)
    {
        float time= float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
        Debug.Log("TIMER START >>>>>>>>"+ time);
        if (TimerType.Equals("M"))
        {
            if (time < 40)
                Roullate_Timer.Inst.StartTimerAnim(time, time, true,TimerType);
            else
                Roullate_Timer.Inst.StartTimerAnim(time, time, false,TimerType);
        }
        else
        {
            Roullate_Timer.Inst.StartTimerAnim(time, time, false,TimerType);
        }
        InfoSetup = true;
    }
    //------------------------------ Timer Start -----------------------------------------------------------


    //------------------------------ BET INFO -----------------------------------------------------------
    public void BET_INFO(JSONObject data)
    {
        if (InfoSetup)
            Roullate_PlayerManager.Inst.PLAYER_CHAAL(data);
    }
    //------------------------------ BET INFO ----------------------------------------------------------- 


    //------------------------------ SPIN -----------------------------------------------------------
    public void START_SPIN(JSONObject data)
    {
        RouletteSpinWheel.Inst.Play(int.Parse(data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry)));
        TableZoomAnim.Play("Table_Anim_Start", 0);
        START_MAIN_TIMER(data, "N");
    }
    //------------------------------ SPIN -----------------------------------------------------------


    //----------- Card Chal Animation----------------
    public void Real_User_Chaal_Animation(string Chaa_Amount, string side)
    {
        GameObject TG;
        if (side.Length > 2)
            TG = GameObject.Find(side);
        else
            TG = GameObject.Find(side + "_b");
        Vector3 target = getRandomPoint(TG);
        //Vector3 target = TG.transform.position;
        GameObject _Coin = Instantiate(PFB_COINS, TG.transform) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            Coin_Audio_Source.PlayOneShot(Roullate_SoundManager.Inst.SFX[40]);
        _Coin.transform.GetComponent<Roullate_PFB_Coins>().SET_COIN(Chaa_Amount);
        _Coin.transform.position = Real_User_Chal_Pos.transform.position;
        StartCoroutine(Coin_Hide(_Coin));
        iTween.ScaleTo(_Coin.gameObject, iTween.Hash("scale", Vector3.one, "speed", 4.5f, "easetype", "linear"));
        iTween.MoveTo(_Coin.gameObject, iTween.Hash("position", target, "time", 1.5f, "easetype", iTween.EaseType.easeOutExpo));
        Chaal_Anim.Play("LeftPlayerChaalAnim",0);
    }
    public void MY_User_Chaal_Animation(string Chaa_Amount, string side)
    {
        GameObject TG;
        if (side.Length > 2)
            TG = GameObject.Find(side);
        else
            TG = GameObject.Find(side + "_b");

        Vector3 target = getRandomPoint(TG);
        //Vector3 target = TG.transform.position;
        GameObject _Coin = Instantiate(PFB_COINS, TG.transform) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            Coin_Audio_Source.PlayOneShot(Roullate_SoundManager.Inst.SFX[40]);
        _Coin.transform.GetComponent<Roullate_PFB_Coins>().SET_COIN(Chaa_Amount);
        _Coin.transform.position = MyUser_Chal_Pos.transform.position;
        //StartCoroutine(Coin_Hide(_Coin));
        iTween.ScaleTo(_Coin.gameObject, iTween.Hash("scale", Vector3.one, "speed", 4.5f, "easetype", "linear"));
        iTween.MoveTo(_Coin.gameObject, iTween.Hash("position", target, "time", 1.5f, "easetype", iTween.EaseType.easeOutExpo));
        My_User_ChalAnim.Play("LeftPlayerChaalAnim",0);
    }
    IEnumerator Coin_Hide(GameObject coin)
    {
        yield return new WaitForSeconds(1.1f);
        coin.transform.localScale = Vector3.zero;
    }
    //----------- Card Chal Animation----------------


    //------------ Winning history set -----------------
    public void WINNER_DECLARE(JSONObject data)
    {
        TableZoomAnim.Play("Table_Anim_Stop", 0);
        string win_card = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
        Hendle_HightLight_Glow_Anim(data);
        try
        {
            Roullate_History_Manager.Inst.SET_HISTO(data);
        }
        catch (System.Exception e)
        {
            Debug.Log("History issue :"+e);
        }
    }
    //------------ Winning history set -----------------


    //------------ User Bet Send -----------------
    public void USER_SEND_BET(string Bet)
    {
        SocketHandler.Inst.SendData(SocketEventManager.Inst.ROULETTE_PLACE_BET(Bet));
    }
    //------------ User Bet Send -----------------

    //------------ Win Coin Move to winner user -----------------
    public void WinCoinMove_To_Winner(JSONObject data)
    {
        bool fl = false;
        TargetList.Clear();
        TargetList = new List<GameObject>();
        for (int i = 0; i < data.GetField("users_bets").keys.Count; i++)
        {
            string Uid = data.GetField("users_bets").keys[i].ToString().Trim(Config.Inst.trim_char_arry);
            double value = double.Parse(data.GetField("users_bets").GetField(Uid).ToString().Trim(Config.Inst.trim_char_arry));
            for (int j = 0; j < Total_Player_OBJ_List.Count; j++)
            {
                if (Total_Player_OBJ_List[j].transform.GetComponent<Roullate_Player>().ID.Equals(Uid))
                    Total_Player_OBJ_List[j].transform.GetComponent<Roullate_Player>().WinOrLose_Chips = value;
            }
            if (GS.Inst._userData.Id.Equals(Uid))
            {
                Roullate_PlayerManager.Inst.WinOrLose_Chips = value;
                Roullate_PlayerManager.Inst.Played_Chips = true;
            }

            for (int j = 0; j < Total_Player_OBJ_List.Count; j++)
            {
                if (Total_Player_OBJ_List[j].transform.GetComponent<Roullate_Player>().ID.Equals(Uid))
                {
                    fl = true;
                    TargetList.Add(Total_Player_OBJ_List[j]);
                    Total_Player_OBJ_List[j].transform.GetComponent<Roullate_Player>().Played_Chips = true;
                }
                if (!fl && GS.Inst._userData.Id != Uid)
                {
                    fl = false;
                    TargetList.Add(Real_User_Chal_Pos);
                }

            }
            if (GS.Inst._userData.Id.Equals(Uid))
                TargetList.Add(MyUser_Chal_Pos);

        }
        Roullate_SoundManager.Inst.PlaySFX(39);
        Roullate_EventSetup.PFB_COIN_ROULLATE_WIN_MOVE();
        Invoke("Kill_Win_Coins", 3f);
    }
    void Kill_Win_Coins()
    {
        Roullate_UI_Manager.Inst.Txt_TotalBet.text = "0/0";
        Roullate_EventSetup.PFB_COIN_ROULLATE();
        Roullate_PlayerManager.Inst.Played_Chips = false;
    }

    //------------ Win Coin Move to winner user -----------------


    //------------------------------ Leave User ---------------------------------------------------------
    public void LEAVE_USER(JSONObject data)
    {
        //if (bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry)))
            Roullate_EventSetup.USER_LEAVE(data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry));
    }
    //------------------------------ Leave User ----------------------------------------------------------- 

    //------------------------------ Seat User ---------------------------------------------------------
    public void SEAT_USER(JSONObject data)
    {
        string id= data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
        bool is_Bot = bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry));
        //if (bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry)))
        //Roullate_EventSetup.USER_SEAT(data);
        if (id != GS.Inst._userData.Id)
        {
            bool fl = false;
            for (int i = 0; i < Roullate_PlayerManager.Inst.Player_Bot_List.Count; i++)
            {
                if (Roullate_PlayerManager.Inst.Player_Bot_List[i]._Status == Roullate_Player.Status.Null && !is_Bot && !fl)
                {
                    fl = true;
                    Roullate_PlayerManager.Inst.Player_Bot_List[i].SEAT(data);
                }
            }
        }
    }
    //------------------------------ Seat User ----------------------------------------------------------- 

    //----------------------------- HightLight Glow---------------------------------------------------
    public void Hendle_HightLight_Glow_Anim(JSONObject data)
    {
        for (int i = 0; i < data.GetField("all_winner_slots").Count; i++)
        {
            for (int j = 0; j < All_HightLight_Glow_OBJ.Count; j++)
            {
                if (All_HightLight_Glow_OBJ[j].MyBetSelected.Length > 2)
                {
                    if (All_HightLight_Glow_OBJ[j].MyBetSelected.Equals(data.GetField("all_winner_slots")[i].ToString().Trim(Config.Inst.trim_char_arry)))
                        All_HightLight_Glow_OBJ[j].PLAY_GLOW_ANIM();
                }
                else
                {
                    if (All_HightLight_Glow_OBJ[j].MyBetSelected.Remove(All_HightLight_Glow_OBJ[j].MyBetSelected.Length - 2, 2).Equals(data.GetField("all_winner_slots")[i].ToString().Trim(Config.Inst.trim_char_arry)))
                        All_HightLight_Glow_OBJ[j].PLAY_GLOW_ANIM();
                }
            }
        }
    }
    //----------------------------- HightLight Glow---------------------------------------------------

    public void WIN_NO_SOUND(string no)
    {
        Debug.Log("Sound NU :"+no);
            switch (no)
            {
                case "0":
                    Roullate_SoundManager.Inst.PlaySFX(0);
                    break;
                case "1":
                    Roullate_SoundManager.Inst.PlaySFX(1);
                    break;
                case "2":
                    Roullate_SoundManager.Inst.PlaySFX(2);
                    break;
                case "3":
                    Roullate_SoundManager.Inst.PlaySFX(3);
                    break;
                case "4":
                    Roullate_SoundManager.Inst.PlaySFX(4);
                    break;
                case "5":
                    Roullate_SoundManager.Inst.PlaySFX(5);
                    break;
                case "6":
                    Roullate_SoundManager.Inst.PlaySFX(6);
                    break;
                case "7":
                    Roullate_SoundManager.Inst.PlaySFX(7);
                    break;
                case "8":
                    Roullate_SoundManager.Inst.PlaySFX(8);
                    break;
                case "9":
                    Roullate_SoundManager.Inst.PlaySFX(9);
                    break;
                case "10":
                    Roullate_SoundManager.Inst.PlaySFX(10);
                    break;
                case "11":
                    Roullate_SoundManager.Inst.PlaySFX(11);
                    break;
                case "12":
                    Roullate_SoundManager.Inst.PlaySFX(12);
                    break;
                case "13":
                    Roullate_SoundManager.Inst.PlaySFX(13);
                    break;
                case "14":
                    Roullate_SoundManager.Inst.PlaySFX(14);
                    break;
                case "15":
                    Roullate_SoundManager.Inst.PlaySFX(15);
                    break;
                case "16":
                    Roullate_SoundManager.Inst.PlaySFX(16);
                    break;
                case "17":
                    Roullate_SoundManager.Inst.PlaySFX(17);
                    break;
                case "18":
                    Roullate_SoundManager.Inst.PlaySFX(18);
                    break;
                case "19":
                    Roullate_SoundManager.Inst.PlaySFX(19);
                    break;
                case "20":
                    Roullate_SoundManager.Inst.PlaySFX(20);
                    break;
                case "21":
                    Roullate_SoundManager.Inst.PlaySFX(21);
                    break;
                case "22":
                    Roullate_SoundManager.Inst.PlaySFX(22);
                    break;
                case "23":
                    Roullate_SoundManager.Inst.PlaySFX(23);
                    break;
                case "24":
                    Roullate_SoundManager.Inst.PlaySFX(24);
                    break;
                case "25":
                    Roullate_SoundManager.Inst.PlaySFX(25);
                    break;
                case "26":
                    Roullate_SoundManager.Inst.PlaySFX(26);
                    break;
                case "27":
                    Roullate_SoundManager.Inst.PlaySFX(27);
                    break;
                case "28":
                    Roullate_SoundManager.Inst.PlaySFX(28);
                    break;
                case "29":
                    Roullate_SoundManager.Inst.PlaySFX(29);
                    break;
                case "30":
                    Roullate_SoundManager.Inst.PlaySFX(30);
                    break;
                case "31":
                    Roullate_SoundManager.Inst.PlaySFX(31);
                    break;
                case "32":
                    Roullate_SoundManager.Inst.PlaySFX(32);
                    break;
                case "33":
                    Roullate_SoundManager.Inst.PlaySFX(33);
                    break;
                case "34":
                    Roullate_SoundManager.Inst.PlaySFX(34);
                    break;
                case "35":
                    Roullate_SoundManager.Inst.PlaySFX(35);
                    break;
                case "36":
                    Roullate_SoundManager.Inst.PlaySFX(36);
                break;
            }
    }

    //------------ Random Box Point Positions -----------------
    public Vector3 getRandomPoint(GameObject TypeObj)
    {
        return GetRandomPointInBox(TypeObj.transform, .5f, .5f, Vector3.up * 1);
    }

    public Vector3 GetRandomPointInBox(Transform box, float hightBounds, float widthBounds, Vector3 offset = new Vector3())
    {
        return box.transform.position + new Vector3(Random.Range(hightBounds * -1, hightBounds), Random.Range(widthBounds * -1, widthBounds), 0f) + offset;
    }
    //------------ Random Box Point Positions -----------------
}
