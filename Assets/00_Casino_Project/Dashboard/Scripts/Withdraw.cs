using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Withdraw : MonoBehaviour
{
    public static Withdraw Inst;
    public GameObject GreenSelection;
    [SerializeField] GameObject IMG_Bank, IMG_Upi;
    public GameObject Plus_Bank_BTN, Plus_Upi_BTN;
    public GameObject EDIT_Bank_BTN, EDIT_Upi_BTN;
    public Text TxtTotalBalance, TxtWithdrawBalance;
    public Text TxtAccounterName_Text, Txt_AccountDetails_Text;
    public InputField Input_Amount;
    public Button Button_Withdraw;
    string screenName;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_WITHDRAW_INFO(JSONObject data)
    {
        string type= data.GetField("type").ToString().Trim(Config.Inst.trim_char_arry);
        TxtTotalBalance.text = float.Parse(data.GetField("chips").ToString().Trim(Config.Inst.trim_char_arry)).ToString("n2");
        TxtWithdrawBalance.text = float.Parse(data.GetField("withdrawable_chips").ToString().Trim(Config.Inst.trim_char_arry)).ToString("n2");

        if (type.Equals("bank"))
        {
            if (data.GetField("bank_info").GetField("account_no").ToString().Trim(Config.Inst.trim_char_arry) != "")
            {
                Button_Withdraw.interactable = true;
                Plus_Bank_BTN.SetActive(false);
                Plus_Upi_BTN.SetActive(false);
                EDIT_Upi_BTN.SetActive(false);
                EDIT_Bank_BTN.SetActive(true);

                TxtAccounterName_Text.text = data.GetField("bank_info").GetField("bank_name").ToString().Trim(Config.Inst.trim_char_arry);
                Txt_AccountDetails_Text.text = data.GetField("bank_info").GetField("account_no").ToString().Trim(Config.Inst.trim_char_arry);
            }
            else
            {
                TxtAccounterName_Text.text = "Nothing at all";
                Txt_AccountDetails_Text.text = "Nothing at all";
                Button_Withdraw.interactable = false;
                Plus_Bank_BTN.SetActive(true);
                Plus_Upi_BTN.SetActive(false);
                EDIT_Upi_BTN.SetActive(false);
                EDIT_Bank_BTN.SetActive(false);
            }
        }
        else
        {
            if (data.GetField("upi_info").HasField("upi_id") && data.GetField("upi_info").GetField("upi_id").ToString().Trim(Config.Inst.trim_char_arry) != "")
            {
                Button_Withdraw.interactable = true;
                Plus_Bank_BTN.SetActive(false);
                Plus_Upi_BTN.SetActive(false);
                EDIT_Bank_BTN.SetActive(false);
                EDIT_Upi_BTN.SetActive(true);

                Txt_AccountDetails_Text.text = data.GetField("upi_info").GetField("upi_id").ToString().Trim(Config.Inst.trim_char_arry);
                TxtAccounterName_Text.text = "UPI";//data.GetField("upi_info").GetField("name").ToString().Trim(Config.Inst.trim_char_arry);
            }
            else
            {
                TxtAccounterName_Text.text = "Nothing at all";
                Txt_AccountDetails_Text.text = "Nothing at all";

                Button_Withdraw.interactable = false;
                Plus_Upi_BTN.SetActive(true);
                Plus_Bank_BTN.SetActive(false);
                EDIT_Upi_BTN.SetActive(false);
                EDIT_Bank_BTN.SetActive(false);
            }
        }
    }

    public void OPEN_Withdraw()
    {
        SoundManager.Inst.PlaySFX(0);
        screenName = "bank";
        GS.Inst.iTwin_Open(this.gameObject);
        GreenSelection.transform.localPosition = IMG_Bank.transform.localPosition;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.WITHDRAW_INFO("bank"));
    }
    public void CLOSE_Withdraw()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }

    public void OPEN_BANK()
    {
        SoundManager.Inst.PlaySFX(0);
        TxtAccounterName_Text.text = "Nothing at all";
        Txt_AccountDetails_Text.text = "Nothing at all";
        Button_Withdraw.interactable = false;
        Plus_Bank_BTN.SetActive(true);
        Plus_Upi_BTN.SetActive(false);
        EDIT_Upi_BTN.SetActive(false);
        EDIT_Bank_BTN.SetActive(false);

        screenName = "bank";
        SocketHandler.Inst.SendData(SocketEventManager.Inst.WITHDRAW_INFO("bank"));
        GreenSelection.transform.position = IMG_Bank.transform.position;
    }
    public void OPEN_UPI()
    {
        SoundManager.Inst.PlaySFX(0);
        TxtAccounterName_Text.text = "Nothing at all";
        Txt_AccountDetails_Text.text = "Nothing at all";

        Button_Withdraw.interactable = false;
        Plus_Upi_BTN.SetActive(true);
        Plus_Bank_BTN.SetActive(false);
        EDIT_Upi_BTN.SetActive(false);
        EDIT_Bank_BTN.SetActive(false);
        screenName = "upi";
        SocketHandler.Inst.SendData(SocketEventManager.Inst.WITHDRAW_INFO("upi"));
        GreenSelection.transform.position = IMG_Upi.transform.position;
    }

    public void BTN_ADD_BANK()
    {
        SoundManager.Inst.PlaySFX(0);
        AddBank.Inst.OPEN_ADD_ACCOUNY("b");
    }
    public void BTN_ADD_UPI()
    {
        SoundManager.Inst.PlaySFX(0);
        AddBank.Inst.OPEN_ADD_ACCOUNY("u");
    }

    public void BTN_Withdraw()
    {
        SoundManager.Inst.PlaySFX(0);
        if (Input_Amount.text != "" && Input_Amount.text != " ")
            SocketHandler.Inst.SendData(SocketEventManager.Inst.WITHDRAW_PLACE(screenName, Input_Amount.text));
        else
            Alert_MSG.Inst.MSG("Please enter proper withdrawal amount.!");
    }

    public void UPDATE_DETAILS()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.WITHDRAW_INFO(screenName));
    }
}
