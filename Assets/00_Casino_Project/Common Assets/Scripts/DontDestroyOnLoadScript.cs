using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{

    public static DontDestroyOnLoadScript Inst;

   
    private void Awake()
    {
        if(Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this.gameObject);  
        }
        DontDestroyOnLoad(this.gameObject);       
    }
}
