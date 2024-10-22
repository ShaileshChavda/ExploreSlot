using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HR_TimerHendler : MonoBehaviour
{
    public static HR_TimerHendler Inst;
    public bool Timer_flag = false;
    public float Current_Ammount;
    public float End_Ammount, TimerCountEndAmount;
    public float speed = 1;
    internal bool check = false;
    [SerializeField] TextMeshProUGUI TXT_Timer_Counter;

    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    internal void StartTimerAnim(float startTimer, float endTimer, bool rejoin)
    {
        reset_turn_timer();
        this.transform.localScale = Vector3.one;
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

                if (TimerCountEndAmount == 6)
                    HR_SoundManager.Inst.PlaySFX(3);

                if (TimerCountEndAmount == 4)
                {
                    HR_First_HorseLine.Inst.transform.localScale = Vector3.one;
                    //HR_UI_Manager.Inst.X_Ground_Box(false);
                    HR_Manager.Inst.START_RACE();
                }
                if (TimerCountEndAmount == 3)
                    HR_Manager.Inst.First_Horse_Seed();
            //if(TimerCountEndAmount<4)
            //    HR_Manager.Inst.UPDATE_HORSE_SPEED(null);
        }
        else
        {
            this.transform.localScale = Vector3.zero;
            TXT_Timer_Counter.text = "0";
            CancelInvoke("Time_Count");
        }
    }

    public void reset_turn_timer()
    {
        CancelInvoke("Time_Count");
        TXT_Timer_Counter.text = "0";
        Timer_flag = false;
        check = false;
        Current_Ammount = 0;
        TimerCountEndAmount = 0;
        End_Ammount = 0;
    }
}
