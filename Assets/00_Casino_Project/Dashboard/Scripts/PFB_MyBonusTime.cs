using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_MyBonusTime : MonoBehaviour
{
    public static PFB_MyBonusTime Inst;
    [SerializeField] Text TxtTime, TxtMSG,TxtBonus;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_DATA(JSONObject data)
    {
        TxtTime.text = data.GetField("date").ToString().Trim(Config.Inst.trim_char_arry);
        TxtMSG.text = "BONUS";//data.GetField("trnx_type_txt").ToString().Trim(Config.Inst.trim_char_arry);
        TxtBonus.text = data.GetField("total_amount").ToString().Trim(Config.Inst.trim_char_arry);
    }
}
