using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crash_Manu : MonoBehaviour
{
    public static Crash_Manu Inst;
    [SerializeField] GameObject settingMenu;
    [SerializeField] Image IMG_MUSIC, IMG_SOUND;
    [SerializeField] Sprite Music_ON_Sprite, Music_OF_Sprite, Sound_ON_Sprite, Sound_OF_Sprite;
    [SerializeField] RectTransform Rules_SC;
    [SerializeField] RectTransform Rules_Content;

    [Header("Manu")]
    [SerializeField] Text TxtM_Lobby;
    [SerializeField] Text TxtM_Rules;
    [SerializeField] Text TxtM_Music;
    [SerializeField] Text TxtM_Sound;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        LNG_SETUP();
        if (!PlayerPrefs.HasKey("music"))
        {
            PlayerPrefs.SetInt("music", 0);
            PlayerPrefs.SetInt("sound", 0);

            Music_ON_OFF();
            Sound_ON_OFF();
        }
        else
            Saved_Last_Setting();
    }
    bool IsMenuOpen=false;
    public void Open_Manu()
    {
        if (!IsMenuOpen)
        {
            IsMenuOpen = true;
            iTween.MoveTo(settingMenu, iTween.Hash("position", GameObject.Find("SettingDestination").transform.position, "time", 0.3f, "easetype", iTween.EaseType.easeOutExpo));
        }
        else
        {           
            Close_Manu();
        }
    }

    public void Close_Manu()
    {
        IsMenuOpen = false;
        iTween.MoveTo(settingMenu, iTween.Hash("position", GameObject.Find("SettingSource").transform.position, "time", 0.3f, "easetype", iTween.EaseType.easeInExpo));
    }

    public void Rules_Open()
    {
        Rules_Content.gameObject.SetActive(false);
        GS.Inst.iTwin_Open(Rules_SC.gameObject);// GameObject.Find("Rules_SC"));
        Invoke(nameof(Enable_Rules_Scroll), 0.3f);
    }

    void Enable_Rules_Scroll()
    {
        Rules_Content.anchoredPosition = new Vector2(Rules_Content.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        Rules_Content.gameObject.SetActive(true);

    }

    public void Rules_Close()
    {
        GS.Inst.iTwin_Close(Rules_SC.gameObject, 0.3f);// GameObject.Find("Rules_SC"),0.3f);
    }

    public void Music_ON_OFF()
    {
        //SoundManager.Inst.PlaySFX(3);
        if (PlayerPrefs.GetInt("music").Equals(1))
        {
            IMG_MUSIC.sprite = Music_OF_Sprite;
            PlayerPrefs.SetInt("music", 0);
            Crash_SoundManager.Inst.BGAudio.mute = true;
        }
        else
        {
            IMG_MUSIC.sprite = Music_ON_Sprite;
            PlayerPrefs.SetInt("music", 1);
            Crash_SoundManager.Inst.BGAudio.mute = false;
        }
    }

    public void Sound_ON_OFF()
    {
        //SoundManager.Inst.PlaySFX(3);
        if (PlayerPrefs.GetInt("sound").Equals(1))
        {
            IMG_SOUND.sprite = Sound_OF_Sprite;
            PlayerPrefs.SetInt("sound", 0);
            Crash_SoundManager.Inst.SFXAudio.mute = true;
        }
        else
        {
            IMG_SOUND.sprite = Sound_ON_Sprite;
            PlayerPrefs.SetInt("sound", 1);
            Crash_SoundManager.Inst.SFXAudio.mute = false;
        }
    }


    public void Saved_Last_Setting()
    {
        if (PlayerPrefs.GetInt("music").Equals(0))
        {
            IMG_MUSIC.sprite = Music_OF_Sprite;
            PlayerPrefs.SetInt("music", 0);
            Crash_SoundManager.Inst.BGAudio.mute = true;
        }
        else
        {
            IMG_MUSIC.sprite = Music_ON_Sprite;
            PlayerPrefs.SetInt("music", 1);
            Crash_SoundManager.Inst.BGAudio.mute = false;
        }

        if (PlayerPrefs.GetInt("sound").Equals(0))
        {
            IMG_SOUND.sprite = Sound_OF_Sprite;
            PlayerPrefs.SetInt("sound", 0);
            Crash_SoundManager.Inst.SFXAudio.mute = true;
            Crash_SoundManager.Inst.SFX_OHERS.mute = true;
        }
        else
        {
            IMG_SOUND.sprite = Sound_ON_Sprite;
            PlayerPrefs.SetInt("sound", 1);
            Crash_SoundManager.Inst.SFXAudio.mute = false;
            Crash_SoundManager.Inst.SFX_OHERS.mute = false;
        }
    }
    void LNG_SETUP()
    {
        switch (PlayerPrefs.GetInt("LNG"))
        {
            case 0:
                //english
                TxtM_Lobby.text = "Lobby";
                TxtM_Rules.text = "Rules";
                TxtM_Music.text = "Music";
                TxtM_Sound.text = "Sound";
                break;
            case 1:
                //Nepali
                TxtM_Lobby.text = "लबी";
                TxtM_Rules.text = "नियमहरू";
                TxtM_Music.text = "संगीत";
                TxtM_Sound.text = "ध्विन";
                break;
            case 2:
                //urdu
                TxtM_Lobby.text = "لابی";
                TxtM_Rules.text = "قواعد";
                TxtM_Music.text = "آواز";
                TxtM_Sound.text = "موسیقی";
                break;
            case 3:
                //bangali
                TxtM_Lobby.text = "লবি";
                TxtM_Rules.text = "নিয়ম";
                TxtM_Music.text = "শব্দ";
                TxtM_Sound.text = "সঙ্গীত";
                break;
            case 4:
                //Marathi
                TxtM_Lobby.text = "लॉबी";
                TxtM_Rules.text = "नियम";
                TxtM_Music.text = "संगीत";
                TxtM_Sound.text = "आवाज";
                break;
            case 5:
                //telugu
                TxtM_Lobby.text = "లాబీ";
                TxtM_Rules.text = "నియమాలు";
                TxtM_Music.text = "ధ్వని";
                TxtM_Sound.text = "సంగీతం";
                break;
            default:
                break;
        }
    }
}
