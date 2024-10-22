using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_DailyBonus : MonoBehaviour
{
    public static PFB_DailyBonus Inst;
    public GameObject IMG_Checked;
    public Image BgImage;
    public int MyDay;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
        IMG_Checked = this.transform.GetChild(2).gameObject;
        BgImage = this.GetComponent<Image>();
        IMG_Checked.SetActive(false);
    }
    public void SET_MY_DATA(int day,bool available)
    {
        if (day==MyDay)
        {
            if (available)
            {
                IMG_Checked.SetActive(false);
                BgImage.sprite = DailyBonus.Inst.DailyBonus_PFB_BG_Green;
                BgImage.color = Color.white;
                this.GetComponent<Button>().enabled = true;
            }
            else
            {
                IMG_Checked.SetActive(true);
                BgImage.sprite = DailyBonus.Inst.DailyBonus_PFB_BG_Green;
                BgImage.color = Color.gray;
                this.GetComponent<Button>().enabled = false;
            }
        }
        else if (MyDay > day)
        {
            IMG_Checked.SetActive(false);
            BgImage.sprite = DailyBonus.Inst.DailyBonus_PFB_BG_Gold;
            BgImage.color = Color.white;
            this.GetComponent<Button>().enabled = true;
        }
        else if(MyDay < day)
        {
            IMG_Checked.SetActive(true);
            BgImage.sprite = DailyBonus.Inst.DailyBonus_PFB_BG_Green;
            BgImage.color = Color.gray;
            this.GetComponent<Button>().enabled = false;
        }
    }
}
