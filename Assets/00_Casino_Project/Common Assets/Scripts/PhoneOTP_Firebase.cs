using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using System.IO;

public class PhoneOTP_Firebase : MonoBehaviour
{
    public static PhoneOTP_Firebase Inst;
    private string phoneId;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    Firebase.Auth.FirebaseAuth auth;
    public string Token_ID;
    PhoneAuthProvider provider;

    public InputField Input_Mobile, Input_OTP;
    public Text msgText;
    public InputField ReferalCode;
    public GameObject RefCode_Lable_OBJ, RefCode_Input_OBJ;
    private void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        //if (UniClipboard.GetText().Length == 9)
        //    ReferalCode.text = UniClipboard.GetText();

        //if (ReferalCode.text.Length == 9)
        //{
        //    RefCode_Lable_OBJ.transform.localScale = Vector3.zero;
        //    RefCode_Input_OBJ.transform.localScale = Vector3.zero;
        //}
        //else
        //{
        //    RefCode_Lable_OBJ.transform.localScale = Vector3.one;
        //    RefCode_Input_OBJ.transform.localScale = Vector3.one;
        //}

        // Read_RefCode_From_textfile();
        //if (!Application.isEditor)
        //{
        //    Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        //    {
        //        dependencyStatus = task.Result;
        //        if (dependencyStatus == Firebase.DependencyStatus.Available)
        //        {
        //            print("Firebase initializing...");
        //            InitializeFirebase();
        //        }
        //        else
        //        {
        //            //print("Could not resolve all Firebase dependencies: " + dependencyStatus, "ERR");
        //        }
        //    });
        //}
    }
    //void InitializeFirebase()
    //{
    //    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    //    auth.StateChanged += AuthStateChanged;
    //    AuthStateChanged(this, null);
    //}
  
    // Track state changes of the auth object.
    //void AuthStateChanged(object sender, System.EventArgs eventArgs)
    //{
    //    print("Auth State Changed");
    //}

    //void OnDestroy()
    //{
    //    if (!Application.isEditor)
    //    {
    //        auth.StateChanged -= AuthStateChanged;
    //        auth = null;
    //    }
    //}

    public void VERIFY_DATA(JSONObject data)
    {
        if (bool.Parse(data.GetField("code_verify").ToString().Trim(Config.Inst.trim_char_arry)))
        {
            msgText.text = "Mobile Verified";
            if (data.GetField("type").ToString().Trim(Config.Inst.trim_char_arry).Equals("signup"))
            {
                if (GS.Inst.GetCurrentScene != "Dashboard")
                    SocketHandler.Inst.SendData(SocketEventManager.Inst.PLAY_AS_MOBILE_NUMBER(Input_Mobile.text, ReferalCode.text));
            }
            else
            {
                if (GS.Inst.GetCurrentScene == "Dashboard")
                    SocketHandler.Inst.SendData(SocketEventManager.Inst.VERIFY_OTP("link", Input_Mobile.text));
            }
        }
    }
    public void BTN_GET_OTP()
    {
        if (Input_Mobile.text == "")
        {
            msgText.text = "Enter Mobile Number";
        }
        else if (Input_Mobile.text.Length < 10)
        {
            msgText.text = "Invalid Mobile Number";
        }
        else
        {
            PreeLoader.Inst.Show();
            //CheckValidate_mobile();
            if (GS.Inst.GetCurrentScene == "Dashboard")
                SocketHandler.Inst.SendData(SocketEventManager.Inst.SEND_OTP("link", Input_Mobile.text));
            else
                SocketHandler.Inst.SendData(SocketEventManager.Inst.SEND_OTP("signup", Input_Mobile.text));
        }
    }
    public void BTN_VERIFY_OTP()
    {
        if (Input_OTP.text == "")
        {
            msgText.text = "Enter OTP";
            return;
        }
        SocketHandler.Inst.SendData(SocketEventManager.Inst.VERIFY_OTP(Input_OTP.text));
        //Credential credential = provider.GetCredential(phoneId, Input_OTP.text);
        //OnVerifyCodeMobile(credential);
    }
   
    //public void CheckValidate_mobile()
    //{
    //    msgText.text = ""; 

    //    provider = PhoneAuthProvider.GetInstance(auth);
    //    Debug.Log("Verify Phone Number" + Input_Mobile.text);     

    //    provider.VerifyPhoneNumber(
    //         new Firebase.Auth.PhoneAuthOptions
    //         {
    //             PhoneNumber = "+91" + Input_Mobile.text,
    //             TimeoutInMilliseconds = 60000,
    //             ForceResendingToken = null
    //         },
    //    verificationCompleted: (credential) =>
    //    {
    //        Debug.Log("Auto-sms-retrieval or instant validation has succeeded (Android only).");
    //        OnVerifyCodeMobile(credential);
    //    },
    //    verificationFailed: (error) =>
    //    {
    //        Debug.Log("The verification code was not sent.");
    //        Debug.Log(error.ToString());
    //        msgText.text = "OTP was not sent";
    //        PreeLoader.Inst.Stop_Loader();
    //    },
    //    codeSent: (id, token) =>
    //    {
    //        Debug.Log("Verification code was successfully sent via SMS.");
    //        Debug.Log("Id: " + id);
    //        Debug.Log("Token: " + token);
    //        msgText.text = "OTP Sent";
    //        phoneId = id;
    //        Token_ID = id;
    //        PreeLoader.Inst.Stop_Loader();
    //    },
    //    codeAutoRetrievalTimeOut: (id) =>
    //    {
    //        print("Auto-sms-retrieval has timed out");
    //        print("Id: " + id);
    //        msgText.text = "OTP has timed out";
    //        PreeLoader.Inst.Stop_Loader();
    //    });
    //}
      
   
    //private void OnVerifyCodeMobile(Credential credential)
    //{
    //    PreeLoader.Inst.Show();
    //    msgText.text = "";

    //    auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task =>
    //    {
    //        if (task.IsFaulted)
    //        {
    //            PreeLoader.Inst.Stop_Loader();
    //            msgText.text = "OTP Wrong";
    //            Debug.Log("SignInWithCredentialAsync encountered an error: " + task.Exception);
    //            return;
    //        }
    //        //VERIFY_ID();
    //        Debug.Log("Phone Sign In successed.");
    //        //PreeLoader.Inst.Stop_Loader();
    //        msgText.text = "Mobile Verified";

    //        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
    //        user.TokenAsync(true).ContinueWith(task2 => {
    //            if (task2.IsCanceled)
    //            {
    //                Debug.Log("TokenAsync was canceled.");
    //                return;
    //            }
    //            if (task2.IsFaulted)
    //            {
    //                Debug.Log("TokenAsync encountered an error: " + task2.Exception);
    //                return;
    //            }
    //            string idToken = task2.Result;
    //            VERIFY_ID();
    //        });
           
    //    });
    //}
    //public void VERIFY_ID()
    //{
    //    Firebase.Auth.FirebaseUser user = auth.CurrentUser;
    //    user.TokenAsync(true).ContinueWith(task => {
    //        if (task.IsCanceled)
    //        {
    //            Debug.Log("TokenAsync was canceled.");
    //            return;
    //        }

    //        if (task.IsFaulted)
    //        {
    //            Debug.Log("TokenAsync encountered an error: " + task.Exception);
    //            return;
    //        }

    //        string idToken = task.Result;
    //        if (GS.Inst.GetCurrentScene == "Dashboard")
    //            SocketHandler.Inst.SendData(SocketEventManager.Inst.VERIFY_OTP("link", Input_Mobile.text, Token_ID));
    //        else
    //            SocketHandler.Inst.SendData(SocketEventManager.Inst.PLAY_AS_MOBILE_NUMBER(Input_Mobile.text, ReferalCode.text, Token_ID));
    //        // Send token to your backend via HTTPS
    //        // ...
    //    });
    //}
}
