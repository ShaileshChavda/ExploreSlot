using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TP_Tip_Hendler : MonoBehaviour
{
    public static TP_Tip_Hendler Inst;
    [SerializeField] Text Txt_TipAmount;
    int amountNo;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        amountNo = 0;
        Txt_TipAmount.text = GS.Inst.FullTableInfoData.GetField("dealer_tip_slot")[amountNo].ToString().Trim(Config.Inst.trim_char_arry);
    }

    public void OPEN_TIP_BOX()
    {
        GS.Inst.iTwin_Open(gameObject);
    }

    public void CLOSE_TIP_BOX()
    {
        GS.Inst.iTwin_Close(gameObject,0.3f);
    }

    public void BTN_TIP_OK()
    {
        GS.Inst.iTwin_Close(gameObject,0.3f);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_DealerTip(Txt_TipAmount.text));
    }

    public void BTN_TIP_PLUS_MINUS(string pm)
    {
        if (pm.Equals("p") && amountNo < GS.Inst.FullTableInfoData.GetField("dealer_tip_slot").Count)
        {
            amountNo++;
            Txt_TipAmount.text=GS.Inst.FullTableInfoData.GetField("dealer_tip_slot")[amountNo].ToString().Trim(Config.Inst.trim_char_arry);
        }
        else if(amountNo>=1)
        {
            amountNo--;
            Txt_TipAmount.text = GS.Inst.FullTableInfoData.GetField("dealer_tip_slot")[amountNo].ToString().Trim(Config.Inst.trim_char_arry);
        }
    }
}
