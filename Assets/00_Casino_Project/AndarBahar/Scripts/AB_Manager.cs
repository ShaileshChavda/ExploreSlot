using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AB_Manager : MonoBehaviour
{
    public static AB_Manager Inst;
    public GameObject PFB_COINS;
    public GameObject Real_User_Chal_Pos, MyUser_Chal_Pos;
    public int Selected_Bet_Amount;
    public GameObject Selected_Bet_Ring;
    public Sprite Back_Card;
    public List<GameObject> Total_Player_OBJ_List;
    public List<GameObject> TargetList;
    public List<GameObject> Junk_Coin_List;
    [SerializeField] Animator Chaal_Anim, My_User_ChalAnim;
    [SerializeField] bool InfoSetup;
    public string GameID;
    public AudioSource Coin_Audio_Source;
    public string Win_Side;
    string DragonCard, TigerCard;
    public JSONObject ThrowCardJsonList = new JSONObject();
    public List<Sprite> Card_List;

    //Bet selection animation
    public Animator BetLeft_Right_ANIM;
    public Image BTN_Bet_LeftRight_IMG;
    public Sprite BTN_Bet_Left_SP, BTN_Bet_Right_SP;

    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.ANDAR_BAHAR_GAME_INFO());
    }

    //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE ------------------------------------
    public void JOIN_OLD_GAME(JSONObject data)
    {
        InfoSetup = false;
        string GameState = data.GetField("game_state").ToString().Trim(Config.Inst.trim_char_arry);
        float time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
        GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
        string winCARD = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
        int remaining_deal_cards=int.Parse(data.GetField("remaining_deal_cards").ToString().Trim(Config.Inst.trim_char_arry));
        AB_UI_Manager.Inst.TxtGameID.text = GameID;
        AB_PlayerManager.Inst.SET_PLAYER_DATA(data);
        AB_Full_History_Manager.Inst.SET_DATA(data.GetField("win_probability"));
        AB_UI_Manager.Inst.SET_USER_BET_ONBOARD(data);
        REJOIN_CHIPS(data);
        switch (GameState)
        {
            case "init_game":
                START_MAIN_TIMER(data, true);
                break;
            case "game_timer_start":
                START_MAIN_TIMER(data, false);
                AB_Dealcard.Inst.REJOIN_Spin_JockerCard(data.GetField("joker_cards").ToString().Trim(Config.Inst.trim_char_arry));
                break;
            case "start_spin":
                AB_Dealcard.Inst.REJOIN_Spin_JockerCard(data.GetField("joker_cards").ToString().Trim(Config.Inst.trim_char_arry));
                AB_Timer.Inst.HIDE_TIMER();
                ThrowCardJsonList.Clear();
                ThrowCardJsonList = data.GetField("win_info").GetField("cards");
                if (remaining_deal_cards > 0)
                    AB_Dealcard.Inst.REJOIN_Card_Throw_Anim_Start(remaining_deal_cards);
                else
                    AB_Dealcard.Inst.REJOIN_DEAL_CARD();
                break;
            case "winner_declare_done":
                break;
            case "finish_state":
                break;
        }
        GS.Inst.Rejoin = false;
    }
    //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE --------------------------------------


    //------------------------------ Timer Start -----------------------------------------------------------
    public void START_MAIN_TIMER(JSONObject data,bool isfree)
    {
        float time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));

        if (time < 20)
            AB_Timer.Inst.StartTimerAnim(time, time, true, isfree);
        else
            AB_Timer.Inst.StartTimerAnim(time, time, false, isfree);

        InfoSetup = true;
    }
    //------------------------------ Timer Start -----------------------------------------------------------


    //------------------------------ BET INFO -----------------------------------------------------------
    public void BET_INFO(JSONObject data)
    {
        if (InfoSetup)
            AB_PlayerManager.Inst.PLAYER_CHAAL(data);
    }
    //------------------------------ BET INFO ----------------------------------------------------------- 


    //------------------------------ SPIN -----------------------------------------------------------
    public void START_SPIN(JSONObject data)
    {
        AB_Timer.Inst.HIDE_TIMER();
        ThrowCardJsonList.Clear();
        ThrowCardJsonList = data.GetField("deal_card");
        string win_card = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
        AB_Dealcard.Inst.Card_Throw_Anim_Start();
    }
    //------------------------------ SPIN -----------------------------------------------------------


    //----------- Card Chal Animation----------------
    public void Real_User_Chaal_Animation(string Chaa_Amount, GameObject TargetOBJ, GameObject Coin_MoveOBJ, int type)
    {
        Vector3 target = getRandomPoint(Coin_MoveOBJ, type);
        GameObject _Coin = Instantiate(PFB_COINS) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            Coin_Audio_Source.PlayOneShot(AB_SoundManager.Inst.SFX[2]);
        _Coin.transform.GetComponent<AB_PFB_COINS>().SET_COIN(Chaa_Amount);
        _Coin.transform.SetParent(TargetOBJ.transform, false);
        _Coin.transform.position = Real_User_Chal_Pos.transform.position;
        _Coin.transform.GetComponent<AB_PFB_COINS>().Move_Anim(target);
        //iTween.ScaleTo(_Coin.gameObject, iTween.Hash("scale", Vector3.one, "speed", 4.5f, "easetype", "linear"));
        //iTween.MoveTo(_Coin.gameObject, iTween.Hash("position", target, "time", 2f, "easetype", iTween.EaseType.easeOutExpo));
        Chaal_Anim.Play("LeftPlayerChaalAnim");
    }
    public void MY_User_Chaal_Animation(string Chaa_Amount, GameObject TargetOBJ, GameObject Coin_MoveOBJ, int type)
    {
        Vector3 target = getRandomPoint(Coin_MoveOBJ, type);
        GameObject _Coin = Instantiate(PFB_COINS) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            Coin_Audio_Source.PlayOneShot(AB_SoundManager.Inst.SFX[2]);
        _Coin.transform.GetComponent<AB_PFB_COINS>().SET_COIN(Chaa_Amount);
        _Coin.transform.SetParent(TargetOBJ.transform, false);
        _Coin.transform.position = MyUser_Chal_Pos.transform.position;
        _Coin.transform.GetComponent<AB_PFB_COINS>().Move_Anim(target);
        //iTween.ScaleTo(_Coin.gameObject, iTween.Hash("scale", Vector3.one, "speed", 4.5f, "easetype", "linear"));
        //iTween.MoveTo(_Coin.gameObject, iTween.Hash("position", target, "time", 2f, "easetype", iTween.EaseType.easeOutExpo));
        My_User_ChalAnim.Play("LeftPlayerChaalAnim");
    }
    //----------- Card Chal Animation----------------

    //------------ Winning history set -----------------
    public void WINNER_DECLARE(JSONObject data)
    {
        string win_card = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
        Win_Side = win_card;
        AB_UI_Manager.Inst.Active_Glow_Anim(data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry), data.GetField("win_box").ToString().Trim(Config.Inst.trim_char_arry));
        AB_Full_History_Manager.Inst.SET_DATA(data.GetField("win_probability"));
    }
    //------------ Winning history set -----------------


    //------------ User Bet Send -----------------
    public void USER_SEND_BET(string BetBox)
    {
        SocketHandler.Inst.SendData(SocketEventManager.Inst.ANDAR_BAHAR_PLACE_BET(BetBox,Selected_Bet_Amount));
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
                if (Total_Player_OBJ_List[j].transform.GetComponent<AB_Player>().ID.Equals(Uid))
                    Total_Player_OBJ_List[j].transform.GetComponent<AB_Player>().WinOrLose_Chips = value;
            }
            if (GS.Inst._userData.Id.Equals(Uid))
            {
                AB_PlayerManager.Inst.WinOrLose_Chips = value;
                AB_PlayerManager.Inst.Played_Chips = true;
            }

            for (int j = 0; j < Total_Player_OBJ_List.Count; j++)
            {
                if (Total_Player_OBJ_List[j].transform.GetComponent<AB_Player>().ID.Equals(Uid))
                {
                    fl = true;
                    TargetList.Add(Total_Player_OBJ_List[j]);
                    Total_Player_OBJ_List[j].transform.GetComponent<AB_Player>().Played_Chips = true;
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

        AB_SoundManager.Inst.PlaySFX(3);
        AB_EventSetup.PFB_COIN_DT_WIN_MOVE();
        Invoke("Kill_Win_Coins", 1.5f);
        Invoke("Stop_Glow", 3.5f);
    }
    void Kill_Win_Coins()
    {
        AB_EventSetup.PFB_COIN_DT();
        AB_PlayerManager.Inst.Played_Chips = false;
    }
    void Stop_Glow()
    {
        AB_UI_Manager.Inst.RESET_UI_NEXT_ROUDN();
    }

    //------------ Win Coin Move to winner user -----------------

    //------------ Random Box Point Positions -----------------
    public Vector3 getRandomPoint(GameObject TypeObj, int type)
    {
        Vector3 pos = Vector3.zero;
        switch (type)
        {
            case 0:
                pos = GetRandomPointInBox(TypeObj.transform, 1f, 0.03f/*, Vector3.up * 2*/);
                break;
            case 1:
                pos = GetRandomPointInBox(TypeObj.transform, 1f, 0.03f/*, Vector3.up * 2*/);
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
        AB_EventSetup.USER_LEAVE(data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry));
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
            for (int i = 0; i < AB_PlayerManager.Inst.Player_Bot_List.Count; i++)
            {
                if (AB_PlayerManager.Inst.Player_Bot_List[i]._Status == AB_Player.Status.Null && !is_Bot && !fl)
                {
                    fl = true;
                    AB_PlayerManager.Inst.Player_Bot_List[i].SEAT(data);
                }
            }
        }
    }
    //------------------------------ Seat User ----------------------------------------------------------- 

    //------------------------------ Rejoin Chips ---------------------------------------------------------
    public void REJOIN_CHIPS(JSONObject data)
    {
        int andar = 0, bahar = 0, _1_5 = 0, _6_10 = 0, _11_15=0, _16_25=0, _26_30=0, _31_35 = 0, _36_40 = 0, _41_48 = 0;

        if (data.GetField("dis_total_bet_on_cards").HasField("andar"))
            andar = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("andar").ToString().Trim(Config.Inst.trim_char_arry));
        if (data.GetField("dis_total_bet_on_cards").HasField("bahar"))
            bahar = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("bahar").ToString().Trim(Config.Inst.trim_char_arry));
        if (data.GetField("dis_total_bet_on_cards").HasField("1-5"))
            _1_5 = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("1-5").ToString().Trim(Config.Inst.trim_char_arry));
        if (data.GetField("dis_total_bet_on_cards").HasField("6-10"))
            _6_10 = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("6-10").ToString().Trim(Config.Inst.trim_char_arry));
        if (data.GetField("dis_total_bet_on_cards").HasField("11-15"))
            _11_15 = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("11-15").ToString().Trim(Config.Inst.trim_char_arry));
        if (data.GetField("dis_total_bet_on_cards").HasField("16-25"))
            _16_25 = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("16-25").ToString().Trim(Config.Inst.trim_char_arry));
        if (data.GetField("dis_total_bet_on_cards").HasField("26-30"))
            _26_30 = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("26-30").ToString().Trim(Config.Inst.trim_char_arry));
        if (data.GetField("dis_total_bet_on_cards").HasField("31-35"))
            _31_35 = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("31-35").ToString().Trim(Config.Inst.trim_char_arry));
        if (data.GetField("dis_total_bet_on_cards").HasField("36-40"))
            _36_40 = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("36-40").ToString().Trim(Config.Inst.trim_char_arry));
        if (data.GetField("dis_total_bet_on_cards").HasField("41-48"))
            _41_48 = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("41-48").ToString().Trim(Config.Inst.trim_char_arry));

        if (andar > 0)
        {
            int bet_count = 25;
            if (andar >= 10 && andar <= 50)
                bet_count = 5;
            else if (andar > 100 && andar <= 200)
                bet_count = 10;
            else if (andar > 200 && andar <= 500)
                bet_count = 20;
            else
                bet_count = 25;

            for (int i = 0; i < bet_count; i++)
            {
                Vector3 target = getRandomPoint(AB_PlayerManager.Inst.Andar_Coin_Local_OBJ, 0);
                GameObject _Coin = Instantiate(PFB_COINS, AB_PlayerManager.Inst.AndarBox.transform) as GameObject;
                Coin_Move(_Coin, target);
            }
        }
        if (bahar > 0)
        {
            int bet_count = 25;
            if (bahar >= 10 && bahar <= 50)
                bet_count = 5;
            else if (bahar > 100 && bahar <= 200)
                bet_count = 10;
            else if (bahar > 200 && bahar <= 500)
                bet_count = 20;
            else
                bet_count = 25;

            for (int i = 0; i < bet_count; i++)
            {
                Vector3 target = getRandomPoint(AB_PlayerManager.Inst.Bahar_Coin_Local_OBJ, 0);
                GameObject _Coin = Instantiate(PFB_COINS, AB_PlayerManager.Inst.BaharBox.transform) as GameObject;
                Coin_Move(_Coin, target);
            }
        }
        if (_1_5 > 0)
        {
            int bet_count = 25;
            if (_1_5 >= 10 && _1_5 <= 50)
                bet_count = 5;
            else if (_1_5 > 100 && _1_5 <= 200)
                bet_count = 10;
            else if (_1_5 > 200 && _1_5 <= 500)
                bet_count = 20;
            else
                bet_count = 25;

            for (int i = 0; i < bet_count; i++)
            {
                Vector3 target = getRandomPoint(AB_PlayerManager.Inst.Box1_Coin_Local_OBJ, 0);
                GameObject _Coin = Instantiate(PFB_COINS, AB_PlayerManager.Inst.Box1.transform) as GameObject;
                Coin_Move(_Coin, target);
            }
        }
        if (_6_10 > 0)
        {
            int bet_count = 25;
            if (_6_10 >= 10 && _6_10 <= 50)
                bet_count = 5;
            else if (_6_10 > 100 && _6_10 <= 200)
                bet_count = 10;
            else if (_6_10 > 200 && _6_10 <= 500)
                bet_count = 20;
            else
                bet_count = 25;

            for (int i = 0; i < bet_count; i++)
            {
                Vector3 target = getRandomPoint(AB_PlayerManager.Inst.Box2_Coin_Local_OBJ, 0);
                GameObject _Coin = Instantiate(PFB_COINS, AB_PlayerManager.Inst.Box2.transform) as GameObject;
                Coin_Move(_Coin, target);
            }
        }
        if (_11_15 > 0)
        {
            int bet_count = 25;
            if (_11_15 >= 10 && _11_15 <= 50)
                bet_count = 5;
            else if (_11_15 > 100 && _11_15 <= 200)
                bet_count = 10;
            else if (_11_15 > 200 && _11_15 <= 500)
                bet_count = 20;
            else
                bet_count = 25;

            for (int i = 0; i < bet_count; i++)
            {
                Vector3 target = getRandomPoint(AB_PlayerManager.Inst.Box3_Coin_Local_OBJ, 0);
                GameObject _Coin = Instantiate(PFB_COINS, AB_PlayerManager.Inst.Box3.transform) as GameObject;
                Coin_Move(_Coin, target);
            }
        }
        if (_16_25 > 0)
        {
            int bet_count = 25;
            if (_16_25 >= 10 && _16_25 <= 50)
                bet_count = 5;
            else if (_16_25 > 100 && _16_25 <= 200)
                bet_count = 10;
            else if (_16_25 > 200 && _16_25 <= 500)
                bet_count = 20;
            else
                bet_count = 25;

            for (int i = 0; i < bet_count; i++)
            {
                Vector3 target = getRandomPoint(AB_PlayerManager.Inst.Box4_Coin_Local_OBJ, 0);
                GameObject _Coin = Instantiate(PFB_COINS, AB_PlayerManager.Inst.Box4.transform) as GameObject;
                Coin_Move(_Coin, target);
            }
        }
        if (_26_30 > 0)
        {
            int bet_count = 25;
            if (_26_30 >= 10 && _26_30 <= 50)
                bet_count = 5;
            else if (_26_30 > 100 && _26_30 <= 200)
                bet_count = 10;
            else if (_26_30 > 200 && _26_30 <= 500)
                bet_count = 20;
            else
                bet_count = 25;

            for (int i = 0; i < bet_count; i++)
            {
                Vector3 target = getRandomPoint(AB_PlayerManager.Inst.Box5_Coin_Local_OBJ, 0);
                GameObject _Coin = Instantiate(PFB_COINS, AB_PlayerManager.Inst.Box5.transform) as GameObject;
                Coin_Move(_Coin, target);
            }
        }
        if (_31_35 > 0)
        {
            int bet_count = 25;
            if (_31_35 >= 10 && _31_35 <= 50)
                bet_count = 5;
            else if (_31_35 > 100 && _31_35 <= 200)
                bet_count = 10;
            else if (_31_35 > 200 && _31_35 <= 500)
                bet_count = 20;
            else
                bet_count = 25;

            for (int i = 0; i < bet_count; i++)
            {
                Vector3 target = getRandomPoint(AB_PlayerManager.Inst.Box6_Coin_Local_OBJ, 0);
                GameObject _Coin = Instantiate(PFB_COINS, AB_PlayerManager.Inst.Box6.transform) as GameObject;
                Coin_Move(_Coin, target);
            }
        }
        if (_36_40 > 0)
        {
            int bet_count = 25;
            if (_36_40 >= 10 && _36_40 <= 50)
                bet_count = 5;
            else if (_36_40 > 100 && _36_40 <= 200)
                bet_count = 10;
            else if (_36_40 > 200 && _36_40 <= 500)
                bet_count = 20;
            else
                bet_count = 25;

            for (int i = 0; i < bet_count; i++)
            {
                Vector3 target = getRandomPoint(AB_PlayerManager.Inst.Box7_Coin_Local_OBJ, 0);
                GameObject _Coin = Instantiate(PFB_COINS, AB_PlayerManager.Inst.Box7.transform) as GameObject;
                Coin_Move(_Coin, target);
            }
        }
        if (_41_48 > 0)
        {
            int bet_count = 25;
            if (_41_48 >= 10 && _41_48 <= 50)
                bet_count = 5;
            else if (_41_48 > 100 && _41_48 <= 200)
                bet_count = 10;
            else if (_41_48 > 200 && _41_48 <= 500)
                bet_count = 20;
            else
                bet_count = 25;

            for (int i = 0; i < bet_count; i++)
            {
                Vector3 target = getRandomPoint(AB_PlayerManager.Inst.Box8_Coin_Local_OBJ, 0);
                GameObject _Coin = Instantiate(PFB_COINS, AB_PlayerManager.Inst.Box8.transform) as GameObject;
                Coin_Move(_Coin, target);
            }
        }
    }
    void Coin_Move(GameObject _Coin,Vector3 target)
    {
        int[] coins = new int[] { 10, 50, 100, 500, 10, 50, 50, 1000, 10, 10, 100, 100 };
        _Coin.transform.GetComponent<AB_PFB_COINS>().SET_COIN(coins[Random.Range(0, coins.Length)].ToString());
        _Coin.transform.GetComponent<AB_PFB_COINS>().Move_Anim(target);
    }
    //------------------------------ Rejoin Chips ---------------------------------------------------------


    public void BTN_BET_LEFT_RIGHT_ANIM()
    {
        if (BTN_Bet_LeftRight_IMG.sprite.name.Equals("left"))
        {
            BTN_Bet_LeftRight_IMG.sprite = BTN_Bet_Right_SP;
            BetLeft_Right_ANIM.Play("BetAnim_Left", 0);
        }
        else
        {
            BTN_Bet_LeftRight_IMG.sprite = BTN_Bet_Left_SP;
            BetLeft_Right_ANIM.Play("BetAnim_Rigjht", 0);
        }
    }
}
