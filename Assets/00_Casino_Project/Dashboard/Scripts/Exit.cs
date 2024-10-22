using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public static Exit Inst;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.transform.localScale.x <= 0)
                Open_Popup();
            else
                Close_Popup();
            return;
        }
    }

    public void Open_Popup()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void Close_Popup()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
    public void BTN_YES()
    {
        SoundManager.Inst.PlaySFX(0);
        Application.Quit();
    }
}
