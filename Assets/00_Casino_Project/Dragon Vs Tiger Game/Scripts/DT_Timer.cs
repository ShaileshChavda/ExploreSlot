using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DT_Timer : MonoBehaviour
{
    public static DT_Timer Inst;
    public bool Timer_flag = false;
    public float Current_Ammount;
    public float End_Ammount, TimerCountEndAmount;
    public float speed = 1;
    internal bool check = false;
    bool Last3Sec = false;
    [SerializeField] TextMeshProUGUI TXT_Timer_Counter;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        //StartTimerAnim(20, 20, false);
    }

    internal void StartTimerAnim(float startTimer, float endTimer, bool rejoin)
    {
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
        TXT_Timer_Counter.text = "Start Betting : " + TimerCountEndAmount + "s";
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
            TXT_Timer_Counter.text = "Start Betting : " + TimerCountEndAmount + "s";
            if (!Last3Sec && TimerCountEndAmount < 4)
            {
                Last3Sec = true;
                DT_SoundManager.Inst.PlaySFX_Others(7);
            }
            if (TimerCountEndAmount < 1)
            {
                DT_SoundManager.Inst.StopOTHER_SFX();
                DT_UI_Manager.Inst.NEW_ROUND_START_STOP(true, "");
            }
        }
        else
        {
            DT_UI_Manager.Inst.BetttingBG.SetActive(false);
           // StartCoroutine(FREE_TIME());
           // TXT_Timer_Counter.text = "Start Betting : " + 0 + "s";
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
        Current_Ammount = 0;
        TimerCountEndAmount = 0;
        End_Ammount = 0;
    }
    public void FREE_TIME_Coroutine()
    {
        DT_UI_Manager.Inst.BetttingBG.SetActive(true);
        StartCoroutine(FREE_TIME());
    }
    public IEnumerator FREE_TIME()
    {
        for (int i = 4; i >= 0; i--)
        {
            TXT_Timer_Counter.text = "Free Time "+i;
            yield return new WaitForSeconds(1);
        }
    }
}
