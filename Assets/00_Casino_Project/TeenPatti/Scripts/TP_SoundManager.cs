using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_SoundManager : MonoBehaviour
{
    public static TP_SoundManager Inst;
    [SerializeField] public AudioClip[] BG;
    [SerializeField] AudioClip[] SFX;

    [SerializeField] public AudioSource BGAudio;
    [SerializeField] public AudioSource SFXAudio, SFX_OHERS;


    void Awake()
    {

        // If there is not already an instance of SoundManager, set it to this.
        //if (Inst == null)
        // {
        Inst = this;
        PlayBG(0);
        //}
        ////If an instance already exists, destroy whatever this object is to enforce the singleton.
        //else if (Inst != this)
        //{
        //    Destroy(gameObject);
        //}

        ////Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        //DontDestroyOnLoad(gameObject);

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
