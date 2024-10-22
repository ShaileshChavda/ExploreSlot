namespace ZooRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class ZooRouletteTimer : MonoBehaviour
    {
        public static ZooRouletteTimer Inst;
        public bool Timer_flag = false;
        public float Current_Ammount;
        public float End_Ammount, TimerCountEndAmount;
        public float speed = 1;
        internal bool check = false;
        string _TimerType;
        bool Last3Sec = false;
        [SerializeField] private GameObject Obj_Last3Sec;
        [SerializeField] private Text Txt_Last3Sec;
        [SerializeField] public TextMeshProUGUI TXT_Timer_Counter,Tmp_Last3Sec, TXT_Timer_Counter_Pro, Txt_Timer_Status;

        void Start()
        {
            Inst = this;
        }

        internal void StartTimerAnim(float startTimer, float endTimer, bool rejoin, string TimerType)
        {
            _TimerType = TimerType;
            reset_turn_timer();

            if (rejoin)
            {
                Current_Ammount = endTimer - startTimer;
                TimerCountEndAmount = startTimer;
            }
            else
            {
                TimerCountEndAmount = endTimer;
                Current_Ammount = startTimer;
            }

            End_Ammount = endTimer;
            Timer_flag = true;
            check = false;
            TXT_Timer_Counter.text = TimerCountEndAmount.ToString();
            InvokeRepeating("Time_Count", 1, 1);
        }

        void Time_Count()
        {
            if (TimerCountEndAmount > 0)
            {
                TimerCountEndAmount--;
                TXT_Timer_Counter.text = TimerCountEndAmount.ToString();
                // UnityEngine.Debug.Log("TIMER COUNT: " + TimerCountEndAmount);
                if (ZooRoulette_GameManager.instance._timerStatus == TimerStatus.BETTING)
                {
                    if (TimerCountEndAmount > 0 && TimerCountEndAmount < 4)
                    {
                        //UnityEngine.Debug.Log("Last3Sec");
                        Zoo_Roulette_Sound.Inst.PlaySFX_Timer(4);// Play Timer Tic tic tic sound
                        ZooRoulette_UIManager._instance.PlaceTextAnimation(0, 0.5f);
                    }
                    else if (TimerCountEndAmount == 0)
                    {
                        //UnityEngine.Debug.Log("STOP BETTING");
                        ZooRoulette_UIManager._instance.NEW_ROUND_START_STOP(false);
                        Zoo_Roulette_Sound.Inst.StopSFX_Timer(4);
                    }
                }
                else
                {
                    Zoo_Roulette_Sound.Inst.StopSFX_Timer(4); // stop Timer Tic tic tic sound
                }           
            }
            else
            {
                //UnityEngine.Debug.Log("TIMER FINISH");
                //FREE_TIME();
                //TXT_Timer_Counter.text = "Start Betting : " + 0 + "s";
                CancelInvoke("Time_Count");
            }
        }

        public void reset_turn_timer()
        {
            CancelInvoke("Time_Count");
            Timer_flag = false;
            check = false;
            Last3Sec = false;
            Current_Ammount = 0;
            TimerCountEndAmount = 0;
            End_Ammount = 0;
            ZooRoulette_UIManager._instance._placeTextIndex = 0;
            Zoo_Roulette_Sound.Inst.StopSFX_Timer(4); // stop Timer Tic tic tic sound
        }

        public void FREE_TIME()
        {
            UnityEngine.Debug.Log("FREE TIME");
            TXT_Timer_Counter.text = "Free Time";
        }
    }
}