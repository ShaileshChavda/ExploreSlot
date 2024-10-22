using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alert_MSG : MonoBehaviour
{
    public static Alert_MSG Inst;
    public Text txtMsg;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void MSG(string msg)
    {
        txtMsg.text = msg;
        transform.localScale = Vector3.one;
        Invoke("close_alert", 1.5f);
    }

    void close_alert()
    {
        transform.localScale = Vector3.zero;
    }
}
