using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommenMSG : MonoBehaviour
{
    public static CommenMSG Inst;
    public Text txtHeader,txtMsg;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void BTN_OK() {
        CancelInvoke("close_pop");
        transform.localScale = Vector3.zero;
        txtHeader.text = "";
        txtMsg.text = "";
    }

    public void MSG(string header,string msg)
    {
        txtHeader.text = header;
        txtMsg.text = msg;
        transform.localScale = Vector3.one;
        Invoke("close_pop", 3);
    }

    void close_pop()
    {
        BTN_OK();
    }
}
