using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_REF_RANK : MonoBehaviour
{
    public static PFB_REF_RANK Inst;
    [SerializeField]Text TxtName, TxtChips,TxtGameID;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_RANK_DATA(JSONObject data)
    {
        TxtName.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        TxtChips.text = float.Parse(data.GetField("total_bonus").ToString().Trim(Config.Inst.trim_char_arry)).ToString("n2");
        TxtGameID.text = data.GetField("id").ToString().Trim(Config.Inst.trim_char_arry).ToString();
    }
}
