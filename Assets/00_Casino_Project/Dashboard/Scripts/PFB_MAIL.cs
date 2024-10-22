using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_MAIL : MonoBehaviour
{
    public static PFB_MAIL Inst;
    [SerializeField] Text TxtMessage, TxtDateTime,Txt_Bonus;
    [SerializeField] GameObject BonusBox, Read_Symbol;
    [SerializeField] Image Mail_icon;
    [SerializeField] Sprite Mail_icon_Read_SP;
    string Mail_ID;
    bool isReaded;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_DATA(JSONObject data)
    {
        Mail_ID = data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
        TxtMessage.text = data.GetField("title").ToString().Trim(Config.Inst.trim_char_arry);
        TxtDateTime.text = data.GetField("create_time").ToString().Trim(Config.Inst.trim_char_arry);
        bool isBonus = bool.Parse(data.GetField("is_bonus").ToString().Trim(Config.Inst.trim_char_arry));

        if (isBonus)
        {
            BonusBox.SetActive(true);
            Txt_Bonus.text= data.GetField("bonus").ToString().Trim(Config.Inst.trim_char_arry);
        }
        else
            BonusBox.SetActive(false);

        if (data.HasField("is_readed"))
        {
            isReaded = bool.Parse(data.GetField("is_readed").ToString().Trim(Config.Inst.trim_char_arry));
            READED(isReaded);
        }
    }

    public void READED(bool action)
    {
        if (action)
        {
            Mail_icon.sprite = Mail_icon_Read_SP;
            TxtMessage.color = Color.white;
            TxtDateTime.color = Color.white;
            Txt_Bonus.color = Color.white;
            Read_Symbol.SetActive(true);
        }
    }

    public void OPEN_MAIL()
    {
        PreeLoader.Inst.Show();
        Mail.Inst.OPEN_MAIL_POPUP(Mail_ID);
        READED(true);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.MAIL_INFO(Mail_ID));
    }

    public void DELETE_MAIL()
    {
        PreeLoader.Inst.Show();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.MAIL_REMOVE(Mail_ID));
        this.gameObject.SetActive(false);
    }
}
