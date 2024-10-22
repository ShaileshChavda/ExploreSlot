using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crash_UI_Manager : MonoBehaviour
{
    public static Crash_UI_Manager Inst;
    public Text TxtGameID;
    [SerializeField] GameObject StartBet, StopBet;
    public GameObject BlockUIFull,BlockUIAutoMode;

    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void START_VS_SCREEN(bool action)
    {
        if (action)
            GS.Inst.iTwin_Open(GameObject.Find("VS_SCREEN"));
        else
            GS.Inst.iTwin_Close(GameObject.Find("VS_SCREEN"), 0.1f);
    }
   
    public void NEW_ROUND_START_STOP(bool action, string screen)
    {      
        if (screen.Equals("sb"))
        {
            //StartBet.SetActive(true);
            //StopBet.SetActive(false);
            BlockUIFull.SetActive(false);
        }
        else
        {
            //StartBet.SetActive(false);
           // StopBet.SetActive(true);
            BlockUIFull.SetActive(true);
        }

        /*if (action)
            GS.Inst.iTwin_Open(GameObject.Find("StartSTopBet_SCREEN"));
        else
            GS.Inst.iTwin_Close(GameObject.Find("StartSTopBet_SCREEN"), 0.2f);

        Invoke(nameof(close_START_STOP), 0.6f);*/
    }

    void close_START_STOP()
    {
        GS.Inst.iTwin_Close(GameObject.Find("StartSTopBet_SCREEN"), 0.1f);
    }  

}
