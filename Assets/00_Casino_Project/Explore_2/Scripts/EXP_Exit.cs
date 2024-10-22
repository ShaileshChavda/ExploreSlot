using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EXP_Exit : MonoBehaviour
{
    public static EXP_Exit Inst;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Open_Popup()
    {
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void Close_Popup()
    {
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
    public void BTN_YES()
    {
        //Roullate_SoundManager.Inst.PlaySFX(38);
        //Roullate_EventSetup.Inst.CLEAR_EVENT_DATA();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SLOT_CLOSE_GAME());
        SceneManager.LoadScene(2);
    }
}
