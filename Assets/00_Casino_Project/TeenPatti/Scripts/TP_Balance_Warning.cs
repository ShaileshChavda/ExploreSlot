using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TP_Balance_Warning : MonoBehaviour
{
    public static TP_Balance_Warning Inst;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void Open_Warning()
    {
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void Close_Warning()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject,0.3f);
    }

    public void BTN_YES()
    {
        Close_Warning();
        PreeLoader.Inst.Show();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_LeaveTable());
    }
    public void BTN_CANCEL()
    {
        Close_Warning();
        PreeLoader.Inst.Show();
        GS.Inst.low_balance_warning = true;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_LeaveTable());
    }
}
