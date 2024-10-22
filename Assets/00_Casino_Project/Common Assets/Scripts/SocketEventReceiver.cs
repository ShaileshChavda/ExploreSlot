using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using CarRoulette_Game;
public class SocketEventReceiver : MonoBehaviour
{
    public static SocketEventReceiver Inst;
    public Coroutine _WaitMSG_Couratine;
    void Awake()
    {
        Inst = this;
    }
  
    private void OnEnable()
    {
        SocketHandler.OnSocketResponse += ReceiveData;
    }
    private void OnDisable()
    {
        SocketHandler.OnSocketResponse -= ReceiveData;
    }
    bool Applaunch = false;
    internal  void ReceiveData(JSONObject e)
    {
        var en = e.GetField("en").ToString().Trim(new char[] { '"' });
        var data = e.GetField("data");
       // Debug.Log("event name-----"+en);
        if (Applaunch)
            return;
        if (en.Equals("AppLunchDetails"))
            Applaunch = true;

        PreeLoader.Inst.Stop_Loader();
        switch (en)
        {
            case "AppLunchDetails":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    GS.Inst._userData.Id = data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
                    GS.Inst._userData.Name = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
                    GS.Inst._userData.PicUrl = data.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry);
                    GS.Inst._userData.Chips = float.Parse(data.GetField("chips").ToString().Trim(Config.Inst.trim_char_arry));
                    GS.Inst._userData.Bonus = float.Parse(data.GetField("bonus").ToString().Trim(Config.Inst.trim_char_arry));
                    GS.Inst._userData.UID = data.GetField("id").ToString().Trim(Config.Inst.trim_char_arry);
                    GS.Inst._userData.LoginType = data.GetField("user_type").ToString().Trim(Config.Inst.trim_char_arry);
                    GS.Inst._userData.User_VIP_Level = int.Parse(data.GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry));
                    GS.Inst._userData.Mobile = data.GetField("mobile_number").ToString().Trim(Config.Inst.trim_char_arry);
                    GS.Inst.Rejoin = bool.Parse(data.GetField("rejoin").ToString().Trim(Config.Inst.trim_char_arry));
                    GS.Inst.GameType = data.GetField("game_type").ToString().Trim(Config.Inst.trim_char_arry);
                    GS.Inst._userData.MyRefCode = data.GetField("refferal_code").ToString().Trim(Config.Inst.trim_char_arry);
                    GS.Inst._userData.MyChannelCode = data.GetField("channel_code").ToString().Trim(Config.Inst.trim_char_arry);

                    PlayerPrefs.SetString("Last_Login_User", GS.Inst._userData.LoginType);
                    PlayerPrefs.SetString("mobile", GS.Inst._userData.Mobile);

