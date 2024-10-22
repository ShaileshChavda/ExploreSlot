using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeenPatti_Login : MonoBehaviour
{
    public static TeenPatti_Login Inst;
    [SerializeField] GameObject LoginBox, Otp_Box;
    [SerializeField] InputField Input_Mobile, Input_OTP;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void LOGIN()
    {
        Input_OTP.text = "";
        LoginBox.transform.localScale = Vector3.zero;
        GS.Inst.iTwin_Open(Otp_Box);
    }
    public void CHANGE_NUMBER()
    {
        Otp_Box.transform.localScale = Vector3.zero;
        GS.Inst.iTwin_Open(LoginBox);
    }

    public void VERIFY_OTP()
    {
      
    }

    public void RESEND_OTP()
    {
        Input_OTP.text = "";
    }
}
