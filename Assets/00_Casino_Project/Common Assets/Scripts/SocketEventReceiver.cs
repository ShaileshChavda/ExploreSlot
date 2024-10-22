using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
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

                if (SceneManager.GetActiveScene().name == "Dashboard")
                {
                    GS.Inst._userData.Chips = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
                    DashboardManager.Inst.SET_DASHBOARD_DATA();
                }
                if (SceneManager.GetActiveScene().name == "SlotMachin")
                {
                    GS.Inst._userData.Chips = float.Parse(data.GetField("total_wallet").ToString().Trim(Config.Inst.trim_char_arry));
                    Slot_UI_Manager.Inst.TxtChips.text = GS.Inst._userData.Chips.ToString("n2");
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
}
