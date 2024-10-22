using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roullate_UI_Manager : MonoBehaviour
{
    public static Roullate_UI_Manager Inst;
    public Text TxtGameID,TxtVersion,Txt_TotalBet;
    public GameObject _TotalVBet_Chip_Icon;
    [SerializeField] GameObject StartBet, StopBet;
    public GameObject Wait_For_NewRound;
    public Animator Start_Bet_Anim, Stop_Bet_Anim;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        TxtVersion.text ="V "+ Config.Inst.Version.ToString();
    }

    public void Exit_Game()
    {
        Roullate_Exit_Popup.Inst.Open_Popup();
    }

    public void BTN_GROUP_LIST()
    {
        Roullate_SoundManager.Inst.PlaySFX_Others(39);
        Roullate_Online_User_Manager.Inst.BTN_OPEN();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.ROULETTE_JOINED_USER_LISTS());
    }
    public void NEW_ROUND_START_STOP(bool action, string screen)
    {
        if (screen.Equals("sb"))
            Start_Bet_Anim.Play("StartBet_Anim");
        else
            Stop_Bet_Anim.Play("StartBet_Anim");
    }

    //public void NEW_ROUND_START_STOP(bool action,string screen)
    //{
    //    if (screen.Equals("sb"))
    //    {
    //        StartBet.SetActive(true);
    //        StopBet.SetActive(false);
    //    }
    //    else
    //    {
    //        StartBet.SetActive(false);
    //        StopBet.SetActive(true);
    //    }

    //    if (action)
    //        GS.Inst.iTwin_Open(GameObject.Find("VS_SCREEN"));
    //    else
    //        GS.Inst.iTwin_Close(GameObject.Find("VS_SCREEN"), 0.1f);

    //    Invoke("close_START_STOP", 0.7f);
    //}

    void close_START_STOP()
    {
        GS.Inst.iTwin_Close(GameObject.Find("VS_SCREEN"), 0.1f);
    }

    public void Wait_Next_Round_POP(bool action)
    {
        if(action)
            GS.Inst.iTwin_Open(Wait_For_NewRound);
        else
            GS.Inst.iTwin_Close(Wait_For_NewRound,0.2f);
    }
}
