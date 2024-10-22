using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_SHOP : MonoBehaviour
{
    public static PFB_SHOP Inst;
    [SerializeField]Text Txt_Chips,TxtBonus;
    string PackName;
    [SerializeField]GameObject PlusPoint_OBJ;
    [SerializeField]GameObject Selected_Glow;
    int myIndex;
    string _id;
    float Bonus_F;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        this.GetComponent<Button>().onClick.AddListener(ON_Box_Click);
    }
    public void ON_Box_Click()
    {
        Comen_Event_Setup.Selected_Shop_PFB(myIndex);
    }

    private void OnEnable()
    {
        Comen_Event_Setup._Shop_PFB += IM_SELECTED;
    }

    private void OnDisable()
    {
        Comen_Event_Setup._Shop_PFB -= IM_NOT_SELECTED;
    }

    public void IM_SELECTED(int _index)
    {
        if (_index.Equals(myIndex))
        {
           Shop.Inst.selected_Plan = PackName;
           Selected_Glow.SetActive(true);
           Total_Amount_Calc();
        }
        else
        {
            Selected_Glow.SetActive(false);
        }
    }

    public void IM_NOT_SELECTED(int _index)
    {
        Selected_Glow.SetActive(false);
    }

    public void SET_DATA(JSONObject data,int index)
    {
        Bonus_F = 0;
        myIndex = index;
        PackName = data.GetField("pack_name").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Chips.text = data.GetField("amount").ToString().Trim(Config.Inst.trim_char_arry);
        if (data.GetField("extra_per").ToString().Trim(Config.Inst.trim_char_arry) != "0")
        {
            PlusPoint_OBJ.transform.localScale = Vector3.one;
            TxtBonus.text = "+" + data.GetField("extra_per").ToString().Trim(Config.Inst.trim_char_arry) + "%";
            Bonus_F = float.Parse(data.GetField("extra_per").ToString().Trim(Config.Inst.trim_char_arry));
        }
        else
        {
            Bonus_F = 0;
            TxtBonus.text = "0";
        }

        if (myIndex.Equals(0))
        {
            Shop.Inst.selected_Plan = PackName;
            Selected_Glow.SetActive(true);
            Total_Amount_Calc();
        }
        else
            Selected_Glow.SetActive(false);
    }

   void Total_Amount_Calc()
    {
        Shop.Inst.TxtPrinchipal.text = Txt_Chips.text;
        Shop.Inst.TxtPayAmount.text ="Rs."+Txt_Chips.text;
        Shop.Inst.Final_PayAmount = Txt_Chips.text;

        if (TxtBonus.text != "0")
        {
            Shop.Inst.TxtBonus.text = (float.Parse(Txt_Chips.text)* Bonus_F / 100).ToString();
            Shop.Inst.TxtNewAmount.text = (int.Parse(Shop.Inst.TxtPrinchipal.text) + int.Parse(Shop.Inst.TxtBonus.text)).ToString();
            Shop.Inst.Txt_Withdraw_Amount.text = Shop.Inst.TxtNewAmount.text;
        }
        else
        {
            Bonus_F = 0;
            Shop.Inst.TxtBonus.text = "0";
            Shop.Inst.TxtNewAmount.text = Shop.Inst.TxtPrinchipal.text;
            Shop.Inst.Txt_Withdraw_Amount.text = Shop.Inst.TxtPrinchipal.text;
        }
    }
}
