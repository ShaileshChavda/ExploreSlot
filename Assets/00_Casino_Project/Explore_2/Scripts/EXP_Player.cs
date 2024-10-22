using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EXP_Player : MonoBehaviour
{
    public static EXP_Player Inst { get; set; }
    public TextMeshProUGUI Txt_UserName, TxtChips;
    public TextMeshProUGUI TxtPlusMinus;
    public Image Vip_Ring;
    public string ID;
    public float MyCoins;
    public double WinOrLose_Chips;
    public Status _Status = Status.Null;

    [SerializeField] Animator Win_Plus_Minus_Anim;
    [SerializeField] IMGLoader User_PIC;
    public bool Played_Chips;

    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }


    public enum Status
    {
        Play,
        Null,
    }

    public void SET_PLAYER_DATA(JSONObject data)
    {
        ID = data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_UserName.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        MyCoins = float.Parse(data.GetField("wallet").ToString().Trim(Config.Inst.trim_char_arry));
        TxtChips.text = MyCoins.ToString("n2");
        User_PIC.LoadIMG(data.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false, false);
        Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[int.Parse(data.GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry))];
        _Status = Status.Play;
    }
   

    public void LEAVE(string _id)
    {
        if (_id.Equals(ID))
        {
            _Status = Status.Null;
            Txt_UserName.text = "";
            //User_PIC.icon.sprite = AB_PlayerManager.Inst.EmptySeat_Sprite;
            Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[0];

            _Status = Status.Null;
            ID = "";
            Txt_UserName.text = "";
            TxtChips.text = "";
            MyCoins = 0;
        }
    }

    public void SEAT(JSONObject data)
    {
        SET_PLAYER_DATA(data);
    }
}
