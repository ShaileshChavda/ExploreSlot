using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DT_HIST_CARD : MonoBehaviour
{
    public static DT_HIST_CARD Inst;
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

        if (split_XCard[0].Equals("dragon"))
            Card_Image.sprite = DT_HistoryManager.Inst.D_Hist_Sprite;
        else if(split_XCard[0].Equals("tiger"))
            Card_Image.sprite = DT_HistoryManager.Inst.T_Hist_Sprite;
        else
            Card_Image.sprite = DT_HistoryManager.Inst.TIE_Hist_Sprite;
    }
}
