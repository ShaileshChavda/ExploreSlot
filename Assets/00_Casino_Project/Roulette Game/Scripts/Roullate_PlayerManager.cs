using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roullate_PlayerManager : MonoBehaviour
{
    public static Roullate_PlayerManager Inst;
    public List<Roullate_Player> Player_Bot_List;
    public Roullate_Player Chal_Player;
    public Text TxtUserName, TxtUserChips, TxtPlusMinus;
    public double WinOrLose_Chips;
    public bool Played_Chips;
    public IMGLoader UserPic;
    public Image User_Vip_Ring;
    public Sprite EmptySeat_Sprite;
    public List<Sprite> Chips_Sprite_List;
    public Animator Win_Plus_Minus_Anim;
    public double _User_TotalBet;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        _User_TotalBet = 0;
    }
    public void SET_PLAYER_DATA(JSONObject data)
    {
        //set boat user
        //if (data.GetField("join_robots").Count > 6)
        //{
            for (int i = 0; i < Player_Bot_List.Count; i++)
            {
                Player_Bot_List[i].SET_PLAYER_DATA(data.GetField("join_robots")[i]);
            }
        //}
        //else
        //{
        //    for (int i = 0; i < data.GetField("join_robots").Count; i++)
        //    {
        //        Player_Bot_List[i].SET_PLAYER_DATA(data.GetField("join_robots")[i]);
        //    }
        //}

        //set my user
        TxtUserName.text = data.GetField("user_info").GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        TxtUserChips.text = float.Parse(data.GetField("user_info").GetField("wallet").ToString().Trim(Config.Inst.trim_char_arry)).ToString("n2");
        UserPic.LoadIMG(data.GetField("user_info").GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false, false);
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
            _User_TotalBet = _User_TotalBet + Roullate_Manager.Inst.Selected_Bet_Amount;

        Roullate_UI_Manager.Inst.Txt_TotalBet.text = _User_TotalBet + "/"+data.GetField("total_bet").ToString().Trim(Config.Inst.trim_char_arry);
        if (data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry).Equals(GS.Inst._userData.Id))
            Roullate_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry));
        else if (Check_Player_In_Board(data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry)))
        {
            Roullate_EventSetup.BET_CHAAL(data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry));
            Chal_Player.TxtChips.text = (float.Parse(Chal_Player.TxtChips.text) - float.Parse(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry))).ToString("n2");
            Chal_Player.Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry));
        }
        else
            Roullate_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry));
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
    internal Roullate_Player GetPlayer_UsingID(string id)
    {
        Roullate_Player p = new Roullate_Player();
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
            TxtPlusMinus.color = Color.red;
            TxtPlusMinus.text = "-" + WinOrLose_Chips.ToString().Replace("-", "");
        }
        else
        {
            TxtPlusMinus.color = Color.green;
            TxtPlusMinus.text = "+" + WinOrLose_Chips.ToString();
        }

        if(Played_Chips)
            Win_Plus_Minus_Anim.Play("WinPlusMinus_Anim", 0);
    }
}
