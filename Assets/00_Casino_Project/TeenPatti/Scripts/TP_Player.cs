using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TP_Player : MonoBehaviour
{
    public static TP_Player Inst;
    public int SeatIndex;
    public string PlayerID;
    public SeatStatus _SeatStatus;
    public GameObject DealerIcon;
    public Text Txt_UserName;
    public Text Txt_UserChips;
    public Text Card_Status,Card_Txt_Status_On_button;
    public TP_IMGLoader profilePic;
    public TP_Timer_Filler Timer_Filler;
    [SerializeField] GameObject MyCard_OBJ;
    [SerializeField] List<Image> My_Card_IMG_List;
    [SerializeField] GameObject WinAnimation_OBJ;
    public bool is_watch_MODE;
    public Image Vip_Ring;
    public GameObject Card_See_Eye;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
        Player_Seat_RESET();
    }

    public void SET_MY_INFO(JSONObject info)
    {
        SeatIndex = int.Parse(info.GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
        PlayerID = info.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
        _SeatStatus = SeatStatus.Seated;
        Txt_UserName.text = info.GetField("username").ToString().Trim(Config.Inst.trim_char_arry);
        if (GS.Inst._userData.MySeatIndex.Equals(SeatIndex))
            Txt_UserChips.text = info.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry);
        else
            Txt_UserChips.text = info.GetField("last_chal_value").ToString().Trim(Config.Inst.trim_char_arry);
        profilePic.LoadIMG(info.GetField("profile").ToString().Trim(Config.Inst.trim_char_arry));
        Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[int.Parse(info.GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry))];
        string CardStatus = info.GetField("play_status").ToString().Trim(Config.Inst.trim_char_arry);
        transform.localScale = Vector3.one;
        MyCard_OBJ.transform.localScale = Vector3.zero;
        if (CardStatus != "")
        {
            if (CardStatus == "Chaal")
            {
                if(PlayerID.Equals(GS.Inst._userData.Id))
                    Card_Txt_Status_On_button.text = "Chaal";
                Card_Status.text = "Seen";
                Card_See_Eye.SetActive(true);
            }
            else
            {
                if (PlayerID.Equals(GS.Inst._userData.Id))
                    Card_Txt_Status_On_button.text = "Blind";
                Card_Status.text = "Blind";
                Card_See_Eye.SetActive(false);
            }
        }

        //if watch mode user
        string status= info.GetField("status").ToString().Trim(Config.Inst.trim_char_arry);
        if (status.Equals(""))
            is_watch_MODE = true;
        else
            is_watch_MODE = false;
    }

    //-----------Card Deal Animation----------------
    public void ShowCard_DEAL_Animation()
    {
        Vector3 target = transform.position;
        GameObject cardRect = Instantiate(TP_GameManager.Inst.CardBack_Prefhab) as GameObject;
        cardRect.transform.localScale = Vector3.one;
        cardRect.transform.SetParent(TP_GameManager.Inst.CardDeal_StartPOS.transform.parent, false);
        iTween.MoveTo(cardRect.gameObject, iTween.Hash("position", target, "time", 2f, "easetype", iTween.EaseType.easeOutExpo));
        iTween.ScaleTo(cardRect.gameObject, iTween.Hash("scale", Vector3.zero, "time", 0.1f, "delay", 0.5f));
    }
    //-----------Card Deal Animation----------------

    //-----------Card Chal Animation----------------
    public void Chaal_Animation(string Chaa_Amount,int seatIndex)
    {
        TP_SoundManager.Inst.PlaySFX_Others(9);
        if (GS.Inst._userData.MySeatIndex != seatIndex)
            Txt_UserChips.text = Chaa_Amount.ToString();
        Vector3 target =TP_GameManager.Inst.Chaal_AnimMove_Object.transform.position;
        GameObject cardRect = Instantiate(TP_GameManager.Inst.Chaal_Coin_PFB) as GameObject;
        cardRect.transform.GetComponent<PFB_Chal_Chips>().Chaal_Anim(Chaa_Amount);
        cardRect.transform.SetParent(TP_GameManager.Inst.Chaal_Coin_Perent.transform, false);
        cardRect.transform.position = transform.position;
        cardRect.transform.localScale = Vector3.one;
        iTween.MoveTo(cardRect.gameObject, iTween.Hash("position", target, "time", 2f, "easetype", iTween.EaseType.easeOutExpo));
        iTween.ScaleTo(cardRect.gameObject, iTween.Hash("scale", Vector3.zero, "time", 0.1f, "delay", 0.6f));
    }
    //-----------Card Chal Animation----------------

    //-----------Tip Animation----------------
    public void Tip_Animation(string Chaa_Amount)
    {
        Vector3 target = TP_GameManager.Inst.Tip_AnimMove_Object.transform.position;
        GameObject cardRect = Instantiate(TP_GameManager.Inst.Chaal_Coin_PFB) as GameObject;
        cardRect.transform.GetComponent<PFB_Chal_Chips>().Chaal_Anim(Chaa_Amount);
        cardRect.transform.SetParent(TP_GameManager.Inst.Chaal_Coin_Perent.transform, false);
        cardRect.transform.position = transform.position;
        cardRect.transform.localScale = Vector3.one;
        iTween.MoveTo(cardRect.gameObject, iTween.Hash("position", target, "time", 2f, "easetype", iTween.EaseType.easeOutExpo));
        iTween.ScaleTo(cardRect.gameObject, iTween.Hash("scale", Vector3.zero, "time", 0.1f, "delay", 0.6f));
    }
    //-----------Tip Animation----------------

    //-----------Boot collection Animation----------------
    public void Boot_Collection_Animation(float Boot_Amount)
    {
        if (_SeatStatus == SeatStatus.Seated)
        {
            is_watch_MODE = false;
            if (GS.Inst._userData.MySeatIndex!=SeatIndex)
                Txt_UserChips.text = Boot_Amount.ToString();
            TP_GameManager.Inst.TotalBoot_Plus(Boot_Amount);
            Vector3 target = TP_GameManager.Inst.Chaal_AnimMove_Object.transform.position;
            GameObject cardRect = Instantiate(TP_GameManager.Inst.Chaal_Coin_PFB) as GameObject;
            cardRect.transform.GetComponent<PFB_Chal_Chips>().Chaal_Anim(Boot_Amount.ToString());
            cardRect.transform.SetParent(TP_GameManager.Inst.Chaal_Coin_Perent.transform, false);
            cardRect.transform.position = transform.position;
            cardRect.transform.localScale = Vector3.one;
            iTween.MoveTo(cardRect.gameObject, iTween.Hash("position", target, "time", 2f, "easetype", iTween.EaseType.easeOutExpo));
            iTween.ScaleTo(cardRect.gameObject, iTween.Hash("scale", Vector3.zero, "time", 0.1f, "delay", 0.6f));
        }
    }
    //-----------Boot collection Animation----------------

    //-----------Reset Player seat-------------------
    public void Player_Seat_RESET()
    {
        //SeatIndex = -1;
        PlayerID = "";
        is_watch_MODE = false;
        _SeatStatus = SeatStatus.Empty;
        Txt_UserName.text = "";
        ; Txt_UserChips.text = "";
        WinAnimation_OBJ.SetActive(false);
    }
    //-----------Reset Player seat-------------------


    //-----------Player Card back show-------------------
    public void Show_PlayerCard_Back()
    {
        if(PlayerID!=GS.Inst._userData.Id)
            MyCard_OBJ.transform.localScale = new Vector3(1.5f,1.5f,1);
        else
            MyCard_OBJ.transform.localScale = new Vector3(1.3f,1.3f,1);
        Card_Status.gameObject.transform.parent.localScale = Vector3.one;
        Card_See_Eye.SetActive(false);
        Rest_BackCard();
    }
    void Rest_BackCard()
    {
        for (int i = 0; i < My_Card_IMG_List.Count; i++)
        {
            My_Card_IMG_List[i].sprite = TP_GameManager.Inst.Back_Card_Sprite;
            My_Card_IMG_List[i].color = Color.white;
        }
    }
    void BackCard_GRAY()
    {
        for (int i = 0; i < My_Card_IMG_List.Count; i++)
        {
            My_Card_IMG_List[i].color =Color.gray;
        }
    }
    public void SET_Card_SEE(JSONObject data)
    {
        if (Card_Status.text != "Packed" && Card_Status.text != "Pack" && Card_Status.text != "pack")
        {
            Card_See_Eye.SetActive(false);
            if (PlayerID != GS.Inst._userData.Id)
                MyCard_OBJ.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            else
                MyCard_OBJ.transform.localScale = new Vector3(1.3f, 1.3f, 1);
            for (int i = 0; i < data.GetField("cards").Count; i++)
            {
                string Card = data.GetField("cards")[i].ToString().Trim(Config.Inst.trim_char_arry);
                My_Card_IMG_List[i].sprite = GS.Inst.TP_GetSprite(Card.ToUpper());
            }
        }
    }

    public void RJ_SET_Card_SEE(JSONObject data)
    {
        Card_See_Eye.SetActive(false);
        if (PlayerID != GS.Inst._userData.Id)
            MyCard_OBJ.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        else
            MyCard_OBJ.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        for (int i = 0; i < data.GetField("cards").Count; i++)
        {
                string Card = data.GetField("cards")[i].ToString().Trim(Config.Inst.trim_char_arry);
                My_Card_IMG_List[i].sprite = GS.Inst.TP_GetSprite(Card.ToUpper());
        }
    }

    //-----------Player Card back show-------------------

    //-----------Player Card back Hide-------------------
    public void Hide_PlayerCard_Back()
    {
        Card_See_Eye.SetActive(false);
        MyCard_OBJ.transform.localScale = Vector3.zero;
        Card_Status.text = "Blind";
        if (PlayerID.Equals(GS.Inst._userData.Id))
            Card_Txt_Status_On_button.text = "Blind";
        Rest_BackCard();
    }
    //-----------Player Card back Hide-------------------


    //-----------Player Card PACK-------------------
    public void IM_PCKED()
    {
        if(Card_Status.text!="Seen")
            Rest_BackCard();
        if (PlayerID.Equals(GS.Inst._userData.Id))
            Card_Txt_Status_On_button.text = "Packed";
        Card_Status.text = "Packed";
        Timer_Filler.reset_turn_timer();
        BackCard_GRAY();
    }
    //-----------Player Card PACk-------------------

    //-----------Leave-------------------
    public void IM_LEAVE()
    {
        if (PlayerID.Equals(GS.Inst._userData.Id))
            Card_Txt_Status_On_button.text = "Blind";
        Card_Status.text = "Blind";
        Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[0];
        Hide_PlayerCard_Back();
        Player_Seat_RESET();
        Card_See_Eye.SetActive(false);
        WinAnimation_OBJ.SetActive(false);
        transform.localScale = Vector3.zero;
    }
    //-----------Player Card PACk-------------------


    //------------- Win Animation ----------------
    public void IM_Win()
    {
        WinAnimation_OBJ.SetActive(true);
    }
    public void Stop_Win_Anim()
    {
        WinAnimation_OBJ.SetActive(false);
    }
    //------------- Win Animation ----------------

    public void MY_Timer_Start(float startTime,float endTime,bool Rejoin)
    {
        Timer_Filler.StartTimerAnim(startTime, endTime, Rejoin);
    }
}
