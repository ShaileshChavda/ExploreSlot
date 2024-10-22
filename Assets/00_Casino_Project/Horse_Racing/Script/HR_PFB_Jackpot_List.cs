using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HR_PFB_Jackpot_List : MonoBehaviour
{
    public static HR_PFB_Jackpot_List Inst;
    [SerializeField] TextMeshProUGUI Txt_Time, Txt_Rewards, Txt_Name, Txt_Bet_Get, Txt_Winners;
    [SerializeField] Image Img_Winner_Horse;
    [SerializeField] IMGLoader User_DP;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_LIST_DATA(JSONObject data)
    {
        Txt_Time.text = data.GetField("time").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Name.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Rewards.text = data.GetField("jackport_amount").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Bet_Get.text = "BET: "+data.GetField("bet").ToString().Trim(Config.Inst.trim_char_arry)+" GET: "+ data.GetField("win_amount").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Winners.text = data.GetField("winners").ToString().Trim(Config.Inst.trim_char_arry);
        string[] no=data.GetField("multiples").ToString().Trim(Config.Inst.trim_char_arry).Split('|');
        int result_No = int.Parse(no[0]);
        Img_Winner_Horse.sprite = HR_UI_Manager.Inst.Horse_with_No_Box_List[result_No];
        User_DP.LoadIMG(data.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false, false);
    }
}
