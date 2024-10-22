using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AB_PlayerManager : MonoBehaviour
{
    public static AB_PlayerManager Inst;
    public List<AB_Player> Player_Bot_List;
    public AB_Player Chal_Player;
    public TextMeshProUGUI Txt_MyUserName;
    public TextMeshProUGUI Txt_MyUser_Chips, TxtPlusMinus;
    public AB_Circle_Anim WinCircleANim;
    public double WinOrLose_Chips;
    public bool Played_Chips;
    public IMGLoader User_Pic;
    public Image User_Vip_Ring;
    public GameObject AndarBox, BaharBox, Box1, Box2, Box3, Box4, Box5, Box6, Box7, Box8;
    public GameObject Andar_Coin_Local_OBJ, Bahar_Coin_Local_OBJ,
        Box1_Coin_Local_OBJ,
        Box2_Coin_Local_OBJ,
        Box3_Coin_Local_OBJ,
        Box4_Coin_Local_OBJ,
        Box5_Coin_Local_OBJ,
        Box6_Coin_Local_OBJ,
        Box7_Coin_Local_OBJ,
        Box8_Coin_Local_OBJ;

    public Sprite EmptySeat_Sprite, BackCard;
    public List<Sprite> Card_List;
    public List<Sprite> Chips_Sprite_List;
    public Animator Win_Plus_Minus_Anim;
    public double _User_TotalBet_Andar, _User_TotalBet_Bahar,
        _User_TotalBet_1_5,
        _User_TotalBet_6_10,
        _User_TotalBet_11_15,
        _User_TotalBet_16_25,
        _User_TotalBet_26_30,
        _User_TotalBet_31_35,
        _User_TotalBet_36_40,
        _User_TotalBet_41_48;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        _User_TotalBet_Andar = 0;
        _User_TotalBet_Bahar = 0;
        _User_TotalBet_1_5 = 0;
        _User_TotalBet_6_10 = 0;
        _User_TotalBet_11_15 = 0;
        _User_TotalBet_16_25 = 0;
        _User_TotalBet_26_30 = 0;
        _User_TotalBet_31_35 = 0;
        _User_TotalBet_36_40 = 0;
        _User_TotalBet_41_48 = 0;
    }
    public void Set_Current_Bets(JSONObject data)
    {
        if (data.GetField("current_bets").HasField("andar"))
            _User_TotalBet_Andar = double.Parse(data.GetField("current_bets").GetField("andar").ToString().Trim(Config.Inst.trim_char_arry));

        if (data.GetField("current_bets").HasField("bahar"))
            _User_TotalBet_Bahar = double.Parse(data.GetField("current_bets").GetField("bahar").ToString().Trim(Config.Inst.trim_char_arry));

        if (data.GetField("current_bets").HasField("1-5"))
            _User_TotalBet_1_5 = double.Parse(data.GetField("current_bets").GetField("box1").ToString().Trim(Config.Inst.trim_char_arry));

        if (data.GetField("current_bets").HasField("6-10"))
            _User_TotalBet_6_10 = double.Parse(data.GetField("current_bets").GetField("box1").ToString().Trim(Config.Inst.trim_char_arry));

        if (data.GetField("current_bets").HasField("11-15"))
            _User_TotalBet_11_15 = double.Parse(data.GetField("current_bets").GetField("box1").ToString().Trim(Config.Inst.trim_char_arry));

        if (data.GetField("current_bets").HasField("16-25"))
            _User_TotalBet_16_25 = double.Parse(data.GetField("current_bets").GetField("box1").ToString().Trim(Config.Inst.trim_char_arry));

        if (data.GetField("current_bets").HasField("26-30"))
            _User_TotalBet_26_30 = double.Parse(data.GetField("current_bets").GetField("box1").ToString().Trim(Config.Inst.trim_char_arry));

        if (data.GetField("current_bets").HasField("31-35"))
            _User_TotalBet_31_35 = double.Parse(data.GetField("current_bets").GetField("box1").ToString().Trim(Config.Inst.trim_char_arry));

        if (data.GetField("current_bets").HasField("36-40"))
            _User_TotalBet_36_40 = double.Parse(data.GetField("current_bets").GetField("box1").ToString().Trim(Config.Inst.trim_char_arry));

        if (data.GetField("current_bets").HasField("41-48"))
            _User_TotalBet_41_48 = double.Parse(data.GetField("current_bets").GetField("box1").ToString().Trim(Config.Inst.trim_char_arry));
    }
    public void SET_PLAYER_DATA(JSONObject data)
    {
        int fail_index = -1;
        //set boat user
        if (data.GetField("join_robots").Count > 6)
        {
            for (int i = 0; i < Player_Bot_List.Count; i++)
            {
                if (data.GetField("join_robots")[i].GetField("_id").ToString().Trim(Config.Inst.trim_char_arry) != GS.Inst._userData.Id)
                    Player_Bot_List[i].SET_PLAYER_DATA(data.GetField("join_robots")[i]);
                else
                    fail_index = i;
            }

            if (fail_index != -1 && fail_index != 6)
                Player_Bot_List[fail_index].SET_PLAYER_DATA(data.GetField("join_robots")[6]);
        }
        else
        {
            for (int i = 0; i < data.GetField("join_robots").Count; i++)
            {
                if (data.GetField("join_robots")[i].GetField("_id").ToString().Trim(Config.Inst.trim_char_arry) != GS.Inst._userData.Id)
                    Player_Bot_List[i].SET_PLAYER_DATA(data.GetField("join_robots")[i]);
                //else
                //fail_index = i;
            }
            //if (fail_index != -1)
            //Player_Bot_List[fail_index].SET_PLAYER_DATA(data.GetField("join_robots")[6]);
        }

        //set my user
        Txt_MyUserName.text = data.GetField("user_info").GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_MyUser_Chips.text = float.Parse(data.GetField("user_info").GetField("wallet").ToString().Trim(Config.Inst.trim_char_arry)).ToString("n2");
        User_Pic.LoadIMG(data.GetField("user_info").GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false, false);
        if (data.GetField("user_info").HasField("vip_level"))
            User_Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[int.Parse(data.GetField("user_info").GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry))];
        else
            User_Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[0];
    }

    public void REFRESH_PLAYER_DATA(JSONObject data)
    {
        int count = 0;
        //set boat user
        for (int i = 0; i < data.GetField("user_lists").Count; i++)
        {
            if (GS.Inst._userData.Id != data.GetField("user_lists")[i].GetField("_id").ToString().Trim(Config.Inst.trim_char_arry))
            {
                if (count < 6)
                {
                    Player_Bot_List[count].SET_PLAYER_DATA(data.GetField("user_lists")[i]);
                    count++;
                }
            }
        }
    }

    public void PLAYER_CHAAL(JSONObject data)
    {
        if (data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry).Equals(GS.Inst._userData.Id))
        {
            string side = data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry);
            switch (side)
            {
                case "andar":
                    AB_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), AndarBox, Andar_Coin_Local_OBJ, 1);
                    break;
                case "bahar":
                    AB_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), BaharBox, Bahar_Coin_Local_OBJ, 1);
                    break;
                case "1-5":
                    AB_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box1, Box1_Coin_Local_OBJ, 1);
                    break;
                case "6-10":
                    AB_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box2, Box2_Coin_Local_OBJ, 1);
                    break;
                case "11-15":
                    AB_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box3, Box3_Coin_Local_OBJ, 1);
                    break;
                case "16-25":
                    AB_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box4, Box4_Coin_Local_OBJ, 1);
                    break;
                case "26-30":
                    AB_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box5, Box5_Coin_Local_OBJ, 1);
                    break;
                case "31-35":
                    AB_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box6, Box6_Coin_Local_OBJ, 1);
                    break;
                case "36-40":
                    AB_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box7, Box7_Coin_Local_OBJ, 1);
                    break;
                case "41-48":
                    AB_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box8, Box8_Coin_Local_OBJ, 1);
                    break;
            }

        }
        else if (Check_Player_In_Board(data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry)))
        {
            AB_EventSetup.BET_CHAAL(data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry));
            Chal_Player.TxtChips.text = (float.Parse(Chal_Player.TxtChips.text) - float.Parse(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry))).ToString("n2");
            Chal_Player.Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry));
        }
        else
        {
            string side = data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry);
            switch (side)
            {
                case "andar":
                    AB_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), AndarBox, Andar_Coin_Local_OBJ, 1);
                    break;
                case "bahar":
                    AB_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), BaharBox, Bahar_Coin_Local_OBJ, 1);
                    break;
                case "1-5":
                    AB_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box1, Box1_Coin_Local_OBJ, 1);
                    break;
                case "6-10":
                    AB_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box2, Box2_Coin_Local_OBJ, 1);
                    break;
                case "11-15":
                    AB_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box3, Box3_Coin_Local_OBJ, 1);
                    break;
                case "16-25":
                    AB_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box4, Box4_Coin_Local_OBJ, 1);
                    break;
                case "26-30":
                    AB_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box5, Box5_Coin_Local_OBJ, 1);
                    break;
                case "31-35":
                    AB_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box6, Box6_Coin_Local_OBJ, 1);
                    break;
                case "36-40":
                    AB_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box7, Box7_Coin_Local_OBJ, 1);
                    break;
                case "41-48":
                    AB_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), Box8, Box8_Coin_Local_OBJ, 1);
                    break;
            }

        }
        AB_UI_Manager.Inst.SET_PLAYED_TOTAL_CHIPS(data);
    }

    bool Check_Player_In_Board(string id)
    {
        bool fl = false;
        for (int i = 0; i < Player_Bot_List.Count; i++)
        {
            if (Player_Bot_List[i].ID.Equals(id))
                fl = true;
        }
        return fl;
    }

    //Get player using playerID
    internal AB_Player GetPlayer_UsingID(string id)
    {
        AB_Player p = new AB_Player();
        for (var i = 0; i < Player_Bot_List.Count; i++)
        {
            if (id == Player_Bot_List[i].ID)
                p = Player_Bot_List[i];
        }
        return p;
    }

    public void Play_DiductionAnimation()
    {
        for (int i = 0; i < Player_Bot_List.Count; i++)
        {
            Player_Bot_List[i].Update_Win_Loss_Chips();
        }
        Update_Win_Loss_Chips();
    }

    public void Update_Win_Loss_Chips()
    {
        if (WinOrLose_Chips < 0)
        {
            TxtPlusMinus.color = new Color(0, 157, 255, 255);
            TxtPlusMinus.text = "-" + WinOrLose_Chips.ToString().Replace("-", "");
        }
        else
        {
            if (Played_Chips)
                WinCircleANim.Show();
            TxtPlusMinus.color = new Color(255, 157, 0, 255);
            TxtPlusMinus.text = "+" + WinOrLose_Chips.ToString();
        }

        if (Played_Chips)
        {
            Played_Chips = false;
            Win_Plus_Minus_Anim.Play("WinPlusMinus_Anim", 0);
        }
    }
}
