using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketEventManager : MonoBehaviour
{
    public static SocketEventManager Inst;
    public string MYDATA = "HELLO";

    void Awake()
    {
        Inst = this;
    }

    internal JSONObject PLAY_AS_GUEST(string refcode)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "PLAY_AS_GUEST");
        data.AddField("device_type", GS.Inst.DEVICE_Type);
        data.AddField("device_id", GS.Inst.DEVICE_Id);
        data.AddField("android_version", GS.Inst.OS_Info);
        data.AddField("app_version", Config.Inst.Version);
        data.AddField("refferal_code", refcode);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject PLAY_AS_MOBILE_NUMBER(string TxtMobileNo,string refcode)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "PLAY_AS_MOBILE_NUMBER");
        data.AddField("device_type", GS.Inst.DEVICE_Type);
        data.AddField("device_id", GS.Inst.DEVICE_Id);
        data.AddField("android_version", GS.Inst.OS_Info);
        data.AddField("app_version", Config.Inst.Version);
        data.AddField("mobile_number", TxtMobileNo);
        data.AddField("refferal_code", refcode);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject SEND_OTP(string GetOtp_Type,string TxtMobileNo)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "SEND_OTP");
        data.AddField("type", GetOtp_Type);
        data.AddField("mobile_number", TxtMobileNo);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject VERIFY_OTP(string otp)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "VERIFY_OTP");
        data.AddField("otp", otp);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject VERIFY_OTP(string type, string TxtMobileNo)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "MOBILE_LINK_GUEST");
        data.AddField("type", type);
        data.AddField("mobile_number", TxtMobileNo);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject AppLunchDetails()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "AppLunchDetails");
        data.AddField("device_type", GS.Inst.DEVICE_Type);
        data.AddField("device_id", GS.Inst.DEVICE_Id);
        data.AddField("android_version", GS.Inst.OS_Info);
        data.AddField("app_version", Config.Inst.Version);
        data.AddField("mobile_number", GS.Inst._userData.Mobile);
        obj.AddField("data", data);
        return obj;
    }

    //----------------------- Dashboard ------------------------------------------------
    internal JSONObject PROFILE_INFO()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "PROFILE_INFO");
        data.AddField("_id", GS.Inst._userData.Id);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject AVATARS()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "AVATARS");
        data.AddField("_id", GS.Inst._userData.Id);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject UPDATE_PROFILE(string type,string actiondata)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "UPDATE_PROFILE");
        data.AddField("type", type);
        if(type.Equals("user_name"))
            data.AddField("user_name", actiondata);
        else
            data.AddField("profile_url", actiondata);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject DEPOSIT_LISTS(string Title)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "DEPOSIT_LISTS");
        data.AddField("title", Title);
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject SUCCESS_DEPOSIT(string id,string plan,string refcode)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "SUCCESS_DEPOSIT");
        data.AddField("deposit_id", id);
        data.AddField("plan_id", plan);
        data.AddField("refferal_code", refcode);
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject SUPPORTS()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "SUPPORTS");
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject MANUAL_PAYMENT_INFO()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "MANUAL_PAYMENT_INFO");
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject NEW_MAILS()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "NEW_MAILS");
        data.AddField("", "");
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject MANUAL_PAYMENT(string url,string amount)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "MANUAL_PAYMENT");
        data.AddField("ss_url", url);
        data.AddField("deposit_id", Shop.Inst.selected_ID);
        data.AddField("plan_id", Shop.Inst.selected_Plan);
        data.AddField("referal_code", Shop.Inst.InputRefralCode.text);
        data.AddField("amount", amount);
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject RECORDS(string filter,int page)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "RECORDS");
        data.AddField("filter", filter);
        data.AddField("page", page);
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject REFER_INFO()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "REFER_INFO");
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject SAFE_INFO()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "SAFE_INFO");
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject SAFE_TAKE_IN(string TakeInAmount)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "SAFE_TAKE_IN");
        data.AddField("take_in_amount", TakeInAmount);
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject SAFE_TAKE_OUT(string TakeOutAmount)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "SAFE_TAKE_OUT");
        data.AddField("take_out_amount", TakeOutAmount);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject VIP_INFO()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "VIP_INFO");
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject REFER_BONUS_CLAIM()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "REFER_BONUS_CLAIM");
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject DAILY_BONUS_INFO()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "DAILY_BONUS_INFO");
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject DAILY_BONUS_COLLECT()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "DAILY_BONUS_COLLECT");
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject WEEKLY_BONUS_COLLECT()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "WEEKLY_BONUS_COLLECT");
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject MONTHLY_BONUS_COLLECT()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "MONTHLY_BONUS_COLLECT");
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject LEVEL_BONUS_COLLECT()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "LEVEL_BONUS_COLLECT");
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject RANK_LISTS()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "RANK_LISTS");
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject CHIPS_RANK_LISTS()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "CHIPS_RANK_LISTS");
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject MAIL_LISTS(string title)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "MAIL_LISTS");
        data.AddField("type", title);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject MAIL_INFO(string _id)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "MAIL_INFO");
        data.AddField("mail_id", _id);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject MAIL_BONUS_CLAIM(string _id)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "MAIL_BONUS_CLAIM");
        data.AddField("mail_id", _id);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject MAIL_REMOVE(string _id)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "MAIL_REMOVE");
        data.AddField("mail_id", _id);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject WITHDRAW_INFO(string type)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "WITHDRAW_INFO");
        data.AddField("type", type);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject WITHDRAW_PLACE(string type,string amount)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "WITHDRAW_PLACE");
        data.AddField("via_withdraw", type);
        data.AddField("amount", amount);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject BANK_ADD(string accno,string uname,string ifsc,string BankName,string email)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "BANK_ADD");
        data.AddField("account_no", accno);
        data.AddField("bank_user_name", uname);
        data.AddField("ifsc_code", ifsc);
        data.AddField("bank_name", BankName);
        data.AddField("email", email);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject UPI_ADD(string username,string upiID,string mobile)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "UPI_ADD");
        data.AddField("name", username);
        data.AddField("upi_id", upiID);
        data.AddField("mobile", mobile);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject SPINNER_INFO(string SpinnerType)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "SPINNER_INFO");
        data.AddField("type", SpinnerType);
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject SPINNER_START(string SpinnerType)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "SPINNER_START");
        data.AddField("type", SpinnerType);
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject SPINNER_RECORDS()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "SPINNER_RECORDS");
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject SPINNER_BIG_WINS()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "SPINNER_BIG_WINS");
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject SPINNER_RECORD_CLOSE()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "SPINNER_RECORD_CLOSE");
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject REFER_RULE_INFO()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "REFER_RULE_INFO");
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject REFER_REFERREL_LISTS(int page)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "REFER_REFERREL_LISTS");
        data.AddField("page",page);
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject GET_BONUS_ERN_LIST(string filter,int page)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "GET_BONUS_ERN_LIST");
        data.AddField("filter", filter);
        data.AddField("page", page);
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject GET_BONUS_RECORD(int page)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "GET_BONUS_RECORD");
        data.AddField("page", page);
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject WEEKLY_EXTRA_BONUS_INFO()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "WEEKLY_EXTRA_BONUS_INFO");
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject CLAIM_WEEKLY_EXTRA_BONUS()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "CLAIM_WEEKLY_EXTRA_BONUS");
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject NOTICE_LISTS(string title,string headerType)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "NOTICE_LISTS");
        data.AddField("title", title);
        data.AddField("header", headerType);
        obj.AddField("data", data);
        return obj;
    }
    //----------------------- Dashboard ------------------------------------------------

    //-----------------------  Slot Game ---------------------------------
    internal JSONObject SLOT_GAME_INFO()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "SLOT_GAME_INFO");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject SLOT_CLOSE_GAME()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "SLOT_CLOSE_GAME");
        obj.AddField("data", "");
        return obj;
    }
    internal JSONObject SLOT_START_SPIN(bool auto)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "SLOT_START_SPIN");
        data.AddField("bet",Slot_UI_Manager.Inst.Txt_Bet.text);
        data.AddField("lines", Slot_UI_Manager.Inst.Txt_Line.text);
        data.AddField("auto_play", auto);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject SLOT_STOP_SPIN()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "SLOT_STOP_SPIN");
        obj.AddField("data", "");
        return obj;
    }
    internal JSONObject SLOT_NEW_LUCKY_USERS()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "SLOT_NEW_LUCKY_USERS");
        obj.AddField("data", "");
        return obj;
    }
    internal JSONObject SLOT_LUCKY_USERS_HISTORY()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "SLOT_LUCKY_USERS_HISTORY");
        obj.AddField("data", "");
        return obj;
    }
    //-----------------------  Slot Game ---------------------------------

    //-----------------------  Explorer2 ---------------------------------
    internal JSONObject EXPLORERE_TWO_GAME_INFO()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "EXPLORERE_TWO_GAME_INFO");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject EXPLORERE_TWO_CLOSE_GAME()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "EXPLORERE_TWO_CLOSE_GAME");
        obj.AddField("data", "");
        return obj;
    }
    internal JSONObject EXPLORERE_TWO_START_SPIN(bool auto,int Auto_Count)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "EXPLORERE_TWO_START_SPIN");
        data.AddField("bet", EXP_UI_Manager.Inst.Txt_Bet.text);
        data.AddField("lines", "9");
        data.AddField("auto_play", auto);
        data.AddField("auto_count", Auto_Count);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject EXPLORERE_TWO_STOP_SPIN()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "EXPLORERE_TWO_STOP_SPIN");
        obj.AddField("data", "");
        return obj;
    }


    internal JSONObject EXPLORERE_TWO_NEW_LUCKY_USERS()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "EXPLORERE_TWO_NEW_LUCKY_USERS");
        obj.AddField("data", "");
        return obj;
    }
    internal JSONObject EXPLORERE_TWO_LUCKY_USERS_HISTORY()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "EXPLORERE_TWO_LUCKY_USERS_HISTORY");
        obj.AddField("data", "");
        return obj;
    }
    //-----------------------  Explorer2 ---------------------------------

}
