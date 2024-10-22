using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crash_PlayerManager : MonoBehaviour
{
    public static Crash_PlayerManager Inst;
    public List<DT_Player> Player_Bot_List;
    public DT_Player Chal_Player;
    public Text Txt_MyUserName;
    public Text Txt_MyUser_Chips, TxtPlusMinus;
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
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_PLAYER_DATA(JSONObject data)
    {
        string game_id = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
        Crash_UI_Manager.Inst.TxtGameID.text = game_id;

        //User_Pic.LoadIMG(data.GetField("user_info").GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false, false);
        
        float total_wallet = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
        int user_total_bet_amount = int.Parse(data.GetField("user_total_bet_amount").ToString().Trim(Config.Inst.trim_char_arry));

        CrashController.Instance.SetBetAmountFromServer(user_total_bet_amount);
        CrashController.Instance.SetTotalWalletAmountFromServer(total_wallet);
      
        if (!string.IsNullOrEmpty(data.GetField("user_info").GetField("crash_config").ToString().Trim(Config.Inst.trim_char_arry)))
        {
            
            float flee_condition = float.Parse(data.GetField("user_info").GetField("crash_config").GetField("flee_condition").ToString());
            
            int profit_on_stop = int.Parse(data.GetField("user_info").GetField("crash_config").GetField("profit_on_stop").ToString().Trim(Config.Inst.trim_char_arry));
           
            float profit_win_amount = float.Parse(data.GetField("user_info").GetField("crash_config").GetField("profit_win_amount").ToString().Trim(Config.Inst.trim_char_arry));
           
            int loss_on_stop = int.Parse(data.GetField("user_info").GetField("crash_config").GetField("loss_on_stop").ToString().Trim(Config.Inst.trim_char_arry));
           
            float profit_loss_amount = float.Parse(data.GetField("user_info").GetField("crash_config").GetField("profit_loss_amount").ToString().Trim(Config.Inst.trim_char_arry));
            
            bool auto_remove = bool.Parse(data.GetField("user_info").GetField("crash_config").GetField("auto_remove").ToString().Trim(Config.Inst.trim_char_arry));

            int bet_amount = int.Parse(data.GetField("user_info").GetField("crash_config").GetField("bet_amount").ToString().Trim(Config.Inst.trim_char_arry));


            string mode = data.GetField("user_info").GetField("crash_config").GetField("mode").ToString().Trim(Config.Inst.trim_char_arry);

            CrashController.Instance.SetBetAmountFromServer(bet_amount);

            if (!auto_remove && mode.Equals("auto"))
            {
                Debug.Log("Auto mode Continue when rejoin the player");
            }
            
            CrashController.Instance.SetUserInfoConfigFromServer(flee_condition, profit_on_stop, profit_win_amount, loss_on_stop, profit_loss_amount, auto_remove, mode,false);
           
        }        
     
        User_Pic.LoadIMG(GS.Inst._userData.PicUrl, false, false);

        
        /*if (data.GetField("user_info").HasField("vip_level"))
            User_Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[int.Parse(data.GetField("user_info").GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry))];
        else
            User_Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[0];*/
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
        if (Check_Player_In_Board(data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry)))
        {
            DT_EventSetup.BET_CHAAL(data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry));
            Chal_Player.TxtChips.text = (double.Parse(Chal_Player.TxtChips.text) - double.Parse(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry))).ToString();
            Chal_Player.Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry));
        }
        else if (data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry).Equals(GS.Inst._userData.Id))
        {
            if (data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry).Equals("dragon"))
                DT_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), DragonBox,Dragon_Coin_Local_OBJ,0);
            if (data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry).Equals("tie"))
                DT_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), TieBox,Tie_Coin_Local_OBJ,1);
            if (data.GetField("side").ToString().Trim(Config.Inst.trim_char_arry).Equals("tiger"))
                DT_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), TigerBox,Tiger_Coin_Local_OBJ,0);
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

        if (Played_Chips)
        {
            Played_Chips = false;
            Win_Plus_Minus_Anim.Play("WinPlusMinus_Anim", 0);
        }
    }
}
