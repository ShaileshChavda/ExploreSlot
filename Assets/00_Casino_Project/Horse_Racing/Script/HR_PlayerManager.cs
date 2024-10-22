using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HR_PlayerManager : MonoBehaviour
{
    public static HR_PlayerManager Inst;
    public List<HR_Player> Player_Bot_List;
    public HR_Player Chal_Player;
    public TextMeshProUGUI Txt_MyUserName;
    public TextMeshProUGUI Txt_MyUser_Chips, TxtPlusMinus, Online_TxtPlusMinus;
    public HR_Circle_Anim WinCircleANim;
    public double WinOrLose_Chips, Other_WinOrLose_Chips;
    public bool Played_Chips, Other_Played_Chips;
    public IMGLoader User_Pic;
    public Image User_Vip_Ring;
    public Animator Win_Plus_Minus_Anim, Online_Win_Plus_Minus_Anim;
    public GameObject Winner, Lucky;
    public List<Sprite> Chips_Sprite_List;
    public Sprite EmptySeat_Sprite;
    public List<GameObject> HorseBox_List;
    public List<GameObject> Horse_Coin_Local_OBJ;
    public GameObject Win_Position_OBJ;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_PLAYER_DATA(JSONObject data)
    {
        GS.Inst._userData.Id = data.GetField("user_info").GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);

        //set boat user
        if (data.GetField("user_joins").Count > 6)
        {
            bool MyUserIn_Six=false;
            int index = 0;
            for (int i = 0; i < Player_Bot_List.Count; i++)
            {
                if (data.GetField("user_joins")[i].GetField("_id").ToString().Trim(Config.Inst.trim_char_arry) != GS.Inst._userData.Id)
                    Player_Bot_List[i].SET_PLAYER_DATA(data.GetField("user_joins")[i],i);
                else
                {
                    index = i;
                    MyUserIn_Six = true;
                }
            }
            if (MyUserIn_Six)
            {
                //Player_Bot_List[index].SET_PLAYER_DATA(data.GetField("user_joins")[6]);
                if (index.Equals(0))
                {
                    Winner.SetActive(true);
                    Lucky.SetActive(false);
                }
                else if (index.Equals(1))
                {
                    Lucky.SetActive(true);
                    Winner.SetActive(false);
                }
                else
                {
                    Lucky.SetActive(false);
                    Winner.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < data.GetField("user_joins").Count; i++)
            {
                Player_Bot_List[i].SET_PLAYER_DATA(data.GetField("user_joins")[i],i);
            }
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
        bool MyUserIn_Six = false;
        int index = 0;
        for (int i = 0; i < data.GetField("user_lists").Count; i++)
        {
            if (GS.Inst._userData.Id != data.GetField("user_lists")[i].GetField("_id").ToString().Trim(Config.Inst.trim_char_arry))
            {
                if (count < 6)
                {
                    Player_Bot_List[count].SET_PLAYER_DATA(data.GetField("user_lists")[i],i);
                    count++;
                }
            }
            else
            {
                index = count;
                MyUserIn_Six = true;
            }
        }
        if (MyUserIn_Six)
        {
            //Player_Bot_List[index].SET_PLAYER_DATA(data.GetField("user_joins")[6]);
            if (index.Equals(0))
            {
                Winner.SetActive(true);
                Lucky.SetActive(false);
            }
            else if (index.Equals(1))
            {
                Lucky.SetActive(true);
                Winner.SetActive(false);
            }
            else
            {
                Lucky.SetActive(false);
                Winner.SetActive(false);
            }
        }
    }

    public void PLAYER_CHAAL(JSONObject data)
    {
        int side = int.Parse(data.GetField("sides").ToString().Trim(Config.Inst.trim_char_arry));
        if (data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry).Equals(GS.Inst._userData.Id))
        {
            HR_Manager.Inst.MY_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), HorseBox_List[side-1], Horse_Coin_Local_OBJ[side-1], side);
        }
        else if (Check_Player_In_Board(data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry)))
        {
            HR_EventSetup.BET_CHAAL(data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry));
            Chal_Player.TxtChips.text = (float.Parse(Chal_Player.TxtChips.text) - float.Parse(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry))).ToString("n2");
            Chal_Player.Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), side);
        }
        else
        {
            HR_Manager.Inst.Real_User_Chaal_Animation(data.GetField("user_total_bet").ToString().Trim(Config.Inst.trim_char_arry), HorseBox_List[side - 1], Horse_Coin_Local_OBJ[side - 1], 0);
        }
        //DT_UI_Manager.Inst.SET_PLAYED_TOTAL_CHIPS(data);
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
    internal HR_Player GetPlayer_UsingID(string id)
    {
        HR_Player p = new HR_Player();
        for (var i = 0; i < Player_Bot_List.Count; i++)
        {
            if (id == Player_Bot_List[i].ID)
                p = Player_Bot_List[i];
        }
        return p;
    }

    //Get player using playerID
    internal GameObject GetPlayer_UsingID_CHAT(string id)
    {
        bool action = false;
        GameObject p = new GameObject();
        for (var i = 0; i < Player_Bot_List.Count; i++)
        {
            if (id == Player_Bot_List[i].ID)
            {
                action = true;
                p = Player_Bot_List[i].gameObject;
            }
        }
        if (!action)
            p=HR_Manager.Inst.Real_User_Chal_Pos;
        return p;
    }

    public void Play_DiductionAnimation()
    {
        for (int i = 0; i < Player_Bot_List.Count; i++)
        {
            Player_Bot_List[i].Update_Win_Loss_Chips();
        }
        Update_Win_Loss_Chips();
        Other_Update_Win_Loss_Chips();
    }

    public void Update_Win_Loss_Chips()
    {
        if (Played_Chips)
        {
            if (WinOrLose_Chips < 0)
            {
                TxtPlusMinus.text = "";
                //TxtPlusMinus.color = Color.red;
                //TxtPlusMinus.text = "-" + WinOrLose_Chips.ToString().Replace("-", "");
            }
            else
            {
                WinCircleANim.Show();
                //TxtPlusMinus.color = Color.green;
                TxtPlusMinus.text = "+" + WinOrLose_Chips.ToString();
            }

            Played_Chips = false;
            Win_Plus_Minus_Anim.Play("WinPlusMinus_Anim", 0);
        }
    }

    public void Other_Update_Win_Loss_Chips()
    {
        if (Other_Played_Chips)
        {
            if (Other_WinOrLose_Chips < 0)
            {
                Online_TxtPlusMinus.text = "";
                //TxtPlusMinus.color = Color.red;
                //TxtPlusMinus.text = "-" + WinOrLose_Chips.ToString().Replace("-", "");
            }
            else
            {
                //WinCircleANim.Show();
                //TxtPlusMinus.color = Color.green;
                Online_TxtPlusMinus.text = "+" + Other_WinOrLose_Chips.ToString();
            }

            Other_Played_Chips = false;
            Online_Win_Plus_Minus_Anim.Play("WinPlusMinus_Anim", 0);
        }
    }
}
