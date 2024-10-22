using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DT_PFB_HIST_FIGHT : MonoBehaviour
{
    public static DT_PFB_HIST_FIGHT Inst;
    [SerializeField] GameObject Dragon_Ring, Tiger_Ring;
    [SerializeField] Text Dragon_No, Tiger_No;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_DATA(string data)
    {
        string[] split_X = null;
        string[] split_No = null;
        string[] split_No_0 = null;
        string[] split_No_1 = null;

        split_X = data.Split('|');
        string WinCard = split_X[0];
        split_No = split_X[1].Split('&');

        split_No_0 = split_No[0].Split('-');
        split_No_1 = split_No[1].Split('-');

        Dragon_No.text = Resurn_Card(split_No_0[1]);
        Tiger_No.text = Resurn_Card(split_No_1[1]);

        if (WinCard.Equals("dragon"))
        {
            Dragon_Ring.SetActive(true);
            Tiger_Ring.SetActive(false);
        }
        else if (WinCard.Equals("tiger"))
        {
            Tiger_Ring.SetActive(true);
            Dragon_Ring.SetActive(false);
        }
        else
        {
            Dragon_Ring.SetActive(false);
            Tiger_Ring.SetActive(false);
        }
    }

    string Resurn_Card(string no)
    {
        string goNo = no;
        if (no.Equals("11"))
            goNo = "J";
        else if (no.Equals("12"))
            goNo = "Q";
        else if (no.Equals("13"))
            goNo = "K";
        else if (no.Equals("1"))
            goNo = "A";
        else
            goNo = no;

        return goNo;
    }
}
