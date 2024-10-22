using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public static Settings Inst;
    [SerializeField] Image IMG_MUSIC, IMG_SOUND;
    [SerializeField] Sprite Music_ON_Sprite, Music_OF_Sprite, Sound_ON_Sprite, Sound_OF_Sprite;
    public GameObject GreenSelection;
    [SerializeField] GameObject IMG_ENGLISH, IMG_NEPALI,IMG_2,IMG_3,IMG_MARATHI,IMG_5;

    [Header("Setting LNG")]
    [SerializeField] Text TxtHeader;
    [SerializeField] Text Txt_Music;
    [SerializeField] Text Txt_Sound;
    [SerializeField] Text Txt_LogOut_BTN;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        if (!PlayerPrefs.HasKey("LNG"))
            PlayerPrefs.SetInt("LNG", 0);

            LNG_SETUP();
            LanguageButton_Action(PlayerPrefs.GetInt("LNG"));

        if (!PlayerPrefs.HasKey("music"))
        {
            PlayerPrefs.SetInt("music", 1);
            PlayerPrefs.SetInt("sound", 1);
        }
        Saved_Last_Setting();
    }
    public void OPEN_SETTING()
    {
        SoundManager.Inst.PlaySFX(0);
        LanguageButton_Action(PlayerPrefs.GetInt("LNG"));
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void CLOSE_SETTING()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }

    public void Btn_LOG_OUT()
    {
        SoundManager.Inst.PlaySFX(0);
        PreeLoader.Inst.Show();
        PlayerPrefs.DeleteAll();
        SocketHandler.Inst.CloseSocket();
        PlayerPrefs.SetString("Last_Login_User", "");
        SceneManager.LoadScene("Login");
    }
    public void BTN_TERMS_CONDITION()
    {
        SoundManager.Inst.PlaySFX(0);
        Application.OpenURL(GS.Inst.TermsCondition_link_URL);
    }
    public void BTN_PRIVECY_POLICY()
    {
        SoundManager.Inst.PlaySFX(0);
        Application.OpenURL(GS.Inst.PrivecyPolicy_link_URL);
    }
    public void BTN_CENCEL_REFUND()
    {
        SoundManager.Inst.PlaySFX(0);
        Application.OpenURL(GS.Inst.Cancellation_policy_link_URL);
    }
    public void BTN_ABOUT_US()
    {
        SoundManager.Inst.PlaySFX(0);
        Application.OpenURL(GS.Inst.AboutUs_link_URL);
    }
    public void BTN_SERVICE()
    {
        SoundManager.Inst.PlaySFX(0);
        Application.OpenURL(GS.Inst.Service_link_URL);
    }

    public void LanguageButton_Action(int index)
    {
        SoundManager.Inst.PlaySFX(0);
        PlayerPrefs.SetInt("LNG", index);
        switch (index)
        {
            case 0:
                GreenSelection.transform.localPosition = IMG_ENGLISH.transform.localPosition;
                break;
            case 1:
                GreenSelection.transform.localPosition = IMG_NEPALI.transform.localPosition;
                break;
            case 2:
                GreenSelection.transform.localPosition = IMG_2.transform.localPosition;
                break;
            case 3:
                GreenSelection.transform.localPosition = IMG_3.transform.localPosition;
                break;
            case 4:
                GreenSelection.transform.localPosition = IMG_MARATHI.transform.localPosition;
                break;
            case 5:
                GreenSelection.transform.localPosition = IMG_5.transform.localPosition;
                break;
        }
        LNG_SETUP();
    }

    public void Music_ON_OFF()
    {
        SoundManager.Inst.PlaySFX(0);
        if (PlayerPrefs.GetInt("music").Equals(1))
        {
            IMG_MUSIC.sprite = Music_OF_Sprite;
            PlayerPrefs.SetInt("music", 0);
            SoundManager.Inst.BGAudio.mute = true;
        }
        else
        {
            IMG_MUSIC.sprite = Music_ON_Sprite;
            PlayerPrefs.SetInt("music", 1);
            SoundManager.Inst.BGAudio.mute = false;
        }
    }

    public void Sound_ON_OFF()
    {
        SoundManager.Inst.PlaySFX(0);
        if (PlayerPrefs.GetInt("sound").Equals(1))
        {
            IMG_SOUND.sprite = Sound_OF_Sprite;
            PlayerPrefs.SetInt("sound", 0);
            SoundManager.Inst.SFXAudio.mute = true;
        }
        else
        {
            IMG_SOUND.sprite = Sound_ON_Sprite;
            PlayerPrefs.SetInt("sound", 1);
            SoundManager.Inst.SFXAudio.mute = false;
        }
    }


    public void Saved_Last_Setting()
    {
        if (PlayerPrefs.GetInt("music").Equals(0))
        {
            IMG_MUSIC.sprite = Music_OF_Sprite;
            PlayerPrefs.SetInt("music", 0);
            SoundManager.Inst.BGAudio.mute = true;
        }
        else
        {
            IMG_MUSIC.sprite = Music_ON_Sprite;
            PlayerPrefs.SetInt("music", 1);
            SoundManager.Inst.BGAudio.mute = false;
        }

        if (PlayerPrefs.GetInt("sound").Equals(0))
        {
            IMG_SOUND.sprite = Sound_OF_Sprite;
            PlayerPrefs.SetInt("sound", 0);
            SoundManager.Inst.SFXAudio.mute = true;
        }
        else
        {
            IMG_SOUND.sprite = Sound_ON_Sprite;
            PlayerPrefs.SetInt("sound", 1);
            SoundManager.Inst.SFXAudio.mute = false;
        }
    }

    void LNG_SETUP()
    {
        switch (PlayerPrefs.GetInt("LNG"))
        {
            case 0:
                //english
                TxtHeader.text = "Setting";
                Txt_Music.text = "Music";
                Txt_Sound.text = "Sound";
                Txt_LogOut_BTN.text = "Logout";
                break;
            case 1:
                //Nepali
                TxtHeader.text = "सेटिङ";
                Txt_Music.text = "संगीत";
                Txt_Sound.text = "ध्विन";
                Txt_LogOut_BTN.text = "बाहिर निस्कनु";
                break;
            case 2:
                //Urdu
                TxtHeader.text = "ترتیب";
                Txt_Music.text = "آواز";
                Txt_Sound.text = "موسیقی";
                Txt_LogOut_BTN.text = "لاگ آوٹ";
                break;
            case 3:
                //Bangali
                TxtHeader.text = "বিন্যাস";
                Txt_Music.text = "শব্দ";
                Txt_Sound.text = "সঙ্গীত";
                Txt_LogOut_BTN.text = "প্রস্থান";
                break;
            case 4:
                //Marathi
                TxtHeader.text = "सेटिंग";
                Txt_Music.text = "संगीत";
                Txt_Sound.text = "आवाज";
                Txt_LogOut_BTN.text = "बाहेर पडणे";
                break;
            case 5:
                //Telugu
                TxtHeader.text = "అమరిక";
                Txt_Music.text = "ధ్వని";
                Txt_Sound.text = "సంగీతం";
                Txt_LogOut_BTN.text = "లాగ్అవుట్";
                break;
            default:
                TxtHeader.text = "Setting";
                Txt_Music.text = "Music";
                Txt_Sound.text = "Sound";
                Txt_LogOut_BTN.text = "Logout";
                break;
        }
    }
}
