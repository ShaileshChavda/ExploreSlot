using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Crash_Timer : MonoBehaviour
{
    public static Crash_Timer Inst;
    public bool Timer_flag = false;
    public float Current_Ammount;
    public float End_Ammount, TimerCountEndAmount;
    public float speed = 1;
    internal bool check = false;

    //[SerializeField] TextMeshProUGUI TXT_Timer_Counter;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;       
        //StartTimerAnim(20, 20, false);
    }

    internal void StartTimerAnim(float startTimer, float endTimer, bool rejoin)
    {
        reset_turn_timer();
        Debug.Log("StartTimerAnim" + (int)startTimer);
        CrashController.Instance.GameStartTimer((int)startTimer);
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

        timeRemaining = startTimer;
        timerIsRunning = true;
       //CrashController.Instance.gameStartInText.text = "" + TimerCountEndAmount + "s";
       //InvokeRepeating(nameof(Time_Count), 1, 1);
    }
   
    private float timeRemaining = 0;
    public bool timerIsRunning = false;
    public Text timeText;
   
    
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);                
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                isCashInCalled = false;
                CrashController.Instance.gameStartInText.text = "" + 0.0 + "s";                      
            }
        }       
    }
    bool isCashInCalled = false;
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = Mathf.FloorToInt((timeToDisplay * 100) % 100f);       
        string timerWithMS = string.Format("{0:00}:{1:00}", seconds, milliSeconds);

        CrashController.Instance.gameStartInText.text = "" + timerWithMS + "s";
        if (seconds == 2 && !isCashInCalled)
        {
            CrashController.Instance.CashInOnClickBetPlace();
        }
        if (seconds == 1 && !isCashInCalled)
        {
            isCashInCalled = true;           
            CrashController.Instance.CashInEvent();
            Crash_UI_Manager.Inst.BlockUIFull.SetActive(true);
        }
    }
    void Time_Count()
    {
        if (TimerCountEndAmount > 0)
        {
            TimerCountEndAmount--;
            DisplayTime(TimerCountEndAmount);
            //CrashController.Instance.gameStartInText.text = "" + TimerCountEndAmount + "s";
            if (TimerCountEndAmount == 1)
            {
                Debug.Log("TimerCountEndAmount: 1");
                CrashController.Instance.CashInEvent();
            }
            if (TimerCountEndAmount < 1)
            {
                Crash_UI_Manager.Inst.BlockUIFull.SetActive(true);// Start Betting popup avse
                //Crash_UI_Manager.Inst.NEW_ROUND_START_STOP(true, "");
            }
        }
        else
        {
            CrashController.Instance.gameStartInText.text = "" + 0 + "s";
            CancelInvoke(nameof(Time_Count));
        }
    }

    public void reset_turn_timer()
    {
        CancelInvoke(nameof(Time_Count));
        CrashController.Instance.gameStartInText.text = "" + 0.0 + "s";
        Timer_flag = false;
        check = false;
        Current_Ammount = 0;
        TimerCountEndAmount = 0;
        End_Ammount = 0;
    }
}
