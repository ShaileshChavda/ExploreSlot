using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_Weekly_Extrabonus : MonoBehaviour
{
    public static PFB_Weekly_Extrabonus Inst;
    [SerializeField] Text TxtBonusFrome, TxtBonusTo,TxtWeeklyExtraBonus;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_DATA(JSONObject data)
    {
        TxtBonusFrome.text = data.GetField("start_value").ToString().Trim(Config.Inst.trim_char_arry);
        TxtBonusTo.text = data.GetField("end_value").ToString().Trim(Config.Inst.trim_char_arry);
        TxtWeeklyExtraBonus.text = data.GetField("bonus").ToString().Trim(Config.Inst.trim_char_arry);
    }
}
