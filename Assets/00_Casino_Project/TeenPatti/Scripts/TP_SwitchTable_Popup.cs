using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TP_SwitchTable_Popup : MonoBehaviour
{
    public static TP_SwitchTable_Popup Inst;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void BTN_Switch_YES()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        PreeLoader.Inst.Show();
        transform.localScale = Vector3.zero;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_SwitchTable());
    }

    public void BTN_Switch_NO()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject,0.3f);
    }
}
