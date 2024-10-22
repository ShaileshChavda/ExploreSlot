using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_LIVE_SPIN_RECORD : MonoBehaviour
{
    public static PFB_LIVE_SPIN_RECORD Inst;
    [SerializeField] Text Txt_Message, TxtAmount;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_RECORD_DATA(JSONObject data)
    {
        Txt_Message.text = data.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry);
        TxtAmount.text = data.GetField("win_amount").ToString().Trim(Config.Inst.trim_char_arry);
    }
}
