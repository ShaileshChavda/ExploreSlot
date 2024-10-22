using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    public static Profile Inst;
    public IMGLoader Main_Profile_Pic, Avatar_Profile_Pic, Heade_Profile_Pic;
    public Image Main_Profile_Pic_Ring, Avatar_Profile_Ring, Heade_Profile_Ring;
    [Header("Profile")]
    public InputField Input_Name;
    public Text TxtUserID;
    public Text TxtCapital;
    public Text TxtBonus;
    public Text TxtMobileBind;
    public InputField InputUpdate_Name;
    [SerializeField] Image IMG_NameEdit;
    [SerializeField] Sprite Sprite_Name_Edit, Sprite_Name_Right;
    public PFB_AVATAR _PFB_AVATAR;
    public RectTransform DataParent;
    public List<GameObject> AvatarCellList;
    [Header("BindMobile")]
    public InputField InputMobile;
    public InputField Input_OTP;

    [Header("REF_AND_EARN LNG")]
    [SerializeField] Text TxtHeader;
    [SerializeField] Text Txt_Name_L;
    [SerializeField] Text Txt_Balance_L;
    [SerializeField] Text Txt_Bind_Title_L;
    [SerializeField] Text Txt_YES_BTN;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void OPEN_PROFILE()
    {
        PreeLoader.Inst.Show();
        SoundManager.Inst.PlaySFX(0);
        LNG_SETUP();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.PROFILE_INFO());
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void OPEN_PROFILE_MOBILE_BOUND()
    {
        PreeLoader.Inst.Show();
        LNG_SETUP();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.PROFILE_INFO());
        GS.Inst.iTwin_Open(this.gameObject);
        Notice.Inst.CLOSE_NOTICE_SC();
    }
    public void CLOSE_PROFILE()
    {
        SoundManager.Inst.PlaySFX(0);
        Input_Name.interactable = false;
        GS.Inst.iTwin_Close(this.gameObject,0.3f);
    }
    public void OPEN_AVATAR_SC()
    {
        SoundManager.Inst.PlaySFX(0);
        PreeLoader.Inst.Show();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.AVATARS());
        GS.Inst.iTwin_Open(GameObject.Find("AvatarScreen"));
        Input_Name.interactable = false;
    }
    public void CLOSE_AVATAR_SC()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(GameObject.Find("AvatarScreen"),0.3f);
        Clear_OLD_AVATAR();
    }
    public void OPEN_BIND_MOBILE()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(GameObject.Find("MobileBound_Screen"));
    }
    public void CLOSE_BIND_MOBILE()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(GameObject.Find("MobileBound_Screen"), 0.3f);
    }
    public void BTN_EditName()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(GameObject.Find("NameUpdate_POP"));
    }
    public void Close_EDIT_NAME()
    {
        SoundManager.Inst.PlaySFX(0);
        InputUpdate_Name.text = "";
        GS.Inst.iTwin_Close(GameObject.Find("NameUpdate_POP"), 0.3f);
    }

    public void BTN_UPDATE_NAME_OK()
    {
        SoundManager.Inst.PlaySFX(0);
        if (InputUpdate_Name.text.Length >= 5)
        {
            PreeLoader.Inst.Show();
            SocketHandler.Inst.SendData(SocketEventManager.Inst.UPDATE_PROFILE("user_name", InputUpdate_Name.text));
            Close_EDIT_NAME();
        }
        else
            Alert_MSG.Inst.MSG("please enter minimum 5 characters!");
    }

    public void SET_PROFILE_DATA(JSONObject data)
    {
        GS.Inst._userData.PicUrl = data.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry);
        GS.Inst._userData.Name=data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        Input_Name.text = GS.Inst._userData.Name;
        TxtCapital.text = float.Parse(data.GetField("chips").ToString().Trim(Config.Inst.trim_char_arry)).ToString("n2");
        TxtBonus.text = data.GetField("game_winning").ToString().Trim(Config.Inst.trim_char_arry);
        TxtUserID.text = data.GetField("id").ToString().Trim(Config.Inst.trim_char_arry).ToString();
        Main_Profile_Pic_Ring.sprite = GS.Inst.VIP_RING_LIST[GS.Inst._userData.User_VIP_Level];
        Avatar_Profile_Ring.sprite = GS.Inst.VIP_RING_LIST[GS.Inst._userData.User_VIP_Level];
        Heade_Profile_Ring.sprite = GS.Inst.VIP_RING_LIST[GS.Inst._userData.User_VIP_Level];
        Avatar_Profile_Pic.LoadIMG(GS.Inst._userData.PicUrl, false, false);
        Main_Profile_Pic.LoadIMG(GS.Inst._userData.PicUrl, false, false);
        Heade_Profile_Pic.LoadIMG(GS.Inst._userData.PicUrl, false, false);
        DashboardManager.Inst.SET_DASHBOARD_DATA();

        string mobileNo = data.GetField("mobile_number").ToString().Trim(Config.Inst.trim_char_arry);
        if (mobileNo != "")
        {
            TxtMobileBind.text = mobileNo;
            GameObject.Find("BtnMobileBound").transform.localScale = Vector3.zero;
        }
        else {
            TxtMobileBind.text = "Not bound";
            GameObject.Find("BtnMobileBound").transform.localScale = Vector3.one;
        }

    }

    public void MOBILE_BOUND()
    {
        GS.Inst._userData.LoginType = "mobile_number";
        PlayerPrefs.SetString("Last_Login_User", GS.Inst._userData.LoginType);
        PlayerPrefs.SetString("mobile", PhoneOTP_Firebase.Inst.Input_Mobile.text);
        TxtMobileBind.text = PhoneOTP_Firebase.Inst.Input_Mobile.text;
        GameObject.Find("BtnMobileBound").transform.localScale = Vector3.zero;
        GS.Inst.iTwin_Close(GameObject.Find("MobileBound_Screen"), 0.1f);
    }
    public void UPDATE_ALL_DASHBOARD_AVATAR()
    {
        Main_Profile_Pic.LoadIMG(GS.Inst._userData.PicUrl, false, false);
        Avatar_Profile_Pic.LoadIMG(GS.Inst._userData.PicUrl, false, false);
        Heade_Profile_Pic.LoadIMG(GS.Inst._userData.PicUrl, false, false);
    }

    public void SET_AVATAR_LIST(JSONObject data)
    {
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_AVATAR();
        for (int i = 0; i < data.GetField("lists").Count; i++)
        {
            PFB_AVATAR cell = Instantiate(_PFB_AVATAR)as PFB_AVATAR;
            AvatarCellList.Add(cell.gameObject);
            cell.transform.SetParent(DataParent, false);
            cell.Load_Pic(data.GetField("lists")[i].GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry));
        }
        DataParent.anchoredPosition = new Vector2(DataParent.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }
    public void Clear_OLD_AVATAR()
    {
        if (AvatarCellList.Count > 0)
        {
            for (int i = 0; i < AvatarCellList.Count; i++)
            {
                if (AvatarCellList[i] != null)
                    Destroy(AvatarCellList[i]);
            }
            AvatarCellList.Clear();
        }
    }


    void LNG_SETUP()
    {
        switch (PlayerPrefs.GetInt("LNG"))
        {
            case 0:
                //english
                TxtHeader.text = "User";
                Txt_Name_L.text = "Name";
                Txt_Balance_L.text = "Capital";
                Txt_Bind_Title_L.text = "Binding";
                Txt_YES_BTN.text = "Yes";
                break;
            case 1:
                //Nepali
                TxtHeader.text = "प्रयोगकर्ता";
                Txt_Name_L.text = "नाम";
                Txt_Balance_L.text = "राजधानी";
                Txt_Bind_Title_L.text = "बाइन्डिङ";
                Txt_YES_BTN.text = "हो";
                break;
            case 2:
                //urdu
                TxtHeader.text = "صارف";
                Txt_Name_L.text = "نام";
                Txt_Balance_L.text = "سرمایہ";
                Txt_Bind_Title_L.text = "پابند کرنا";
                Txt_YES_BTN.text = "جی ہاں";
                break;
            case 3:
                //bangali 
                TxtHeader.text = "ব্যবহারকারী";
                Txt_Name_L.text = "নাম";
                Txt_Balance_L.text = "মূলধন";
                Txt_Bind_Title_L.text = "বাঁধাই";
                Txt_YES_BTN.text = "হ্যাঁ";
                break;
            case 4:
                //Marathi
                TxtHeader.text = "वापरकर्ता";
                Txt_Name_L.text = "नाव";
                Txt_Balance_L.text = "भांडवल";
                Txt_Bind_Title_L.text = "बंधनकारक";
                Txt_YES_BTN.text = "होय";
                break;
            case 5:
                //telugu
                TxtHeader.text = "వినియోగదారు";
                Txt_Name_L.text = "పేరు";
                Txt_Balance_L.text = "రాజధాని";
                Txt_Bind_Title_L.text = "బైండింగ్";
                Txt_YES_BTN.text = "అవును";
                break;
            default:
                break;
        }
    }
}
