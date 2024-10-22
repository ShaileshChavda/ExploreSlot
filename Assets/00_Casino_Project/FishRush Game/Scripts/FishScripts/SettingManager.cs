using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public int intSelected = 0;
    public Image imgSelected;
    public Sprite[] sptSelected;
    public GameObject[] goModeAry;

    [Space]
    [Space]
    public Image imgSelfMusic;
    public Image imgFluentMusic;
    public Sprite[] sptMusic;

    [Space]
    [Space]
    public Image imgSelfSound;
    public Image imgFluentSound;
    public Sprite[] sptSound;

    [Space]
    [Space]
    public Image imgSelfShadow;
    public Image imgFluentShadow;
    public Sprite[] sptShadow;

    [Space]
    [Space]
    public Image imgSelfEffect;
    public Image imgFluentEffect;
    public Sprite[] sptEffect;

    // Start is called before the first frame update
    void OnEnable()
    {

        if (!PlayerPrefs.HasKey("musicSelf"))
        {
            PlayerPrefs.SetInt("musicSelf", 1);
            PlayerPrefs.SetInt("soundSelf", 1);
            PlayerPrefs.SetInt("shadowSelf", 1);
            PlayerPrefs.SetInt("effectSelf", 1);

            PlayerPrefs.SetInt("musicFluent", 0);
            PlayerPrefs.SetInt("soundFluent", 0);
            PlayerPrefs.SetInt("shadowFluent", 0);
            PlayerPrefs.SetInt("effectFluent", 0);
        }

        commanMethod();
    }

    public void onClickCloseSetting()
    {
        this.gameObject.SetActive(false);
    }
    public void onClickMode(int val)
    {
        for (int i = 0; i < goModeAry.Length; i++)
        {
            goModeAry[i].SetActive(false);
        }

        intSelected = val;
        imgSelected.sprite = sptSelected[intSelected];

        goModeAry[intSelected].SetActive(true);
    }

    void commanMethod()
    {
        onClickMode(0);
        
        int msIndex = PlayerPrefs.GetInt("musicSelf");
        imgSelfMusic.sprite = sptMusic[msIndex];

        int mfindex = PlayerPrefs.GetInt("musicFluent");
        imgFluentMusic.sprite = sptMusic[(mfindex)];

        int ssindex = PlayerPrefs.GetInt("soundSelf");
        imgSelfSound.sprite = sptSound[ssindex];

        int sfindex = PlayerPrefs.GetInt("soundFluent");
        imgFluentSound.sprite = sptSound[(sfindex)];

        int sssindex = PlayerPrefs.GetInt("shadowSelf");
        imgSelfShadow.sprite = sptShadow[sssindex];

        int ssfindex = PlayerPrefs.GetInt("shadowFluent");
        imgFluentShadow.sprite = sptShadow[(ssfindex)];

        int esindex = PlayerPrefs.GetInt("effectSelf");
        imgSelfEffect.sprite = sptEffect[esindex];

        int efindex = PlayerPrefs.GetInt("effectFluent");
        imgFluentEffect.sprite = sptEffect[(efindex)];
    }
    public void onClickSelfMusic()
    {
        if (PlayerPrefs.GetInt("musicSelf").Equals(1))
        {
            PlayerPrefs.SetInt("musicSelf", 0);
        }
        else
        {
            PlayerPrefs.SetInt("musicSelf", 1);
        }

        int index = PlayerPrefs.GetInt("musicSelf");
        imgSelfMusic.sprite = sptMusic[index];
    }

    public void onClickFluentMusic()
    {
        if (PlayerPrefs.GetInt("musicFluent").Equals(1))
        {
            PlayerPrefs.SetInt("musicFluent", 0);
        }
        else
        {
            PlayerPrefs.SetInt("musicFluent", 1);
        }

        int index = PlayerPrefs.GetInt("musicFluent");
        imgFluentMusic.sprite = sptMusic[(index)];
    }

    public void onClickSelfSound()
    {
        if (PlayerPrefs.GetInt("soundSelf").Equals(1))
        {
            PlayerPrefs.SetInt("soundSelf", 0);
        }
        else
        {
            PlayerPrefs.SetInt("soundSelf", 1);
        }

        int index = PlayerPrefs.GetInt("soundSelf");
        imgSelfSound.sprite = sptSound[index];
    }

    public void onClickFluentSound()   
    {
        if (PlayerPrefs.GetInt("soundFluent").Equals(1))
        {
            PlayerPrefs.SetInt("soundFluent", 0);
        }
        else
        {
            PlayerPrefs.SetInt("soundFluent", 1);
        }

        int index = PlayerPrefs.GetInt("soundFluent");
        imgFluentSound.sprite = sptSound[(index)];
    }

    public void onClickSelfShadow()
    {
        if (PlayerPrefs.GetInt("shadowSelf").Equals(1))
        {
            PlayerPrefs.SetInt("shadowSelf", 0);
        }
        else
        {
            PlayerPrefs.SetInt("shadowSelf", 1);
        }

        int index = PlayerPrefs.GetInt("shadowSelf");
        imgSelfShadow.sprite = sptShadow[index];
    }

    public void onClickFluentShadow()
    {
        if (PlayerPrefs.GetInt("shadowFluent").Equals(1))
        {
            PlayerPrefs.SetInt("shadowFluent", 0);
        }
        else
        {
            PlayerPrefs.SetInt("shadowFluent", 1);
        }

        int index = PlayerPrefs.GetInt("shadowFluent");
        imgFluentShadow.sprite = sptShadow[(index)];
    }

    public void onClickSelfEffect()
    {
        if (PlayerPrefs.GetInt("effectSelf").Equals(1))
        {
            PlayerPrefs.SetInt("effectSelf", 0);
        }
        else
        {
            PlayerPrefs.SetInt("effectSelf", 1);
        }

        int index = PlayerPrefs.GetInt("effectSelf");
        imgSelfEffect.sprite = sptEffect[index];
    }

    public void onClickFluentEffect()
    {
        if (PlayerPrefs.GetInt("effectFluent").Equals(1))
        {
            PlayerPrefs.SetInt("effectFluent", 0);
        }
        else
        {
            PlayerPrefs.SetInt("effectFluent", 1);
        }

        int index = PlayerPrefs.GetInt("effectFluent");
        imgFluentEffect.sprite = sptEffect[(index)];
    }
}
