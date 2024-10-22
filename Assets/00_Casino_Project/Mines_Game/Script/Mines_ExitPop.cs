using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mines_ExitPop : MonoBehaviour
{
    public static Mines_ExitPop Inst;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Open_Popup()
    {
        Mines_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void Close_Popup()
    {
        Mines_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
    public void BTN_YES()
    {
        Mines_SoundManager.Inst.PlaySFX(0);
        Mines_SoundManager.Inst.StopBG();
        Mines_EventSetup.Inst.CLEAR_EVENT_DATA();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.MINES_CLOSE_GAME());
        SceneManager.LoadScene(2);
    }
}
