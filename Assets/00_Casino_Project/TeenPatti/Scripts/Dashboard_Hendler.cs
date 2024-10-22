using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dashboard_Hendler : MonoBehaviour
{
    public static Dashboard_Hendler Inst;
    [SerializeField] Text Txt_Dashborad_balanch,Txt_Dashboard_bonus;
    [SerializeField] InputField Input_Bet_Amount;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;    
    }

    public void Open_Ref_And_Earn_SC()
    {
        GS.Inst.iTwin_Open(GameObject.Find("Ref_And_Earn"));
    }
    public void Close_Ref_And_Earn_SC()
    {
        GS.Inst.iTwin_Close(GameObject.Find("Ref_And_Earn"),0.3f);
    }

    public void Open_Wallete()
    {
        GS.Inst.iTwin_Open(GameObject.Find("Wallet"));
    }
    public void Close_Wallete()
    {
        GS.Inst.iTwin_Close(GameObject.Find("Wallet"), 0.3f);
    }

    public void Open_Mode_Selection()
    {
        GS.Inst.iTwin_Open(GameObject.Find("TeenPatti_Mode_Screen"));
    }

    public void Close_Mode_Selection()
    {
        GS.Inst.iTwin_Close(GameObject.Find("TeenPatti_Mode_Screen"), 0.3f);
    }

    public void Open_Bet_Selection()
    {
        Input_Bet_Amount.text = "";
        GS.Inst.iTwin_Open(GameObject.Find("TeenPatti_Bet_Screen"));
    }

    public void Close_Bet_Selection()
    {
        GS.Inst.iTwin_Close(GameObject.Find("TeenPatti_Bet_Screen"), 0.3f);
    }

    public void PLAY_TEEN_PATTI()
    {

    }
}
