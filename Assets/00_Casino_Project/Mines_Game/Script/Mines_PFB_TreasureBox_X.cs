using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mines_PFB_TreasureBox_X : MonoBehaviour
{
    public static Mines_PFB_TreasureBox_X Inst;
    public Image MyBox_IMG;
    public TextMeshProUGUI Txt_No;
    public Text Txt_X_Value;
    public GameObject Glow;
    public bool TRS_Active;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void My_Box_Avilable(bool action)
    {
        if (action)
        {
            TRS_Active = true;
            MyBox_IMG.color = new Color32(255,255,255,255);
        }
        else
        {
            TRS_Active = false;
            MyBox_IMG.color = new Color32(255,255,255,100);
        }
    }

    public void Active_Glow(bool action)
    {
        if (action)
        {
            Glow.SetActive(true);
        }
        else
        {
            Glow.SetActive(false);
        }
    }
}
