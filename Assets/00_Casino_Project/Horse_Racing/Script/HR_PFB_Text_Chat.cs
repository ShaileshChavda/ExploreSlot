using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HR_PFB_Text_Chat : MonoBehaviour
{
    public static HR_PFB_Text_Chat Inst;
    [SerializeField] TextMeshProUGUI Txt_Msg;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_DATA(string msg)
    {
        Txt_Msg.text = msg;
    }

    public void BTN_SEND_MSG()
    {
        HR_Chat.Inst.Close_Chat();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.HORSE_RACING_CHAT(GS.Inst._userData.Id,"", Txt_Msg.text, false));
    }
}
