using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DT_PlayerManager : MonoBehaviour
{
    public static DT_PlayerManager Inst;
    public List<DT_Player> Player_Bot_List;
    public DT_Player Chal_Player;
    public Text Txt_MyUserName;
    public Text Txt_MyUser_Chips, TxtPlusMinus;
    public DT_Circle_Anim WinCircleANim;
    public double WinOrLose_Chips;
    public bool Played_Chips;
    public IMGLoader User_Pic;
    public Image User_Vip_Ring;
    public GameObject DragonBox, TieBox, TigerBox;
    public GameObject Dragon_Coin_Local_OBJ, Tiger_Coin_Local_OBJ, Tie_Coin_Local_OBJ;
    public Sprite EmptySeat_Sprite,BackCard;
    public List<Sprite> Card_List;
    public List<Sprite> Chips_Sprite_List;
    public Animator Win_Plus_Minus_Anim;
    public Image bgOfWinText;
    public Sprite[] blueYellowSprite;
    public double _User_TotalBet_Dragon, _User_TotalBet_Tie, _User_TotalBet_Tiger;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        _User_TotalBet_Dragon = 0;
        _User_TotalBet_Tie = 0;
        _User_TotalBet_Tiger = 0;
    }
    public void Set_Current_Bets(JSONObject data)
    {
        //DT_PlayerManager.Inst._User_TotalBet_Dragon = double.Parse(data.GetField("current_bets").GetField("dragon").ToString().Trim(Config.Inst.trim_char_arry));
        //DT_PlayerManager.Inst._User_TotalBet_Tie = double.Parse(data.GetField("current_bets").GetField("tie").ToString().Trim(Config.Inst.trim_char_arry));
        //DT_PlayerManager.Inst._User_TotalBet_Tiger = double.Parse(data.GetField("current_bets").GetField("tiger").ToString().Trim(Config.Inst.trim_char_arry));

        if (data.GetField("current_bets").HasField("dragon"))
            _User_TotalBet_Dragon = double.Parse(data.GetField("current_bets").GetField("dragon").ToString().Trim(Config.Inst.trim_char_arry));

        if (data.GetField("current_bets").HasField("tie"))
            _User_TotalBet_Tie = double.Parse(data.GetField("current_bets").GetField("tie").ToString().Trim(Config.Inst.trim_char_arry));

        if (data.GetField("current_bets").HasField("tiger"))
            _User_TotalBet_Tiger = double.Parse(data.GetField("current_bets").GetField("tiger").ToString().Trim(Config.Inst.trim_char_arry));
    }
    public void SET_PLAYER_DATA(JSONObject data)
    {
        int fail_index=-1;
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

            if(fail_index!=-1 && fail_index!=6)
               Player_Bot_List[fail_index].SET_PLAYER_DATA(data.GetField("join_robots")[6]);
        }
        else
        {
            for (int i = 0; i < data.GetField("join_robots").Count; i++)
            {
                if(data.GetField("join_robots")[i].GetField("_id").ToString().Trim(Config.Inst.trim_char_arry)!=GS.Inst._userData.Id)
                    Player_Bot_List[i].SET_PLAYER_DATA(data.GetField("join_robots")[i]);
                //else
                    //fail_index = i;
            }
            //if (fail_index != -1)
                //Player_Bot_List[fail_index].SET_PLAYER_DATA(data.GetField("join_robots")[6]);
        }

        //set my user
        Txt_MyUserName.text=data.GetField("user_info").GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_MyUser_Chips.text=float.Parse(data.GetField("user_info").GetField("wallet").ToString().Trim(Config.Inst.trim_char_arry)).ToString("n2");
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
            if (data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry).Equals("dragon"))
                DT_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), DragonBox,Dragon_Coin_Local_OBJ,0);
            if (data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry).Equals("tie"))
                DT_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), TieBox,Tie_Coin_Local_OBJ,1);
            if (data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry).Equals("tiger"))
                DT_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), TigerBox,Tiger_Coin_Local_OBJ,0);
        }
        else if (Check_Player_In_Board(data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry)))
        {
            DT_EventSetup.BET_CHAAL(data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry));
            Chal_Player.TxtChips.text = (float.Parse(Chal_Player.TxtChips.text) - float.Parse(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry))).ToString("n2");
            Chal_Player.Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry));
        }
        else
        {
            if (data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry).Equals("dragon"))
                DT_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), DragonBox, Dragon_Coin_Local_OBJ, 0);
            if (data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry).Equals("tie"))
                DT_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), TieBox, Tie_Coin_Local_OBJ, 1);
            if (data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry).Equals("tiger"))
                DT_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), TigerBox, Tiger_Coin_Local_OBJ, 0);
        }
        DT_UI_Manager.Inst.SET_PLAYED_TOTAL_CHIPS(data);
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
    internal DT_Player GetPlayer_UsingID(string id)
    {
        DT_Player p = new DT_Player();
        for (var i = 0; i < Player_Bot_List.Count; i++){
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

   /* public void Update_Win_Loss_Chips()
    {
        if (WinOrLose_Chips < 0)
        {
            TxtPlusMinus.color = Color.red;
            TxtPlusMinus.text = "-" + WinOrLose_Chips.ToString().Replace("-", "");
        }
        else
        {
            if (Played_Chips)
                WinCircleANim.Show();
            TxtPlusMinus.color = Color.green;
            TxtPlusMinus.text = "+" + WinOrLose_Chips.ToString();
        }

        if (Played_Chips)
        {
            Played_Chips = false;
            Win_Plus_Minus_Anim.Play("WinPlusMinus_Anim", 0);
        }
    }*/
   
    public void Update_Win_Loss_Chips()
    {
        bgOfWinText.enabled = true;
        if (WinOrLose_Chips < 0)
        {
            //TxtPlusMinus.color = Color.red;
            bgOfWinText.sprite = blueYellowSprite[0]; // 0 means blue image,  minus thay tyare
            TxtPlusMinus.text = "-" + WinOrLose_Chips.ToString().Replace("-", "");
        }
        else
        {
            if (Played_Chips)
                WinCircleANim.Show();

            // TxtPlusMinus.color = Color.green;
            bgOfWinText.sprite = blueYellowSprite[1]; // 1 means yellow image, plus thay tyare
            TxtPlusMinus.text = "+" + WinOrLose_Chips.ToString();
        }

        if (Played_Chips)
        {
            Played_Chips = false; 
            Win_Plus_Minus_Anim.Play("WinPlusMinus_Anim", 0);
        }
        Invoke(nameof(DisableTxtPlusMinus), 2);
    }
    void DisableTxtPlusMinus()
    {
        bgOfWinText.enabled = false;
        TxtPlusMinus.text = "";
    }

}
