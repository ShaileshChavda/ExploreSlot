using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Roullate_Timer : MonoBehaviour
{
    public static Roullate_Timer Inst;
    public bool Timer_flag = false;
    public float Current_Ammount;
    public float End_Ammount, TimerCountEndAmount;
    public float speed = 1;
    internal bool check = false;
    [SerializeField] GameObject Obj_Last3Sec;
    [SerializeField] Text TXT_Timer_Counter,Txt_Last3Sec;
    [SerializeField]public TextMeshProUGUI Tmp_Last3Sec, TXT_Timer_Counter_Pro,Txt_Timer_Status;
    string _TimerType;
    public bool Last3Sec = false;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        //StartTimerAnim(20, 20, false);
    }

    internal void StartTimerAnim(float startTimer, float endTimer, bool rejoin,string TimerType)
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
        TXT_Timer_Counter_Pro.text = TimerCountEndAmount.ToString();
        InvokeRepeating("Time_Count", 1, 1);
       // InvokeRepeating(nameof(Time_Count), 1, 1);
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
            TXT_Timer_Counter_Pro.text = TimerCountEndAmount.ToString();

            if (_TimerType.Equals("M"))
            {
                if (!Last3Sec && TimerCountEndAmount < 6)
                {
                    Last3Sec = true;
                    Roullate_SoundManager.Inst.PlaySFX_Others(41);
                }

                if (TimerCountEndAmount < 1)
                {
                    Roullate_UI_Manager.Inst.NEW_ROUND_START_STOP(true, "");
                    Roullate_SoundManager.Inst.StopOTHER_SFX();
                }

                if (TimerCountEndAmount > 0 && TimerCountEndAmount <= 3)
                    Last_3Sec_Pop(TimerCountEndAmount);

            }
        }
        else
        {
            TXT_Timer_Counter_Pro.text ="0";
            Obj_Last3Sec.transform.localScale = Vector3.zero;
            CancelInvoke("Time_Count");
        }
    }

    public void reset_turn_timer()
    {
        Obj_Last3Sec.transform.localScale = Vector3.zero;
        CancelInvoke("Time_Count");
        TXT_Timer_Counter_Pro.text = "0";
        Timer_flag = false;
        check = false;
        Last3Sec = false;
        Current_Ammount = 0;
        TimerCountEndAmount = 0;
        End_Ammount = 0;
    }

    public void Last_3Sec_Pop(float time)
    {
        //Txt_Last3Sec.text = time.ToString();
        Tmp_Last3Sec.text = time.ToString();
        iTween.ScaleTo(Obj_Last3Sec, iTween.Hash("x", 1, "y", 1, "time",0.5f, "easetype", iTween.EaseType.easeOutExpo));
        Invoke("Hide_3SecTimer_POPUP", 1f);
    }

    public void Hide_3SecTimer_POPUP()
    {
        Obj_Last3Sec.transform.localScale = Vector3.zero;
    }
}
