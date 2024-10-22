using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_Bonus_Record : MonoBehaviour
{
    public static PFB_Bonus_Record Inst;
    [SerializeField] Text TxtWeekTime, TxtCollectionTime, TxtBonus,TxtType;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_DATA(JSONObject data)
    {
        TxtWeekTime.text = data.GetField("week_format").ToString().Trim(Config.Inst.trim_char_arry);
        TxtCollectionTime.text = data.GetField("collect_date").ToString().Trim(Config.Inst.trim_char_arry);
        TxtBonus.text = data.GetField("amount").ToString().Trim(Config.Inst.trim_char_arry);
        TxtType.text = data.GetField("type_txt").ToString().Trim(Config.Inst.trim_char_arry);
    }
}
