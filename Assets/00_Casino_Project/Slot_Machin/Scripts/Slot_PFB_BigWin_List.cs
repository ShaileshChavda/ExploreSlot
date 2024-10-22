using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot_PFB_BigWin_List : MonoBehaviour
{
    public static Slot_PFB_BigWin_List Inst;
    [SerializeField] Image IMG_Crown;
    [SerializeField] TextMeshProUGUI Txt_No,Txt_Name, Txt_Date_time, Txt_Chips;
    [SerializeField] Image OBJ_BigWin;
    [SerializeField] Sprite SP_BigWin, SP_Jackpot, SP_777, SP_Bar;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    public void SET_DATA(JSONObject data,int no)
    {
        if (no < 3)
            IMG_Crown.sprite = Slot_LuckyPlayer.Inst.Crown_SP_List[no];
        else
            IMG_Crown.transform.localScale = Vector3.zero;

        Txt_No.text = (no+1).ToString();
        Txt_Name.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Date_time.text = data.GetField("createdAt").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Chips.text = data.GetField("amount").ToString().Trim(Config.Inst.trim_char_arry);

        string win_type=data.GetField("result_card").ToString().Trim(Config.Inst.trim_char_arry);
        if (win_type.Equals("big_winner"))
            OBJ_BigWin.sprite = SP_BigWin;
        else if (win_type.Equals("jackpot"))
            OBJ_BigWin.sprite = SP_Jackpot;
        else if (win_type.Equals("777"))
            OBJ_BigWin.sprite = SP_777;
        else if (win_type.Equals("bar"))
            OBJ_BigWin.sprite = SP_Bar;
    }
}
