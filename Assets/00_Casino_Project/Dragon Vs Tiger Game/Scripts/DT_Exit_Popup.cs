using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DT_Exit_Popup : MonoBehaviour
{
    public static DT_Exit_Popup Inst;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Open_Popup()
    {
        DT_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void Close_Popup()
    {
        DT_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
    public void BTN_YES()
    {
        DT_SoundManager.Inst.PlaySFX(0);
        DT_SoundManager.Inst.StopBG();
        DT_EventSetup.Inst.CLEAR_EVENT_DATA();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.DRAGON_TIGER_CLOSE_GAME());
        SceneManager.LoadScene(2);
    }
}
