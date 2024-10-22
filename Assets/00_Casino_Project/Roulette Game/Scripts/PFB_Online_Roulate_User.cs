using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_Online_Roulate_User : MonoBehaviour
{
    public static PFB_Online_Roulate_User Inst;
    [SerializeField] Text Txt_Name, TXt_Chips;
    [SerializeField] IMGLoader Pic;
    [SerializeField] Image Vip_Ring;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_USER_DATA(string PicURL, string Name, string Chips, int vipLevel)
    {
        Txt_Name.text = Name;
        TXt_Chips.text = float.Parse(Chips).ToString("n2");
        Pic.LoadIMG(PicURL, false, false);
        Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[vipLevel];
    }
}
