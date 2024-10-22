using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotyPopup : MonoBehaviour
{
    public static NotyPopup Inst;
    [SerializeField] GameObject Anim_Sourc, Anim_Destination;
    [SerializeField] IMGLoader Pic;
    [SerializeField] Image Vip_Ring;
    [SerializeField] Text TxtName,TxtGameName,TxtChips;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void Open_Noty_Popup()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Anim_Destination.transform.position, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
        Invoke("Close_Noty_Popup", 3f);
    }
    public void Close_Noty_Popup()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Anim_Sourc.transform.position, "time", 0.2f, "easetype", iTween.EaseType.easeOutExpo));
    }
    public void SET_NOTY_DATA(JSONObject data)
    {
        Open_Noty_Popup();
        TxtName.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        TxtGameName.text = data.GetField("cta").ToString().Trim(Config.Inst.trim_char_arry);
        TxtChips.text = data.GetField("winning_amount").ToString().Trim(Config.Inst.trim_char_arry);
        Pic.LoadIMG(data.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry),false, false);

        Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[int.Parse(data.GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry))];
    }
}
