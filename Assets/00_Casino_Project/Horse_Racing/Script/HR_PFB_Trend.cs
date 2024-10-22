using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HR_PFB_Trend : MonoBehaviour
{
    public static HR_PFB_Trend Inst;
    [SerializeField] List<TextMeshProUGUI> Txt_Result;
    [SerializeField] List<GameObject> Jackpot;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_TREND_RESULT(JSONObject data)
    {
       string[] Result=data.ToString().Trim(Config.Inst.trim_char_arry).Split("|");
       int No = int.Parse(Result[0]);  
       string x = Result[1];  
       bool _jackpot = bool.Parse(Result[2]);
       Txt_Result[No - 1].text = x+"x";
       Jackpot[No - 1].SetActive(_jackpot);
    }
}
