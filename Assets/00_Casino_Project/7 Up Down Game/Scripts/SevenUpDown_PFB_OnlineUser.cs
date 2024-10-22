using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SevenUpDown_PFB_OnlineUser : MonoBehaviour
{
    public static SevenUpDown_PFB_OnlineUser Inst;
    [SerializeField] Text Txt_Index, Txt_Name, TXt_Chips;
    [SerializeField] IMGLoader UserPIC;
    [SerializeField] Image Vip_Ring;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_USER_DATA(int Index,string Name,string Chips,string PicURL,int vip)
    {
        Txt_Index.text = Index.ToString();
        Txt_Name.text = Name;
        TXt_Chips.text = float.Parse(Chips).ToString("n2");
        UserPIC.LoadIMG(PicURL, false, false);
        Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[vip];
    }
}
