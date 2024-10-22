using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot_SoundManager : MonoBehaviour
{
    public static Slot_SoundManager Inst;
    [SerializeField] public AudioClip[] BG;
    [SerializeField] public AudioClip[] SFX;

    [SerializeField] public AudioSource BGAudio;
    [SerializeField] public AudioSource SFXAudio, SFX_OHERS;


    void Awake()
    {
        Inst = this;
        PlayBG(0);
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
    internal void StopOTHER_SFX()
    {
        SFX_OHERS.Stop();
    }

    internal void PlaySFX(int clipID)
    {
        if (PlayerPrefs.GetInt("sound").Equals(1))
        {
            SFXAudio.clip = SFX[clipID];
            SFXAudio.Play();
        }
    }

    internal void PlaySFX_Others(int clipID)
    {
        if (PlayerPrefs.GetInt("sound").Equals(1))
        {
            SFX_OHERS.clip = SFX[clipID];
            SFX_OHERS.Play();
        }
    }

    internal void MuteUnmuteAudio(bool tag)
    {
        BGAudio.mute = tag;
        SFXAudio.mute = tag;
    }
}
