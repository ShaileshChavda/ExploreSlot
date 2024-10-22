using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoTimerLeaveGame : MonoBehaviour
{
    int timer=30;
    public TextMeshProUGUI txtTimer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 30;
        InvokeRepeating(nameof(TimerMethod),0f,1f);
    }

    public void TimerMethod()
    {
        txtTimer.text = timer.ToString();
        timer--;

        if(timer == 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
