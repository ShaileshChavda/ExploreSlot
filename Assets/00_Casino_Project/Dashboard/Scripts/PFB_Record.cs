using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_Record : MonoBehaviour
{
    public static PFB_Record Inst;
    [SerializeField] Text TxtOrder, TxtAmount, TxtTime, TxtStatus;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_RECORD_DATA(JSONObject data,int no,string filter)
    {
        TxtOrder.text = no.ToString();
        TxtTime.text = data.GetField("create_date").ToString().Trim(Config.Inst.trim_char_arry);
        if (filter.Equals("deposit"))
        {
            TxtAmount.text = data.GetField("amount").ToString().Trim(Config.Inst.trim_char_arry);
            TxtStatus.text = data.GetField("status").ToString().Trim(Config.Inst.trim_char_arry);
        }
        else
        {
            TxtAmount.text = data.GetField("trnx_amount").ToString().Trim(Config.Inst.trim_char_arry);
            TxtStatus.text = data.GetField("trnx_type_txt").ToString().Trim(Config.Inst.trim_char_arry);
        }
    }
}
