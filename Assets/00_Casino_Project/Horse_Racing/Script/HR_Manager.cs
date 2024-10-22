using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using TMPro;

public class HR_Manager : MonoBehaviour
{
    public static HR_Manager Inst;
    public GameObject PFB_COINS;
    public GameObject Real_User_Chal_Pos, MyUser_Chal_Pos;
    public int Selected_Bet_Amount;
    public GameObject Selected_Bet_Ring, Selected_Bet_Tick;
    public List<GameObject> Total_Player_OBJ_List;
    public List<GameObject> TargetList;
    public List<GameObject> Junk_Coin_List;
    [SerializeField] Animator Chaal_Anim, My_User_ChalAnim;
    public bool InfoSetup;
    public string GameID;
    public AudioSource Coin_Audio_Source;
    public int Win_Side;
    string DragonCard, TigerCard;

    [Header("Horse Animation")]
    public List<HR_HorseAnimation> All_Hore_Animation;

    [Header("Win Animation")]
    public List<SkeletonDataAsset> Skelton_data_List;
    public SkeletonGraphic Win_Skelton_data_OBJ;
    [SerializeField] TextMeshProUGUI Txt_Win_Amount_X;
    [SerializeField] GameObject Win_Box_End, Win_Box_Start, Win_Box;
    [SerializeField] Image Horse_No_Win_IMG;
    [SerializeField] List<Sprite> Horse_No_List_SP;
    Coroutine _HorseSlow_Corotine;

