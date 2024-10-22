using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCheck : MonoBehaviour
{
    public static NetworkCheck Inst;

    private void Awake()
    { 
        if(Inst==null)
            Inst = this;
        else
            Destroy(this.gameObject);
    }

    public static bool IsInternetConnection()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
}
