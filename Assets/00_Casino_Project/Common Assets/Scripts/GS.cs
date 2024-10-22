 using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Android;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class UserData
{
    public string Id;
    public string Name;
    public string Mobile;
    public string PicUrl;
    public double Chips;
    public double Bonus;
    public string CurrentTableId;
    public string UID;
    public string MyRefCode;
    public string MyChannelCode;
    public string UPASS;
    public string LoginType;
    public int User_VIP_Level;
    public int MySeatIndex;

    public UserData()
    {
        Id = "";
        Name ="";
        PicUrl = "";
        MyRefCode = "";
        MyChannelCode = "";
        Mobile = "0000041253";
        LoginType = "";
        UID = "";
        UPASS = "";
        Chips = 0;
        Bonus = 0;
        User_VIP_Level = 0;
        CurrentTableId = "";
    }

}
public class GS : MonoBehaviour
{
    public static GS Inst;

    internal UserData _userData = new UserData();
    internal string DEVICE_Id,OS_Info,DEVICE_Info, DEVICE_Type, AT;
    internal bool Rejoin = false;
    public bool Share_Notice_Popup=false;
    public string GameType = "";
    public List<Sprite> VIP_RING_LIST, VIP_LEVEL_LIST;
    public string Game_Download_URL;
    public string Service_link_URL;
    public string Share_RefCode_MSG;
    public string TermsCondition_link_URL;
    public string PrivecyPolicy_link_URL;
    public string Cancellation_policy_link_URL;
    public string AboutUs_link_URL;
    public bool low_balance_warning;
    //------------------TeenPatti Block-------------------
    public bool PrivateTable;
    public int ActivePlayer;
    public List<Sprite> TP_Card_List;
    public JSONObject FullTableInfoData = new JSONObject();
    //------------------TeenPatti Block-------------------

    private void Awake()
    {
        Inst = this;
        Application.targetFrameRate = 60;
        //Read_RefCode_From_textfile();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DEVICE_Id = SystemInfo.deviceUniqueIdentifier;
        Debug.Log("DEVICE ID :" + DEVICE_Id);
        OS_Info = SystemInfo.deviceModel.ToString() + " " + SystemInfo.deviceType.ToString();
        DEVICE_Info = SystemInfo.operatingSystem;
        SetDeviceType();

        #if UNITY_ANDROID
              Application.runInBackground = false;
        #else
              Application.runInBackground = true;
        #endif
    }
    private void Start()
    {

    }
    public void iTwin_Open(GameObject obj)
    {
        iTween.ScaleTo(obj.gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.3f, "easetype", iTween.EaseType.easeOutExpo));
    }

    public void iTwin_Close(GameObject obj, float time)
    {
        iTween.ScaleTo(obj.gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "time", time, "easetype", iTween.EaseType.easeOutExpo));
    }

    void SetDeviceType()
    {
        if (Application.platform == RuntimePlatform.Android)
            DEVICE_Type = "android";
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            DEVICE_Type = "ios";
        else
            DEVICE_Type = "android";
    }

    public string CheckURLContain(string userurl)
    {
        if (NetworkCheck.IsInternetConnection())
            userurl = Config.Inst.BaseURL + userurl;
        return userurl;
    }

    internal string GetIndianFormat(string INR)
    {
        string fare = INR;
        decimal parsed = decimal.Parse(fare, CultureInfo.InvariantCulture);
        CultureInfo hindi = new CultureInfo("hi-IN");
        string text = string.Format(hindi, "{0:c}", parsed);
        //Debug.Log("<Color> text </Color>" + text);
        string[] x = text.Split('₹');
        string tempAmount = x[1];
        string[] y = tempAmount.Split('.');
        double tempPointVal = double.Parse(y[1]);
        //Debug.Log("<Color> total pinted value </Color>" + x[1]+"   brfore point value  >"+ y[0]+"   after point val  >"+y[1]);
        if (tempPointVal <= 0)
            return y[0];
        else
            return x[1];
    }

    //VALID MOBILE VALIDATION
    public static bool IsMobileNumber(string number)
    {
        var regex = new Regex(@"^[0-9]{10}$");
        return regex.IsMatch(number);
    }

    //VALID OTP VALIDATION
    public static bool IsValidOtp(string number)
    {
        var regex = new Regex(@"^[0-9]{6}$");
        return regex.IsMatch(number);
    }


    //VALID EMAIL VALIDATION IS HERE
    public static bool IsValidEmail(string email)
    {
        var regex = new Regex(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
          + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$");
        return regex.IsMatch(email);
    }
    internal string GetCurrentScene
    {
        get { return SceneManager.GetActiveScene().name; }
    }
    internal Sprite TP_GetSprite(string name)
    {
        Sprite sp = null;
        for (int i = 0; i < TP_Card_List.Count; i++)
        {
            if (TP_Card_List[i].name.Equals(name))
                sp = TP_Card_List[i];
        }
        return sp;
    }
}
