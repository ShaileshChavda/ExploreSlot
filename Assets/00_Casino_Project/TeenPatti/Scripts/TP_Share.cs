using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class TP_Share : MonoBehaviour
{
    public static TP_Share Inst;

    private void Awake()
    {
        Inst = this;
    }

    public bool CheckIfAppInstalled(string packageName)
    {

        //create a class reference of unity player activity
        AndroidJavaClass unityActivity =
            new AndroidJavaClass("com.unity3d.player.UnityPlayer");
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
            if (packageNew.CompareTo(packageName) == 0)
            {
                return true;
            }
        }
        return false;

    }


    public void FaceBookShare(string msg)
    {
        if (CheckIfAppInstalled("com.facebook.orca"))
        {
            Share_Privet_Open(ImagePath(), msg, "com.facebook.orca");
        }
        else
        {
            Alert_MSG.Inst.MSG("Facebook Messenger is not install on Your Device!");
        }
    }

    public void WhatsAppShare(string msg)
    {
        if (CheckIfAppInstalled("com.whatsapp"))
        {
            Share_Privet_Open(ImagePath(), msg, "com.whatsapp");
        }
        else
        {
            Alert_MSG.Inst.MSG("Whatsapp is not install on Your Device!");
        }

    }
    public void TelegramAppShare(string msg)
    {
        if (CheckIfAppInstalled("org.telegram.messenger"))
        {
            Share_Privet_Open(ImagePath(), msg, "org.telegram.messenger");
        }
        else
        {
            Alert_MSG.Inst.MSG("Telegram is not install on Your Device!");
        }

    }

    public void Share(string msg)
    {
        // AllShare(msg, "Share your room code");
        new NativeShare().AddFile(ImagePath()).SetSubject("Share your room code").SetTitle("Share your room code").SetText(msg).Share();
    }


    void Share_Privet_Open(string path, string msg, string packagename)
    {
        new NativeShare().AddFile(path).SetSubject("TeenPatti").SetTitle("TeenPatti").SetText(msg).AddTarget(packagename).Share();
    }

    string ImagePath()
    {
        Texture2D image = Resources.Load("ShareWithCode", typeof(Texture2D)) as Texture2D;
        string filepath = Path.Combine(Application.temporaryCachePath, "logo.png");
        File.WriteAllBytes(filepath, image.EncodeToPNG());
        return filepath;
    }
}
