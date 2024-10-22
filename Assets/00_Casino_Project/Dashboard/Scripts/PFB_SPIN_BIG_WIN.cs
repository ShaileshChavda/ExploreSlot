using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_SPIN_BIG_WIN : MonoBehaviour
{
    public static PFB_SPIN_BIG_WIN Inst;
    [SerializeField] Text Txt_Time, TxtName, TxtType, TxtPrice;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_RECORD_DATA(JSONObject data)
    {
        Txt_Time.text = data.GetField("create_date").ToString().Trim(Config.Inst.trim_char_arry);
        TxtName.text = data.GetField("name").ToString().Trim(Config.Inst.trim_char_arry);
        TxtType.text = data.GetField("type").ToString().Trim(Config.Inst.trim_char_arry);
        TxtPrice.text = data.GetField("win_amount").ToString().Trim(Config.Inst.trim_char_arry);
    }
}
