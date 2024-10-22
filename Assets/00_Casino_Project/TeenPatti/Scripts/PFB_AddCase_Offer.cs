using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_AddCase_Offer : MonoBehaviour
{
    public static PFB_AddCase_Offer Inst;
    [SerializeField] Image OfferPic;
    [SerializeField] Text TxtOffer_Btn_Price, Txt_Offer_Price, Txt_Extra_Price;
    private string Offer_ID;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_OFFER_DATA(JSONObject data)
    {
        Debug.Log("D ::" + data);
        Offer_ID = data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
        TxtOffer_Btn_Price.text = data.GetField("amount").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Offer_Price.text = data.GetField("amount").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Extra_Price.text = data.GetField("extra_amount").ToString().Trim(Config.Inst.trim_char_arry);
    }

    public void Buy_OFFER()
    {
        //AddMoneyController.Inst.amountInput.text = Txt_Offer_Price.text;
    }
}
