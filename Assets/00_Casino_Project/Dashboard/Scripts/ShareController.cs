using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class ShareController : MonoBehaviour
{
    public static ShareController Inst;
    private bool isFocus = false;
    private bool isProcessing = false;
    private string packageName = "";
    private void Awake()
    {
        Inst = this;
    }
    void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }
    private bool CheckIfAppInstalled()
    {

#if UNITY_ANDROID

        //create a class reference of unity player activity
        AndroidJavaClass unityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        //get the context of current activity
        AndroidJavaObject context = unityActivity.GetStatic<AndroidJavaObject>("currentActivity");

        //get package manager reference
        AndroidJavaObject packageManager = context.Call<AndroidJavaObject>("getPackageManager");

        //get the list of all the apps installed on the device
        AndroidJavaObject appsList = packageManager.Call<AndroidJavaObject>("getInstalledPackages", 1);

        //get the size of the list for app installed apps
        int size = appsList.Call<int>("size");

        for (int i = 0; i < size; i++)
        {
            AndroidJavaObject appInfo = appsList.Call<AndroidJavaObject>("get", i);
            string packageNew = appInfo.Get<string>("packageName");
            Debug.Log("APPs :"+ packageNew);
            if (packageNew.CompareTo(packageName) == 0)
            {
                return true;
            }
        }

        return false;

#endif
        return false;
    }

