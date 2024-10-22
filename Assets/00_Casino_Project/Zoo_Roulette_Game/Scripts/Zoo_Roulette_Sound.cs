namespace ZooRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Zoo_Roulette_Sound : MonoBehaviour
    {
        public static Zoo_Roulette_Sound Inst;
        [SerializeField] public AudioClip[] BG;
        [SerializeField] public AudioClip[] SFX;

        [SerializeField] public AudioSource BGAudio;
        [SerializeField] public AudioSource SFXAudio, SFX_OHERS,SFX_Timer;
        [SerializeField] public AudioSource auSpin;
        [SerializeField] public AudioSource auBtn;
        [SerializeField] public AudioSource auMixCoin;

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


        internal void PlayReel(float pitch)
        {
            if (PlayerPrefs.GetInt("sound").Equals(1))
            {              
                auSpin.pitch = pitch;               
                auSpin.PlayOneShot(auSpin.clip);
            }
        }

        internal void StopReel()
        {
            auSpin.Stop();
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

       
        internal void PlaySFX_Timer(int clipID)
        {
            if (PlayerPrefs.GetInt("sound").Equals(1))
            {
                SFX_Timer.clip = SFX[clipID];
                SFX_Timer.Play();
            }
        }

        internal void StopSFX_Timer(int clipID)
        {
            if (PlayerPrefs.GetInt("sound").Equals(1))
            {
                SFX_Timer.clip = SFX[clipID];
                SFX_Timer.Stop();
            }
        }

        internal void BtnSFX(int clipID)
        {
            if (PlayerPrefs.GetInt("sound").Equals(1))
            {
                auBtn.clip = SFX[clipID];
                auBtn.Play();
            }
        }

        internal void MuteUnmuteAudio(bool tag)
        {
            BGAudio.mute = tag;
            SFXAudio.mute = tag;
        }

        //internal void PlayMixCoin()
        //{
        //    if (PlayerPrefs.GetInt("sound").Equals(1))
        //    {
        //        auMixCoin.PlayOneShot(auMixCoin.clip);
        //    }
        //}

        internal void PlayMixCoin()
        {
            if (PlayerPrefs.GetInt("sound").Equals(1))
            {
                if (!auMixCoin.isPlaying)
                    auMixCoin.Play();
            }
        }
    }
}