using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Win_Slomotion : MonoBehaviour
{
    public static HR_Win_Slomotion Inst;
    public bool Slow_Action;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Horse") && !Slow_Action)
        {
            Slow_Action = true;
            HR_Manager.Inst.Start_SlowMotion();
        }
    }
}
