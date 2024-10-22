using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SevenUpDown_UI_Manager : MonoBehaviour
{
    public static SevenUpDown_UI_Manager Inst;
    public Text TxtGameID;
    public Text Txt_Total_Dragon_PlayCoin, Txt_Total_Tie_PlayCoin, Txt_Total_Tiger_PlayCoin;
    public Text Txt_Total_Dragon_PlayCoinPlayer, Txt_Total_Tie_PlayCoinPlayer, Txt_Total_Tiger_PlayCoinPlayer;
    public GameObject DragonBox_Light, TigerBox_Light, TieBox_Light;
    public Animator Start_Bet_Anim, Stop_Bet_Anim;
    public Text TxtLogMessage;


    public float Get_RNG_Position()
    {
        return Random.Range(-1f, 1f);
    }
    public void SetLogMessage(string eventName)
    {
        TxtLogMessage.text = eventName;
    }
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Exit_Game()
    {
        SevenUpDown_SoundManager.Inst.PlaySFX(0);
        SevenUpDown_Exit_Popup.Inst.Open_Popup();
    }

    public void BTN_GROUP_LIST()
    {
        SevenUpDown_SoundManager.Inst.PlaySFX(0);
        SevenUpDown_Online_User_Manager.Inst.BTN_OPEN();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SEVEN_UP_JOINED_USER_LISTS());
    }
    public void NEW_ROUND_START_STOP(bool action, string screen)
    {
        SevenUpDown_SoundManager.Inst.PlaySFX(1);
        if (screen.Equals("sb"))
            Start_Bet_Anim.Play("StartBet_Anim");
        else
            Stop_Bet_Anim.Play("StartBet_Anim");
    }

    public void SET_PLAYED_TOTAL_CHIPS(JSONObject data)
    {        
        if (data.HasField("two_six"))
        {
            Txt_Total_Dragon_PlayCoin.text = data.GetField("two_six").ToString().Trim(Config.Inst.trim_char_arry);
            Txt_Total_Dragon_PlayCoinPlayer.text = SevenUpDown_PlayerManager.Inst._User_TotalBet_Dragon.ToString();
        }

        if (data.HasField("seven"))
        {
            Txt_Total_Tie_PlayCoin.text = data.GetField("seven").ToString().Trim(Config.Inst.trim_char_arry);
            Txt_Total_Tie_PlayCoinPlayer.text = SevenUpDown_PlayerManager.Inst._User_TotalBet_Tie.ToString();
        }
        if (data.HasField("eight_twelve"))
        {
            Txt_Total_Tiger_PlayCoin.text = data.GetField("eight_twelve").ToString().Trim(Config.Inst.trim_char_arry);
            Txt_Total_Tiger_PlayCoinPlayer.text = SevenUpDown_PlayerManager.Inst._User_TotalBet_Tiger.ToString();
        }
    }


    public void Active_Light_Anim(string DT)
    {
        if (DT.Equals("D"))
        {
            TigerBox_Light.SetActive(false);
            DragonBox_Light.SetActive(true);
        }
        else if (DT.Equals("T")) 
        {
            DragonBox_Light.SetActive(false);
            TigerBox_Light.SetActive(true);
        }
        else if (DT.Equals("Tie"))
        {
            DragonBox_Light.SetActive(false);
            TigerBox_Light.SetActive(false);
            TieBox_Light.SetActive(true);
        }        
    }     

    public void RESET_UI_NEXT_ROUDN()
    {
        Txt_Total_Dragon_PlayCoin.text = "";
        Txt_Total_Tie_PlayCoin.text = "";
        Txt_Total_Tiger_PlayCoin.text = "";
        Txt_Total_Dragon_PlayCoinPlayer.text = "";
        Txt_Total_Tie_PlayCoinPlayer.text = "";
        Txt_Total_Tiger_PlayCoinPlayer.text = "";
        DragonBox_Light.SetActive(false);
        TigerBox_Light.SetActive(false);
        TieBox_Light.SetActive(false);
    }

    public List<GameObject> _goPlaceTextList; 
    private Vector3 v3TimerScale = new Vector3(0.5f, 0.5f, 0.5f); // scale down stage
    private Vector3 _v3DefalutPlaceTextSize = new Vector3(2f, 2f, 1);
    public void PlaceTextAnimation(int _placeTextIndex, float timer)
    {    
        _goPlaceTextList[_placeTextIndex].SetActive(true);
        _goPlaceTextList[_placeTextIndex].transform.DOScale(v3TimerScale, timer).SetEase(Ease.Linear);
        StartCoroutine(ResetPlaceTextAnimation(_placeTextIndex));
    }

    public IEnumerator ResetPlaceTextAnimation(int index)
    {
        yield return new WaitForSeconds(1);
        _goPlaceTextList[index].transform.localScale = _v3DefalutPlaceTextSize;
        _goPlaceTextList[index].SetActive(false);
    }   
}
