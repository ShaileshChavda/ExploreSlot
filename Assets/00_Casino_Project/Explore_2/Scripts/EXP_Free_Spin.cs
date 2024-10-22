using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EXP_Free_Spin : MonoBehaviour
{
    public static EXP_Free_Spin Inst;
    public TextMeshProUGUI Txt_FreeSpin_Count;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void OPEN_FREE_SPIN(int count)
    {
        Txt_FreeSpin_Count.text = count.ToString();
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void CLOSE_FREE_SPIN()
    {
        GS.Inst.iTwin_Close(this.gameObject, 0.2f);
    }
}
