using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddBank : MonoBehaviour
{
    public static AddBank Inst;
    [SerializeField] GameObject BankBox_OBJ, UPIBox_OBJ;

    [Header(": BANK DETAILS :")]
    [SerializeField] InputField Input_Account_NO;
    [SerializeField] InputField Input_BankUser_Name;
    [SerializeField] InputField Input_IFSC;
    [SerializeField] InputField Input_BankName;
    [SerializeField] InputField Input_Email;

    [Header(": UPI DETAILS :")]
    [SerializeField] InputField Input_UpiUserName;
    [SerializeField] InputField Input_UPI;
    [SerializeField] InputField Input_Mobile;

    private void Start()
    {
        Inst = this;
    }

    public void OPEN_ADD_ACCOUNY(string Bank_upi)
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(this.gameObject);
        if(Bank_upi.Equals("b"))
            BANK_BOX(true);
        else
            BANK_BOX(false);
    }

    public void CLOSE_ADD_ACCOUNT()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }

    public void BANK_BOX(bool action)
    {
        if (action)
        {
            BankBox_OBJ.transform.localScale = Vector3.one;
            UPIBox_OBJ.transform.localScale = Vector3.zero;
        }
        else
        {
            UPIBox_OBJ.transform.localScale = Vector3.one;
            BankBox_OBJ.transform.localScale = Vector3.zero;
        }
    }

    public void BTN_SAVE_BANK()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.BANK_ADD(Input_Account_NO.text,Input_BankUser_Name.text,Input_IFSC.text,Input_BankName.text,Input_Email.text));
    }
    public void BTN_SAVE_UPI()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.UPI_ADD(Input_UpiUserName.text, Input_UPI.text, Input_Mobile.text));
    }
}
