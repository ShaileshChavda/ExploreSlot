using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HR_Exit_Popup : MonoBehaviour
{
    public static HR_Exit_Popup Inst;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Open_Popup()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void Close_Popup()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
    public void BTN_YES()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        HR_SoundManager.Inst.StopBG();
        HR_EventSetup.Inst.CLEAR_EVENT_DATA();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.HORSE_RACING_CLOSE_GAME());
        SceneManager.LoadScene(2);
    }
}
