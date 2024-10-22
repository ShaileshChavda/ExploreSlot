using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyBonus : MonoBehaviour
{
    public static DailyBonus Inst;
    [SerializeField] Text TxtClaimed_Amount;
    public Sprite DailyBonus_PFB_BG_Green, DailyBonus_PFB_BG_Gold;
    public List<PFB_DailyBonus> PFB_Bonus_list;
    bool available;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void OPEN_DAILY_BONUS()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.DAILY_BONUS_INFO());
    }
    public void CLOSE_DAILY_BONUS()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
    public void Btn_Claimed_Bonus()
    {
        SoundManager.Inst.PlaySFX(0);
        if (available)
            SocketHandler.Inst.SendData(SocketEventManager.Inst.DAILY_BONUS_COLLECT());
        else
            Alert_MSG.Inst.MSG("Daily bonus Not available now!");
    }
    public void OPEN_CLAIMED_MSG(string Bonus)
    {
        TxtClaimed_Amount.text = Bonus;
        GS.Inst.iTwin_Open(GameObject.Find("BONUS_CLAIM_POP"));
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
    public void CLOSE_CLAIMED_MSG()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(GameObject.Find("BONUS_CLAIM_POP"), 0.3f);
    }

    public void SET_DAILY_BONUS_DATA(JSONObject data)
    {
        available= bool.Parse(data.GetField("day_bonus_availble").ToString().Trim(Config.Inst.trim_char_arry));
        if (available)
        {
            GS.Inst.iTwin_Open(this.gameObject);
            int day = int.Parse(data.GetField("day_curr").ToString().Trim(Config.Inst.trim_char_arry));
            for (int i = 0; i < PFB_Bonus_list.Count; i++)
            {
                PFB_Bonus_list[i].SET_MY_DATA(day, available);
            }
        }
        else
        {
            CLOSE_DAILY_BONUS();
            Alert_MSG.Inst.MSG("Daily bonus Not available now!");
        }
    }
}