    public List<float> Horse_Rejoin_PosX_List;
    int dummyTime;
    public JSONObject Speed_data=new JSONObject();
    public List<int> Total_BetBox_Click_List=new List<int>();
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.HORSE_RACING_GAME_INFO());
    }
    private void Update()
    {
    }
    //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE ------------------------------------
    public void JOIN_OLD_GAME(JSONObject data)
    {
        print(data);
        InfoSetup = false;
        GS.Inst.Rejoin = false;
        string GameState = data.GetField("game_state").ToString().Trim(Config.Inst.trim_char_arry);
        GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
        string WinNo = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
        HR_UI_Manager.Inst.Update_JACKPOT(data);
        HR_Chat.Inst.Txt_Chat_Counter.text = data.GetField("online_users_count").ToString().Trim(Config.Inst.trim_char_arry);
        HR_UI_Manager.Inst.TxtGameID.text = GameID;
        HR_PlayerManager.Inst.SET_PLAYER_DATA(data);
        HR_UI_Manager.Inst.X_Ground_Box(false);
        HR_GroundManager.Inst.Win_Line_Idel();
        Speed_data=data.GetField("speed_info");
        float time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
        switch (GameState)
        {
            case "init_game":
                HR_SoundManager.Inst.PlaySFX_Others(2);
                HR_GroundManager.Inst.Anim_for_idel();
                HR_First_HorseLine.Inst.transform.localScale = Vector3.zero;
                HR_UI_Manager.Inst.X_Ground_Box(true);
                break;
            case "game_timer_start":
                if (time <= 5)
                {
                    switch (time)
                    {
                        case 5:
                            SET_HORSE_ON_GROUND(0);
                            break;
                        case 4:
                            SET_HORSE_ON_GROUND(0);
                            break;
                        case 3:
                            HR_SoundManager.Inst.PlaySFX_Others(3);
                            SET_HORSE_ON_GROUND(1);
                            break;
                        case 2:
                            SET_HORSE_ON_GROUND(1);
                            break;
                        case 1:
                            SET_HORSE_ON_GROUND(2);
                            break;
                        case 0:
                            SET_HORSE_ON_GROUND(2);
                            break;
                    }
                    HR_First_HorseLine.Inst.transform.localScale = Vector3.one;
                    START_RACE();
                }
                else
                {
                    HR_SoundManager.Inst.PlaySFX_Others(2);
                    HR_GroundManager.Inst.Anim_for_idel();
                    HR_First_HorseLine.Inst.transform.localScale = Vector3.zero;
                    HR_UI_Manager.Inst.X_Ground_Box(true);
                }
                HR_First_HorseLine.Inst.FIRST_LINE_LABLE();
                START_MAIN_TIMER(data);
                //InfoSetup = true;
                break;
            case "bet_timer_start":
                switch (time)
                {
                    case 5:
                        SET_HORSE_ON_GROUND(0);
                        break;
                    case 4:
                        SET_HORSE_ON_GROUND(0);
                        break;
                    case 3:
                        SET_HORSE_ON_GROUND(1);
                        break;
                    case 2:
                        SET_HORSE_ON_GROUND(2);
                        break;
                    case 1:
                        SET_HORSE_ON_GROUND(3);
                        break;
                    case 0:
                        SET_HORSE_ON_GROUND(3);
                        break;
                }
                START_MAIN_TIMER(data);
                InfoSetup = true;
                break;
            case "start_race":
                switch (time)
                {
                    case 12:
                        SET_HORSE_ON_GROUND(5);
                        break;
                    case 11:
                        SET_HORSE_ON_GROUND(5);
                        break;
                    case 10:
                        SET_HORSE_ON_GROUND(5);
                        break;
                    case 9:
                        SET_HORSE_ON_GROUND(6);
                        break;
                    case 8:
                        SET_HORSE_ON_GROUND(6);
                        break;
                    case 7:
                        SET_HORSE_ON_GROUND(7);
                        break;
                    case 6:
                        SET_HORSE_ON_GROUND(8);
                        break;
                    case 5:
                        SET_HORSE_ON_GROUND(9);
                        break;
                    case 4:
                        SET_HORSE_ON_GROUND(10);
                        break;
                    case 3:
                        SET_HORSE_ON_GROUND(10);
                        break;
                    case 2:
                        SET_HORSE_ON_GROUND(10);
                        break;
                    case 1:
                        //time = 2;
                        SET_HORSE_ON_GROUND(15);
                        break;
                    case 0:
                        //time = 2;
                        SET_HORSE_ON_GROUND(15);
                        break;
                }
                HR_First_HorseLine.Inst.transform.localScale = Vector3.one;
                UPDATE_HORSE_SPEED(data.GetField("speed_info"));
                if(time>1)
                    START_RACE();
                if (bool.Parse(data.GetField("win_info").GetField("is_jackport").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    if (time > 4)
                    {
                        //HR_UI_Manager.Inst.Update_JACKPOT(data.GetField("win_info"));
                        StartCoroutine(HR_UI_Manager.Inst.JackPot_CountBox_ON(int.Parse(data.GetField("win_info").GetField("curr_jackport_amount").ToString().Trim(Config.Inst.trim_char_arry))));
                    }
                }
                break;
            case "winner_declare":
                SET_HORSE_ON_GROUND(15);
                WINNER_DECLARE(data);
                HR_First_HorseLine.Inst.transform.localScale = Vector3.zero;
                if (bool.Parse(data.GetField("win_info").GetField("is_jackport").ToString().Trim(Config.Inst.trim_char_arry))){
                    HR_UI_Manager.Inst.Open_Jackpot_Popup(data.GetField("win_info"));
                }
                break;
            case "winner_declare_done":
                SET_HORSE_ON_GROUND(15);
                WINNER_DECLARE(data);
                if (bool.Parse(data.GetField("win_info").GetField("is_jackport").ToString().Trim(Config.Inst.trim_char_arry))){
                    HR_UI_Manager.Inst.Open_Jackpot_Popup(data.GetField("win_info"));
                }
                break;
            case "finish_state":
                HR_GroundManager.Inst.Anim_for_idel();
                HR_UI_Manager.Inst.X_Ground_Box(true);
                HR_First_HorseLine.Inst.transform.localScale = Vector3.zero;
                break;
        }
        REJOIN_CHIPS(data);
        HR_HistoryManager.Inst.SET_HISTO(data);
        HR_UI_Manager.Inst.SET_TOTAL_X_CHIPS(data.GetField("win_xs"));
        HR_UI_Manager.Inst.SET_USER_PLAYED_TOTAL_CHIPS(data.GetField("card_details"));
        HR_UI_Manager.Inst.SET_OTHER_PLAYED_TOTAL_CHIPS(data.GetField("dis_total_bet_on_cards"));
        InfoSetup = true;
    }
    //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE --------------------------------------


    //------------------------------ Timer Start -----------------------------------------------------------
    public void START_MAIN_TIMER(JSONObject data)
    {
        float time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
        if (time < 15)
            HR_TimerHendler.Inst.StartTimerAnim(time, time, true);
        else
            HR_TimerHendler.Inst.StartTimerAnim(time, time, false);
    }
    public void Start_SlowMotion()
    {
        HR_GroundManager.Inst.Win_Line(true);
        if (_HorseSlow_Corotine != null)
            StopCoroutine(_HorseSlow_Corotine);
        _HorseSlow_Corotine=StartCoroutine(Start_SlowMotion_Line());
    }
    IEnumerator Start_SlowMotion_Line()
    {
        yield return new WaitForSeconds(0.55f);
        HR_SoundManager.Inst.PlaySFX(4);
        Horse_Anim_Slow(true);
        yield return new WaitForSeconds(0.7f);
        Horse_Anim_Slow(false);
    }
    //public void Start_SlowMotion()
    //{
    //    HR_GroundManager.Inst.Win_Line(true);
    //    Invoke(nameof(Start_SlowMotion_Line), 0.5f);
    //}
    //public void Start_SlowMotion_Line()
    //{
    //    HR_SoundManager.Inst.PlaySFX(4);
    //    Horse_Anim_Slow(true);
    //    Invoke(nameof(Speed_Normal), 0.5f);
    //}

    //void Speed_Normal()
    //{
    //    Horse_Anim_Slow(false);
    //}
    //------------------------------ Timer Start -----------------------------------------------------------


    //------------------------------ BET INFO -----------------------------------------------------------
    public void BET_INFO(JSONObject data)
    {
        if (InfoSetup)
        {
            HR_PlayerManager.Inst.PLAYER_CHAAL(data);
            HR_UI_Manager.Inst.Update_JACKPOT(data);
            if (data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry) != GS.Inst._userData.Id)
                HR_UI_Manager.Inst.SET_OTHER_PLAYED_TOTAL_CHIPS(data.GetField("dis_total_bet_on_cards"));
            else
                HR_UI_Manager.Inst.UPDATE_USER_BET(data);
        }
    }
    //------------------------------ BET INFO ----------------------------------------------------------- 


    //------------------------------ SPIN -----------------------------------------------------------
    public void START_RACE()
    {
        HR_SoundManager.Inst.PlaySFX_Others(6);
        for (int i = 0; i < All_Hore_Animation.Count; i++)
        {
            All_Hore_Animation[i].Horse_Run_Start();
        }
        HR_All_Horse_Move.Inst.Run_Move = true;
        HR_GroundManager.Inst.Start_Ground_Anim();
        Invoke(nameof(Speed_Catch), 1.5f);
    }
    void Speed_Catch()
    {
        UPDATE_HORSE_SPEED(Speed_data);
    }
    public void UPDATE_HORSE_SPEED(JSONObject data)
    {
        //int goNum = Random.Range(0, 5);
        //for (int i = 0; i < All_Hore_Animation.Count; i++)
        //{
        //    if(goNum==i)
        //        All_Hore_Animation[i].Move_New_Position(Random.Range(2, 5));
        //    else
        //        All_Hore_Animation[i].Move_New_Position(Random.Range(0, 3));
        //}

        Speed_data = data;
        //New Logic
        for (int i = 0; i < All_Hore_Animation.Count; i++)
        {
             All_Hore_Animation[i].Move_New_Position(int.Parse(data[i].ToString().Trim(Config.Inst.trim_char_arry)));
        }
    }
    public void First_Horse_Seed()
    {
        for (int i = 0; i < All_Hore_Animation.Count; i++)
        {
            if(int.Parse(Speed_data[i].ToString().Trim(Config.Inst.trim_char_arry))>3)
               All_Hore_Animation[i].Move_New_Position(int.Parse(Speed_data[i].ToString().Trim(Config.Inst.trim_char_arry)));
        }
    }

    public void SET_RACE_DATA(JSONObject data)
    {
        string win_card = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
        if (bool.Parse(data.GetField("win_info").GetField("is_jackport").ToString().Trim(Config.Inst.trim_char_arry))){
            StartCoroutine(HR_UI_Manager.Inst.JackPot_CountBox_ON(int.Parse(data.GetField("win_info").GetField("curr_jackport_amount").ToString().Trim(Config.Inst.trim_char_arry))));
        }
    }
    public void RESET_GROUND_DATA()
    {
        HR_Win_Slomotion.Inst.Slow_Action = false;
        for (int i = 0; i < All_Hore_Animation.Count; i++)
        {
            All_Hore_Animation[i].RESET_HORSE();
        }
        HR_GroundManager.Inst.Reset_Ground_Anim();
    }
    public void RESTART_ROUND()
    {
        HR_All_Horse_Move.Inst.Run_Move = false;
        HR_All_Horse_Move.Inst.MySpeed = 0.7f;
        HR_Win_Slomotion.Inst.Slow_Action = false;
        HR_GroundManager.Inst.Anim_for_idel();
    }
    public void SET_HORSE_ON_GROUND(int xpos)
    {
        HR_All_Horse_Move.Inst.SET_REJOIN_POSITION(Horse_Rejoin_PosX_List[xpos]);
        //for (int i = 0; i < All_Hore_Animation.Count; i++)
        //{
        //    All_Hore_Animation[i].SET_REJOIN_POSITION(Horse_Rejoin_PosX_List[xpos]);
        //}
    }
    public void Horse_Anim_Slow(bool action)
    {
        if (action)
        {
            //HR_All_Horse_Move.Inst.Run_Move = false;
            HR_All_Horse_Move.Inst.MySpeed =0.3f;
            for (int i = 0; i < All_Hore_Animation.Count; i++)
            {
                //iTween.Stop(All_Hore_Animation[i].gameObject);
                All_Hore_Animation[i].Horse_Anim.speed = 0.2f;
                All_Hore_Animation[i].Dust_Partical_Anim.speed = 0.2f;
            }
        }
        else
        {
            //HR_All_Horse_Move.Inst.Run_Move = true;
            HR_All_Horse_Move.Inst.MySpeed = 5f;
            for (int i = 0; i < All_Hore_Animation.Count; i++)
            {
                All_Hore_Animation[i].Horse_Anim.speed = 1f;
                All_Hore_Animation[i].Dust_Partical_Anim.speed = 1f;
                //All_Hore_Animation[i].Move_Win_Position(5);
            }
        }
    }
    //------------------------------ SPIN -----------------------------------------------------------


    //----------- Card Chal Animation----------------
    public void Real_User_Chaal_Animation(string Chaa_Amount, GameObject TargetOBJ, GameObject Coin_MoveOBJ, int type)
    {
        Vector3 target = getRandomPoint(Coin_MoveOBJ);
        GameObject _Coin = Instantiate(PFB_COINS, TargetOBJ.transform) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            Coin_Audio_Source.PlayOneShot(HR_SoundManager.Inst.SFX[1]);
        _Coin.transform.GetComponent<HR_PFB_COINS>().SET_COIN(Chaa_Amount);
        _Coin.transform.position = Real_User_Chal_Pos.transform.position;
        _Coin.transform.GetComponent<HR_PFB_COINS>().Move_Anim(target);
        Chaal_Anim.Play("LeftPlayerChaalAnim");
    }
    public void MY_User_Chaal_Animation(string Chaa_Amount, GameObject TargetOBJ, GameObject Coin_MoveOBJ, int type)
    {
        Vector3 target = getRandomPoint(Coin_MoveOBJ);
        GameObject _Coin = Instantiate(PFB_COINS, TargetOBJ.transform) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            Coin_Audio_Source.PlayOneShot(HR_SoundManager.Inst.SFX[1]);
        _Coin.transform.GetComponent<HR_PFB_COINS>().SET_COIN(Chaa_Amount);
        _Coin.transform.position = MyUser_Chal_Pos.transform.position;
        _Coin.transform.GetComponent<HR_PFB_COINS>().Move_Anim(target);
        My_User_ChalAnim.Play("LeftPlayerChaalAnim");
    }
    //----------- Card Chal Animation----------------


    //------------ Winning history set -----------------
    public void WINNER_DECLARE(JSONObject data)
    {
        string[] winCard = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry).Split("|");
        Win_Side = int.Parse(winCard[0]);
        Txt_Win_Amount_X.text = winCard[1]+"X";
        Horse_No_Win_IMG.sprite= Horse_No_List_SP[Win_Side - 1];
        WIN_Move_Anim(Win_Box_End.transform.position);
        Win_Skelton_data_OBJ.skeletonDataAsset = Skelton_data_List[Win_Side-1];
        Win_Skelton_data_OBJ.Initialize(true);
        HR_UI_Manager.Inst.Win_Highlight(Win_Side-1, winCard[1]);
        HR_HistoryManager.Inst.SET_HISTO(data);
    }
    public void WIN_Move_Anim(Vector3 target)
    {
        iTween.MoveTo(Win_Box, iTween.Hash("position", target, "time", 0.35f, "easetype", iTween.EaseType.easeOutExpo));
        Win_Skelton_data_OBJ.timeScale = 1;
        Win_Skelton_data_OBJ.AnimationState.SetAnimation(0, "animation", false);
    }
    public void Close_WinAnim()
    {
        Win_Skelton_data_OBJ.timeScale = 0;
        WIN_Move_Anim(Win_Box_Start.transform.position);
    }
    //------------ Winning history set -----------------


    //------------ User Bet Send -----------------
    public void USER_SEND_BET(string HorseNo)
    {
        if (Check_BetBox_Not3(int.Parse(HorseNo)))
        {
            SocketHandler.Inst.SendData(SocketEventManager.Inst.HORSE_RACING_PLACE_BET(HorseNo, Selected_Bet_Amount.ToString()));        }
        else
        {
            Alert_MSG.Inst.MSG("Players cannot bet more then the maximum betting area");
        }
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
                if (Total_Player_OBJ_List[j].transform.GetComponent<HR_Player>().ID.Equals(Uid) && value>0)
                    Total_Player_OBJ_List[j].transform.GetComponent<HR_Player>().WinOrLose_Chips = value;
            }
            if (GS.Inst._userData.Id.Equals(Uid))
            {
                if (value > 0)
                {
                    HR_PlayerManager.Inst.WinOrLose_Chips = value;
                    HR_PlayerManager.Inst.Played_Chips = true;
                }
            }
            for (int j = 0; j < Total_Player_OBJ_List.Count; j++)
            {
                if (Total_Player_OBJ_List[j].transform.GetComponent<HR_Player>().ID.Equals(Uid))
                {
                    fl = true;
                    if (value > 0)
                    {
                        TargetList.Add(Total_Player_OBJ_List[j]);
                        Total_Player_OBJ_List[j].transform.GetComponent<HR_Player>().Played_Chips = true;
                    }
                }
                if (!fl && GS.Inst._userData.Id != Uid)
                {
                    fl = false;
                    if (value > 0)
                    {
                        HR_PlayerManager.Inst.Other_WinOrLose_Chips = HR_PlayerManager.Inst.Other_WinOrLose_Chips + value;
                        HR_PlayerManager.Inst.Other_Played_Chips = true;
                        TargetList.Add(Real_User_Chal_Pos);
                    }
                }
            }
            if (GS.Inst._userData.Id.Equals(Uid))
            {
                if (value > 0)
                {
                    HR_PlayerManager.Inst.Played_Chips = true;
                    TargetList.Add(MyUser_Chal_Pos);
                }
            }
        }

        if (TargetList.Count <= 0)
        {
            HR_PlayerManager.Inst.Other_Played_Chips = true;
            HR_PlayerManager.Inst.Other_WinOrLose_Chips = Random.Range(200, 5000);
            TargetList.Add(Real_User_Chal_Pos);
        }

        Invoke(nameof(Close_jackpot), 1.5f);
        HR_SoundManager.Inst.PlaySFX(5);
        if (!bool.Parse(data.GetField("is_jackport").ToString().Trim(Config.Inst.trim_char_arry)))
        {
            HR_EventSetup.PFB_COIN_DT_WIN_MOVE();
            Invoke(nameof(Kill_Win_Coins), 3f);
        }
        else
        {
            Invoke(nameof(Late_Coin), 1.5f);
            Invoke(nameof(Kill_Win_Coins), 4.5f);
        }
    }
    void Late_Coin()
    {
        HR_EventSetup.PFB_COIN_DT_WIN_MOVE();
    }
    void Close_jackpot()
    {
        HR_UI_Manager.Inst.Close_Jackpot_Popup();
    }
    void Kill_Win_Coins()
    {
        HR_SoundManager.Inst.PlaySFX_Others(2);
        HR_UI_Manager.Inst.RESET_UI_NEXT_ROUDN();
        HR_EventSetup.PFB_COIN_DT();
        HR_PlayerManager.Inst.Played_Chips = false;
    }
    //------------ Win Coin Move to winner user -----------------


    //------------ Random Box Point Positions -----------------
    public Vector3 getRandomPoint(GameObject TypeObj)
    {
        Vector3 pos = GetRandomPointInBox(TypeObj.transform, .6f, .3f);
        return pos;
    }

    public Vector3 GetRandomPointInBox(Transform box, float hightBounds, float widthBounds)
    {
        return box.transform.position + new Vector3(Random.Range(hightBounds * -1, hightBounds), Random.Range(widthBounds * -1, widthBounds), 0f);
    }
    //------------ Random Box Point Positions -----------------


    //------------------------------ Leave User ---------------------------------------------------------
    public void LEAVE_USER(JSONObject data)
    {
        //if(bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry)))
        HR_EventSetup.USER_LEAVE(data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry));
        HR_Chat.Inst.Txt_Chat_Counter.text = data.GetField("online_users_count").ToString().Trim(Config.Inst.trim_char_arry);
    }
    //------------------------------ Leave User ----------------------------------------------------------- 


    //------------------------------ Seat User ---------------------------------------------------------
    public void SEAT_USER(JSONObject data)
    {
        string id = data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
        bool is_Bot = bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry));
        HR_Chat.Inst.Txt_Chat_Counter.text = data.GetField("online_users_count").ToString().Trim(Config.Inst.trim_char_arry);
        if (id != GS.Inst._userData.Id)
        {
            bool fl = false;
            //if (bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry)))
            //DT_EventSetup.USER_SEAT(data);
            for (int i = 0; i < HR_PlayerManager.Inst.Player_Bot_List.Count; i++)
            {
                if (HR_PlayerManager.Inst.Player_Bot_List[i]._Status == HR_Player.Status.Null && !is_Bot && !fl)
                {
                    fl = true;
                    HR_PlayerManager.Inst.Player_Bot_List[i].SEAT(data,i);
                }
            }
        }
    }
    //------------------------------ Seat User ----------------------------------------------------------- 


    //------------------------------ Rejoin Chips ---------------------------------------------------------
    public void REJOIN_CHIPS(JSONObject data)
    {
        int[] coins = new int[] { 10, 50, 100, 500};
        int bet_count;
        for (int j = 0; j < 6; j++)
        {
            int chip_bet = 0;
            if (data.GetField("dis_total_bet_on_cards").GetField((j+1).ToString()).ToString().Trim(Config.Inst.trim_char_arry)!="0")
                chip_bet=int.Parse(data.GetField("dis_total_bet_on_cards").GetField((j+1).ToString()).ToString().Trim(Config.Inst.trim_char_arry));
            if (chip_bet > 0)
            {
                bet_count = 25;
                if (chip_bet >= 10 && chip_bet <= 50)
                    bet_count = 5;
                else if (chip_bet > 100 && chip_bet <= 200)
                    bet_count = 10;
                else if (chip_bet > 200 && chip_bet <= 500)
                    bet_count = 20;
                else
                    bet_count = 25;

                for (int i = 0; i < bet_count; i++)
                {
                    Vector3 target = getRandomPoint(HR_PlayerManager.Inst.Horse_Coin_Local_OBJ[j]);
                    GameObject _Coin = Instantiate(PFB_COINS, HR_PlayerManager.Inst.HorseBox_List[j].transform) as GameObject;
                    _Coin.transform.GetComponent<HR_PFB_COINS>().SET_COIN(coins[Random.Range(0, coins.Length)].ToString());
                    _Coin.transform.GetComponent<HR_PFB_COINS>().Move_Anim(target);
                }
            }
        }
    }
    //------------------------------ Rejoin Chips ---------------------------------------------------------

    public bool Check_BetBox_Not3(int horsNO)
    {
        bool action=false;
        for (int i = 0; i < Total_BetBox_Click_List.Count; i++)
        {
            if (Total_BetBox_Click_List[i].Equals(horsNO))
                action = true;
        }
        if (!action && Total_BetBox_Click_List.Count < 3)
        {
            Total_BetBox_Click_List.Add(horsNO);
            action = true;
        }
        return action;
    }

    public void Reset_BetBox_Not3()
    {
        Total_BetBox_Click_List.Clear();
    }
}
