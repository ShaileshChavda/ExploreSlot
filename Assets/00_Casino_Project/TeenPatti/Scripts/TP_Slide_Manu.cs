using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TP_Slide_Manu : MonoBehaviour
{
    public static TP_Slide_Manu Inst;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void OPEN_Slide_Manu()
    {
        this.transform.localScale = Vector3.one;
    }

    public void CLOSE_Slide_Manu()
    {
        TP_SoundManager.Inst.PlaySFX(0);
        this.transform.localScale = Vector3.zero;
    }
}