                    Applaunch = false;
                    SceneManager.LoadScene(2);
                }
                break;
            case "MOBILE_LINK_GUEST":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    Alert_MSG.Inst.MSG(data.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                    Profile.Inst.MOBILE_BOUND();
                }
                break;
            case "SEND_OTP":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                   Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    PhoneOTP_Firebase.Inst.msgText.text = "OTP Sent";
                }
                break;
            case "VERIFY_OTP":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                   Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    PhoneOTP_Firebase.Inst.VERIFY_DATA(data);
                }
                break;
            case "hb":
                SocketHandler.Inst._socketState = SocketState.Running;
                SocketHandler.Inst.isPongReceived = true;
                break;
            case "SETTING":
                GS.Inst.Game_Download_URL=data.GetField("refer_info").GetField("game_link").ToString().Trim(Config.Inst.trim_char_arry);
                GS.Inst.Service_link_URL=data.GetField("support_link").ToString().Trim(Config.Inst.trim_char_arry);
                GS.Inst.Share_RefCode_MSG=data.GetField("refer_info").GetField("msg").ToString().Trim(Config.Inst.trim_char_arry);
                GS.Inst.TermsCondition_link_URL=data.GetField("setting").GetField("terms_condi").ToString().Trim(Config.Inst.trim_char_arry);
                GS.Inst.PrivecyPolicy_link_URL=data.GetField("setting").GetField("privacy _policy").ToString().Trim(Config.Inst.trim_char_arry);
                GS.Inst.Cancellation_policy_link_URL=data.GetField("setting").GetField("cancellation_policy").ToString().Trim(Config.Inst.trim_char_arry);
                GS.Inst.AboutUs_link_URL=data.GetField("setting").GetField("about_us").ToString().Trim(Config.Inst.trim_char_arry);
                break;
            case "OSR":
                CommenMSG.Inst.MSG("MULTIPLE LOGIN", "Other user login same account...!");
                SocketHandler.Inst._socketState = SocketState.Close;
                SceneManager.LoadScene("Login");
                break;
          
            case "USER_BLOCK":
                bool block = bool.Parse(data.GetField("is_block").ToString().Trim(Config.Inst.trim_char_arry));
                if (block)
                {
                    if (SceneManager.GetActiveScene().name == "Login")
                        CommenMSG.Inst.MSG("USER BLOCK", data.GetField("message").ToString().Trim(Config.Inst.trim_char_arry));
                    else
                    {
                        CommenMSG.Inst.MSG("USER BLOCK", data.GetField("message").ToString().Trim(Config.Inst.trim_char_arry));
                        SceneManager.LoadScene("Login");
                    }
                }
                break;
            case "UPDATED_WALLET":
                GS.Inst._userData.Chips = float.Parse(data.GetField("chips").ToString().Trim(Config.Inst.trim_char_arry));
                //GS.Inst._userData.WonChips = double.Parse(data.GetField("game_winning").ToString().Trim(Config.Inst.trim_char_arry));
                GS.Inst._userData.Bonus = float.Parse(data.GetField("bonus").ToString().Trim(Config.Inst.trim_char_arry));

                if (SceneManager.GetActiveScene().name == "DragonTiger")
                {
                    GS.Inst._userData.Chips = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
                    DT_PlayerManager.Inst.Txt_MyUser_Chips.text = GS.Inst._userData.Chips.ToString("n2");
                }
                if (SceneManager.GetActiveScene().name == "Roulette")
                {
                    GS.Inst._userData.Chips = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
                    Roullate_PlayerManager.Inst.TxtUserChips.text = GS.Inst._userData.Chips.ToString("n2");
                }
                if (SceneManager.GetActiveScene().name == "Dashboard")
                {
                    GS.Inst._userData.Chips = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
                    DashboardManager.Inst.SET_DASHBOARD_DATA();
                }
                if (SceneManager.GetActiveScene().name == "Crash")
                {
                    GS.Inst._userData.Chips = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
                    CrashController.Instance.SetTotalWalletAmountFromServer((GS.Inst._userData.Chips));
                }
                if (SceneManager.GetActiveScene().name == "SlotMachin")
                {
                    GS.Inst._userData.Chips = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
                    Slot_UI_Manager.Inst.TxtChips.text = GS.Inst._userData.Chips.ToString("n2");
                }
                if (SceneManager.GetActiveScene().name == "HorsRacing")
                {
                    GS.Inst._userData.Chips = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
                    HR_PlayerManager.Inst.Txt_MyUser_Chips.text = GS.Inst._userData.Chips.ToString("n2");
                }
                if (SceneManager.GetActiveScene().name == "CarRouletteScene")
                {
                    GS.Inst._userData.Chips = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
                    CarRoulette_PlayerManager.Inst.TxtUserChips.text = GS.Inst._userData.Chips.ToString("n2");
                }
                if (SceneManager.GetActiveScene().name == "7UpDown")
                {
                    GS.Inst._userData.Chips = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
                    SevenUpDown_PlayerManager.Inst.Txt_MyUser_Chips.text = GS.Inst._userData.Chips.ToString("n2");
                }
                if (SceneManager.GetActiveScene().name == "AndarBahar")
                {
                    GS.Inst._userData.Chips = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
                    AB_PlayerManager.Inst.Txt_MyUser_Chips.text = GS.Inst._userData.Chips.ToString("n2");
                }
                if (SceneManager.GetActiveScene().name == "ZooRoulette")
                {
                    GS.Inst._userData.Chips = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
                    ZooRoulette_Game.ZooRoulette_PlayerManager.Inst.TxtUserChips.text = GS.Inst._userData.Chips.ToString("n2");
                }
                if (SceneManager.GetActiveScene().name == "Mines")
                {
                    GS.Inst._userData.Chips = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
                    Mines_UI_Manager.Inst.Txt_Player_Chips.text = GS.Inst._userData.Chips.ToString("n2");
                }
                break;

            //------------------------------- Dashboard -------------------------------------------------
            case "PROFILE_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Profile.Inst.SET_PROFILE_DATA(data);
                break;
            case "AVATARS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Profile.Inst.SET_AVATAR_LIST(data);
                break;
            case "DEPOSIT_LISTS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Shop.Inst.SET_DEPOSIT_DATA(data);
                break;
            case "SUCCESS_DEPOSIT":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Shop.Inst.Add_Chips_Success(data);
                break;
            case "SUPPORTS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Shop.Inst.SET_SUPPORT_DATA(data);
                break;
            case "RECORDS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Shop_Record.Inst.SET_RECORD_DATA(data);
                break;
            case "REFER_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Share.Inst.SET_DATA(data);
                break;
            case "SAFE_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Safe.Inst.SET_SAFE_DATA(data);
                break;
            case "SAFE_TAKE_IN":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Safe.Inst.SET_SAFE_DATA(data);
                break;
            case "SAFE_TAKE_OUT":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Safe.Inst.SET_SAFE_DATA(data);
                break;
            case "VIP_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    VIP.Inst.SET_VIP_DATA(data);
                break;
            case "WEEKLY_EXTRA_BONUS_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    RefAndEarn.Inst.SET_WEEKLY_EXTRABONUS_DATA(data);
                break;
            case "CLAIM_WEEKLY_EXTRA_BONUS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    RefAndEarn.Inst.SET_WEEKLY_EXTRABONUS_DATA(data);
                break;
            case "DAILY_BONUS_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    DailyBonus.Inst.SET_DAILY_BONUS_DATA(data);
                break;
            case "DAILY_BONUS_COLLECT":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    DailyBonus.Inst.OPEN_CLAIMED_MSG(data.GetField("bonus").ToString().Trim(Config.Inst.trim_char_arry));
                break;
            case "WEEKLY_BONUS_COLLECT":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    DailyBonus.Inst.OPEN_CLAIMED_MSG(data.GetField("bonus").ToString().Trim(Config.Inst.trim_char_arry));
                break;
            case "MONTHLY_BONUS_COLLECT":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    DailyBonus.Inst.OPEN_CLAIMED_MSG(data.GetField("bonus").ToString().Trim(Config.Inst.trim_char_arry));
                break;
            case "LEVEL_BONUS_COLLECT":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    DailyBonus.Inst.OPEN_CLAIMED_MSG(data.GetField("bonus").ToString().Trim(Config.Inst.trim_char_arry));
                break;
            case "RANK_LISTS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    //if (RefAndEarn.Inst.transform.localScale.x <= 0)
                    //    Ranking.Inst.SET_RANK_LIST(data);
                    //else
                        RefAndEarn.Inst.SET_RANK_LIST(data);
                }
                break;
            case "CHIPS_RANK_LISTS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                     Ranking.Inst.SET_RANK_LIST(data);
                }
                break;
            case "MAIL_LISTS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Mail.Inst.SET_MAIL_LIST(data);
                break;
            case "NEW_MAILS":
                if (!bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    if (SceneManager.GetActiveScene().name == "Dashboard")
                        Mail.Inst.MAIL_DOT_ACTION(bool.Parse(data.GetField("is_new_mail").ToString().Trim(Config.Inst.trim_char_arry)));
                break;
            case "MAIL_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Mail.Inst.SET_POPUP_DATA(data);
                break;
            case "MAIL_BONUS_CLAIM":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Alert_MSG.Inst.MSG(data.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                break;
            case "MAIL_REMOVE":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Alert_MSG.Inst.MSG(data.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                break;
            case "WITHDRAW_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Withdraw.Inst.SET_WITHDRAW_INFO(data);
                break;
            case "WITHDRAW_PLACE":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    Withdraw.Inst.CLOSE_Withdraw();
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                break;
            case "BANK_ADD":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                    AddBank.Inst.CLOSE_ADD_ACCOUNT();
                    Withdraw.Inst.UPDATE_DETAILS();
                }
                break;
            case "UPI_ADD":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                    AddBank.Inst.CLOSE_ADD_ACCOUNT();
                    Withdraw.Inst.UPDATE_DETAILS();
                }
                break;
            case "SPINNER_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    Spinner_Hendler.Inst.SET_SPINNER_DATA(data);
                }
                break;
            case "SPINNER_START":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    Spinner_Hendler.Inst.Spinner_START_DATA(data);
                }
                break;
            case "SPINNER_BIG_WINS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    Spinner_Records.Inst.SET_RECORD_DATA(data);
                }
                break;
            case "SPINNER_RECORDS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    Spinner_Records.Inst.SET_RECORD_DATA(data);
                }
                break;
            case "SPINNER_CURRENT_WINS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    Spinner_Records.Inst.SET_CURRENT_WIN_RECORD_DATA(data);
                }
                break;
            case "USER_WINNER_POPUP":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    NotyPopup.Inst.SET_NOTY_DATA(data);
                }
                break;
            case "REFER_BONUS_CLAIM":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    Alert_MSG.Inst.MSG(data.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                    RefAndEarn.Inst.TxtCurrent_Bonus.text = data.GetField("bonus").ToString().Trim(Config.Inst.trim_char_arry);
                }
                break;
            case "REFER_RULE_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    RefAndEarn.Inst.SET_REFER_RULES_DATA(data);
                }
                break;
            case "REFER_REFERREL_LISTS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    RefAndEarn.Inst.SET_REFERALS_DATA(data);
                }
                break;
            case "GET_BONUS_ERN_LIST":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    RefAndEarn.Inst.SET_BONUS_EARN_LIST(data);
                }
                break;
            case "GET_BONUS_RECORD":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    RefAndEarn.Inst.SET_BONUS_RECORD_LIST(data);
                }
                break;
            case "NOTICE_LISTS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    Notice.Inst.SET_HOT_AND_NOTICE_DATA(data);
                }
                break;
            case "MANUAL_PAYMENT_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    Shop.Inst.SET_MANUAL_PAYMENT_DATA(data);
                }
                break;
            case "MANUAL_PAYMENT":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Shop.Inst.OPEN_DEPOSIT();
                break;
            //------------------------------- Dashboard ---------------------------------------------------------

            //------------------------------- ROULETTE_EUROPEAN -------------------------------------------------
            case "ROULETTE_GAME_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    SceneManager.LoadScene(2);
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                    Roullate_Manager.Inst.JOIN_OLD_GAME(data);
                break;
            case "ROULETTE_INIT_GAME":
                Roullate_Timer.Inst.Txt_Timer_Status.text = "Open Time";
                Roullate_Manager.Inst.START_MAIN_TIMER(data, "N");
                Roullate_UI_Manager.Inst.Wait_Next_Round_POP(true);
                Roullate_PlayerManager.Inst.REFRESH_PLAYER_DATA(data);
                break;
            case "ROULETTE_GAME_TIMER_START":
                Roullate_Manager.Inst.Total_Bet_Pos_Count=0;
                Roullate_Manager.Inst.RESET_ALL_BOX_GLOW();
                Roullate_EventSetup.PFB_COIN_ROULLATE();
                Roullate_UI_Manager.Inst.NEW_ROUND_START_STOP(true,"sb");
                Roullate_UI_Manager.Inst.Wait_Next_Round_POP(false);
                Roullate_Manager.Inst.GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
                Roullate_UI_Manager.Inst.TxtGameID.text = Roullate_Manager.Inst.GameID;
                Roullate_PlayerManager.Inst._User_TotalBet = 0;
                Roullate_UI_Manager.Inst.Txt_TotalBet.text = "0/0";
                Roullate_Timer.Inst.Txt_Timer_Status.text = "Start Betting";
                Roullate_Manager.Inst.START_MAIN_TIMER(data,"M");
                break;
            case "ROULETTE_BET_INFO":
                Roullate_Manager.Inst.BET_INFO(data);
                break;
            case "ROULETTE_PLACE_BET":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                break;
            case "ROULETTE_START_SPIN":
                Roullate_Timer.Inst.Txt_Timer_Status.text = "Free Time";
                Roullate_Manager.Inst.START_SPIN(data);
                break;
            case "ROULETTE_WINNER_DECLARE":
                Roullate_Manager.Inst.WINNER_DECLARE(data);
                break;
            case "ROULETTE_WINNERS_INFO":
                Roullate_Manager.Inst.WinCoinMove_To_Winner(data);
                break;
            case "ROULETTE__LOSE":
                break;
            case "ROULETTE_LEAVE_USERS":
                Roullate_Manager.Inst.LEAVE_USER(data);
                break;
            case "ROULETTE_JOIN_USERS":
                Roullate_Manager.Inst.SEAT_USER(data);
                break;
            case "ROULETTE_JOINED_USER_LISTS":
                Roullate_Online_User_Manager.Inst.SET_ONLINE_USER_LIST(data);
                break;
            case "ROULETTE_CLOSE_GAME":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    SceneManager.LoadScene(2);
                break;

            //------------------------------- ROULETTE_EUROPEAN -------------------------------------------------

            //------------------------------- Dragon vs Tiger --------------------------------------
            case "DRAGON_TIGER_GAME_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    SceneManager.LoadScene(2);
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                    DT_Manager.Inst.JOIN_OLD_GAME(data);
                break;
            case "DRAGON_TIGER_INIT_GAME":
                DT_RatePercent.instance.SetRatePercentAndProgressBar();
                DT_UI_Manager.Inst.START_VS_SCREEN(true);
                DT_UI_Manager.Inst.RESET_UI_NEXT_ROUDN();
                DT_PlayerManager.Inst.REFRESH_PLAYER_DATA(data);
                break;
            case "DRAGON_TIGER_GAME_TIMER_START":
                DT_PlayerManager.Inst.Played_Chips = false;
                DT_Manager.Inst.RESET_CARD();
                DT_SoundManager.Inst.StopSFX();               
                DT_UI_Manager.Inst.START_VS_SCREEN(false);
                DT_EventSetup.PFB_COIN_DT();
                DT_UI_Manager.Inst.NEW_ROUND_START_STOP(true, "sb");
                DT_Manager.Inst.GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
                DT_UI_Manager.Inst.TxtGameID.text = DT_Manager.Inst.GameID;
                DT_Manager.Inst.START_MAIN_TIMER(data);
                break;
            case "DRAGON_TIGER_BET_INFO":
                DT_Manager.Inst.BET_INFO(data);
                break;
            case "DRAGON_TIGER_PLACE_BET":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                        if (data.GetField("card_details").HasField("dragon"))
                            DT_PlayerManager.Inst._User_TotalBet_Dragon = DT_PlayerManager.Inst._User_TotalBet_Dragon + DT_Manager.Inst.Selected_Bet_Amount;
                        else if (data.GetField("card_details").HasField("tie"))
                            DT_PlayerManager.Inst._User_TotalBet_Tie = DT_PlayerManager.Inst._User_TotalBet_Tie + DT_Manager.Inst.Selected_Bet_Amount;
                        else if (data.GetField("card_details").HasField("tiger"))
                            DT_PlayerManager.Inst._User_TotalBet_Tiger = DT_PlayerManager.Inst._User_TotalBet_Tiger + DT_Manager.Inst.Selected_Bet_Amount;
                }
                break;
            case "DRAGON_TIGER_START_SPIN":
                StartCoroutine(DT_Manager.Inst.START_SPIN(data));
                break;
            case "DRAGON_TIGER_WINNER_DECLARE":
                DT_Manager.Inst.WINNER_DECLARE(data);
                DT_RatePercent.instance.SetBaseProgressBar();
                break;
            case "DRAGON_TIGER_WINNERS_INFO":
                DT_Manager.Inst.WinCoinMove_To_Winner(data);
                break;
            case "DRAGON_TIGER_LOSE":
                break;
            case "DRAGON_TIGER_LEAVE_USERS":
                DT_Manager.Inst.LEAVE_USER(data);
                break;
            case "DRAGON_TIGER_JOIN_USERS":
                DT_Manager.Inst.SEAT_USER(data);
                break;
            case "DRAGON_TIGER_RESULT_HISTORY":
                DT_Full_HistoryManager.Inst.SET_DATA(data);
                break;
            case "DRAGON_TIGER_JOINED_USER_LISTS":
                DT_Online_User_Manager.Inst.SET_ONLINE_USER_LIST(data);
                break;
            case "DRAGON_TIGER_CLOSE_GAME":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    SceneManager.LoadScene(2);
                break;
            //------------------------------- Dragon vs Tiger --------------------------------------

            //------------------------------- CRASH --------------------------------------
            case "CRASH_GAME_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    SceneManager.LoadScene(2);
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                    Crash_Manager.Inst.CRASH_GAME_INFO(data);
                break;
            case "CRASH_INIT_GAME":
                CrashController.Instance.START_SCREEN(true);
                CrashController.Instance.ResetGame();
                break;
            case "CRASH_GAME_TIMER_START":                            
                Crash_PlayerManager.Inst.Played_Chips = false;
                Crash_SoundManager.Inst.StopSFX();
                CrashController.Instance.START_SCREEN(true);
                Crash_UI_Manager.Inst.BlockUIFull.SetActive(false);             
                Crash_Manager.Inst.START_MAIN_TIMER(data);
                break;
            case "CRASH_PLAN_START":                
                Crash_Manager.Inst.CRASH_PLAN_START(data);
                break;           
            case "CRASH_PLAN_CRASH":
                Debug.Log("CRASH_PLAN_CRASH: " + data.ToString());
                Crash_Manager.Inst.CRASH_PLAN_CRASH(data);
                break;
            case "CRASH_USER_CASH_OUT":
                Debug.Log("CRASH_USER_CASH_OUT: " + data.ToString());
                Crash_Manager.Inst.CRASH_USER_CASH_OUT(data);
                break;

            case "CRASH_CASH_IN":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Crash_Manager.Inst.CRASH_CASH_IN(data);
                break;
           
            case "CRASH_CASH_OUT":
                Debug.Log("CRASH_CASH_OUT: " + data.ToString());
                Crash_Manager.Inst.CRASH_CASH_OUT(data);
                break;
            case "CRASH_AUTO_REMOVE":
                Debug.Log("CRASH_AUTO_REMOVE: " + data.ToString());
                Crash_Manager.Inst.CRASH_AUTO_REMOVE(data);
                break;
            case "CRASH_RESULT_HISTORY":
                Debug.Log("CRASH_RESULT_HISTORY: "+data.ToString());
                Crash_Full_HistoryManager.Inst.SET_DATA(data);
                break;
            case "CRASH_CLOSE_GAME":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    SceneManager.LoadScene(2);
                break;
            //------------------------------- Crash --------------------------------------

            //------------------------------- Slot --------------------------------------
            case "SLOT_GAME_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    SceneManager.LoadScene(2);
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                    Slot_Manager.Inst.JOIN_OLD_GAME(data);
                break;
            case "SLOT_START_SPIN":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Slot_Manager.Inst.Spin_Started = false;
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                break;
            case "SLOT_PLACE_BET":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                break;
            case "SLOT_STOP_SPIN":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    if (!data.HasField("done"))
                    {
                        Slot_Manager.Inst.CLICK_ACTION = false;
                        Slot_SoundManager.Inst.PlaySFX(1);
                        StartCoroutine(Slot_Manager.Inst.SpinSlot_Anim(data));
                    }
                }
                break;
            case "SLOT_JACKPOT_AMOUNT_UPDATE":
                Slot_UI_Manager.Inst.Update_JACKPOT(data);
                break;
            case "SLOT_LUCKY_PLAYER":
                Slot_UI_Manager.Inst.Txt_LuckyPlayerName.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
                break;
            case "SLOT_CLOSE_GAME":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    SceneManager.LoadScene(2);
                break;
            case "SLOT_NEW_LUCKY_USERS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Slot_LuckyPlayer.Inst.SET_LIST_DATA(data);
                break;
            case "SLOT_LUCKY_USERS_HISTORY":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Slot_LuckyPlayer.Inst.SET_LIST_DATA(data);
                break;
            //------------------------------- Slot --------------------------------------

            //------------------------------- Horse racing --------------------------------------
            case "HORSE_RACING_GAME_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    SceneManager.LoadScene(2);
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                    HR_Manager.Inst.JOIN_OLD_GAME(data);
                break;
            case "HORSE_RACING_INIT_GAME":
                HR_Manager.Inst.InfoSetup = true;
                HR_UI_Manager.Inst.JackPot_CountBox_OFF();
                HR_UI_Manager.Inst.Close_Jackpot_Popup();
                HR_UI_Manager.Inst.Win_Highlight(7, "");
                HR_GroundManager.Inst.Win_Line_Idel();
                HR_First_HorseLine.Inst.transform.localScale = Vector3.zero;
                HR_PlayerManager.Inst.REFRESH_PLAYER_DATA(data);
                break;
            case "HORSE_RACING_SPEED_UPDATE":
                HR_Manager.Inst.UPDATE_HORSE_SPEED(data.GetField("speed_info"));
                break;
            case "HORSE_RACING_GAME_TIMER_START":
                HR_Manager.Inst.Reset_BetBox_Not3();
                HR_Manager.Inst.RESET_GROUND_DATA();
                HR_Manager.Inst.InfoSetup = true;
                HR_First_HorseLine.Inst.FIRST_LINE_LABLE();
                HR_UI_Manager.Inst.X_Ground_Box(true);
                HR_Manager.Inst.Close_WinAnim();
                HR_Manager.Inst.RESTART_ROUND();
                HR_UI_Manager.Inst.RESET_UI_NEXT_ROUDN();
                HR_UI_Manager.Inst.Close_Jackpot_Popup();
                HR_PlayerManager.Inst.Played_Chips = false;
                HR_SoundManager.Inst.StopSFX();
                HR_EventSetup.PFB_COIN_DT();
                HR_Manager.Inst.GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
                HR_Manager.Inst.InfoSetup = true;
                HR_UI_Manager.Inst.TxtGameID.text = HR_Manager.Inst.GameID;
                HR_UI_Manager.Inst.SET_TOTAL_X_CHIPS(data.GetField("win_xs"));
                HR_Manager.Inst.Speed_data = data.GetField("speed_info");
                HR_Manager.Inst.START_MAIN_TIMER(data);
                break;
            case "HORSE_RACING_BET_INFO":
                HR_Manager.Inst.BET_INFO(data);
                break;
            case "HORSE_RACING_PLACE_BET":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                break;
            case "HORSE_RACING_START_RACE":
                int time = int.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
                HR_Manager.Inst.SET_RACE_DATA(data);
                //HR_Manager.Inst.UPDATE_HORSE_SPEED(data.GetField("speed_info"));
                break;
            case "HORSE_RACING_WINNER_DECLARE":
                HR_SoundManager.Inst.StopOTHER_SFX();
                HR_First_HorseLine.Inst.transform.localScale = Vector3.zero;
                HR_Manager.Inst.WINNER_DECLARE(data);
                break;
            case "HORSE_RACING_WINNERS_INFO":
                if (bool.Parse(data.GetField("is_jackport").ToString().Trim(Config.Inst.trim_char_arry)))
                    HR_UI_Manager.Inst.Open_Jackpot_Popup(data);
                HR_GroundManager.Inst.Win_Line_Idel();
                HR_Manager.Inst.RESET_GROUND_DATA();
                HR_Manager.Inst.WinCoinMove_To_Winner(data);
                break;
            case "HORSE_RACING_LEAVE_USERS":
                HR_Manager.Inst.LEAVE_USER(data);
                break;
            case "HORSE_RACING_JOIN_USERS":
                HR_Manager.Inst.SEAT_USER(data);
                break;
            case "HORSE_RACING_RESULT_HISTORY":
                break;
            case "HORSE_RACING_LUCKY_USERS_HISTORY":
                HR_LuckyPlayer_Hendler.Inst.SET_LIST_DATA(data);
                break;
            case "HORSE_RACING_ONLINE_USERS_HISTORY":
                HR_LuckyPlayer_Hendler.Inst.SET_LIST_GAME_PLAYER_DATA(data);
                break;
            case "HORSE_RACING_TREND_REPORTS":
                HR_Trend.Inst.SET_TREND_DATA(data.GetField("game_lists")[0]);
                break;
            case "HORSE_RACING_JOINED_USER_LISTS":
                //DT_Online_User_Manager.Inst.SET_ONLINE_USER_LIST(data);
                break;
            case "HORSE_RACING_JACKPORT_USER_LISTS":
                HR_Jackpot_List_Hendler.Inst.SET_JACKPOT_DATA(data);
                break;
            case "HORSE_RACING_CHAT":
                HR_Chat.Inst.SET_AND_MOVE_STICKER(data);
                break;
            case "HORSE_RACING_CLOSE_GAME":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    SceneManager.LoadScene(2);
                break;
            //------------------------------- Horse racing --------------------------------------

            //------------------------------- Car roulette --------------------------------------
            case "CAR_GAME_GAME_INFO":
                CarRoulette_GameManager.instance.JOIN_OLD_GAME(data);
                break;
            case "CAR_GAME_INIT_GAME":
                CarRoulette_GameManager.instance.INIT_GAME(data);
                break;
            case "CAR_GAME_JOIN_USERS":
                CarRoulette_GameManager.instance.SEAT_USER(data);
                break;
            case "CAR_GAME_JOINED_USER_LISTS":
                if(CarRoulette_UIManager._instance.isUserList)
                {
                    Car_Roullate_Online_User_Manager.Inst.SET_ONLINE_USER_LIST(data);
                }
                else
                {
                    Car_Roullate_Online_User_Manager.Inst.GET_USERLIST(data);
                }
                break;
            case "CAR_GAME_TIMER_START":
                CarRoulette_GameManager.instance.PLACING_BET(data);
                break;
            case "CAR_GAME_START_SPIN":
                CarRoulette_GameManager.instance.START_SPIN(data);
                break;
            case "CAR_GAME_PLACE_BET":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                    CarRoulette_UIManager._instance.isWaitCond = false;
                    CarRoulette_GameManager.instance.StopSideChek();
                }
                else
                {
                    CarRoulette_UIManager._instance.btnRebet.interactable = false;
                    if (CarRoulette_GameManager.instance.isNewRound && !CarRoulette_UIManager._instance.isRebet)
                    {
                        CarRoulette_GameManager.instance.isNewRound = false;
                        CarRoulette_GameManager.instance.saveManager.ResetSaveRebetData();
                    }
                }
                break;
            case "CAR_GAME_BET_INFO":
                CarRoulette_GameManager.instance.BET_INFO(data);
                break;
            case "CAR_GAME_WINNER_DECLARE":
                CarRoulette_GameManager.instance.WINNER_DECLARE(data);
                break;
            case "CAR_GAME_WINNERS_INFO":
                CarRoulette_GameManager.instance.WINNER_TO_MOVE_CHIP(data);
                break;
            case "CAR_GAME_LOSE":
                break;
            case "CAR_GAME_LEAVE_USERS":
                CarRoulette_GameManager.instance.LEAVE_USER(data);
                break;
            case "CAR_GAME_CLOSE_GAME":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    SceneManager.LoadScene(2);
                break;
                //------------------------------- Car roulette --------------------------------------

                //-------------------- TeenPatti ------------------------
            case "TEENPATTI_BetList":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    PreeLoader.Inst.Stop_Loader();
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    TP_BetSelection.Inst.BTN_OPEN_BET_SCREEN();
                    TP_BetSelection.Inst.SET_BETLIST_DATA(data);
                }
                break;
            case "TEENPATTI_JoinTable":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    PreeLoader.Inst.Stop_Loader();
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                break;
            case "TEENPATTI_NewUserJoin"://If New User Table join then get this event to all user.
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    PreeLoader.Inst.Stop_Loader();
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    if (GS.Inst.GetCurrentScene.Equals("TeenPatti"))
                        TP_GameManager.Inst.SET_NEW_USER_JOIN_INFO(data);
                    else if (GS.Inst.GetCurrentScene.Equals("Dashboard"))
                        StartCoroutine(TEEN_PATTI_NEW_USER_SET_DATA(data));
                    else
                        DashboardManager.Inst.Addrasable_Scene_List[6].LoadScene();
                    //SceneManager.LoadScene("TeenPatti");
                }
                break;
            case "TEENPATTI_TableInfo"://If new user Join the get new user full table info.
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    PreeLoader.Inst.Stop_Loader();
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    GS.Inst.FullTableInfoData = data;
                    //SceneManager.LoadScene("TeenPatti");
                    DashboardManager.Inst.Addrasable_Scene_List[6].LoadScene();
                }
                break;
            case "TEENPATTI_Dealer_Msg":
                TP_GameManager.Inst.Activity_Message(data.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                break;
            case "TEENPATTI_LeaveTable":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    PreeLoader.Inst.Stop_Loader();
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    PreeLoader.Inst.Stop_Loader();
                    TP_GameManager.Inst.Table_Leave(data);
                }
                break;
            case "TEENPATTI_DealerTip":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    TP_GameManager.Inst.User_Tip(data);
                }
                break;
            case "TEENPATTI_GameTimerStart":
                TP_GameManager.Inst.Show_Round_Timer(int.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry)));
                break;
            case "TEENPATTI_BootCollect":
                TP_GameManager.Inst.MatchMaking_ANIM.SetActive(false);
                TP_GameManager.Inst.Boot_Collection(data);
                break;
            case "TEENPATTI_UserCard":
                break;
            case "TEENPATTI_UserWalletUpdate":
                TP_GameManager.Inst.Wallete_Update(data);
                break;
            case "TEENPATTI_CardDeal":
                TP_GameManager.Inst.Card_Deal_Now(data);
                break;
            case "TEENPATTI_TurnStart":
                TP_GameManager.Inst.User_Turn_Start(data);
                break;
            case "TEENPATTI_Chal":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    TP_GameManager.Inst.User_Chaal(data);
                }
                break;
            case "TEENPATTI_SideShowRequest":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    TP_SIdeShow_Hendler.Inst.HENDLE_SIDE_SHOW_REQ(data);
                }
                break;
            case "TEENPATTI_SideShowRequestReject":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    TP_SIdeShow_Hendler.Inst.HENDLE_SIDE_SHOW_REJECT(data);
                }
                break;
            case "TEENPATTI_SideShowRequestAccept":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    TP_SIdeShow_Hendler.Inst.HENDLE_SIDE_SHOW_ACCEPT(data);
                }
                break;
            case "TEENPATTI_Show":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    TP_GameManager.Inst.User_Chaal(data);
                }
                break;
            case "TEENPATTI_ViewCard":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    TP_GameManager.Inst.User_Card_See(data);
                }
                break;
            case "TEENPATTI_ViewCardInfo":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    TP_GameManager.Inst.User_Card_See_DATA(data);
                }
                break;
            case "TEENPATTI_CardPack":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    TP_GameManager.Inst.User_CardPack(data);
                }
                break;
            case "TEENPATTI_Winners":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    TP_GameManager.Inst.Winning_DATA_SET(data);
                }
                break;
            case "TEENPATTI_FinishRound":
                break;
            case "TEENPATTI_WaittingOppoMsg":
                //if (SceneManager.GetActiveScene().name == "Dashboard")
                _WaitMSG_Couratine = StartCoroutine(TEEN_PATTI_WAIT_OPP(data));
                //else
                //    TP_GameManager.Inst.SET_USER_WAIT_MSG(data);
                break;
            case "TEENPATTI_TableShortInfo":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    TP_TableInfo.Inst.SET_TABLE_INFO_DATA(data);
                }
                break;
            case "TEENPATTI_SwitchTable":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    PreeLoader.Inst.Stop_Loader();
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                break;
            case "TEENPATTI_PrivateTable":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    PreeLoader.Inst.Stop_Loader();
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                break;
            case "TEENPATTI_LowWalletPopup":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    PreeLoader.Inst.Stop_Loader();
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    TP_Balance_Warning.Inst.Open_Warning();
                }
                break;
            case "TEENPATTI_JoinPrivateTable":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    PreeLoader.Inst.Stop_Loader();
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                break;
            case "TEENPATTI_ReJoinTable":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    PreeLoader.Inst.Stop_Loader();
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    GS.Inst.FullTableInfoData = data;
                    GS.Inst.PrivateTable = bool.Parse(data.GetField("is_private").ToString().Trim(Config.Inst.trim_char_arry));
                    //if (!GS.Inst.PrivateTable)
                    //{
                    //    GS.Inst.isFromRejoin = false;
                    //    SocketHandler.Inst.SendData(SocketEventManager.Inst.LUDO_LeaveTable());
                    //    LoadingScreen.Inst.OpenLoadingScreen("Please wait..");
                    //    SceneManager.LoadScene("Dashboard");
                    //}
                    //else
                    //{
                        PreeLoader.Inst.Show();
                        //LoadingScreen.Inst.OpenLoadingScreen("Reconnecting..");
                        //SceneManager.LoadScene("TeenPatti");
                        DashboardManager.Inst.Addrasable_Scene_List[6].LoadScene();
                    //}
                }
                break;
            //-----------------TeenPatti-----------------------------


            //------------------------------- Seven Up Down --------------------------------------
            case "SEVEN_UP_GAME_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    SceneManager.LoadScene(2);
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                    SevenUpDown_Manager.Inst.JOIN_OLD_GAME(data);
                break;
            case "SEVEN_UP_INIT_GAME":
                SevenUpDown_UI_Manager.Inst.SetLogMessage("SEVEN_UP_INIT_GAME");
                SevenUpDown_UI_Manager.Inst.RESET_UI_NEXT_ROUDN();
                SevenUpDown_PlayerManager.Inst.REFRESH_PLAYER_DATA(data);
                break;
            case "SEVEN_UP_GAME_TIMER_START":
                SevenUpDown_UI_Manager.Inst.SetLogMessage("SEVEN_UP_GAME_TIMER_START & " + "Timer: " + data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
                SevenUpDown_PlayerManager.Inst.Played_Chips = false;
                SevenUpDown_SoundManager.Inst.StopSFX();
                SevenUpDown_EventSetup.PFB_COIN_DT();
                SevenUpDown_UI_Manager.Inst.NEW_ROUND_START_STOP(true, "sb");
                SevenUpDown_Manager.Inst.GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
                SevenUpDown_UI_Manager.Inst.TxtGameID.text = SevenUpDown_Manager.Inst.GameID;
                SevenUpDown_Manager.Inst.START_MAIN_TIMER(data);
                break;
            case "SEVEN_UP_BET_INFO":                
                SevenUpDown_Manager.Inst.BET_INFO(data);
                break;
            case "SEVEN_UP_PLACE_BET":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    if (data.GetField("card_details").HasField("two_six"))
                        SevenUpDown_PlayerManager.Inst._User_TotalBet_Dragon = SevenUpDown_PlayerManager.Inst._User_TotalBet_Dragon + SevenUpDown_Manager.Inst.Selected_Bet_Amount;
                    else if (data.GetField("card_details").HasField("seven"))
                        SevenUpDown_PlayerManager.Inst._User_TotalBet_Tie = SevenUpDown_PlayerManager.Inst._User_TotalBet_Tie + SevenUpDown_Manager.Inst.Selected_Bet_Amount;
                    else if (data.GetField("card_details").HasField("eight_twelve"))
                        SevenUpDown_PlayerManager.Inst._User_TotalBet_Tiger = SevenUpDown_PlayerManager.Inst._User_TotalBet_Tiger + SevenUpDown_Manager.Inst.Selected_Bet_Amount;
                }
                break;
            case "SEVEN_UP_START_SPIN":
                SevenUpDown_UI_Manager.Inst.SetLogMessage("SEVEN_UP_START_SPIN & " + "Timer: " + data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
                StartCoroutine(SevenUpDown_Manager.Inst.START_SPIN(data));
                break;
            case "SEVEN_UP_WINNER_DECLARE":
                SevenUpDown_UI_Manager.Inst.SetLogMessage("SEVEN_UP_WINNER_DECLARE & " + "next_deal_time: " + data.GetField("next_deal_time").ToString().Trim(Config.Inst.trim_char_arry));
                SevenUpDown_Manager.Inst.WINNER_DECLARE(data);
                break;
            case "SEVEN_UP_WINNERS_INFO":
                SevenUpDown_Manager.Inst.WinCoinMove_To_Winner(data);
                break;
            case "SEVEN_UP_LOSE":
                break;
            case "SEVEN_UP_LEAVE_USERS":
                SevenUpDown_Manager.Inst.LEAVE_USER(data);
                break;
            case "SEVEN_UP_JOIN_USERS":
                SevenUpDown_Manager.Inst.SEAT_USER(data);
                break;
            case "SEVEN_UP_RESULT_HISTORY":
                // SevenUpDown_Full_HistoryManager.Inst.SET_DATA(data);
                break;
            case "SEVEN_UP_JOINED_USER_LISTS":
                SevenUpDown_Online_User_Manager.Inst.SET_ONLINE_USER_LIST(data);
                break;
            case "SEVEN_UP_CLOSE_GAME":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    SceneManager.LoadScene(2);
                break;
            //------------------------------- Seven Up Down --------------------------------------

            //------------------------------- Zoo roulette --------------------------------------
            case "ZOO_GAME_GAME_INFO":
                ZooRoulette_Game.ZooRoulette_GameManager.instance.JOIN_OLD_GAME(data);
                break;
            case "ZOO_GAME_INIT_GAME":
                ZooRoulette_Game.ZooRoulette_GameManager.instance.INIT_GAME(data);
                break;
            case "ZOO_GAME_JOIN_USERS":
                ZooRoulette_Game.ZooRoulette_GameManager.instance.SEAT_USER(data);
                break;
            case "ZOO_GAME_JOINED_USER_LISTS":

                ZooRoulette_Game.ZooRoulette_Online_User_Manager.Inst.SET_ONLINE_USER_LIST(data);

                /*if (ZooRoulette_Game.ZooRoulette_UIManager._instance.isUserList)
                {
                    ZooRoulette_Game.ZooRoulette_Online_User_Manager.Inst.SET_ONLINE_USER_LIST(data);
                }
                else
                {
                    ZooRoulette_Game.ZooRoulette_Online_User_Manager.Inst.GET_USERLIST(data);
                }*/
                break;
            case "ZOO_GAME_TIMER_START":
                ZooRoulette_Game.ZooRoulette_GameManager.instance.PLACING_BET(data);
                break;
            case "ZOO_GAME_START_SPIN":
                ZooRoulette_Game.ZooRoulette_GameManager.instance.START_SPIN(data);
                break;
            case "ZOO_GAME_PLACE_BET":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                    ZooRoulette_Game.ZooRoulette_UIManager._instance.isWaitCond = false;
                    ZooRoulette_Game.ZooRoulette_GameManager.instance.StopSideChek();
                }
                else
                {
                    ZooRoulette_Game.ZooRoulette_UIManager._instance.btnRebet.interactable = false;
                    if (ZooRoulette_Game.ZooRoulette_GameManager.instance.isNewRound && !ZooRoulette_Game.ZooRoulette_UIManager._instance.isRebet)
                    {
                        ZooRoulette_Game.ZooRoulette_GameManager.instance.isNewRound = false;
                        ZooRoulette_Game.ZooRoulette_GameManager.instance.saveManager.ResetSaveRebetData();
                    }
                }
                break;
            case "ZOO_GAME_BET_INFO":
                ZooRoulette_Game.ZooRoulette_GameManager.instance.BET_INFO(data);
                break;
            case "ZOO_GAME_WINNER_DECLARE":
                ZooRoulette_Game.ZooRoulette_GameManager.instance.WINNER_DECLARE(data);
                break;
            case "ZOO_GAME_WINNERS_INFO":
                ZooRoulette_Game.ZooRoulette_GameManager.instance.WINNER_TO_MOVE_CHIP(data);
                break;
            case "ZOO_GAME_LOSE":
                break;
            case "ZOO_GAME_LEAVE_USERS":
                ZooRoulette_Game.ZooRoulette_GameManager.instance.LEAVE_USER(data);
                break;
            case "ZOO_GAME_CLOSE_GAME":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    SceneManager.LoadScene(2);
                break;

                 //------------------------------- Andar Bahar --------------------------------------
            case "ANDAR_BAHAR_GAME_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    SceneManager.LoadScene(2);
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                    AB_Manager.Inst.JOIN_OLD_GAME(data);
                break;
            case "ANDAR_BAHAR_INIT_GAME":
                AB_Full_History_Manager.Inst.RESET_Probability();
                AB_PlayerManager.Inst.REFRESH_PLAYER_DATA(data);
                AB_Dealcard.Inst.Reset_Cards();
                AB_Manager.Inst.START_MAIN_TIMER(data,true);
                AB_UI_Manager.Inst.RESET_UI_NEXT_ROUDN();
                break;
            case "ANDAR_BAHAR_GAME_TIMER_START":
                AB_PlayerManager.Inst.Played_Chips = false;
                AB_Dealcard.Inst.Destroyee_Old_ThrowCards();
                AB_SoundManager.Inst.StopSFX();
                AB_EventSetup.PFB_COIN_DT();
                AB_UI_Manager.Inst.NEW_ROUND_START_STOP(true, "sb");
                AB_Manager.Inst.GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
                AB_UI_Manager.Inst.TxtGameID.text = AB_Manager.Inst.GameID;
                AB_Manager.Inst.START_MAIN_TIMER(data,false);
                StartCoroutine(AB_Dealcard.Inst.Spin_JockerCard(data.GetField("joker_cards").ToString().Trim(Config.Inst.trim_char_arry)));
                break;
            case "ANDAR_BAHAR_BET_INFO":
                AB_Manager.Inst.BET_INFO(data);
                break;
            case "ANDAR_BAHAR_PLACE_BET":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    if (data.GetField("card_details").HasField("andar"))
                        AB_PlayerManager.Inst._User_TotalBet_Andar = AB_PlayerManager.Inst._User_TotalBet_Andar + AB_Manager.Inst.Selected_Bet_Amount;
                    else if (data.GetField("card_details").HasField("bahar"))
                        AB_PlayerManager.Inst._User_TotalBet_Bahar = AB_PlayerManager.Inst._User_TotalBet_Bahar + AB_Manager.Inst.Selected_Bet_Amount;
                    else if (data.GetField("card_details").HasField("1-5"))
                        AB_PlayerManager.Inst._User_TotalBet_1_5 = AB_PlayerManager.Inst._User_TotalBet_1_5 + AB_Manager.Inst.Selected_Bet_Amount;
                    else if (data.GetField("card_details").HasField("6-10"))
                        AB_PlayerManager.Inst._User_TotalBet_6_10 = AB_PlayerManager.Inst._User_TotalBet_6_10 + AB_Manager.Inst.Selected_Bet_Amount;
                    else if (data.GetField("card_details").HasField("11-15"))
                        AB_PlayerManager.Inst._User_TotalBet_11_15 = AB_PlayerManager.Inst._User_TotalBet_11_15 + AB_Manager.Inst.Selected_Bet_Amount;
                    else if (data.GetField("card_details").HasField("16-25"))
                        AB_PlayerManager.Inst._User_TotalBet_16_25 = AB_PlayerManager.Inst._User_TotalBet_16_25 + AB_Manager.Inst.Selected_Bet_Amount;
                    else if (data.GetField("card_details").HasField("26-30"))
                        AB_PlayerManager.Inst._User_TotalBet_26_30 = AB_PlayerManager.Inst._User_TotalBet_26_30 + AB_Manager.Inst.Selected_Bet_Amount;
                    else if (data.GetField("card_details").HasField("31-35"))
                        AB_PlayerManager.Inst._User_TotalBet_31_35 = AB_PlayerManager.Inst._User_TotalBet_31_35 + AB_Manager.Inst.Selected_Bet_Amount;
                    else if (data.GetField("card_details").HasField("36-40"))
                        AB_PlayerManager.Inst._User_TotalBet_36_40 = AB_PlayerManager.Inst._User_TotalBet_36_40 + AB_Manager.Inst.Selected_Bet_Amount;
                    else if (data.GetField("card_details").HasField("41-48"))
                        AB_PlayerManager.Inst._User_TotalBet_41_48 = AB_PlayerManager.Inst._User_TotalBet_41_48 + AB_Manager.Inst.Selected_Bet_Amount;
                }
                break;
            case "ANDAR_BAHAR_START_SPIN":
                AB_Manager.Inst.START_SPIN(data);
                break;
            case "ANDAR_BAHAR_WINNER_DECLARE":
                AB_Manager.Inst.WINNER_DECLARE(data);
                break;
            case "ANDAR_BAHAR_WINNERS_INFO":
                AB_Manager.Inst.WinCoinMove_To_Winner(data);
                break;
            case "ANDAR_BAHAR_LOSE":
                break;
            case "ANDAR_BAHAR_LEAVE_USERS":
                AB_Manager.Inst.LEAVE_USER(data);
                break;
            case "ANDAR_BAHAR_JOIN_USERS":
                AB_Manager.Inst.SEAT_USER(data);
                break;
            case "ANDAR_BAHAR_RESULT_HISTORY":
                DT_Full_HistoryManager.Inst.SET_DATA(data);
                break;
            case "ANDAR_BAHAR_JOINED_USER_LISTS":
                AB_Online_User_Manager.Inst.SET_ONLINE_USER_LIST(data);
                break;
            case "ANDAR_BAHAR_CLOSE_GAME":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    SceneManager.LoadScene(2);
                break;
            //------------------------------- Andar Bahar --------------------------------------

            //------------------------------- Mines --------------------------------------
            case "MINES_GAME_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    SceneManager.LoadScene(2);
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                    Mines_Manager.Inst.JOIN_OLD_GAME(data);
                break;
            case "MINES_START_SPIN":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    Mines_Manager.Inst.GameState = "spin_start";
                    Mines_Winning.Inst.Reset_Win();
                    Mines_Manager.Inst.BTN_SPIN_Disable.SetActive(true);
                    Mines_Manager.Inst.BTN_CLEAR_Disable.SetActive(true);
                    Mines_Manager.Inst.Btn_Left_TRX.interactable = false;
                    Mines_Manager.Inst.Btn_Right_TRX.interactable = false;
                    Mines_Manager.Inst.TRS_GLOW_RESET();
                    Mines_Manager.Inst.RESET_SCRATCH_BOX();
                    Mines_Manager.Inst.REJOIN_TRS_MOVE(int.Parse(Mines_UI_Manager.Inst.Txt_Mines.text));
                    Mines_Manager.Inst.GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
                    Mines_UI_Manager.Inst.Txt_GameID.text = Mines_Manager.Inst.GameID;
                    Mines_Manager.Inst.Round_Ticket_ID = data.GetField("ticket_id").ToString().Trim(Config.Inst.trim_char_arry);
                }
                break;
            case "MINES_CARD_CRASH":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Mines_Manager.Inst.SET_CRATCH_DATA(data);
                break;
            case "MINES_CLAIM_WIN":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    Mines_Manager.Inst.GameState = "start_win";
                    Mines_Winning.Inst.SET_WIN_DATA(data);
                }
                break;
            case "MINES_CLOSE_GAME":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    SceneManager.LoadScene(2);
                break;
            //------------------------------- Mines --------------------------------------


            //------------------------------- Explorere2 --------------------------------------
            case "EXPLORERE_TWO_GAME_INFO":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    SceneManager.LoadScene(2);
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                    EXP_Manager.Inst.JOIN_OLD_GAME(data);
                break;
            case "EXPLORERE_TWO_START_SPIN":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    EXP_Manager.Inst.Spin_Started = false;
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                }
                break;
            case "EXPLORERE_TWO_PLACE_BET":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                break;
            case "EXPLORERE_TWO_STOP_SPIN":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                {
                    if (!data.HasField("done"))
                    {
                        EXP_Manager.Inst.CLICK_ACTION = false;
                        EXP_SoundManager.Inst.PlaySFX(1);
                        StartCoroutine(EXP_Manager.Inst.SpinSlot_Anim(data));
                    }
                }
                break;
            case "EXPLORERE_TWO_JACKPOT_AMOUNT_UPDATE":
                EXP_UI_Manager.Inst.Update_JACKPOT(data);
                break;
            //case "SLOT_LUCKY_PLAYER":
            //    Slot_UI_Manager.Inst.Txt_LuckyPlayerName.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
            //    break;
            case "EXPLORERE_TWO_CLOSE_GAME":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    SceneManager.LoadScene(2);
                break;
            case "EXPLORERE_TWO_NEW_LUCKY_USERS":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Slot_LuckyPlayer.Inst.SET_LIST_DATA(data);
                break;
            case "EXPLORERE_TWO_LUCKY_USERS_HISTORY":
                if (bool.Parse(e.GetField("err").ToString().Trim(Config.Inst.trim_char_arry)))
                    Alert_MSG.Inst.MSG(e.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry));
                else
                    Slot_LuckyPlayer.Inst.SET_LIST_DATA(data);
                break;
                //------------------------------- Explorere2 --------------------------------------

        }
    }
    IEnumerator TEEN_PATTI_WAIT_OPP(JSONObject data)
    {
        yield return new WaitForSeconds(2f);
        TP_GameManager.Inst.SET_USER_WAIT_MSG(data);
        //Debug
        //if (GS.Inst.PrivateTable)
        //{
        //    TP_RoomCodeShare.Inst.Private_timer = int.Parse(data.GetField("time").ToString().Trim(Config.Inst.trim_char_arry));
        //    if (TP_RoomCodeShare.Inst.Private_timer > 0)
        //        TP_RoomCodeShare.Inst.Start_timer();
        //}
    }
    IEnumerator TEEN_PATTI_NEW_USER_SET_DATA(JSONObject data)
    {
        yield return new WaitForSeconds(1.5f);
        TP_GameManager.Inst.SET_NEW_USER_JOIN_INFO(data);

        //if (GS.Inst.PrivateTable)
        //{
        //    TP_RoomCodeShare.Inst.Private_timer = int.Parse(data.GetField("time").ToString().Trim(Config.Inst.trim_char_arry));
        //    if (TP_RoomCodeShare.Inst.Private_timer > 0)
        //        TP_RoomCodeShare.Inst.Start_timer();
        //}
    }
}