#if UNITY_ANDROID
    public IEnumerator ShareTextToWhatsContact()
    {

        isProcessing = true;

        if (!Application.isEditor)
        {
            //var url = "https://api.whatsapp.com/send?phone=${mobileNumber}&text=You%20can%20now%20send%20me%20audio%20and%20video%20messages%20on%20the%20app%20-%20Chirp.%20%0A%0Ahttps%3A//bit.ly/chirp_android";
            var url = "https://api.whatsapp.com/send?phone=+919876543210&text=You%20can%20now%20send%20me%20audio%20and%20video%20messages%20on%20the%20app%20-%20Chirp.%20%0A%0Ahttps%3A//bit.ly/chirp_android";

            var shareSubject = "Luck And Skill";
            var shareMessage = "I want to play Luck And Skill with you! Believe me this is awesome game. Please install from Android : ";// + GS.Inst._userData.App_Download_URL;

            //Create intent for action send
            AndroidJavaClass intentClass =
                new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject =
                new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>
            ("setAction", intentClass.GetStatic<string>("ACTION_VIEW"));

            //uri class
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");


            //put text and subject extra
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>
                ("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), shareSubject);
            intentObject.Call<AndroidJavaObject>
                ("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);

            //set data
            intentObject.Call<AndroidJavaObject>("setData",
                uriClass.CallStatic<AndroidJavaObject>("parse", url));

            //set the package to whatsapp package
            intentObject.Call<AndroidJavaObject>("setPackage", packageName);

            //call start activity method
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity =
                unity.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intentObject);
        }

        yield return new WaitUntil(() => isFocus);
        isProcessing = false;
    }

#endif

    public void FaceBookShare(string msg)
    {
        packageName = "com.facebook.katana";
        //if (CheckIfAppInstalled())
        //{
            //ShareTextToWhatsContact();
        //}
        //else
        //{
        //    print("<Color><b> Facebook is not install on Your Device◘◘◘◘◘◘◘ </Color></b>");
        //    CommenMSG.Inst.MSG("Share", "Facebook is not install on Your Device!");
        //}
    }

    public void WhatsAppShare(string msg)
    {
        packageName = "com.whatsapp";
        Application.OpenURL("https://api.whatsapp.com/send?text="+msg);
        //if (CheckIfAppInstalled())
        //{
        //ShareTextToWhatsContact();
        //}
        //else
        //{
        ////    print("<Color><b> Whatsapp is not install on Your Device ◘◘◘◘◘◘◘ </Color></b>");
        //    CommenMSG.Inst.MSG("Share","Whatsapp is not install on Your Device!");
        //}

    }

    public void TelegramAppShare(string msg)
    {
        packageName = "org.telegram.messenger";
        //if (CheckIfAppInstalled())
        //{
            Share_Privet_Open(msg, "org.telegram.messenger");
        //}
        //else
       // {
            //Alert_MSG.Inst.MSG("Telegram is not install on Your Device!");
       // }

    }

    public void Share(string msg)
    {
      // AllShare(msg, "Share your room code");
       // new NativeShare().SetSubject("Share your referral code").SetTitle("Tuk Tuk Ludo").SetText(msg).Share();
    }


    public void MsgShare(string msg)
    {
        Debug.Log("<Color=blue><b>♥ MsgShare ♥</b></Color>");
        try
        {
            AndroidJavaClass intentClass = new
                         AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new
                             AndroidJavaObject("android.content.Intent");
            //set action to that intent object
            intentObject.Call<AndroidJavaObject>
        ("setAction", intentClass.GetStatic<string>("ACTION_VIEW"));
            //var shareSubject = "I challenge you to beat my high score in" +
            //           "Fire Block";
            var shareMessage = msg;
            //set the type as text and put extra subject and text to share
            intentObject.Call<AndroidJavaObject>("setType", "vnd.android-dir/mms-sms");
            intentObject.Call<AndroidJavaObject>("putExtra", "sms_body", shareMessage);
            //intentObject.Call<AndroidJavaObject>("putExtra",
            //intentClass.GetStatic<string>("EXTRA_SUBJECT"), shareSubject);
            AndroidJavaClass unity = new
                    AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity =
                         unity.GetStatic<AndroidJavaObject>("currentActivity");
            //call createChooser method of activity class
            AndroidJavaObject chooser =
                    intentClass.CallStatic<AndroidJavaObject>("createChooser",
                                 intentObject, "Share your referral code");
            currentActivity.Call("startActivity", chooser);
        }
        catch (System.Exception)
        {
            //ErrorMessage.Inst.OpenErrorPopUp("SIM is not insert in your Device", "About Your Device");
            //ErrorMessage.Inst.ButtonPosition(1);
            Debug.Log("SIM is not insert in your Device ◘◘◘◘◘◘◘ ");
            throw;
        }

    }
   
    //public void OnShareButtonClick()
    //{
    //    string msg = "I want to play Ludo legends with you! Believe me this is awesome game. Please install from Android : " + Ludo_GS.Inst.userinfo.App_Download_URL;
    //    new NativeShare().SetSubject("Share your referral code").SetTitle("Tuk Tuk Ludo").SetText(msg).Share();
    //    //AllShare("I want to play Ludo legends with you! Believe me this is awesome game. Please install from Android : " + Ludo_GS.Inst.userinfo.App_Download_URL, "Share Ludo Legends");
    //}

    public void AllShare(string msg)
    {
        Debug.Log("<Color=blue><b>♥ Share ♥</b></Color>");

        Debug.Log("♥ • SHARE BTN CALL • ♥");

        AndroidJavaClass intentClass = new
                     AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new
                         AndroidJavaObject("android.content.Intent");
        //set action to that intent object
        intentObject.Call<AndroidJavaObject>
    ("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

        //var shareSubject = "I challenge you to beat my high score in" +
        //           "Fire Block";
        var shareMessage = msg;

        //set the type as text and put extra subject and text to share
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);

        //intentObject.Call<AndroidJavaObject>("putExtra",
        //intentClass.GetStatic<string>("EXTRA_SUBJECT"), shareSubject);
        AndroidJavaClass unity = new
                AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity =
                     unity.GetStatic<AndroidJavaObject>("currentActivity");
        //call createChooser method of activity class
        AndroidJavaObject chooser =
                intentClass.CallStatic<AndroidJavaObject>("createChooser",
                             intentObject, "Share your referral code");
        currentActivity.Call("startActivity", chooser);
    }

    void Share_Privet_Open(string msg,string packagename)
    {
        new NativeShare().SetSubject("Private Table Code").SetTitle("Share").SetText(msg).AddTarget(packagename).Share();
    }

    string ImagePath()
    {
        Texture2D image = Resources.Load("ShareWithCode", typeof(Texture2D)) as Texture2D;
        string filepath = Path.Combine(Application.temporaryCachePath, "logo.png");
        File.WriteAllBytes(filepath, image.EncodeToPNG());
        return filepath;
    }
}
