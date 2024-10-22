using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DT_Manager : MonoBehaviour
{
    public static DT_Manager Inst;
    public GameObject PFB_COINS;
    public GameObject Real_User_Chal_Pos, MyUser_Chal_Pos;
    public int Selected_Bet_Amount;
    public GameObject Selected_Bet_Ring, Selected_Bet_Tick;
    public GameObject Left_Card;
    public GameObject Right_Card;
    public Sprite Back_Card;
    public List<GameObject> Total_Player_OBJ_List;
    public List<GameObject> TargetList;
    public List<GameObject> Junk_Coin_List;
    [SerializeField] Animator Chaal_Anim, My_User_ChalAnim;
    [SerializeField]bool InfoSetup;
    public string GameID;
    public AudioSource Coin_Audio_Source;
    public string Win_Side;
    string DragonCard, TigerCard;

    public RectTransform prefabHistCoin;
    public Sprite[] histCoinSprite;
    public RectTransform histCoinMoveTarget;
    public RectTransform DragonHistCoinPos, TigerHistCoinPos, TieHistCoinPos;
    public CanvasGroup cavasGroup;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
       SocketHandler.Inst.SendData(SocketEventManager.Inst.DRAGON_TIGER_GAME_INFO());
    }

    //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE ------------------------------------
    public void JOIN_OLD_GAME(JSONObject data)
    {
        InfoSetup = false;
        DT_UI_Manager.Inst.Active_Fir_Anim(true);
        string GameState = data.GetField("game_state").ToString().Trim(Config.Inst.trim_char_arry);
        GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
        string winCARD = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
        DT_UI_Manager.Inst.TxtGameID.text = GameID;
        DT_PlayerManager.Inst.SET_PLAYER_DATA(data);
        DT_HistoryManager.Inst.SET_HISTO(data);
        DT_UI_Manager.Inst.SET_PLAYED_TOTAL_CHIPS(data.GetField("dis_total_bet_on_cards"));
        DT_Manager.Inst.REJOIN_CHIPS(data);      

        DT_PlayerManager.Inst.Set_Current_Bets(data);
        START_MAIN_TIMER(data);
        GS.Inst.Rejoin = false;
    }
    //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE --------------------------------------


    //------------------------------ Timer Start -----------------------------------------------------------
    public void START_MAIN_TIMER(JSONObject data)
    {
        float time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
        if(time<20)
            DT_Timer.Inst.StartTimerAnim(time, time, true);
        else
            DT_Timer.Inst.StartTimerAnim(time, time, false);

        InfoSetup = true;
        DT_UI_Manager.Inst.Active_Fir_Anim(true);
    }
    //------------------------------ Timer Start -----------------------------------------------------------


    //------------------------------ BET INFO -----------------------------------------------------------
    public void BET_INFO(JSONObject data)
    {
        if(InfoSetup)
            DT_PlayerManager.Inst.PLAYER_CHAAL(data);
    }
    //------------------------------ BET INFO ----------------------------------------------------------- 
    
        
    //------------------------------ SPIN -----------------------------------------------------------
    public IEnumerator START_SPIN(JSONObject data)
    {
        DT_SoundManager.Inst.PlaySFX(1);
        DT_UI_Manager.Inst.BetttingBG.SetActive(false);
        //DT_Timer.Inst.FREE_TIME();
        DT_UI_Manager.Inst.Active_Fir_Anim(false);

        string win_card = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
        DragonCard = data.GetField("win_info").GetField("first_card").ToString().Trim(Config.Inst.trim_char_arry);
        TigerCard = data.GetField("win_info").GetField("second_card").ToString().Trim(Config.Inst.trim_char_arry);

        iTween.ScaleTo(Left_Card, new Vector3(1.3f,1.3f,1.3f),0.2f);
        yield return new WaitForSeconds(0.2f);
        iTween.RotateBy(Left_Card, iTween.Hash("y", 0.5f, "time", 0.4f, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(0.4f);
        iTween.ScaleTo(Left_Card, new Vector3(1f, 1f, 1f), 0.2f);

        for (int i = 0; i < DT_PlayerManager.Inst.Card_List.Count; i++)
        {
            if (DT_PlayerManager.Inst.Card_List[i].name.Equals(DragonCard))
                Left_Card.GetComponent<Image>().sprite = DT_PlayerManager.Inst.Card_List[i];
        }

        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(Right_Card, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);
        yield return new WaitForSeconds(0.2f);
        iTween.RotateBy(Right_Card, iTween.Hash("y", 0.5f, "time", 0.4f, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(0.4f);
        iTween.ScaleTo(Right_Card, new Vector3(1f, 1f, 1f), 0.2f);

        for (int i = 0; i < DT_PlayerManager.Inst.Card_List.Count; i++)
        {
            if (DT_PlayerManager.Inst.Card_List[i].name.Equals(TigerCard))
                Right_Card.GetComponent<Image>().sprite = DT_PlayerManager.Inst.Card_List[i];
        }
        //yield return new WaitForSeconds(0.4f);
        DT_UI_Manager.Inst.Active_Light_Anim(win_card);
        /*if (win_card.Equals("dragon"))
            DT_UI_Manager.Inst.Active_Light_Anim("D");
        else if (win_card.Equals("tiger"))
            DT_UI_Manager.Inst.Active_Light_Anim("T");
        else
            DT_UI_Manager.Inst.Active_Light_Anim("Tie");*/
    }
    //------------------------------ SPIN -----------------------------------------------------------


    //----------- Card Chal Animation----------------
    public void Real_User_Chaal_Animation(string Chaa_Amount, GameObject TargetOBJ, GameObject Coin_MoveOBJ,int type)
    {
        Vector3 target=getRandomPoint(Coin_MoveOBJ,type);
        GameObject _Coin = Instantiate(PFB_COINS) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            Coin_Audio_Source.PlayOneShot(DT_SoundManager.Inst.SFX[6]);
        _Coin.transform.GetComponent<DT_PFB_COINS>().SET_COIN(Chaa_Amount);
        _Coin.transform.SetParent(TargetOBJ.transform, false);
        _Coin.transform.position = Real_User_Chal_Pos.transform.position;
        _Coin.transform.GetComponent<DT_PFB_COINS>().Move_Anim(target);
        //iTween.ScaleTo(_Coin.gameObject, iTween.Hash("scale", Vector3.one, "speed", 4.5f, "easetype", "linear"));
        //iTween.MoveTo(_Coin.gameObject, iTween.Hash("position", target, "time", 2f, "easetype", iTween.EaseType.easeOutExpo));
        Chaal_Anim.Play("LeftPlayerChaalAnim");
    }
    public void MY_User_Chaal_Animation(string Chaa_Amount, GameObject TargetOBJ,GameObject Coin_MoveOBJ,int type)
    {
        Vector3 target = getRandomPoint(Coin_MoveOBJ,type);
        GameObject _Coin = Instantiate(PFB_COINS) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            Coin_Audio_Source.PlayOneShot(DT_SoundManager.Inst.SFX[6]);
        _Coin.transform.GetComponent<DT_PFB_COINS>().SET_COIN(Chaa_Amount);
        _Coin.transform.SetParent(TargetOBJ.transform, false);
        _Coin.transform.position = MyUser_Chal_Pos.transform.position;
        _Coin.transform.GetComponent<DT_PFB_COINS>().Move_Anim(target);
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

        /*if (win_card.Equals("dragon"))
            DT_UI_Manager.Inst.Active_Light_Anim("D");
        else if (win_card.Equals("tiger"))
            DT_UI_Manager.Inst.Active_Light_Anim("T");
        else
            DT_UI_Manager.Inst.Active_Light_Anim("Tie");*/

        cavasGroup.alpha = 0;
        DT_UI_Manager.Inst.RoarAnimtion(win_card);

        /*if (win_card.Equals("dragon"))
            DT_UI_Manager.Inst.RoarAnimtion("D");
        else if (win_card.Equals("tiger"))
            DT_UI_Manager.Inst.RoarAnimtion("T");
        else
            DT_UI_Manager.Inst.RoarAnimtion("Tie");*/

        StartCoroutine(SetHistoryCard(data));      
    }
    IEnumerator SetHistoryCard(JSONObject data)
    {
        yield return new WaitForSeconds(2);       
        DT_HistoryManager.Inst.SET_HISTO(data);
        StartCoroutine(MoveHistoryCoins());
    }

   /* void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("GetMouseButtonDown");
            Win_Side = "tiger";
            StartCoroutine(MoveHistoryCoins());
        }
    }*/

    public IEnumerator MoveHistoryCoins()
    {
        cavasGroup.alpha = 1;
        if (Win_Side.Equals("dragon")) 
        {
            Debug.Log("MoveHistoryCoins dragon");
            prefabHistCoin.GetComponent<Image>().sprite = histCoinSprite[0];
            for (int i = 0; i < 15; i++)
            {
                RectTransform histCoin = Instantiate(prefabHistCoin,DragonHistCoinPos);
                histCoin.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 0.3f);
                histCoin.DOMove(histCoinMoveTarget.position, 0.3f);
                histCoin.SetParent(histCoinMoveTarget);
                yield return new WaitForSeconds(0.1f);
                histCoin.DOScale(new Vector3(0, 0, 0), 0.3f).SetDelay(0.4f).OnComplete(() => Destroy(histCoin.gameObject));
            }
        }
        else if (Win_Side.Equals("tiger"))
        {
            prefabHistCoin.GetComponent<Image>().sprite = histCoinSprite[1];
            for (int i = 0; i < 15; i++)
            {
                RectTransform histCoin = Instantiate(prefabHistCoin, TigerHistCoinPos);
                histCoin.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 0.3f);
                histCoin.DOMove(histCoinMoveTarget.position, 0.3f);
                histCoin.SetParent(histCoinMoveTarget);
                yield return new WaitForSeconds(0.1f);
                histCoin.DOScale(new Vector3(0, 0, 0), 0.3f).SetDelay(0.4f).OnComplete(() => Destroy(histCoin.gameObject)); 
            }
        }
       else
        {
            prefabHistCoin.GetComponent<Image>().sprite = histCoinSprite[2];
            for (int i = 0; i < 15; i++)
            {
                RectTransform histCoin = Instantiate(prefabHistCoin, TieHistCoinPos);
                histCoin.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 0.3f);
                histCoin.DOMove(histCoinMoveTarget.position, 0.3f);
                histCoin.SetParent(histCoinMoveTarget);
                yield return new WaitForSeconds(0.1f);
                histCoin.DOScale(new Vector3(0, 0, 0), 0.3f).SetDelay(0.4f).OnComplete(() => Destroy(histCoin.gameObject));
            }
        }
       
        /*for (int i = 0; i < histCoinMoveTarget.transform.childCount; i++)
        {
            Destroy(histCoinMoveTarget.transform.GetChild(i).gameObject);
        }*/
    }
    //------------ Winning history set -----------------


    //------------ User Bet Send -----------------
    public void USER_SEND_BET(string Bet)
    {
        if(Bet.Equals("dragon"))
            SocketHandler.Inst.SendData(SocketEventManager.Inst.DRAGON_TIGER_PLACE_BET(Selected_Bet_Amount.ToString(),"0","0"));
        else if (Bet.Equals("tie"))
            SocketHandler.Inst.SendData(SocketEventManager.Inst.DRAGON_TIGER_PLACE_BET("0",Selected_Bet_Amount.ToString(), "0"));
        else
            SocketHandler.Inst.SendData(SocketEventManager.Inst.DRAGON_TIGER_PLACE_BET("0","0", Selected_Bet_Amount.ToString()));
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
                if (Total_Player_OBJ_List[j].transform.GetComponent<DT_Player>().ID.Equals(Uid))
                    Total_Player_OBJ_List[j].transform.GetComponent<DT_Player>().WinOrLose_Chips = value;
            }
            if (GS.Inst._userData.Id.Equals(Uid))
            {
                DT_PlayerManager.Inst.WinOrLose_Chips = value;
                DT_PlayerManager.Inst.Played_Chips = true;
            }

            for (int j = 0; j < Total_Player_OBJ_List.Count; j++)
            {
                if (Total_Player_OBJ_List[j].transform.GetComponent<DT_Player>().ID.Equals(Uid))
                {
                    fl = true;
                    TargetList.Add(Total_Player_OBJ_List[j]);
                    Total_Player_OBJ_List[j].transform.GetComponent<DT_Player>().Played_Chips = true;
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

        DT_SoundManager.Inst.PlaySFX(5);
        DT_EventSetup.PFB_COIN_DT_WIN_MOVE();
        Invoke("Kill_Win_Coins", 1.5f);
    }
    void Kill_Win_Coins()
    {
        DT_UI_Manager.Inst.RESET_UI_NEXT_ROUDN();
        DT_EventSetup.PFB_COIN_DT();
        DT_PlayerManager.Inst.Played_Chips = false;
    }


    //------------ Win Coin Move to winner user -----------------

    //------------ Random Box Point Positions -----------------
    public Vector3 getRandomPoint(GameObject TypeObj,int type)
    {
        Vector3 pos = Vector3.zero;
        switch (type)
        {
            case 0:
                pos = GetRandomPointInBox(TypeObj.transform, 0.9f, 0.9f/*, Vector3.up * 2*/);
                break;
            case 1:
                pos = GetRandomPointInBox(TypeObj.transform, 0.9f, 0.9f/*, Vector3.up * 2*/);
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
         DT_EventSetup.USER_LEAVE(data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry));
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
            for (int i = 0; i < DT_PlayerManager.Inst.Player_Bot_List.Count; i++)
            {
                if (DT_PlayerManager.Inst.Player_Bot_List[i]._Status == DT_Player.Status.Null && !is_Bot && !fl )
                {
                    fl = true;
                    DT_PlayerManager.Inst.Player_Bot_List[i].SEAT(data);
                }
            }
        }
    }
    //------------------------------ Seat User ----------------------------------------------------------- 

    //------------------------------ Rejoin Chips ---------------------------------------------------------
    public void REJOIN_CHIPS(JSONObject data)
    {
        int[] coins = new int[] { 10,50,100,500,10,50,50,1000,10,10,100,100};
        int bet_count;
            int dragon = 0;
            int tie = 0;
            int tiger = 0;

            if(data.GetField("dis_total_bet_on_cards").HasField("dragon"))
                dragon = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("dragon").ToString().Trim(Config.Inst.trim_char_arry));
            if (data.GetField("dis_total_bet_on_cards").HasField("tie"))
                tie = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("tie").ToString().Trim(Config.Inst.trim_char_arry));
            if (data.GetField("dis_total_bet_on_cards").HasField("tiger"))
                tiger = int.Parse(data.GetField("dis_total_bet_on_cards").GetField("tiger").ToString().Trim(Config.Inst.trim_char_arry));

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
                    Vector3 target = getRandomPoint(DT_PlayerManager.Inst.Dragon_Coin_Local_OBJ,0);
                    GameObject _Coin = Instantiate(PFB_COINS, DT_PlayerManager.Inst.DragonBox.transform) as GameObject;
                    _Coin.transform.GetComponent<DT_PFB_COINS>().SET_COIN(coins[Random.Range(0, coins.Length)].ToString());
                    _Coin.transform.GetComponent<DT_PFB_COINS>().Move_Anim(target);
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
                    Vector3 target = getRandomPoint(DT_PlayerManager.Inst.Tie_Coin_Local_OBJ, 0);
                    GameObject _Coin = Instantiate(PFB_COINS, DT_PlayerManager.Inst.TieBox.transform) as GameObject;
                    _Coin.transform.GetComponent<DT_PFB_COINS>().SET_COIN(coins[Random.Range(0, coins.Length)].ToString());
                    _Coin.transform.GetComponent<DT_PFB_COINS>().Move_Anim(target);
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
                    Vector3 target = getRandomPoint(DT_PlayerManager.Inst.Tiger_Coin_Local_OBJ, 0);
                    GameObject _Coin = Instantiate(PFB_COINS, DT_PlayerManager.Inst.TigerBox.transform) as GameObject;
                    _Coin.transform.GetComponent<DT_PFB_COINS>().SET_COIN(coins[Random.Range(0, coins.Length)].ToString());
                    _Coin.transform.GetComponent<DT_PFB_COINS>().Move_Anim(target);
                }
            }
    }
    //------------------------------ Rejoin Chips ---------------------------------------------------------


    public void RESET_CARD()
    {
        iTween.Stop(Left_Card);
        iTween.Stop(Right_Card);
        Left_Card.GetComponent<Image>().sprite = DT_PlayerManager.Inst.BackCard;
        Right_Card.GetComponent<Image>().sprite = DT_PlayerManager.Inst.BackCard;
        Vector3 newRotation = new Vector3(0, 180, 0);
        Left_Card.transform.eulerAngles = newRotation;
        Right_Card.transform.eulerAngles = newRotation;
    }
}
