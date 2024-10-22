using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HR_Player : MonoBehaviour
{
    public static HR_Player Inst { get; set; }
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
    public HR_Circle_Anim WinCircleANim;
    public GameObject Sticker_Screen;
    public RectTransform DataParent_Sticker;
    public GameObject Winner, Lucky;

    [Header("My User Chat data")]
    public GameObject Text_Chat_BOX;
    public TextMeshProUGUI Txt_MyChat;
    public Image MyChat_Emoji_IMG;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    private void OnEnable()
    {
        HR_EventSetup._DT_CHAAL += CHAAL;
        HR_EventSetup._DT_LEAVE += LEAVE;
        //DT_EventSetup._DT_SEAT += SEAT;
    }

    private void OnDisable()
    {
        HR_EventSetup._DT_CHAAL -= CHAAL;
        HR_EventSetup._DT_LEAVE -= LEAVE;
        //DT_EventSetup._DT_SEAT -= SEAT;
    }

    public void CHAAL(string id)
    {
        if (id.Equals(ID))
            HR_PlayerManager.Inst.Chal_Player = this;
    }

    public enum Status
    {
        Play,
        Null,
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
            if (WinOrLose_Chips > 0)
            {
                if (TxtChips.text != "" && TxtChips.text != " ")
                    TxtChips.text = (double.Parse(TxtChips.text) + WinOrLose_Chips).ToString();
            }
            Win_Plus_Minus_Anim.Play("WinPlusMinus_Anim", 0);
        }
    }


    public void SET_PLAYER_DATA(JSONObject data,int index)
    {
        ID = data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_UserName.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        MyCoins = float.Parse(data.GetField("wallet").ToString().Trim(Config.Inst.trim_char_arry));
        TxtChips.text = MyCoins.ToString("n2");
        Is_Bot = bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry));
        User_PIC.LoadIMG(data.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false, false);
        Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[int.Parse(data.GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry))];
        _Status = Status.Play;
        if (index.Equals(0))
        {
            Winner.SetActive(true);
            Lucky.SetActive(false);
        }else if (index.Equals(1))
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

    //----------- Card Chal Animation----------------
    public void Chaal_Animation(string Chaa_Amount, int side)
    {
        int type = 0;

        Vector3 target = HR_Manager.Inst.getRandomPoint(HR_PlayerManager.Inst.Horse_Coin_Local_OBJ[side - 1]);
        GameObject _Coin = Instantiate(HR_Manager.Inst.PFB_COINS, HR_PlayerManager.Inst.HorseBox_List[side - 1].transform) as GameObject;
        if (PlayerPrefs.GetInt("sound").Equals(1))
            HR_Manager.Inst.Coin_Audio_Source.PlayOneShot(HR_SoundManager.Inst.SFX[1]);
        _Coin.transform.GetComponent<HR_PFB_COINS>().SET_COIN(Chaa_Amount);
        _Coin.transform.position = transform.position;
        _Coin.transform.GetComponent<HR_PFB_COINS>().Move_Anim(target);
        Chaal_Anim.Play("LeftPlayerChaalAnim");
    }
    //----------- Card Chal Animation----------------

    public void LEAVE(string _id)
    {
        if (_id.Equals(ID))
        {
            _Status = Status.Null;
            Txt_UserName.text = "";
            User_PIC.icon.sprite = HR_PlayerManager.Inst.EmptySeat_Sprite;
            Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[0];

            _Status = Status.Null;
            ID = "";
            Txt_UserName.text = "";
            TxtChips.text = "";
            MyCoins = 0;
            Is_Bot = false;
            WinCircleANim.Stop_Loader();
            Close_MyChat_Box();
            CLOSE_STICKER();
        }
    }

    public void SEAT(JSONObject data,int index)
    {
        //if (_Status==Status.Null)
        SET_PLAYER_DATA(data, index);
    }

    public void OPEN_STICKER()
    {
        if (ID != "")
        {
            if (Sticker_Screen.transform.localScale.x <= 0)
            {
                DataParent_Sticker.parent.parent.GetComponent<ScrollRect>().enabled = false;
                Sticker_Screen.transform.localScale = Vector3.one;
                DataParent_Sticker.anchoredPosition = new Vector2(0, DataParent_Sticker.GetComponent<RectTransform>().anchoredPosition.y);
                DataParent_Sticker.parent.parent.GetComponent<ScrollRect>().enabled = true;
                Invoke(nameof(CLOSE_STICKER), 5f);
            }
            else
            {
                CancelInvoke(nameof(CLOSE_STICKER));
                Sticker_Screen.transform.localScale = Vector3.zero;
            }
        }
    }
    public void CLOSE_STICKER()
    {
        Sticker_Screen.transform.localScale = Vector3.zero;
    }
    public void SEND_STICKER(string stickerNo)
    {
        CLOSE_STICKER();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.HORSE_RACING_CHAT(ID,stickerNo,"",true));
    }

    public void SET_CHAT_BOX(string _Text,string IMG)
    {
        if (_Text != "")
        {
            Text_Chat_BOX.transform.localScale = Vector3.one;
            Txt_MyChat.text = _Text;
        }
        else
        {
            MyChat_Emoji_IMG.transform.localScale = Vector3.one;
            MyChat_Emoji_IMG.sprite = HR_Chat.Inst.Sardar_sprite_List[int.Parse(IMG)];
        }
        Invoke(nameof(Close_MyChat_Box), 5f);
    }
    void Close_MyChat_Box()
    {
        MyChat_Emoji_IMG.transform.localScale = Vector3.zero;
        Text_Chat_BOX.transform.localScale = Vector3.zero;
    }
}
