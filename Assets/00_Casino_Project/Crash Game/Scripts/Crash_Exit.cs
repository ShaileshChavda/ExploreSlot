using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crash_Exit : MonoBehaviour
{
    public static Crash_Exit Inst;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Open_Popup()
    {
        Crash_SoundManager.Inst.PlaySFX(2);
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void Close_Popup()
    {
        Crash_SoundManager.Inst.PlaySFX(2);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
    public void BTN_YES()
    {
        Crash_SoundManager.Inst.PlaySFX(2);
        Crash_EventSetup.Inst.CLEAR_EVENT_DATA();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.CRASH_CLOSE_GAME());
        //SceneManager.LoadScene(2);
    }
}
