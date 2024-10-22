using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TP_Exit : MonoBehaviour
{
    public static TP_Exit Inst;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(transform.localScale.x!=0)
                transform.localScale = Vector3.one;
            else
                transform.localScale = Vector3.zero;
        }
    }

    public void BTN_Exit_OK()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        transform.localScale = Vector3.zero;
        PreeLoader.Inst.Show();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_LeaveTable());
    }

    public void BTN_Exit_Cancel()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
}
