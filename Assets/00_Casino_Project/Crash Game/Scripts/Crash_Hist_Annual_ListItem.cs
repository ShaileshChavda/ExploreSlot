using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Crash_Hist_Annual_ListItem : MonoBehaviour
{
    public static Crash_Hist_Annual_ListItem Inst;    
    public TextMeshProUGUI crashAtText;   
    void Awake()
    {
        Inst = this;      
    }
    public void SetTextAndColor(GameObject listItem,float crashAt)
    {
        crashAtText.text = crashAt.ToString();
        if (crashAt >= 2 && crashAt <= 10)
        {
            listItem.GetComponent<Crash_Hist_Annual_ListItem>().crashAtText.color = Color.yellow;
        }
        if (crashAt > 10)
        {
            listItem.GetComponent<Crash_Hist_Annual_ListItem>().crashAtText.color = Color.red;
        }
       
    }
}
