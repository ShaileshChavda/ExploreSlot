using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TP_Timer_Filler : MonoBehaviour
{
    public static TP_Timer_Filler Inst;
    public bool Timer_flag = false;
    public float Current_Ammount;
    public float End_Ammount;
    public float speed = 1;
    bool check = false;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    internal void StartTimerAnim(float startTimer, float endTimer, bool rejoin)
    {
        this.GetComponent<Image>().color = new Color32(0, 255, 0, 103);
        this.transform.localScale = Vector3.one;

        if (rejoin)
        {
            Current_Ammount = endTimer-startTimer;
            this.GetComponent<Image>().fillAmount = startTimer / endTimer;
        }
        else
        {
            Current_Ammount = 0;
        }

        End_Ammount = endTimer;
        Timer_flag = true;
       //check = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Timer_flag)
        {
            //green
            this.GetComponent<Image>().color = new Color32(0, 255, 0, 103);
            if (Current_Ammount <= End_Ammount)
            {

                Current_Ammount += speed * Time.deltaTime;
                this.GetComponent<Image>().fillAmount = Current_Ammount / End_Ammount;

                if (this.GetComponent<Image>().fillAmount >= 0.75f)
                {
                    //red
                    this.GetComponent<Image>().color = new Color32(255, 0, 14, 103);
                    if (TP_GameManager.Inst.IsMyTurn)
                    {
                        if (!check)
                        {
                            check = true;
                            TP_SoundManager.Inst.PlaySFX(2);
                        }
                    }

                }
            }
            else
            {
                reset_turn_timer();
            }
        }
    }

    public void reset_turn_timer()
    {
        check = false;
        this.GetComponent<Image>().color = new Color32(0, 255, 0, 103);
        this.transform.localScale = Vector3.zero;
        Timer_flag = false;
        this.GetComponent<Image>().fillAmount = 0;
        Current_Ammount = 0;
        End_Ammount = 0;
    }
}
