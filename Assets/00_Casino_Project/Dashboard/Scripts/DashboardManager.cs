using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DashboardManager : MonoBehaviour
{
    public static DashboardManager Inst;
    [Header("Dashboard")]
    [SerializeField] Text Txt_Dash_Name;
    [SerializeField] Text Txt_Dash_Chips;
    [SerializeField] Text Txt_Dash_Bonus;
    [SerializeField] Text Txt_Dash_ID;
    [SerializeField] IMGLoader Pic_Dash;
    [SerializeField] Image User_VipRing;
    [SerializeField] GameObject Girl,Girl_Source, Girl_Destination;
    [SerializeField] Sprite Girl1_SP, Girl2_SP;
    public List<PreloadAddressable> Addrasable_Scene_List;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        UnityEngine.Debug.Log("REJOIN: "+ GS.Inst.GameType);
        if (GS.Inst.Rejoin)
        {
            
            if (GS.Inst.GameType.Equals("SlotMachin"))
            {
                PreeLoader.Inst.Show();
                Addrasable_Scene_List[3].LoadScene();
            }
        }

        Girl_Come();
        SET_DASHBOARD_DATA();

        if (!GS.Inst.Share_Notice_Popup && !GS.Inst.Rejoin)
        {
            GS.Inst.Share_Notice_Popup = true;
            RefAndEarn.Inst.OPEN_RefAndEarn_SC();
            Notice.Inst.OPEN_NOTICE_SC_START();
        }
    }
    public void Girl_Come()
    {
        if(Girl.transform.GetComponent<Image>().sprite.name.Equals(Girl1_SP.name))
            Girl.transform.GetComponent<Image>().sprite = Girl2_SP;
        else
            Girl.transform.GetComponent<Image>().sprite = Girl1_SP;

        iTween.MoveTo(Girl.gameObject, iTween.Hash("position", Girl_Destination.transform.position, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
        Invoke("Girl_GO", 1f);
    }
    public void Girl_GO()
    {
        iTween.MoveTo(Girl.gameObject, iTween.Hash("position", Girl_Source.transform.position, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
        Invoke("Girl_Come", 1f);
    }
    public void CLICK_GIRL(Image sp)
    {
        if (sp.sprite.name.Equals("ad_1"))
            VIP.Inst.OPEN_VIP();
        else
            RefAndEarn.Inst.OPEN_RefAndEarn_SC();
    }
    public void PLAY_GAME()
    {
        SoundManager.Inst.PlaySFX(0);
        SoundManager.Inst.StopBG();
        PreeLoader.Inst.Show();
        SceneManager.LoadScene(3);
    }

    public void BTN_Header_Support()
    {
        SoundManager.Inst.PlaySFX(0);
        Application.OpenURL(GS.Inst.Service_link_URL);
    }

    public void Btn_Header_Bonus_Info()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(GameObject.Find("Bonus_Qequition_Info"));
    }
    public void Btn_Header_Bonus_Info_YES()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(GameObject.Find("Bonus_Qequition_Info"),0.3f);
    }

    public void SET_DASHBOARD_DATA()
    {
        Txt_Dash_Name.text = GS.Inst._userData.Name;
        Txt_Dash_Chips.text = GS.Inst._userData.Chips.ToString("n2");
        Txt_Dash_Bonus.text = GS.Inst._userData.Bonus.ToString("n2");
        Txt_Dash_ID.text ="ID:"+ GS.Inst._userData.UID;
        Pic_Dash.LoadIMG(GS.Inst._userData.PicUrl,false,false);
        User_VipRing.sprite = GS.Inst.VIP_RING_LIST[GS.Inst._userData.User_VIP_Level];
    }
}
