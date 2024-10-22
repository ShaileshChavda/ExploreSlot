using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roullate_Player : MonoBehaviour
{
    public static Roullate_Player Inst { get; set; }
    public Text Txt_UserName,TxtChips,TxtPlusMinus;
    public Image Vip_Ring;
    public string ID;
    public float MyCoins;
    public double WinOrLose_Chips;
    public bool Played_Chips;
    public Status _Status = Status.Null;
    public bool Is_Bot = false;
    [SerializeField] Animator Chaal_Anim,Win_Plus_Minus_Anim;
    [SerializeField] IMGLoader User_PIC;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
        //Txt_UserName = transform.GetChild(1).GetComponent<Text>();
        //TxtChips = transform.GetChild(2).GetComponent<Text>();
        //Vip_Ring = transform.GetChild(3).GetComponent<Image>();
        //TxtPlusMinus = transform.GetChild(4).GetComponent<Text>();
        //Win_Plus_Minus_Anim = transform.GetChild(4).GetComponent<Animator>();
        //User_PIC = this.GetComponent<IMGLoader>();
        //Chaal_Anim = this.GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Roullate_EventSetup._Roullate_CHAAL += CHAAL;
        Roullate_EventSetup._Roullate_LEAVE += LEAVE;
       // Roullate_EventSetup._Roullate_SEAT += SEAT;
    }

    private void OnDisable()
    {
        Roullate_EventSetup._Roullate_CHAAL -= CHAAL;
        Roullate_EventSetup._Roullate_LEAVE -= LEAVE;
       // Roullate_EventSetup._Roullate_SEAT -= SEAT;
    }

    public void CHAAL(string id)
    {
        if (id.Equals(ID))
        {
            Roullate_PlayerManager.Inst.Chal_Player = this;
        }
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
        Is_Bot = bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry));
        User_PIC.LoadIMG(data.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false, false);
        Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[int.Parse(data.GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry))];
        _Status = Status.Play;
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
            if (WinOrLose_Chips > 0)
            {
                if (TxtChips.text != "" && TxtChips.text != " ")
                    TxtChips.text = (double.Parse(TxtChips.text) + WinOrLose_Chips).ToString();
            }
            Win_Plus_Minus_Anim.Play("WinPlusMinus_Anim", 0);
        }
    }



    //----------- Card Chal Animation----------------
    public void Chaal_Animation(string Chaa_Amount, string side)
    {
        GameObject TG;
        if (side.Length > 2)
            TG = GameObject.Find(side);
        else
            TG = GameObject.Find(side + "_b");

        Vector3 target = Roullate_Manager.Inst.getRandomPoint(TG);
        //Vector3 target = TG.transform.position;
        GameObject _Coin = Instantiate(Roullate_Manager.Inst.PFB_COINS, TG.transform) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            Roullate_Manager.Inst.Coin_Audio_Source.PlayOneShot(Roullate_SoundManager.Inst.SFX[40]);
        _Coin.transform.GetComponent<Roullate_PFB_Coins>().SET_COIN(Chaa_Amount);
        _Coin.transform.position = transform.position;
        StartCoroutine(Coin_Hide(_Coin));
        iTween.ScaleTo(_Coin.gameObject, iTween.Hash("scale", Vector3.one, "speed", 4.5f, "easetype", "linear"));
        iTween.MoveTo(_Coin.gameObject, iTween.Hash("position", target, "time", 1.5f, "easetype", iTween.EaseType.easeOutExpo));
        Chaal_Anim.Play("LeftPlayerChaalAnim",0);
    }
    IEnumerator Coin_Hide(GameObject coin)
    {
        yield return new WaitForSeconds(1.1f);
        coin.transform.localScale = Vector3.zero;
    }
    //----------- Card Chal Animation----------------

    public void LEAVE(string _id)
    {
        if (_id.Equals(ID))
        {
            _Status = Status.Null;
            ID = "";
            Txt_UserName.text = "";
            TxtChips.text = "";
            MyCoins = 0;
            Is_Bot = false;
            User_PIC.icon.sprite = Roullate_PlayerManager.Inst.EmptySeat_Sprite;
            Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[0];
        }
    }

    public void SEAT(JSONObject data)
    {
      // if (_Status == Status.Null)
            SET_PLAYER_DATA(data);
    }
}
