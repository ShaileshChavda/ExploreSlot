using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HR_PFB_LuckyPlayer : MonoBehaviour
{
    public static HR_PFB_LuckyPlayer Inst;
    [SerializeField] TextMeshProUGUI Txt_Time, Txt_Won, Txt_Bet, Txt_x, Txt_UserName;
    [SerializeField] IMGLoader IMG_User_DP;
    [SerializeField] Image Img_Horse;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_LIST_DATA(JSONObject data)
    {
        string[] split_result = data.GetField("result").ToString().Trim(Config.Inst.trim_char_arry).Split("|");
        int result_No = int.Parse(split_result[0]);
        string result_X = split_result[1];

        Txt_Time.text = data.GetField("createdAt").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Won.text = data.GetField("win_amount").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Bet.text = data.GetField("bet_amount").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_x.text = result_X;
        Txt_UserName.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        Img_Horse.sprite = HR_UI_Manager.Inst.Horse_with_No_Box_List[result_No - 1];
        IMG_User_DP.LoadIMG(data.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false,false);
    }
}
