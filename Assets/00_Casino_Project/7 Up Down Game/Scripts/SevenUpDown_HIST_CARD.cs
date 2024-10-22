using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SevenUpDown_HIST_CARD : MonoBehaviour
{
    public static SevenUpDown_HIST_CARD Inst;
    [SerializeField] Image Card_Image;
    public string CardName;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
        Card_Image = this.GetComponent<Image>();
    }
    public void SET_HIST_CARD_DATA(string CardName)
    {
        string[] split_XCard = CardName.Split('|');

        if (split_XCard[0].Equals("two_six"))
        {
            string split_YCard = split_XCard[1];
            string[] split_Card = split_YCard.Split('&');
            int no1 = int.Parse(split_Card[0]);
            int no2 = int.Parse(split_Card[1]);
            int mul = no1 + no2;           
            Card_Image.sprite = SevenUpDown_HistoryManager.Inst.D_Hist_Sprite;
            Card_Image.transform.GetChild(0).GetComponent<Text>().text = mul.ToString();
        }
        else if (split_XCard[0].Equals("eight_twelve"))
        {
            string split_YCard = split_XCard[1];
            string[] split_Card = split_YCard.Split('&');
            int no1 = int.Parse(split_Card[0]);
            int no2 = int.Parse(split_Card[1]);
            int mul = no1 + no2;          
            Card_Image.sprite = SevenUpDown_HistoryManager.Inst.T_Hist_Sprite;
            Card_Image.transform.GetChild(0).GetComponent<Text>().text = mul.ToString();
        }
        else
        {
            string split_YCard = split_XCard[1];
            string[] split_Card = split_YCard.Split('&');
            int no1 = int.Parse(split_Card[0]);
            int no2 = int.Parse(split_Card[1]);
            int mul = no1 + no2;
            Card_Image.sprite = SevenUpDown_HistoryManager.Inst.TIE_Hist_Sprite;
            Card_Image.transform.GetChild(0).GetComponent<Text>().text = mul.ToString();
        }
    }
}
