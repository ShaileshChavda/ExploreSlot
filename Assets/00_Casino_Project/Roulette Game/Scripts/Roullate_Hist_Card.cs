using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roullate_Hist_Card : MonoBehaviour
{
    public static Roullate_Hist_Card Inst;
    [SerializeField] Text Txt_Hist_No;
    [SerializeField] Image BallIMG;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
        Txt_Hist_No = this.transform.GetChild(0).GetComponent<Text>();
        BallIMG = this.GetComponent<Image>();
    }
    public void SET_HIST(string No)
    {
        Txt_Hist_No.text = No;
        if (No.Equals("0"))
            BallIMG.sprite = Roullate_History_Manager.Inst.Ball_Sprite[0];
        else if(No=="1"||No =="3" || No == "5" || No == "7" || No=="9"||No=="12"|| No == "14" || No == "16" || No == "18" || No == "19" || No == "21" || No == "23" || No == "25" || No == "27" || No == "30" || No == "32" || No == "34" || No == "36")
            BallIMG.sprite = Roullate_History_Manager.Inst.Ball_Sprite[1];
        else
            BallIMG.sprite = Roullate_History_Manager.Inst.Ball_Sprite[2];
    }
}
