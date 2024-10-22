using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AB_Exit_Popup : MonoBehaviour
{
    public static AB_Exit_Popup Inst;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Open_Popup()
    {
        AB_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void Close_Popup()
    {
        AB_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
    public void BTN_YES()
    {
        AB_SoundManager.Inst.PlaySFX(0);
        AB_SoundManager.Inst.StopBG();
        AB_EventSetup.Inst.CLEAR_EVENT_DATA();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.ANDAR_BAHAR_CLOSE_GAME());
        SceneManager.LoadScene(2);
    }
}
