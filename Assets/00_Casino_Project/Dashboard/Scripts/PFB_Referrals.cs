using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_Referrals : MonoBehaviour
{
    public static PFB_Referrals Inst;
    [SerializeField] Text TxtID, TxtName, Txt_TodaysBonus, TxtBonus,TxtRefBonus,Txt_Text_Bonus;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_DATA(JSONObject data)
    {
        TxtID.text= data.GetField("user_id").ToString().Trim(Config.Inst.trim_char_arry);
        TxtName.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_TodaysBonus.text = data.GetField("today_bonus").ToString().Trim(Config.Inst.trim_char_arry);
        TxtBonus.text = data.GetField("total_bonus").ToString().Trim(Config.Inst.trim_char_arry);
        TxtRefBonus.text = data.GetField("total_signup_bonus").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Text_Bonus.text = data.GetField("total_tax_bonus").ToString().Trim(Config.Inst.trim_char_arry);
    }
}
