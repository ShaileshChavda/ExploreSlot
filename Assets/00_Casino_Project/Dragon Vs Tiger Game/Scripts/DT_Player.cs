using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DT_Player : MonoBehaviour
{
    public static DT_Player Inst { get; set; }
    public Text Txt_UserName, TxtChips, TxtPlusMinus;
    public Image Vip_Ring;
    public string ID;
    public float MyCoins;
    public double WinOrLose_Chips;
    public Status _Status= Status.Null;
    public bool Is_Bot=false;
    [SerializeField] Animator Chaal_Anim, Win_Plus_Minus_Anim;
    [SerializeField] IMGLoader User_PIC;
    public bool Played_Chips;
    public DT_Circle_Anim WinCircleANim;
    public Image bgOfWinText;
    public Sprite[] blueYellowSprite;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        Txt_UserName = transform.GetChild(1).GetComponent<Text>();
        TxtChips = transform.GetChild(2).GetComponent<Text>();
        Vip_Ring = transform.GetChild(3).GetComponent<Image>();
        User_PIC = this.GetComponent<IMGLoader>();
        Chaal_Anim = this.GetComponent<Animator>();

        TxtPlusMinus = transform.GetChild(5).GetChild(0).GetComponent<Text>();
        Win_Plus_Minus_Anim = transform.GetChild(5).GetComponent<Animator>();
        bgOfWinText = transform.GetChild(5).GetComponent<Image>();
        WinCircleANim = transform.GetChild(6).GetComponent<DT_Circle_Anim>();
    }

    private void OnEnable()
    {
        DT_EventSetup._DT_CHAAL += CHAAL;
        DT_EventSetup._DT_LEAVE += LEAVE;
        //DT_EventSetup._DT_SEAT += SEAT;
    }

    private void OnDisable()
    {
        DT_EventSetup._DT_CHAAL -= CHAAL;
        DT_EventSetup._DT_LEAVE -= LEAVE;
        //DT_EventSetup._DT_SEAT -= SEAT;
    }

    public void CHAAL(string id)
    {
        if (id.Equals(ID))
            DT_PlayerManager.Inst.Chal_Player = this;
    }

    public enum Status
    {
        Play,
        Null,
    }

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
            if (WinOrLose_Chips > 0)
            {
                if (TxtChips.text != "" && TxtChips.text != " ")
                    TxtChips.text = (double.Parse(TxtChips.text) + WinOrLose_Chips).ToString();
            }

            Win_Plus_Minus_Anim.Play("WinPlusMinus_Anim", 0);
        }
        Invoke(nameof(DisableTxtPlusMinus), 1.5f);
    }

    void DisableTxtPlusMinus()
    {
        bgOfWinText.enabled = false;
        TxtPlusMinus.text = "";
    }

    public void SET_PLAYER_DATA(JSONObject data)
    {
        ID= data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_UserName.text= data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        MyCoins = float.Parse(data.GetField("wallet").ToString().Trim(Config.Inst.trim_char_arry));
        TxtChips.text=MyCoins.ToString("n2");
        Is_Bot = bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry));
        User_PIC.LoadIMG(data.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry),false, false);
        Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[int.Parse(data.GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry))];
        _Status = Status.Play;
    }

    //----------- Card Chal Animation----------------
    public void Chaal_Animation(string Chaa_Amount,string side)
    {
        int type = 0;
        if (side.Equals("dragon")) 
            type = 0;
        else if (side.Equals("tie"))
            type = 1;
        else
            type = 0;

        Vector3 target = DT_Manager.Inst.getRandomPoint(Side_FIND(side), type);
        GameObject _Coin = Instantiate(DT_Manager.Inst.PFB_COINS) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            DT_Manager.Inst.Coin_Audio_Source.PlayOneShot(DT_SoundManager.Inst.SFX[6]);
        _Coin.transform.GetComponent<DT_PFB_COINS>().SET_COIN(Chaa_Amount);
        _Coin.transform.SetParent(Side_FIND(side).transform, false);
        _Coin.transform.position = transform.position;
        _Coin.transform.GetComponent<DT_PFB_COINS>().Move_Anim(target);
        Chaal_Anim.Play("LeftPlayerChaalAnim");
        //iTween.MoveTo(_Coin.gameObject, iTween.Hash("position", target, "time", 2f, "easetype", iTween.EaseType.easeOutExpo));
    }
    //----------- Card Chal Animation----------------

    GameObject Side_FIND(string side)
    {
        if (side.Equals("dragon"))
            return DT_PlayerManager.Inst.DragonBox;
        if (side.Equals("tie"))
            return DT_PlayerManager.Inst.TieBox;
        if (side.Equals("tiger"))
            return DT_PlayerManager.Inst.TigerBox;
        return null;
    }

    public void LEAVE(string _id)
    {
        if (_id.Equals(ID))
        {
            _Status = Status.Null;
            Txt_UserName.text = "";
            User_PIC.icon.sprite = DT_PlayerManager.Inst.EmptySeat_Sprite;
            Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[0];

            _Status = Status.Null;
            ID = "";
            Txt_UserName.text = "";
            TxtChips.text = "";
            MyCoins = 0;
            Is_Bot = false;
            //WinCircleANim.Stop_Loader();
        }
    }

    public void SEAT(JSONObject data)
    {
        //if (_Status==Status.Null)
            SET_PLAYER_DATA(data);
    }
}
