using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash_Manager : MonoBehaviour
{
    public static Crash_Manager Inst;
    public int Selected_Bet_Amount;
    public GameObject Selected_Bet_Ring, Selected_Bet_Tick;
    [SerializeField] bool InfoSetup;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        Inst = this;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.CRASH_GAME_INFO());
    }

    //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE ------------------------------------
    public void CRASH_GAME_INFO(JSONObject data)
    {
        Debug.Log("CRASH_GAME_INFO: " + data);
        InfoSetup = false;       
      
        string game_state = data.GetField("game_state").ToString().Trim(Config.Inst.trim_char_arry);
        
        Crash_PlayerManager.Inst.SET_PLAYER_DATA(data);

        switch (game_state)
        {
            case "init_game":
                //Roullate_Timer.Inst.Txt_Timer_Status.text = "Start Betting";
                //START_MAIN_TIMER(data, "M");
                break;
            case "bet_timer_start":
                Crash_PlayerManager.Inst.Played_Chips = false;
                Crash_SoundManager.Inst.StopSFX();                
                CrashController.Instance.ResetGame();
                CrashController.Instance.START_SCREEN(true);
                Crash_UI_Manager.Inst.BlockUIFull.SetActive(false);// Start Betting popup avse
               // Crash_UI_Manager.Inst.NEW_ROUND_START_STOP(true, "sb");  // Start Betting popup avse
                START_MAIN_TIMER(data);
                break;
            case "start_plan":
                // rocket ne fly karine pa6i jetli second avi hoe ae second lae ne position set kari devi ane tya thi regular fly karvanu chalu karse
                float planeCrashTime = 0.0f;
                Debug.Log("start_plan Event ");
                if (!string.IsNullOrEmpty(data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    planeCrashTime = float.Parse(data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry));
                    float timer = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));

                    float currTimeOfPlanePos = planeCrashTime - timer;
                    if (currTimeOfPlanePos < 0)
                    {
                        currTimeOfPlanePos = 0;
                    }
                    Debug.Log("start_plan TIME: "+ currTimeOfPlanePos);
                    Debug.Log("PLANE CRASH TIME: " + planeCrashTime);

                    if (!string.IsNullOrEmpty(data.GetField("win_info").GetField("crash_reward").ToString().Trim(Config.Inst.trim_char_arry)))
                    {
                        float crash_reward = float.Parse(data.GetField("win_info").GetField("crash_reward").ToString().Trim(Config.Inst.trim_char_arry));

                        StartCoroutine(FlyRocketDelay(crash_reward, currTimeOfPlanePos, false, planeCrashTime));
                       // CrashController.Instance.FlyRocket(crash_reward, currTimeOfPlanePos, false, planeCrashTime); // with rejoin the game
                    }
                }
                string cashOutStatus = data.GetField("bet_info").GetField("status").ToString().Trim(Config.Inst.trim_char_arry);

                if (cashOutStatus.Equals("pendding"))
                {
                    // cash out nu button show karvu
                    CrashController.Instance.cashOutButton.SetActive(true);
                }
                Crash_UI_Manager.Inst.BlockUIFull.SetActive(true);// Start Betting popup avse
                //Crash_UI_Manager.Inst.NEW_ROUND_START_STOP(true, "");  // Stop Betting popup avse
                break;
            case "crash_plan":
                /*if (!string.IsNullOrEmpty(data.GetField("win_info").GetField("crash_reward").ToString().Trim(Config.Inst.trim_char_arry)))
                {
                    float crash_reward = float.Parse(data.GetField("win_info").GetField("crash_reward").ToString().Trim(Config.Inst.trim_char_arry));
                    CrashController.Instance.GameCrashAt(crash_reward);
                }*/
                break;
            case "finish_state":
                //CrashController.Instance.ResetGame();
                break;
        }

        Crash_HistoryManager.Inst.SET_HISTO(data);
        GS.Inst.Rejoin = false;
    }
    IEnumerator FlyRocketDelay(float crash_reward, float currTimeOfPlanePos,bool value, float planeCrashTime)
    {
        yield return null;
  
        CrashController.Instance.FlyRocket(crash_reward, currTimeOfPlanePos, value, planeCrashTime); // with rejoin the game
    }
    public void USER_SEND_BET(int BetAmount, float flee_condition = 0.0f, int profit_on_stop = 0, int loss_on_stop = 0,string mode="")
    {
        Debug.Log("Total Place Bet:  " + BetAmount);

        SocketHandler.Inst.SendData(SocketEventManager.Inst.CRASH_CASH_IN(BetAmount, flee_condition,  profit_on_stop, loss_on_stop,mode));
    }
    public void CRASH_CASH_OUT(float time)
    {
        SocketHandler.Inst.SendData(SocketEventManager.Inst.CRASH_CASH_OUT(time));
    }
    public void CRASH_CASH_IN(JSONObject data) 
    { 
        string game_id = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
        string user_name = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        float start_point = float.Parse(data.GetField("start_point").ToString().Trim(Config.Inst.trim_char_arry));
        int total_bet_amount = int.Parse(data.GetField("total_bet_amount").ToString().Trim(Config.Inst.trim_char_arry));
        string status = data.GetField("status").ToString().Trim(Config.Inst.trim_char_arry);
       
        Debug.Log("Bet Placed Success: "+ total_bet_amount);
        Debug.Log("cash in status: " + status); // if status == "pending" means cash in success thayu chhe etle cash out nu button show karvu
        if (status.Equals("pendding"))
        {
            CrashController.Instance.cashOutBtnStatus = true;
        }
    }
    //------------------------------ Timer Start -----------------------------------------------------------
    public void START_MAIN_TIMER(JSONObject data)
    {
        Debug.Log("CRASH_GAME_TIMER_START: " + data);       
        float time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
        Debug.Log("CRASH_GAME_TIMER_START: "+ time);

        if (time < 20)
            Crash_Timer.Inst.StartTimerAnim(time, time, true);
        else
            Crash_Timer.Inst.StartTimerAnim(time, time, false);

        InfoSetup = true;       
    }
    //------------------------------ Timer Start -----------------------------------------------------------

    public void CRASH_PLAN_START(JSONObject data)
    {
        Debug.Log("CRASH_PLAN_START: "+ data);
        float timer = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
        float sec = float.Parse(data.GetField("win_info").GetField("sec").ToString().Trim(Config.Inst.trim_char_arry));
        float crash_reward = float.Parse(data.GetField("win_info").GetField("crash_reward").ToString().Trim(Config.Inst.trim_char_arry));
        Debug.Log("PLAN_CRASH Time: " + timer);
        CrashController.Instance.FlyRocket(crash_reward, sec,true, timer); // without rejoin the game
        InfoSetup = true;
    }
    public void CRASH_PLAN_CRASH(JSONObject data)
    {
        Debug.Log("CRASH_PLAN_CRASH: ");
        CrashController.Instance.cashOutButton.SetActive(false);
        Crash_HistoryManager.Inst.SET_HISTO(data);     

        /*float crash_time = float.Parse(data.GetField("crash_time").ToString().Trim(Config.Inst.trim_char_arry));
        float per_sec_reward = float.Parse(data.GetField("per_sec_reward").ToString().Trim(Config.Inst.trim_char_arry));
        float crash_reward = crash_time * per_sec_reward;
        CrashController.Instance.GameCrashAt(crash_reward);*/

        // CrashController.Instance.CrashRocket(true);
        InfoSetup = true;
    }
    public void CRASH_USER_CASH_OUT(JSONObject data)
    {
        string name = data.GetField("name").ToString().Trim(Config.Inst.trim_char_arry);
        float crash_reward = float.Parse(data.GetField("crash_reward").ToString().Trim(Config.Inst.trim_char_arry));
        
        CrashController.Instance.ShowOtherPlayerCashout(name, crash_reward);
        InfoSetup = true;
    }

    public void CRASH_CASH_OUT(JSONObject data)
    {
        string game_id = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
        string user_name = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        string status = data.GetField("status").ToString().Trim(Config.Inst.trim_char_arry);
        float total_win_amount = float.Parse(data.GetField("total_win_amount").ToString().Trim(Config.Inst.trim_char_arry));
        float start_point = float.Parse(data.GetField("start_point").ToString().Trim(Config.Inst.trim_char_arry));               

        if (status == "complete")
        {
            Debug.Log("1111111111");
            CrashController.Instance.ShowWinningAmount(total_win_amount);

            if (!string.IsNullOrEmpty(data.GetField("crash_config").ToString().Trim(Config.Inst.trim_char_arry)))
            {
                Debug.Log("2222222222222222");

                float flee_condition = float.Parse(data.GetField("crash_config").GetField("flee_condition").ToString());
                int profit_on_stop = int.Parse(data.GetField("crash_config").GetField("profit_on_stop").ToString().Trim(Config.Inst.trim_char_arry));
                float profit_win_amount = float.Parse(data.GetField("crash_config").GetField("profit_win_amount").ToString().Trim(Config.Inst.trim_char_arry));
                int loss_on_stop = int.Parse(data.GetField("crash_config").GetField("loss_on_stop").ToString().Trim(Config.Inst.trim_char_arry));
                float profit_loss_amount = float.Parse(data.GetField("crash_config").GetField("profit_loss_amount").ToString().Trim(Config.Inst.trim_char_arry));
                bool auto_remove = bool.Parse(data.GetField("crash_config").GetField("auto_remove").ToString().Trim(Config.Inst.trim_char_arry));
                int bet_amount = int.Parse(data.GetField("crash_config").GetField("bet_amount").ToString().Trim(Config.Inst.trim_char_arry));
                string mode = data.GetField("crash_config").GetField("mode").ToString().Trim(Config.Inst.trim_char_arry);

                CrashController.Instance.SetBetAmountFromServer(bet_amount);
                if (auto_remove && mode.Equals("auto"))
                {
                    Debug.Log("Auto mode stopped by CRASH_CASH_OUT complete");
                    CrashController.Instance.SetUserInfoConfigFromServer(flee_condition, profit_on_stop, profit_win_amount, loss_on_stop, profit_loss_amount, auto_remove, mode, true);

                    CrashController.Instance.StopAutoModeWhenConditionCompleted();
                }
                /*if (mode.Equals("auto"))
                {                   
                    CrashController.Instance.SetUserInfoConfigFromServer(flee_condition, profit_on_stop, profit_win_amount, loss_on_stop, profit_loss_amount, auto_remove, mode,true);
                }*/
            }
        }
    }
    public void CRASH_AUTO_REMOVE(JSONObject data)
    {
        if (!string.IsNullOrEmpty(data.GetField("crash_config").ToString().Trim(Config.Inst.trim_char_arry)))
        {  
            float flee_condition = float.Parse(data.GetField("crash_config").GetField("flee_condition").ToString());
            int profit_on_stop = int.Parse(data.GetField("crash_config").GetField("profit_on_stop").ToString().Trim(Config.Inst.trim_char_arry));
            float profit_win_amount = float.Parse(data.GetField("crash_config").GetField("profit_win_amount").ToString().Trim(Config.Inst.trim_char_arry));
            int loss_on_stop = int.Parse(data.GetField("crash_config").GetField("loss_on_stop").ToString().Trim(Config.Inst.trim_char_arry));
            float profit_loss_amount = float.Parse(data.GetField("crash_config").GetField("profit_loss_amount").ToString().Trim(Config.Inst.trim_char_arry));
            bool auto_remove = bool.Parse(data.GetField("crash_config").GetField("auto_remove").ToString().Trim(Config.Inst.trim_char_arry));
            int bet_amount = int.Parse(data.GetField("crash_config").GetField("bet_amount").ToString().Trim(Config.Inst.trim_char_arry));
            string mode = data.GetField("crash_config").GetField("mode").ToString().Trim(Config.Inst.trim_char_arry);

            CrashController.Instance.SetBetAmountFromServer(bet_amount);
            if (auto_remove && mode.Equals("auto"))
            {
                Debug.Log("Auto mode stopped by CRASH_AUTO_REMOVE");
                CrashController.Instance.SetUserInfoConfigFromServer(flee_condition, profit_on_stop, profit_win_amount, loss_on_stop, profit_loss_amount, auto_remove, mode, true);

                CrashController.Instance.StopAutoModeWhenConditionCompleted();
            }
            /*if (mode.Equals("auto"))
            {
                CrashController.Instance.SetUserInfoConfigFromServer(flee_condition, profit_on_stop, profit_win_amount, loss_on_stop, profit_loss_amount, auto_remove, mode, true);
            }*/
        }        
    }
}
