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

    //----------------------- Europian Roullate -----------------------------------------
    internal JSONObject ROULETTE_PLACE_BET(string betID)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        JSONObject cards = new JSONObject();

        obj.AddField("en", "ROULETTE_PLACE_BET");
        cards.AddField(betID, Roullate_Manager.Inst.Selected_Bet_Amount);
        data.AddField("card_details", cards);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject ROULETTE_GAME_INFO()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "ROULETTE_GAME_INFO");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject ROULETTE_CLOSE_GAME()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "ROULETTE_CLOSE_GAME");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject ROULETTE_JOINED_USER_LISTS()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "ROULETTE_JOINED_USER_LISTS");
        obj.AddField("data", "");
        return obj;
    }
    //----------------------- Europian Roullate -----------------------------------------

    //----------------------- Dragon vs Tiger -----------------------------------------
    internal JSONObject DRAGON_TIGER_PLACE_BET(string dragon, string tie, string tiger)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        JSONObject cards = new JSONObject();

        obj.AddField("en", "DRAGON_TIGER_PLACE_BET");
        if (dragon != "0")
            cards.AddField("dragon", int.Parse(dragon));
        if (tiger != "0")
            cards.AddField("tiger", int.Parse(tiger));
        if (tie != "0")
            cards.AddField("tie", int.Parse(tie));
        data.AddField("card_details", cards);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject DRAGON_TIGER_GAME_INFO()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "DRAGON_TIGER_GAME_INFO");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject DRAGON_TIGER_CLOSE_GAME()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "DRAGON_TIGER_CLOSE_GAME");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject DRAGON_TIGER_JOINED_USER_LISTS()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "DRAGON_TIGER_JOINED_USER_LISTS");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject DRAGON_TIGER_RESULT_HISTORY()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "DRAGON_TIGER_RESULT_HISTORY");
        obj.AddField("data", "");
        return obj;
    }
    //----------------------- Dragon vs Tiger ---------------------------------



    //----------------------- Crash Game --------------------------------- 


    internal JSONObject CRASH_GAME_INFO()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "CRASH_GAME_INFO");
        obj.AddField("data", "");
        return obj;
    }
    internal JSONObject CRASH_CLOSE_GAME()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "CRASH_CLOSE_GAME");
        obj.AddField("data", "");
        return obj;
    }
    internal JSONObject CRASH_CASH_IN(int amount,float flee_condition=0.0f,int profit_on_stop=0,int loss_on_stop=0,string mode="")
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();       

        obj.AddField("en", "CRASH_CASH_IN");
        data.AddField("amount", amount);
        data.AddField("flee_condition", flee_condition);       
        data.AddField("profit_on_stop", profit_on_stop);
        data.AddField("loss_on_stop", loss_on_stop);
        data.AddField("mode", mode);
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject CRASH_CASH_OUT(float time)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "CRASH_CASH_OUT");
        data.AddField("time", time);
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject CRASH_RESULT_HISTORY()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "CRASH_RESULT_HISTORY");
        obj.AddField("data", "");
        return obj;
    }
    //-----------------------  Crash Game ---------------------------------

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


    //-----------------------  Horse racing ---------------------------------
    internal JSONObject HORSE_RACING_GAME_INFO()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "HORSE_RACING_GAME_INFO");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject HORSE_RACING_CLOSE_GAME()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "HORSE_RACING_CLOSE_GAME");
        obj.AddField("data", "");
        return obj;
    }
    internal JSONObject HORSE_RACING_CASH_IN()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "HORSE_RACING_CASH_IN");
        obj.AddField("data", "");
        return obj;
    }
    internal JSONObject HORSE_RACING_CASH_OUT()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "HORSE_RACING_CASH_OUT");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject HORSE_RACING_PLACE_BET(string horseNo,string betvalue)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        JSONObject cards = new JSONObject();

        obj.AddField("en", "HORSE_RACING_PLACE_BET");
        cards.AddField(horseNo, int.Parse(betvalue));
        data.AddField("card_details", cards);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject HORSE_RACING_JOINED_USER_LISTS()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "HORSE_RACING_JOINED_USER_LISTS");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject HORSE_RACING_RESULT_HISTORY()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "HORSE_RACING_RESULT_HISTORY");
        obj.AddField("data", "");
        return obj;
    }
    internal JSONObject HORSE_RACING_TREND_REPORTS()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "HORSE_RACING_TREND_REPORTS");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject HORSE_RACING_LUCKY_USERS_HISTORY()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "HORSE_RACING_LUCKY_USERS_HISTORY");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject HORSE_RACING_JACKPORT_USER_LISTS()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "HORSE_RACING_JACKPORT_USER_LISTS");
        obj.AddField("data", "");
        return obj;
    }
    internal JSONObject HORSE_RACING_ONLINE_USERS_HISTORY(int pageNo)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "HORSE_RACING_ONLINE_USERS_HISTORY");
        data.AddField("page", pageNo);
        obj.AddField("data", data);
        return obj;
    }
    internal JSONObject HORSE_RACING_CHAT(string id,string IMG_id,string text,bool action)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "HORSE_RACING_CHAT");
        data.AddField("s_id", GS.Inst._userData.Id);
        data.AddField("r_id", id);
        data.AddField("image", IMG_id);
        data.AddField("text", text);
        data.AddField("send", action);
        obj.AddField("data", data);
        return obj;
    }
    //-----------------------  horse racing ---------------------------------

    //----------------------- Car-Roulette -----------------------------------------
    internal JSONObject CAR_GAME_INFO()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "CAR_GAME_GAME_INFO");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject CAR_ROULETTE_PLACE_BET(string betID)
    {
        Debug.Log("SEND PLACE BET EVENT: " + betID + "|VALUE|" + CarRoulette_Game.CarRoulette_GameManager.instance._selectedBetItem.intValue);
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        JSONObject cards = new JSONObject();

        obj.AddField("en", "CAR_GAME_PLACE_BET");
        cards.AddField(betID, CarRoulette_Game.CarRoulette_GameManager.instance._selectedBetItem.intValue);
        data.AddField("card_details", cards);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject CAR_ROULETTE_PLACE_RE_BET(string betID, int bet)
    {
        Debug.Log("SEND PLACE RE-BET EVENT: " + betID + "|VALUE|" + bet);
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        JSONObject cards = new JSONObject();

        obj.AddField("en", "CAR_GAME_PLACE_BET");
        cards.AddField(betID, bet);
        data.AddField("card_details", cards);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject CAR_ROULETTE_CLOSE_GAME()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "CAR_GAME_CLOSE_GAME");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject CAR_JOINED_USER_LISTS()
    {
        Debug.Log("CAR_GAME_JOINED_USER_LISTS");

        JSONObject obj = new JSONObject();

        obj.AddField("en", "CAR_GAME_JOINED_USER_LISTS");
        obj.AddField("data", "");
        return obj;
    }
    //----------------------- Car-Roulette -----------------------------------------


    //------------------------------- TeenPatti ------------------------------------------

    internal JSONObject TEENPATTI_BetList()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "TEENPATTI_BetList");
        data.AddField("game_type", "teenpatti");
        data.AddField("sub_type", "simple");
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject TEENPATTI_JoinTable(string betID)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "TEENPATTI_JoinTable");
        data.AddField("bet_id", betID);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject TEENPATTI_LeaveTable()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "TEENPATTI_LeaveTable");
        data.Add("");
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject TEENPATTI_CardPack()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "TEENPATTI_CardPack");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject TEENPATTI_Chal(bool isIncement)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "TEENPATTI_Chal");
        data.AddField("is_increment", isIncement);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject TEENPATTI_SideShowRequest(bool isIncement)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "TEENPATTI_SideShowRequest");
        data.AddField("is_increment", isIncement);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject TEENPATTI_SideShowRequestAccept()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "TEENPATTI_SideShowRequestAccept");
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject TEENPATTI_SideShowRequestReject()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "TEENPATTI_SideShowRequestReject");
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject TEENPATTI_Show(bool isIncement)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "TEENPATTI_Show");
        data.AddField("is_increment", isIncement);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject TEENPATTI_ViewCard()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("en", "TEENPATTI_ViewCard");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject TEENPATTI_DealerTip(string tipAmount)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "TEENPATTI_DealerTip");
        data.AddField("tip_amount", tipAmount);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject TEENPATTI_ReJoinTable()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("en", "TEENPATTI_ReJoinTable");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject TEENPATTI_SwitchTable()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("en", "TEENPATTI_SwitchTable");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject TEENPATTI_PrivateTable(string Bet_ID)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "TEENPATTI_PrivateTable");
        data.AddField("bet_id", Bet_ID);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject TEENPATTI_JoinPrivateTable(string PrivateTB_CODE)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "TEENPATTI_JoinPrivateTable");
        data.AddField("private_game_id", PrivateTB_CODE);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject TEENPATTI_TableShortInfo()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "TEENPATTI_TableShortInfo");
        obj.AddField("data", data);
        return obj;
    }

    //---------------- TeenPatti -------------------------

    //----------------------- Seven Up Down -----------------------------------------
    internal JSONObject SEVEN_UP_PLACE_BET(string dragon, string tie, string tiger)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        JSONObject cards = new JSONObject();

        obj.AddField("en", "SEVEN_UP_PLACE_BET");
        if (dragon != "0")
            cards.AddField("two_six", int.Parse(dragon));
        if (tiger != "0")
            cards.AddField("eight_twelve", int.Parse(tiger));
        if (tie != "0")
            cards.AddField("seven", int.Parse(tie));
        data.AddField("card_details", cards);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject SEVEN_UP_GAME_INFO()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "SEVEN_UP_GAME_INFO");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject SEVEN_UP_CLOSE_GAME()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "SEVEN_UP_CLOSE_GAME");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject SEVEN_UP_JOINED_USER_LISTS()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "SEVEN_UP_JOINED_USER_LISTS");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject SEVEN_UP_RESULT_HISTORY()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "SEVEN_UP_RESULT_HISTORY");
        obj.AddField("data", "");
        return obj;
    }
    //----------------------- Seven Up Down ---------------------------------

    //----------------------- Andar Bahar -----------------------------------------
    internal JSONObject ANDAR_BAHAR_PLACE_BET(string BetBox, int BetAmount)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        JSONObject cards = new JSONObject();

        obj.AddField("en", "ANDAR_BAHAR_PLACE_BET");
        cards.AddField(BetBox, BetAmount);
        data.AddField("card_details", cards);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject ANDAR_BAHAR_GAME_INFO()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "ANDAR_BAHAR_GAME_INFO");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject ANDAR_BAHAR_CLOSE_GAME()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "ANDAR_BAHAR_CLOSE_GAME");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject ANDAR_BAHAR_JOINED_USER_LISTS()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "ANDAR_BAHAR_JOINED_USER_LISTS");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject ANDAR_BAHAR_RESULT_HISTORY()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "ANDAR_BAHAR_RESULT_HISTORY");
        obj.AddField("data", "");
        return obj;
    }
    //----------------------- Andar Bahar ---------------------------------

    //----------------------- Fish Rush -----------------------------------------

    internal JSONObject FISH_GAME_INFO(string betID)
    {
        Debug.Log("SEND FISH_GAME_INFO EVENT: " + betID);
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        obj.AddField("en", "FISH_RUSH_GameInfo");
        data.AddField("bet_id", betID);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject FISH_GAME_BETLIST()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "FISH_RUSH_BetList");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject FISH_GAME_KILL(string betValue, int fishID,int fishKey,string playerKey)
    {
        Debug.Log("SEND PLACE RE-BET EVENT: " + betValue + "|FISH ID|" + fishID);
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "FISH_RUSH_FishKill");
        data.AddField("bet", betValue);
        data.AddField("fish_id", fishID);
        data.AddField("fish_key", fishKey);
        data.AddField("player_key", playerKey);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject FISH_GAME_CLOSE_GAME()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "FISH_RUSH_GameClose");
        obj.AddField("data", "");
        return obj;
    }

    //----------------------- Zoo-Roulette -----------------------------------------
    internal JSONObject ZOO_GAME_INFO()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "ZOO_GAME_GAME_INFO");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject ZOO_ROULETTE_PLACE_BET(string betID)
    {
        Debug.Log("SEND PLACE BET EVENT: " + betID + "|VALUE|" + ZooRoulette_Game.ZooRoulette_GameManager.instance._selectedBetItem.intValue);
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        JSONObject cards = new JSONObject();

        obj.AddField("en", "ZOO_GAME_PLACE_BET");
        cards.AddField(betID, ZooRoulette_Game.ZooRoulette_GameManager.instance._selectedBetItem.intValue);
        data.AddField("card_details", cards);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject ZOO_ROULETTE_PLACE_RE_BET(string betID, int bet)
    {
        Debug.Log("SEND PLACE RE-BET EVENT: " + betID + "|VALUE|" + bet);
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();
        JSONObject cards = new JSONObject();

        obj.AddField("en", "ZOO_GAME_PLACE_BET");
        cards.AddField(betID, bet);
        data.AddField("card_details", cards);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject ZOO_ROULETTE_CLOSE_GAME()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "ZOO_GAME_CLOSE_GAME");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject ZOO_JOINED_USER_LISTS()
    {
        Debug.Log("ZOO_GAME_JOINED_USER_LISTS");

        JSONObject obj = new JSONObject();

        obj.AddField("en", "ZOO_GAME_JOINED_USER_LISTS");
        obj.AddField("data", "");
        return obj;
    }
    //----------------------- Zoo-Roulette -----------------------------------------


    //----------------------- Mines -----------------------------------------
    internal JSONObject MINES_GAME_INFO()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "MINES_GAME_INFO");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject MINES_CLOSE_GAME()
    {
        JSONObject obj = new JSONObject();

        obj.AddField("en", "MINES_CLOSE_GAME");
        obj.AddField("data", "");
        return obj;
    }

    internal JSONObject MINES_START_SPIN()
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "MINES_START_SPIN");
        data.AddField("bet", Mines_UI_Manager.Inst.Txt_BET.text);
        data.AddField("mines", Mines_UI_Manager.Inst.Txt_Mines.text);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject MINES_CARD_CRASH(string ticket_ID, int Position)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "MINES_CARD_CRASH");
        data.AddField("ticket_id", ticket_ID);
        data.AddField("position", Position);
        obj.AddField("data", data);
        return obj;
    }

    internal JSONObject MINES_CLAIM_WIN(string ticket_ID)
    {
        JSONObject obj = new JSONObject();
        JSONObject data = new JSONObject();

        obj.AddField("en", "MINES_CLAIM_WIN");
        data.AddField("ticket_id", ticket_ID);
        obj.AddField("data", data);
        return obj;
    }
    //----------------------- Mines -----------------------------------------


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
