using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HR_HistCard : MonoBehaviour
{
    public static HR_HistCard Inst;
    [SerializeField] TextMeshProUGUI Txt_Hist_No;
    [SerializeField] Image IMG;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
        Txt_Hist_No = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        IMG = this.transform.GetComponent<Image>();
    }
    public void SET_HIST(string No)
    {
        Txt_Hist_No.text = No;
        IMG.color = HR_HistoryManager.Inst.Get_Number_Color(int.Parse(No));
    }
}
