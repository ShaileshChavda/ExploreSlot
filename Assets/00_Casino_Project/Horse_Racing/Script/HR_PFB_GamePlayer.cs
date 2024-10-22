using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HR_PFB_GamePlayer : MonoBehaviour
{
    public static HR_PFB_GamePlayer Inst;
    [SerializeField] TextMeshProUGUI Txt_Name, Txt_Chips;
    [SerializeField] IMGLoader IMG_User_DP;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_LIST_DATA(JSONObject data)
    {
        Txt_Name.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Chips.text = data.GetField("win_amount").ToString().Trim(Config.Inst.trim_char_arry);
        IMG_User_DP.LoadIMG(data.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false, false);
    }
}
