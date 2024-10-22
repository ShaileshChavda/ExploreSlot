using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst;
    [SerializeField] public AudioClip[] BG;
    [SerializeField] public AudioClip[] SFX;

    [SerializeField] public AudioSource BGAudio;
    [SerializeField] public AudioSource SFXAudio;

    void Awake()
    {
        Inst = this;
        if (!PlayerPrefs.HasKey("music"))
        {
            PlayerPrefs.SetInt("music", 1);
            PlayerPrefs.SetInt("sound", 1);
        }
    }

    bool isStopBg = false;
    public void PlayBG(int clipID)
    {
        if (PlayerPrefs.GetInt("music").Equals(1))
        {
            BGAudio.clip = BG[clipID];
            BGAudio.Play();
        }
    }


    internal void StopBG()
    {
        BGAudio.Stop();
    }
    internal void StopSFX()
    {
        SFXAudio.Stop();
    }

    internal void PlaySFX(int clipID)
    {
        if (PlayerPrefs.GetInt("sound").Equals(1))
        {
            SFXAudio.clip = SFX[clipID];
            SFXAudio.Play();
        }
    }

    internal void MuteUnmuteAudio(bool tag)
    {
        BGAudio.mute = tag;
        SFXAudio.mute = tag;
    }
}
