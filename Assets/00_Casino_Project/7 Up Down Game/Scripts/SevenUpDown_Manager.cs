﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SevenUpDown_Manager : MonoBehaviour
{
    public static SevenUpDown_Manager Inst;
    public GameObject PFB_COINS;
    public GameObject Real_User_Chal_Pos, MyUser_Chal_Pos;
    public int Selected_Bet_Amount;
    public GameObject Selected_Bet_Ring, Selected_Bet_Tick;
    public List<GameObject> Total_Player_OBJ_List;
    public List<GameObject> TargetList;
    public List<GameObject> Junk_Coin_List;
    [SerializeField] Animator Chaal_Anim, My_User_ChalAnim;
    [SerializeField]bool InfoSetup;
    public string GameID;
    public AudioSource Coin_Audio_Source;
    public string Win_Side;
    string two_six_card, eight_twelve_card;
    int win_card_number;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SEVEN_UP_GAME_INFO());
    }

    //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE ------------------------------------
    public void JOIN_OLD_GAME(JSONObject data)
    {
        Debug.Log("SEVEN_UP_GAME_INFO"+data);
        InfoSetup = false; 
        GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
        SevenUpDown_UI_Manager.Inst.TxtGameID.text = GameID;
        SevenUpDown_PlayerManager.Inst.SET_PLAYER_DATA(data);
        SevenUpDown_HistoryManager.Inst.SET_HISTO(data);
        SevenUpDown_UI_Manager.Inst.SET_PLAYED_TOTAL_CHIPS(data.GetField("dis_total_bet_on_cards"));
        SevenUpDown_Manager.Inst.REJOIN_CHIPS(data);      
        SevenUpDown_PlayerManager.Inst.Set_Current_Bets(data);

        START_MAIN_TIMER(data);
        GS.Inst.Rejoin = false;
    }
    
    //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE --------------------------------------


    //------------------------------ Timer Start -----------------------------------------------------------
    public void START_MAIN_TIMER(JSONObject data)
    {
        float time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
        Debug.Log("New Time: "+ time);
        if(time<=20)
            SevenUpDown_Timer.Inst.StartTimerAnim(time, time, true);
        else
            SevenUpDown_Timer.Inst.StartTimerAnim(time, time, false);

        InfoSetup = true;       
    }
    //------------------------------ Timer Start -----------------------------------------------------------


    //------------------------------ BET INFO -----------------------------------------------------------
    public void BET_INFO(JSONObject data)
    {
        if(InfoSetup)
            SevenUpDown_PlayerManager.Inst.PLAYER_CHAAL(data);
    }
    //------------------------------ BET INFO ----------------------------------------------------------- 
    
        
    //------------------------------ SPIN -----------------------------------------------------------
    public IEnumerator START_SPIN(JSONObject data)
    {
        SevenUpDown_Timer.Inst.Drawing_TIME();
      
        string win_card = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
        two_six_card = data.GetField("win_info").GetField("first_card").ToString().Trim(Config.Inst.trim_char_arry);
        eight_twelve_card = data.GetField("win_info").GetField("second_card").ToString().Trim(Config.Inst.trim_char_arry);
        win_card_number = int.Parse(data.GetField("win_info").GetField("win_card_number").ToString().Trim(Config.Inst.trim_char_arry));

        Debug.Log("Dice Values: "+ two_six_card + "  "+ eight_twelve_card + "  "+ win_card_number);

        yield return new WaitForSeconds(0.0f);
        SevenUpDown_DiceAnimation.instance.StartDiceAnim(int.Parse(two_six_card), int.Parse(eight_twelve_card),win_card_number);

    }
    //------------------------------ SPIN -----------------------------------------------------------


    //----------- Card Chal Animation----------------
    public void Real_User_Chaal_Animation(string Chaa_Amount, GameObject TargetOBJ, GameObject Coin_MoveOBJ,int type)
    {
        Vector3 target=getRandomPoint(Coin_MoveOBJ,type);
        GameObject _Coin = Instantiate(PFB_COINS) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            Coin_Audio_Source.PlayOneShot(SevenUpDown_SoundManager.Inst.SFX[6]);
        _Coin.transform.GetComponent<SevenUpDown_PFB_COINS>().SET_COIN(Chaa_Amount);
        _Coin.transform.SetParent(TargetOBJ.transform, false);
        _Coin.transform.position = Real_User_Chal_Pos.transform.position;
        _Coin.transform.GetComponent<SevenUpDown_PFB_COINS>().Move_Anim(target);
        //iTween.ScaleTo(_Coin.gameObject, iTween.Hash("scale", Vector3.one, "speed", 4.5f, "easetype", "linear"));
        //iTween.MoveTo(_Coin.gameObject, iTween.Hash("position", target, "time", 2f, "easetype", iTween.EaseType.easeOutExpo));
        Chaal_Anim.Play("LeftPlayerChaalAnim");
    }
    public void MY_User_Chaal_Animation(string Chaa_Amount, GameObject TargetOBJ,GameObject Coin_MoveOBJ,int type)
    {
        Vector3 target = getRandomPoint(Coin_MoveOBJ,type);
        GameObject _Coin = Instantiate(PFB_COINS) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            Coin_Audio_Source.PlayOneShot(SevenUpDown_SoundManager.Inst.SFX[6]);
        _Coin.transform.GetComponent<SevenUpDown_PFB_COINS>().SET_COIN(Chaa_Amount);
        _Coin.transform.SetParent(TargetOBJ.transform, false);
        _Coin.transform.position = MyUser_Chal_Pos.transform.position;
        _Coin.transform.GetComponent<SevenUpDown_PFB_COINS>().Move_Anim(target);
        //iTween.ScaleTo(_Coin.gameObject, iTween.Hash("scale", Vector3.one, "speed", 4.5f, "easetype", "linear"));
        //iTween.MoveTo(_Coin.gameObject, iTween.Hash("position", target, "time", 2f, "easetype", iTween.EaseType.easeOutExpo));
        // My_User_ChalAnim.Play("LeftPlayerChaalAnim");
        //My_User_ChalAnim.Play("PlayerChalMovementAnim",0); 

    }
    //----------- Card Chal Animation----------------

    //------------ Winning history set -----------------
    public void WINNER_DECLARE(JSONObject data)
    {    

        SevenUpDown_Timer.Inst.FREE_TIME();

        string win_card = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
        Win_Side = win_card;          

        if (win_card.Equals("two_six"))
            SevenUpDown_UI_Manager.Inst.Active_Light_Anim("D");
        else if (win_card.Equals("eight_twelve"))
            SevenUpDown_UI_Manager.Inst.Active_Light_Anim("T");
        else
            SevenUpDown_UI_Manager.Inst.Active_Light_Anim("Tie");      

        SevenUpDown_HistoryManager.Inst.SET_HISTO(data);
    }
    //------------ Winning history set -----------------


    //------------ User Bet Send -----------------
    public void USER_SEND_BET(string Bet)
    {
        if(Bet.Equals("two_six"))
            SocketHandler.Inst.SendData(SocketEventManager.Inst.SEVEN_UP_PLACE_BET(Selected_Bet_Amount.ToString(),"0","0"));
        else if (Bet.Equals("seven"))
            SocketHandler.Inst.SendData(SocketEventManager.Inst.SEVEN_UP_PLACE_BET("0",Selected_Bet_Amount.ToString(), "0"));
        else
            SocketHandler.Inst.SendData(SocketEventManager.Inst.SEVEN_UP_PLACE_BET("0","0", Selected_Bet_Amount.ToString()));
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
                if (Total_Player_OBJ_List[j].transform.GetComponent<SevenUpDown_Player>().ID.Equals(Uid))
                    Total_Player_OBJ_List[j].transform.GetComponent<SevenUpDown_Player>().WinOrLose_Chips = value;
            }
            if (GS.Inst._userData.Id.Equals(Uid))
            {
                SevenUpDown_PlayerManager.Inst.WinOrLose_Chips = value;
                SevenUpDown_PlayerManager.Inst.Played_Chips = true;
            }

            for (int j = 0; j < Total_Player_OBJ_List.Count; j++)
            {
                if (Total_Player_OBJ_List[j].transform.GetComponent<SevenUpDown_Player>().ID.Equals(Uid))
                {
                    fl = true;
                    TargetList.Add(Total_Player_OBJ_List[j]);
                    Total_Player_OBJ_List[j].transform.GetComponent<SevenUpDown_Player>().Played_Chips = true;
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

        SevenUpDown_SoundManager.Inst.PlaySFX(5);
        SevenUpDown_EventSetup.PFB_COIN_DT_WIN_MOVE();
        Invoke(nameof(Kill_Win_Coins), 1.5f);
    }
    void Kill_Win_Coins()
    {
        SevenUpDown_UI_Manager.Inst.RESET_UI_NEXT_ROUDN();
        SevenUpDown_EventSetup.PFB_COIN_DT();
        SevenUpDown_PlayerManager.Inst.Played_Chips = false;
    }


    //------------ Win Coin Move to winner user -----------------

    //------------ Random Box Point Positions -----------------
    public Vector3 getRandomPoint(GameObject TypeObj,int type)
    {
        Vector3 pos = Vector3.zero;
        switch (type)
        {
            case 0:
                pos = GetRandomPointInBox(TypeObj.transform, 1.1f, 1.1f/*, Vector3.up * 2*/);
                break;
            case 1:
                pos = GetRandomPointInBox(TypeObj.transform, 1.1f, 1.1f/*, Vector3.up * 2*/);
                break;
            default:
                break;
        }
        return pos;
    }

    public Vector3 GetRandomPointInBox(Transform box, float hightBounds, float widthBounds/*, Vector3 offset = new Vector3()*/)
    {
        return box.transform.position + new Vector3(Random.Range(hightBounds * -1, hightBounds), Random.Range(widthBounds * -1, widthBounds), 0f)/* + offset*/;
    }
    //------------ Random Box Point Positions -----------------


    //------------------------------ Leave User ---------------------------------------------------------
    public void LEAVE_USER(JSONObject data)
    {
        //if(bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry)))
        SevenUpDown_EventSetup.USER_LEAVE(data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry));
    }
    //------------------------------ Leave User ----------------------------------------------------------- 

    //------------------------------ Seat User ---------------------------------------------------------
    public void SEAT_USER(JSONObject data)
    {
        string id = data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
        bool is_Bot = bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry));
        if (id != GS.Inst._userData.Id)
        {
            bool fl = false;
            //if (bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry)))
            //DT_EventSetup.USER_SEAT(data);
            for (int i = 0; i < SevenUpDown_PlayerManager.Inst.Player_Bot_List.Count; i++)
            {
                if (SevenUpDown_PlayerManager.Inst.Player_Bot_List[i]._Status == SevenUpDown_Player.Status.Null && !is_Bot && !fl )
                {
                    fl = true;
                    SevenUpDown_PlayerManager.Inst.Player_Bot_List[i].SEAT(data);
                }
            }
        }
    }
    //------------------------------ Seat User ----------------------------------------------------------- 

    //------------------------------ Rejoin Chips ---------------------------------------------------------
    public void REJOIN_CHIPS(JSONObject data)
    {
       // SevenUpDown_PlayerManager.Inst._User_TotalBet_Dragon = 0;
       // SevenUpDown_PlayerManager.Inst._User_TotalBet_Tie = 0;
       // SevenUpDown_PlayerManager.Inst._User_TotalBet_Tiger = 0;

        int[] coins = new int[] { 10,50,100,500,10,50,50,1000,10,10,100,100};
        int bet_count;
            int dragon = 0;
            int tie = 0;
            int tiger = 0;

            if(data.GetField("dis_total_bet_on_cards").HasField("two_six"))
                dragon = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("two_six").ToString().Trim(Config.Inst.trim_char_arry));
            if (data.GetField("dis_total_bet_on_cards").HasField("seven"))
                tie = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("seven").ToString().Trim(Config.Inst.trim_char_arry));
            if (data.GetField("dis_total_bet_on_cards").HasField("eight_twelve"))
                tiger = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("eight_twelve").ToString().Trim(Config.Inst.trim_char_arry));

            if (dragon > 0)
            {
                bet_count = 25;
                if (dragon >= 10 && dragon <= 50)
                    bet_count = 5;
                else if (dragon > 100 && dragon <= 200)
                    bet_count = 10;
                else if (dragon > 200 && dragon <= 500)
                    bet_count = 20;
                else
                    bet_count = 25;

                for (int i = 0; i < bet_count; i++)
                {
                    Vector3 target = getRandomPoint(SevenUpDown_PlayerManager.Inst.Dragon_Coin_Local_OBJ,0);
                    GameObject _Coin = Instantiate(PFB_COINS, SevenUpDown_PlayerManager.Inst.DragonBox.transform) as GameObject;
                    _Coin.transform.GetComponent<SevenUpDown_PFB_COINS>().SET_COIN(coins[Random.Range(0, coins.Length)].ToString());
                    _Coin.transform.GetComponent<SevenUpDown_PFB_COINS>().Move_Anim(target);
                }
            }

            if (tie > 0)
            {
                bet_count = 25;
                if (tie >= 10 && tie <= 50)
                    bet_count = 5;
                else if (tie > 100 && tie <= 200)
                    bet_count = 10;
                else if (tie > 200 && tie <= 500)
                    bet_count = 20;
                else
                    bet_count = 25;

                for (int i = 0; i < bet_count; i++)
                {
                    Vector3 target = getRandomPoint(SevenUpDown_PlayerManager.Inst.Tie_Coin_Local_OBJ, 0);
                    GameObject _Coin = Instantiate(PFB_COINS, SevenUpDown_PlayerManager.Inst.TieBox.transform) as GameObject;
                    _Coin.transform.GetComponent<SevenUpDown_PFB_COINS>().SET_COIN(coins[Random.Range(0, coins.Length)].ToString());
                    _Coin.transform.GetComponent<SevenUpDown_PFB_COINS>().Move_Anim(target);
                }
            }

            if (tiger > 0)
            {
                bet_count = 25;
                if (tiger >= 10 && tiger <= 50)
                    bet_count = 5;
                else if (tiger > 100 && tiger <= 200)
                    bet_count = 10;
                else if (tiger > 200 && tiger <= 500)
                    bet_count = 20;
                else
                    bet_count = 25;

                for (int i = 0; i < bet_count; i++)
                {
                    Vector3 target = getRandomPoint(SevenUpDown_PlayerManager.Inst.Tiger_Coin_Local_OBJ, 0);
                    GameObject _Coin = Instantiate(PFB_COINS, SevenUpDown_PlayerManager.Inst.TigerBox.transform) as GameObject;
                    _Coin.transform.GetComponent<SevenUpDown_PFB_COINS>().SET_COIN(coins[Random.Range(0, coins.Length)].ToString());
                    _Coin.transform.GetComponent<SevenUpDown_PFB_COINS>().Move_Anim(target);
                }
            }
    }
    //------------------------------ Rejoin Chips ---------------------------------------------------------
      
}