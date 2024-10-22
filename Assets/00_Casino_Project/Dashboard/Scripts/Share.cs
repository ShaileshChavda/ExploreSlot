
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Facebook.Unity;
using System;

public class Share : MonoBehaviour
{
    public static Share Inst;
    [SerializeField] Text Txt_RefCOde, Txt_FrindInvited, TxtStartDate;
    [SerializeField] Image IMG_GreenFiller;
    string watsappLink, FaceBookLink;

    [Header("SHARE LNG")]
    [SerializeField] Text TxtHeader;
    [SerializeField] Text Txt_Msg;
    [SerializeField] Text Txt_Rfcode;
    [SerializeField] Text Txt_WTS_BTN, Txt_FB_BTN, Txt_Share_BTN;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;

        //if (FB.IsInitialized)
        //{
        //    FB.ActivateApp();
        //}
        //else
        //{
        //    FB.Init(InitCallback, OnHideUnity);
        //}
    }
    private string ReplaceFirstOccurrence(string Source, string Find, string Replace)
    {
        string result = "";
        if (Source.Contains(Find))
        {
            int Place = Source.IndexOf(Find);
            result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
        }
        else
        {
            CommenMSG.Inst.MSG("SERVER ERROR", "Server share message is not proper, Please contact us.!");
        }
        return result;
    }
    internal void LoginFB()
    {
        //if (FB.IsLoggedIn)
        //{
        //    Debug.Log("Already login >>>>>>>>>");
        //    GetUserDetail();
        //}
        //else
        //{
        //    FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, AuthCallback);
        //}

    }

    //IEnumerator ValidateAccessTocken(string at)
    //{
        //string url = "https://graph.facebook.com/oauth/access_token_info?client_id=" + FB.AppId + "&access_token=" + at;
        //Debug.Log("FB Access URL ::" + url);
        //WWW www = new WWW(url);
        //yield return www;
        //if (string.IsNullOrEmpty(www.text) != true)
        //{
        //    //  LogOut();
        //    PreeLoader.Inst.Stop_Loader();
        //}
        //else
        //{

        //}

    //}

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    private void InitCallback()
    {
        //if (FB.IsInitialized)
        //{
        //    // Signal an app activation App Event
        //    FB.ActivateApp();
        //    if (AccessToken.CurrentAccessToken != null)
        //    {
        //        StartCoroutine(ValidateAccessTocken(AccessToken.CurrentAccessToken.TokenString));
        //    }
        //    // Continue with Facebook SDK
        //    print("InitCallback");
        //}
        //else
        //{
        //    Debug.Log("Failed to Initialize the Facebook SDK");
        //}
    }

    //void AuthCallback(ILoginResult result)
    //{
    //    if (!string.IsNullOrEmpty(result.Error))
    //    {
    //        Debug.Log("Login Error" + result.Error);
    //        PreeLoader.Inst.Stop_Loader();
    //    }
    //    else
    //    {
    //        if (FB.IsLoggedIn)
    //        {
    //            GetUserDetail();
    //        }
    //        else
    //        {
    //            Debug.Log("Login Cancelled You have cancelled Facebook login.");
    //            PreeLoader.Inst.Stop_Loader();
    //            //Reconnection_SCR.Inst.Loader_OFF();
    //        }
    //    }
    //}

    void GetUserDetail()
    {
        // FB.API("/me/picture?redirect=false&height=200&width=200", HttpMethod.GET, ProfilePhotoCallback);
       // FB.API("me?fields=id,name,email", HttpMethod.GET, UserDetailCallback, new Dictionary<string, string>());
    }

    //void UserDetailCallback(IGraphResult result)
    //{
    //    if (result != null)
    //    {
    //        Debug.Log("Rsult>>" + result);
    //    }
    //    else
    //    {
    //        Debug.Log("PERMISION >>>>>>>>>>>>");
    //        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, AuthCallback);
    //    }
    //}

    public void OPEN_Share_SC()
    {
        SoundManager.Inst.PlaySFX(0);
        CL_ALT();
        LNG_SETUP();
        GS.Inst.iTwin_Open(this.gameObject);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.REFER_INFO());
    }
    public void CLOSE_Share_SC()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }

    public void BTN_Watsapp_Share()
    {
        SoundManager.Inst.PlaySFX(0);
        //string msg = ReplaceFirstOccurrence(GS.Inst.Share_RefCode_MSG, "[refferal_code]", Txt_RefCOde.text);
        //string msg2 = ReplaceFirstOccurrence(msg, "[game_link]",GS.Inst.Game_Download_URL);
        string msg2 = GS.Inst.Game_Download_URL+"?from_refcode="+GS.Inst._userData.MyRefCode+"|channel_code="+GS.Inst._userData.MyChannelCode;
        ShareController.Inst.WhatsAppShare(msg2);
    }
    public void BTN_Facebook_Share()
    {
        SoundManager.Inst.PlaySFX(0);
        //FB.ShareLink(new Uri("https://developers.facebook.com/"));
        //string msg = ReplaceFirstOccurrence(GS.Inst.Share_RefCode_MSG, "[refferal_code]", Txt_RefCOde.text);
        //string msg2 = ReplaceFirstOccurrence(msg, "[game_link]", GS.Inst.Game_Download_URL);
        string msg2 = GS.Inst.Game_Download_URL + "?from_refcode=" + GS.Inst._userData.MyRefCode + "|channel_code=" + GS.Inst._userData.MyChannelCode;
        //FB.FeedShare(
                   //string.Empty,
                   //new Uri(GS.Inst.Game_Download_URL),msg2);
    }
    public void BTN_All_Share()
    {
        SoundManager.Inst.PlaySFX(0);
        //string msg = ReplaceFirstOccurrence(GS.Inst.Share_RefCode_MSG, "[refferal_code]", Txt_RefCOde.text);
        //string msg2 = ReplaceFirstOccurrence(msg, "[game_link]", GS.Inst.Game_Download_URL);
        string msg2 = GS.Inst.Game_Download_URL + "?from_refcode=" + GS.Inst._userData.MyRefCode + "|channel_code=" + GS.Inst._userData.MyChannelCode;
        ShareController.Inst.AllShare(msg2);
    }
    public void BTN_CopyLink_Share()
    {
        SoundManager.Inst.PlaySFX(0);
        //string msg = ReplaceFirstOccurrence(GS.Inst.Share_RefCode_MSG, "[refferal_code]", Txt_RefCOde.text);
        //string msg2 = ReplaceFirstOccurrence(msg, "[game_link]", GS.Inst.Game_Download_URL);
        string msg2 = GS.Inst.Game_Download_URL + "?from_refcode="+GS.Inst._userData.MyRefCode + "|channel_code=" + GS.Inst._userData.MyChannelCode;
        UniClipboard.SetText(msg2);
        GameObject.Find("Cop_Alert").transform.localScale = Vector3.one;
        UniClipboard.GetText();
        Invoke("CL_ALT", 0.5f);
    }
    void CL_ALT()
    {
        GameObject.Find("Cop_Alert").transform.localScale = Vector3.zero;
    }
    public void SET_DATA(JSONObject data)
    {
        Txt_RefCOde.text = data.GetField("refferal_code").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_FrindInvited.text = "Friend invited:" + data.GetField("user_counter").ToString().Trim(Config.Inst.trim_char_arry);
        TxtStartDate.text = "Start time : " + data.GetField("date").ToString().Trim(Config.Inst.trim_char_arry);
        watsappLink = data.GetField("refer_info").GetField("whatsapp_link").ToString().Trim(Config.Inst.trim_char_arry);
        FaceBookLink = data.GetField("refer_info").GetField("facebook_link").ToString().Trim(Config.Inst.trim_char_arry);
        int count = int.Parse(data.GetField("user_counter").ToString().Trim(Config.Inst.trim_char_arry));
        if (count <= 0)
            IMG_GreenFiller.fillAmount = 0f;
        else if (count > 0 & count <= 1)
            IMG_GreenFiller.fillAmount = 0.26f;
        else if (count > 1 & count <= 2)
            IMG_GreenFiller.fillAmount = 0.59f;
        else if (count > 2)
            IMG_GreenFiller.fillAmount = 1f;
        UniClipboard.SetText(Txt_RefCOde.text);
    }

    void LNG_SETUP()
    {
        switch (PlayerPrefs.GetInt("LNG"))
        {
            case 0:
                //english
                TxtHeader.text = "Refer & Win";
                Txt_Msg.text = "Win bonus for each friend joining the game";
                Txt_Rfcode.text = "Your Code";
                Txt_WTS_BTN.text = "WhatsApp";
                Txt_FB_BTN.text = "Facebook";
                Txt_Share_BTN.text = "Copy Link";
                break;
            case 1:
                //Nepali
                TxtHeader.text = "सन्दर्भ र जीत";
                Txt_Msg.text = "खेलमा सामेल हुने प्रत्येक साथीको लागि बोनस जित्नुहोस्";
                Txt_Rfcode.text = "तपाईंको कोड";
                Txt_WTS_BTN.text = "व्हाट्सएप";
                Txt_FB_BTN.text = "फेसबुक";
                Txt_Share_BTN.text = "लिङ्क प्रतिलिपि गर्नुहोस्";
                break;
            case 2:
                //urdu
                TxtHeader.text = "رجوع کریں اور جیتیں۔";
                Txt_Msg.text = "گیم میں شامل ہونے والے ہر دوست کے لیے بونس جیتیں۔";
                Txt_Rfcode.text = "آپ کا کوڈ";
                Txt_WTS_BTN.text = "واٹس ایپ";
                Txt_FB_BTN.text = "فیس بک";
                Txt_Share_BTN.text = "لنک کاپی کریں۔";
                break;
            case 3:
                //bangali
                TxtHeader.text = "রেফার করুন এবং জয় করুন";
                Txt_Msg.text = "গেমে যোগদানকারী প্রতিটি বন্ধুর জন্য বোনাস জিতুন";
                Txt_Rfcode.text = "তোমার গোপন সংকেত";
                Txt_WTS_BTN.text = "হোয়াটসঅ্যাপ";
                Txt_FB_BTN.text = "ফেসবুক";
                Txt_Share_BTN.text = "লিংক কপি করুন";
                break;
            case 4:
                //Marathi
                TxtHeader.text = "संदर्भ घ्या आिण जिंका";
                Txt_Msg.text = "गेममध्ये सामील होणाऱ्या प्रत्येक मित्रासाठी बोनस जिंका";
                Txt_Rfcode.text = "तुमचा कोड";
                Txt_WTS_BTN.text = "व्हॉट्सअ‍ॅप";
                Txt_FB_BTN.text = "फेसबुक";
                Txt_Share_BTN.text = "लिंक कॉपी करा";
                break;
            case 5:
                //telugu
                TxtHeader.text = "చూడండి & గెలవండి";
                Txt_Msg.text = "గేమ్‌లో చేరిన ప్రతి స్నేహితుడికి బోనస్ గెలుచుకోండి";
                Txt_Rfcode.text = "మీ కోడ్";
                Txt_WTS_BTN.text = "వాట్సాప్";
                Txt_FB_BTN.text = "ఫేస్బుక్";
                Txt_Share_BTN.text = "లింక్ను కాపీ చేయండి";
                break;
            default:
                break;
        }
    }
}
