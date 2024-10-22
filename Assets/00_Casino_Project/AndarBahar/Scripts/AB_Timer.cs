using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AB_Timer : MonoBehaviour
{
    public static AB_Timer Inst;
    public bool Timer_flag = false;
    public float Current_Ammount;
    public float End_Ammount, TimerCountEndAmount;
    public float speed = 1;
    internal bool check = false;
    bool Last3Sec = false;
    bool _isFree = false;
    [SerializeField] TextMeshProUGUI TXT_Timer_Counter;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        //StartTimerAnim(20, 20, false);
    }

    internal void StartTimerAnim(float startTimer, float endTimer, bool rejoin,bool isFree)
    {
        reset_turn_timer();
        this.transform.localScale = Vector3.one;
        _isFree = isFree;
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
        if(_isFree)
            TXT_Timer_Counter.text = "Free Time 0" + TimerCountEndAmount;
        else
        {
            if (TimerCountEndAmount > 9)
                TXT_Timer_Counter.text = "Start Betting " + TimerCountEndAmount;
            else
                TXT_Timer_Counter.text = "Start Betting 0" + TimerCountEndAmount;
        }
        InvokeRepeating("Time_Count", 1, 1);
    }

    void Update()
    {
        //if (Timer_flag)
        //{
        //    if (Current_Ammount <= End_Ammount)
        //        Current_Ammount += speed * Time.deltaTime;
        //    else
        //        reset_turn_timer();
        //}
    }
    void Time_Count()
    {
        if (TimerCountEndAmount > 0)
        {
            TimerCountEndAmount--;
            if (_isFree)
                TXT_Timer_Counter.text = "Free Time 0" + TimerCountEndAmount;
            else
            {
                if(TimerCountEndAmount>9)
                    TXT_Timer_Counter.text = "Start Betting " + TimerCountEndAmount;
                else
                    TXT_Timer_Counter.text = "Start Betting 0" + TimerCountEndAmount;
            }

            if (!_isFree)
            {
                if (!Last3Sec && TimerCountEndAmount < 4)
                {
                    Last3Sec = true;
                    if (!_isFree)
                        AB_SoundManager.Inst.PlaySFX_Others(1);
                }
                if (TimerCountEndAmount < 1)
                {
                    AB_SoundManager.Inst.StopOTHER_SFX();
                    TXT_Timer_Counter.text = "Start Betting 00";
                    AB_UI_Manager.Inst.NEW_ROUND_START_STOP(true, "");
                }
            }
        }
        else
        {
            if (_isFree)
                TXT_Timer_Counter.text = "Free Time 00";
            else
                HIDE_TIMER();

            CancelInvoke("Time_Count");
        }
    }

    public void reset_turn_timer()
    {
        CancelInvoke("Time_Count");
        //TXT_Timer_Counter.text = "Start Betting : " + 0 + "s";
        Timer_flag = false;
        check = false;
        Last3Sec = false;
        _isFree = false;
        Current_Ammount = 0;
        TimerCountEndAmount = 0;
        End_Ammount = 0;
    }

    public void HIDE_TIMER()
    {
        this.transform.localScale = Vector3.zero;
    }
}
