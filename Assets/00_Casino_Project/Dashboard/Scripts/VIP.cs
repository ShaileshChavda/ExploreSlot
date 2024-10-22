using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VIP : MonoBehaviour
{
    public static VIP Inst;
    [SerializeField] Image Vip_GreenFiller;
    [SerializeField] Image User_VIP_LEVEL, User_VIP_Next_Level;
    [SerializeField] Text TXT_User_VIP_LEVEL, TXT_User_VIP_Next_Level;
    [SerializeField] Text Txt_DailyBonusComes, Txt_WeeklyBonusComes, Txt_MonthBonusComes, Txt_LevelBonusComes;
    [SerializeField] Text TxtUpgradeLevel, TxtUpgradeLevel_Amount,Txt_Privelig_Level;

    [Header("VIP LNG")]
    [SerializeField] Text Txt_VIP0;
    [SerializeField] Text Txt_VIP1;
    [SerializeField] Text Txt_Recharg;
    [SerializeField] Text Txt_Box_Header;
    JSONObject VIP_DATA;
    int Level = 0;
    [SerializeField] Animator LeftRightAnimation;
    [SerializeField] GameObject Left_Arraw, Right_Arraw;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void OPEN_VIP()
    {
        Left_Arraw.transform.localScale = Vector3.zero;
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.VIP_INFO());
        GS.Inst.iTwin_Open(this.gameObject);
        LNG_SETUP();
    }
    public void CLOSE_VIP()
    {
        Level = 0;
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }

    public void BTN_BUY_VIP()
    {
        SoundManager.Inst.PlaySFX(0);
        Shop.Inst.OPEN_SHOP();
    }

    public void OPEN_VIP_INFO()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(GameObject.Find("VIP_INFO_POP"));
    }
    public void CLOSE_VIP_INFO()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(GameObject.Find("VIP_INFO_POP"),0.3f);
    }
    public void Click_Weekly_Bonus()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.WEEKLY_BONUS_COLLECT());
    }
    public void Click_Monthly_Bonus()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.MONTHLY_BONUS_COLLECT());
    }
    public void Click_Level_Bonus()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.LEVEL_BONUS_COLLECT());
    }

    public void SET_VIP_DATA(JSONObject data)
    {
        VIP_DATA = new JSONObject();
        VIP_DATA = data;
        //int Level = 0;
        Level = int.Parse(data.GetField("level").ToString().Trim(Config.Inst.trim_char_arry));
        Txt_Privelig_Level.text = "VIP" + Level + " Privileges";
        string Deposit_Level = data.GetField("deposit_amount").ToString().Trim(Config.Inst.trim_char_arry);
        string name = data.GetField("level_configs")[Level].GetField("name").ToString().Trim(Config.Inst.trim_char_arry);
        string UpgradeAmount;
        if (Level < 21)
            UpgradeAmount = double.Parse(data.GetField("level_configs")[Level+1].GetField("upgrade_amount").ToString().Trim(Config.Inst.trim_char_arry)).ToString();
        else
            UpgradeAmount = double.Parse(data.GetField("level_configs")[Level].GetField("upgrade_amount").ToString().Trim(Config.Inst.trim_char_arry)).ToString();
        Txt_DailyBonusComes.text = data.GetField("level_configs")[Level].GetField("daily_bouns").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_MonthBonusComes.text = data.GetField("level_configs")[Level].GetField("monthly_bonus").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_WeeklyBonusComes.text = data.GetField("level_configs")[Level].GetField("weekly_bonus").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_LevelBonusComes.text = data.GetField("level_configs")[Level].GetField("level_bonus").ToString().Trim(Config.Inst.trim_char_arry);
        TxtUpgradeLevel.text= Deposit_Level+"/"+UpgradeAmount;
        TxtUpgradeLevel_Amount.text = UpgradeAmount;
        if (Level <= 21)
        {
            User_VIP_LEVEL.sprite = GS.Inst.VIP_LEVEL_LIST[Level];
            TXT_User_VIP_LEVEL.text = "VIP" + Level;
            if (Level + 1 <= 21)
            {
                User_VIP_Next_Level.sprite = GS.Inst.VIP_LEVEL_LIST[Level + 1];
                TXT_User_VIP_Next_Level.text = "VIP" + (Level+1);
            }
            else
            {
                User_VIP_Next_Level.sprite = GS.Inst.VIP_LEVEL_LIST[Level];
                TXT_User_VIP_Next_Level.text = "VIP" + Level;
            }
        }
        Vip_GreenFiller.fillAmount = float.Parse(Deposit_Level) / float.Parse(UpgradeAmount);

        if (Level < 19)
        {
            Left_Arraw.transform.localScale = Vector3.one;
            LeftRightAnimation.Play("VIP_BOX_ANIM", 0);
        }
    }

    public void BTN_LEFT_RIGHT_ARROW(string LR)
    {
        SoundManager.Inst.PlaySFX(0);
        if (LR.Equals("R"))
        {
            if (Level < 19)
            {
                Left_Arraw.transform.localScale = Vector3.one;
                LeftRightAnimation.Play("VIP_BOX_ANIM", 0);
                Level++;
                Txt_Privelig_Level.text = "VIP"+Level+ " Privileges";
            }
            else
            {
                Right_Arraw.transform.localScale = Vector3.zero;
            }
        }
        else
        {
            if (Level >= 1)
            {
                Right_Arraw.transform.localScale = Vector3.one;
                LeftRightAnimation.Play("VIP_BOX_ANIM_Back", 0);
                Level--;
                Txt_Privelig_Level.text = "VIP" + Level + " Privileges";
            }
            else {
                Left_Arraw.transform.localScale = Vector3.zero;
            }
        }
        string Deposit_Level = VIP_DATA.GetField("deposit_amount").ToString().Trim(Config.Inst.trim_char_arry);
        string name = VIP_DATA.GetField("level_configs")[Level].GetField("name").ToString().Trim(Config.Inst.trim_char_arry);
        string UpgradeAmount;
        if (Level < 21)
            UpgradeAmount = double.Parse(VIP_DATA.GetField("level_configs")[Level + 1].GetField("upgrade_amount").ToString().Trim(Config.Inst.trim_char_arry)).ToString();
        else
            UpgradeAmount = double.Parse(VIP_DATA.GetField("level_configs")[Level].GetField("upgrade_amount").ToString().Trim(Config.Inst.trim_char_arry)).ToString();
        Txt_DailyBonusComes.text = VIP_DATA.GetField("level_configs")[Level].GetField("daily_bouns").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_MonthBonusComes.text = VIP_DATA.GetField("level_configs")[Level].GetField("monthly_bonus").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_WeeklyBonusComes.text = VIP_DATA.GetField("level_configs")[Level].GetField("weekly_bonus").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_LevelBonusComes.text = VIP_DATA.GetField("level_configs")[Level].GetField("level_bonus").ToString().Trim(Config.Inst.trim_char_arry);
        TxtUpgradeLevel.text = Deposit_Level + "/" + UpgradeAmount;
        TxtUpgradeLevel_Amount.text = UpgradeAmount;
        if (Level <= 21)
        {
            User_VIP_LEVEL.sprite = GS.Inst.VIP_LEVEL_LIST[Level];
            TXT_User_VIP_LEVEL.text = "VIP" + Level;
            if (Level + 1 <= 21) {
                User_VIP_Next_Level.sprite = GS.Inst.VIP_LEVEL_LIST[Level + 1];
                TXT_User_VIP_Next_Level.text = "VIP" + (Level + 1);
            }
            else {
                User_VIP_Next_Level.sprite = GS.Inst.VIP_LEVEL_LIST[Level];
                TXT_User_VIP_Next_Level.text = "VIP" + Level;
            }
        }
        Vip_GreenFiller.fillAmount = float.Parse(Deposit_Level) / float.Parse(UpgradeAmount);
    }

    void LNG_SETUP()
    {
        switch (PlayerPrefs.GetInt("LNG"))
        {
            case 0:
                //english
                Txt_VIP0.text = "VIP0";
                Txt_VIP1.text = "VIP1";
                Txt_Recharg.text = "you can recharge it with";
                Txt_Box_Header.text = "VIP0 Privileges";
                break;
            case 1:
                //Nepali
                Txt_VIP0.text = "वीआईपी0";
                Txt_VIP1.text = "वीआईपी1";
                Txt_Recharg.text = "तपाईं यसलाई रिचार्ज गर्न सक्नुहुन्छ";
                Txt_Box_Header.text = "वीआईपी0 विशेषाधिकारहरू";
                break;
            case 2:
                //urdu
                Txt_VIP0.text = "وی آئی پی 0";
                Txt_VIP1.text = "وی آئی پی 1";
                Txt_Recharg.text = "آپ اسے ری چارج کر سکتے ہیں۔";
                Txt_Box_Header.text = "وی آئی پی مراعات";
                break;
            case 3:
                //bangali
                Txt_VIP0.text = "ভিআইপি 0";
                Txt_VIP1.text = "ভিআইপি 1";
                Txt_Recharg.text = "আপনি এটি দিয়ে রিচার্জ করতে পারেন";
                Txt_Box_Header.text = "ভিআইপি 0 বিশেষাধিকার";
                break;
            case 4:
                //Marathi
                Txt_VIP0.text = "व्हीआयपी0";
                Txt_VIP1.text = "व्हीआयपी1";
                Txt_Recharg.text = "तुम्ही ते रिचार्ज करू शकता";
                Txt_Box_Header.text = "व्हीआयपी0 विशेषाधिकार";
                break;
            case 5:
                //telugu
                Txt_VIP0.text = "విఐపి 0";
                Txt_VIP1.text = "విఐపి 1";
                Txt_Recharg.text = "మీరు దీన్ని రీఛార్జ్ చేయవచ్చు";
                Txt_Box_Header.text = "విఐపి 0 అధికారాలు";
                break;
            default:
                break;
        }
    }
}
