using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot_Big_Items : MonoBehaviour
{
    public static Slot_Big_Items Inst;
    [SerializeField] TextMeshProUGUI TxtPrice;
    [SerializeField] GameObject _X;
    [SerializeField] Image IMG_Result;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_Item_Data(JSONObject data,bool jackpot)
    {
        if (jackpot)
            _X.SetActive(false);
        else
            _X.SetActive(true);
        transform.localScale = Vector3.one;
        TxtPrice.text = data.GetField("reward").ToString().Trim(Config.Inst.trim_char_arry);
        Slot_Manager.Inst.TotalWon_Chips = Slot_Manager.Inst.TotalWon_Chips+double.Parse(data.GetField("win_amount").ToString().Trim(Config.Inst.trim_char_arry));
        Slot_UI_Manager.Inst.Txt_Total_Won.text = Slot_Manager.Inst.TotalWon_Chips.ToString("n2");
        for (int j = 0; j < Slot_UI_Manager.Inst.All_Item_Sprite.Count; j++)
        {
            if (data.GetField("item").ToString().Trim(Config.Inst.trim_char_arry).Equals(Slot_UI_Manager.Inst.All_Item_Sprite[j].name))
                IMG_Result.sprite = Slot_UI_Manager.Inst.All_Item_Sprite[j];
        }
    }
}
