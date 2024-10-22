using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SevenUpDown_Exit_Popup : MonoBehaviour
{
    public static SevenUpDown_Exit_Popup Inst;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Open_Popup()
    {
        SevenUpDown_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void Close_Popup()
    {
        SevenUpDown_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
    public void BTN_YES()
    {
        SevenUpDown_SoundManager.Inst.PlaySFX(0);
        SevenUpDown_SoundManager.Inst.StopBG();
        SevenUpDown_EventSetup.Inst.CLEAR_EVENT_DATA();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SEVEN_UP_CLOSE_GAME());
        SceneManager.LoadScene(2);
    }
}
