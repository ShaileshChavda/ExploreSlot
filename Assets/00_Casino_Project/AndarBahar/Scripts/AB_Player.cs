using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AB_Player : MonoBehaviour
{
    public static AB_Player Inst { get; set; }
    public TextMeshProUGUI Txt_UserName, TxtChips;
    public TextMeshProUGUI TxtPlusMinus;
    public Image Vip_Ring;
    public string ID;
    public float MyCoins;
    public double WinOrLose_Chips;
    public Status _Status = Status.Null;
    public bool Is_Bot = false;
    [SerializeField] Animator Chaal_Anim, Win_Plus_Minus_Anim;
    [SerializeField] IMGLoader User_PIC;
    public bool Played_Chips;
    public AB_Circle_Anim WinCircleANim;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        Txt_UserName = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        TxtChips = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        Vip_Ring = transform.GetChild(1).GetComponent<Image>();
        User_PIC = this.GetComponent<IMGLoader>();
        Chaal_Anim = this.GetComponent<Animator>();

        TxtPlusMinus = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        Win_Plus_Minus_Anim = transform.GetChild(2).GetComponent<Animator>();
        WinCircleANim = transform.GetChild(3).GetComponent<AB_Circle_Anim>();
    }

    private void OnEnable()
    {
        AB_EventSetup._AB_CHAAL += CHAAL;
        AB_EventSetup._AB_LEAVE += LEAVE;
        //DT_EventSetup._DT_SEAT += SEAT;
    }

    private void OnDisable()
    {
        AB_EventSetup._AB_CHAAL -= CHAAL;
        AB_EventSetup._AB_LEAVE -= LEAVE;
        //DT_EventSetup._DT_SEAT -= SEAT;
    }

    public void CHAAL(string id)
    {
        if (id.Equals(ID))
            AB_PlayerManager.Inst.Chal_Player = this;
    }

    public enum Status
    {
        Play,
        Null,
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
            if (WinOrLose_Chips > 0)
            {
                if (TxtChips.text != "" && TxtChips.text != " ")
                    TxtChips.text = (double.Parse(TxtChips.text) + WinOrLose_Chips).ToString();
            }
            Win_Plus_Minus_Anim.Play("WinPlusMinus_Anim", 0);
        }
    }


    public void SET_PLAYER_DATA(JSONObject data)
    {
        ID = data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_UserName.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        MyCoins = float.Parse(data.GetField("wallet").ToString().Trim(Config.Inst.trim_char_arry));
        TxtChips.text = MyCoins.ToString("n2");
        Is_Bot = bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry));
        User_PIC.LoadIMG(data.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false, false);
        Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[int.Parse(data.GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry))];
        _Status = Status.Play;
    }

    //----------- Card Chal Animation----------------
    public void Chaal_Animation(string Chaa_Amount, string side)
    {
        //int type = 0;
        //if (side.Equals("dragon"))
        //    type = 0;
        //else if (side.Equals("tie"))
        //    type = 1;
        //else
        //    type = 0;

        Vector3 target = AB_Manager.Inst.getRandomPoint(Side_FIND(side), 0);
        GameObject _Coin = Instantiate(AB_Manager.Inst.PFB_COINS) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            AB_Manager.Inst.Coin_Audio_Source.PlayOneShot(AB_SoundManager.Inst.SFX[2]);
        _Coin.transform.GetComponent<AB_PFB_COINS>().SET_COIN(Chaa_Amount);
        _Coin.transform.SetParent(Side_FIND(side).transform, false);
        _Coin.transform.position = transform.position;
        _Coin.transform.GetComponent<AB_PFB_COINS>().Move_Anim(target);
        Chaal_Anim.Play("LeftPlayerChaalAnim");
        //iTween.MoveTo(_Coin.gameObject, iTween.Hash("position", target, "time", 2f, "easetype", iTween.EaseType.easeOutExpo));
    }
    //----------- Card Chal Animation----------------

    GameObject Side_FIND(string side)
    {

        if (side.Equals("andar"))
            return AB_PlayerManager.Inst.AndarBox;
        else if (side.Equals("bahar"))
            return AB_PlayerManager.Inst.BaharBox;
        else if (side.Equals("1-5"))
            return AB_PlayerManager.Inst.Box1;
        else if (side.Equals("6-10"))
            return AB_PlayerManager.Inst.Box2;
        else if (side.Equals("11-15"))
            return AB_PlayerManager.Inst.Box3;
        else if (side.Equals("16-25"))
            return AB_PlayerManager.Inst.Box4;
        else if (side.Equals("26-30"))
            return AB_PlayerManager.Inst.Box5;
        else if (side.Equals("31-35"))
            return AB_PlayerManager.Inst.Box6;
        else if (side.Equals("36-40"))
            return AB_PlayerManager.Inst.Box7;
        else if (side.Equals("41-48"))
            return AB_PlayerManager.Inst.Box8;
        return null;
    }

    public void LEAVE(string _id)
    {
        if (_id.Equals(ID))
        {
            _Status = Status.Null;
            Txt_UserName.text = "";
            User_PIC.icon.sprite = AB_PlayerManager.Inst.EmptySeat_Sprite;
            Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[0];

            _Status = Status.Null;
            ID = "";
            Txt_UserName.text = "";
            TxtChips.text = "";
            MyCoins = 0;
            Is_Bot = false;
            WinCircleANim.Stop_Loader();
        }
    }

    public void SEAT(JSONObject data)
    {
        //if (_Status==Status.Null)
        SET_PLAYER_DATA(data);
    }
}
