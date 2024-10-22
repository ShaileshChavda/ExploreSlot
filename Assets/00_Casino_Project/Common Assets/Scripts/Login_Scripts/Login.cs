using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public static Login Inst;
    [SerializeField] Text Txt_Resend_OTP_TIMER,TXT_Version;
    [SerializeField] InputField INPUT_MOBILE;
    [SerializeField] InputField INPUT_OTP;
    [SerializeField] Toggle TOG_REMEMBER;
    public GameObject Refer_Alert_SC;
    string refcode = "";
    bool ref_Available = false;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        TXT_Version.text ="V."+Config.Inst.Version.ToString();

        refcode = UniClipboard.GetText();
        if (refcode.Length == 9)
            ref_Available = true;
        else
        {
            ref_Available = false;
            if(!PlayerPrefs.HasKey("Last_Login_User"))
                Open_Refer_Alert();
        }
    }
    //public void BTN_GET_OTP()
    //{
    //    PreeLoader.Inst.Show();
    //    SocketHandler.Inst.SendData(SocketEventManager.Inst.SEND_OTP("signup",INPUT_MOBILE.text));
    //}
    //public void BTN_VERIFY_OTP()
    //{
    //    PreeLoader.Inst.Show();
    //    SocketHandler.Inst.SendData(SocketEventManager.Inst.VERIFY_OTP(INPUT_OTP.text, INPUT_MOBILE.text));
    //}
    public void BTN_GUEST_LOGIN()
    {
        PreeLoader.Inst.Show();
        if(ref_Available)
            SocketHandler.Inst.SendData(SocketEventManager.Inst.PLAY_AS_GUEST(refcode));
        else
            SocketHandler.Inst.SendData(SocketEventManager.Inst.PLAY_AS_GUEST(""));
    }
    public void OPEN_MOBILE_LOGIN()
    {
        GS.Inst.iTwin_Open(GameObject.Find("Login_Popup"));
    }
    public void CLOSE_MOBILE_LOGIN()
    {
        GS.Inst.iTwin_Close(GameObject.Find("Login_Popup"),0.3f);
    }

    public void Open_Refer_Alert()
    {
        Refer_Alert_SC.transform.localScale = Vector3.one;
    }
    public void Close_Refer_Alert()
    {
        Refer_Alert_SC.transform.localScale = Vector3.zero;
        BTN_GUEST_LOGIN();
    }

    public void BTN_TERMS_CONDITION()
    {
        Application.OpenURL(GS.Inst.TermsCondition_link_URL);
    }
    public void BTN_PRIVECY_POLICY()
    {
        Application.OpenURL(GS.Inst.PrivecyPolicy_link_URL);
    }
    public void BTN_CENCEL_REFUND()
    {
        Application.OpenURL(GS.Inst.Cancellation_policy_link_URL);
    }
    public void BTN_ABOUT_US()
    {
        Application.OpenURL(GS.Inst.AboutUs_link_URL);
    }
    public void BTN_SERVICE()
    {
        Application.OpenURL(GS.Inst.Service_link_URL);
    }
}
