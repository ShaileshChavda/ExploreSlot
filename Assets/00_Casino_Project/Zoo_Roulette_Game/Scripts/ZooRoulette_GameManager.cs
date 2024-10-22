namespace ZooRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;
    using DG.Tweening;
    using System.Diagnostics;
    using System.Collections.Specialized;
    using UnityEngine.EventSystems;
    using System;
    using System.Linq;

    public class ZooRoulette_GameManager : MonoBehaviour
    {
        public static ZooRoulette_GameManager instance;

        public ZooRoulette_UIManager _UIManager;
        public ZooRoulette_HistoryManager _historyManager;
        public ZooRouletteSaveManager saveManager;



        [Header("---- Gameplay Settings ----")]
        public string GameID;
        public bool isNewRound = false;       
        public int resultSymbolIndex; // We will get Result id of car symbol at here
        public GameObject cloneCoinPrefab;
        public GameObject goMyUserChal, goOtherUserChal;

        public List<ZooRoulette_Symbol> SpiningSymbolList;
        private List<ZooRoulette_Symbol> tempSymbolList = new List<ZooRoulette_Symbol>();
        [SerializeField] Transform Chaal_Anim, My_User_ChalAnim;      


        [Header("---- Spin Animation Settings ----")]      
        public float animationSpeed;
        public float symbolActiveSpeed;
        public int rotationCount;
        private int stopAnimAtSymbol;
        public AnimationCurve speedCurve;
        public List<AnimationCurve> curves;
        public float fltMoveDuration = 1f;
        public float fltMoveResetDuration = 1f;
        public float fltDefaultBetPosY;
        public float fltAnimetdBetPosY;
        public Ease easeMove;
        public float fltBetPlacePosX;
        public float fltBetPlacePosY;

        public ZooRoulette_Symbol winRoulette;
        public ZooRoulette_Symbol winRouletteBeatBirds;
        public ZooRoulette_Symbol winItem;

        GameObject goWinItemParticle;
        public int _userWinChip = 0;
        public bool isUserWin = false;           

        [Header("***** BET UI *****")]
        public TimerStatus _timerStatus;
        public ZooRoulette_BetManager _selectedBetItem;
        [SerializeField] private List<ZooRoulette_BetManager> betAmountList = new List<ZooRoulette_BetManager>();
        public List<ZooRoulette_Symbol> PlaceBetButtonList;

        [SerializeField] bool InfoSetup;


        void Awake()
        {
            instance = this;
            isLossClip = false;
            SocketHandler.Inst.SendData(SocketEventManager.Inst.ZOO_GAME_INFO());
        }


        //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE --------------------------------------

        void Start()
        {          
            CurrentBetAnimetd(betAmountList[0]);
        }

        //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE ------------------------------------
        public void JOIN_OLD_GAME(JSONObject data)
        {
            UnityEngine.Debug.Log("REJOIN DATA: " + GS.Inst.Rejoin);
            ZooRoulette_PlayerManager.Inst.SET_PLAYER_DATA(data);
            //UnityEngine.Debug.Log("CAR INFO: "+data);

            for (int i = 0; i < saveManager._userBetDataClass.betPlaceIndexLst.Count; i++)
            {
                for (int j = 0; j < PlaceBetButtonList.Count; j++)
                {
                    if (saveManager._userBetDataClass.betPlaceIndexLst[i] ==
                         PlaceBetButtonList[j].id)
                    {
                        PlaceBetButtonList[j].isUserBet = true;
                        break;
                    }
                }
            }

            _timerStatus = TimerStatus.START;
            InfoSetup = false;
            string GameState = data.GetField("game_state").ToString().Trim(Config.Inst.trim_char_arry);
            GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);          

            _historyManager.SET_HISTORY(data);

            _UIManager.txtGameID.text = GameID;

            _UIManager.txtTotalBet.text = data.GetField("disp_user_total_bet").ToString().Trim(Config.Inst.trim_char_arry);

            UnityEngine.Debug.Log("Old Game join Timer: " + data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));           

            float time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
            GS.Inst.Rejoin = false;
            switch (GameState)
            {
                case "game_timer_start":
                    UnityEngine.Debug.Log("1-Timer: " + time);
                    _timerStatus = TimerStatus.BETTING;
                    ZooRouletteTimer.Inst.Txt_Timer_Status.text = "Betting";
                    isLossClip = false;
                    isNewRound = true;
                    _UIManager.CheckRebetStatus();
                    START_MAIN_TIMER(data, "M", 0);
                    UnityEngine.Debug.Log("game_timer_start: " + data);
                    UserRejoinMethod(data, false);
                    break;
                case "start_spin":
                    UnityEngine.Debug.Log("2-Timer: " + time);
                    _timerStatus = TimerStatus.DRAWING;
                    ZooRouletteTimer.Inst.Txt_Timer_Status.text = "Drawing";
                   // START_MAIN_TIMER(data, "M",1);
                    if (time > 17)
                    {
                        START_SPIN(data,true);
                    }
                    else if (time <= 17 && time > 1)
                    {
                        START_MAIN_TIMER(data, "M", 0);
                        //ZooRouletteTimer.Inst.StartTimerAnim(time, time, true, "0");                       
                    }
                    UserRejoinMethod(data, false);
                    _UIManager.Wait_Next_Round_POP(true);
                    break;
                case "winner_declare":
                    UnityEngine.Debug.Log("3-Timer: " + time);
                    _timerStatus = TimerStatus.DRAWING;
                    ZooRouletteTimer.Inst.Txt_Timer_Status.text = "DRAWING";
                    START_MAIN_TIMER(data, "M", 0);
                    UserRejoinMethod(data, true);
                    saveManager.ResetSaveData();
                    _UIManager.Wait_Next_Round_POP(true);
                    break;
                case "winner_declare_done":  // this event not working 
                    UnityEngine.Debug.Log("4-Timer: " + time);
                    _timerStatus = TimerStatus.IDLE;
                    ZooRouletteTimer.Inst.Txt_Timer_Status.text = "Idle";
                    UserRejoinMethod(data, true);
                    saveManager.ResetSaveData();
                    _UIManager.Wait_Next_Round_POP(true);
                    break;
                case "finish_state":
                    UnityEngine.Debug.Log("5-Timer: " + time);
                    _timerStatus = TimerStatus.IDLE;
                    ZooRouletteTimer.Inst.Txt_Timer_Status.text = "Idle";
                    saveManager.ResetSaveData();
                    _UIManager.Wait_Next_Round_POP(true);
                    break;
            }
          
        }

        public void UserRejoinMethod(JSONObject data, bool winner)
        {
            MyUserSideChipTotal(data);
            AllBetSideCheck();

            if (winner)
            {
                int win = SplitStringToInt((data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry)));
                WinInfo(win);
            }

            int betCard = data.GetField("dis_total_bet_on_cards").Count;
            string strBet = data.GetField("dis_total_bet_on_cards").ToString();
            string[] disBetList = strBet.Split(',').ToArray();

            for (int i = 0; i < betCard; i++)
            {
                string[] split_XCard = disBetList[i].Split(':');
                int.TryParse(ZooRoulette_EventManager.RemoveSpecialCharacters(split_XCard[0]), out int id);
                var betItem = PlaceBetButtonList.Find(number => number.id == id);
                int amt = 0;
                int.TryParse(ZooRoulette_EventManager.RemoveSpecialCharacters(split_XCard[1]), out amt);
                betItem._totalSymbolChip = amt;
                betItem.txtTotalChip.text = amt.ToString();
            }
        }

        //------------ User Bet Send -----------------
        public void USER_SEND_BET(string Bet)
        {
            SocketHandler.Inst.SendData(SocketEventManager.Inst.ZOO_ROULETTE_PLACE_BET(Bet));
        }
        //------------ User Bet Send -----------------

        //------------ User Bet Send -----------------
        public void RE_USER_SEND_BET(string Bet, int betVal)
        {
            SocketHandler.Inst.SendData(SocketEventManager.Inst.ZOO_ROULETTE_PLACE_RE_BET(Bet, betVal));
        }
        //------------ User Bet Send -----------------

        //------------------------------ SPIN -----------------------------------------------------------
        public void INIT_GAME(JSONObject data)
        {
            UnityEngine.Debug.Log("INIT GAME: " + data);

            ResetGame();
            ZooRouletteTimer.Inst.Txt_Timer_Status.text = "Idle";
            _timerStatus = TimerStatus.IDLE;
            START_MAIN_TIMER(data, "N", 2);
            _UIManager.Wait_Next_Round_POP(true);
            ZooRoulette_PlayerManager.Inst.REFRESH_PLAYER_DATA(data);
        }
        //------------------------------ SPIN -----------------------------------------------------------


        public void PLACING_BET(JSONObject data)
        {
            UnityEngine.Debug.Log("START BETTING");

            ZooRoulette_EventManager.PFB_COIN_KILL();
            _UIManager.Wait_Next_Round_POP(false);
            _UIManager.NEW_ROUND_START_STOP(true);
            GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
            _UIManager.txtGameID.text = GameID;
            _UIManager.txtTotalBet.text = "0";
            _timerStatus = TimerStatus.BETTING;
            ZooRouletteTimer.Inst.Txt_Timer_Status.text = "Betting";
            isNewRound = true;
            _UIManager.CheckRebetStatus();
            START_MAIN_TIMER(data, "M", 0);
        }

        //------------------------------ SPIN -----------------------------------------------------------
        public void START_SPIN(JSONObject data,bool rejoin=false)
        {
            UnityEngine.Debug.Log("START SPIN TO WIN CARD: " + data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry));

            _timerStatus = TimerStatus.DRAWING;
            ZooRouletteTimer.Inst.Txt_Timer_Status.text = "Drawing";
            float time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
            StartNewGame(data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry),rejoin, time);
            START_MAIN_TIMER(data, "M", 0);
        }       
        //------------------------------ SPIN -----------------------------------------------------------

        //------------------------------ Timer Start -----------------------------------------------------------
        public void START_MAIN_TIMER(JSONObject data, string TimerType, int startIndex)
        {
            float time = 0;

            if (TimerType.Equals("N")) // N means New Round start
            {
                if (startIndex == 2)
                {
                    UnityEngine.Debug.Log("INIT GAME IDLE TIMER: " + data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
                    time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
                    ZooRouletteTimer.Inst.StartTimerAnim(time, time, false, TimerType);
                }
            }
            else if (TimerType.Equals("M")) // M means old game ne rejoin karvi
            {
                time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));                
                // added by naresh
                if (time < 20) // means vachhe thi chalu gamee join karel chhe
                    ZooRouletteTimer.Inst.StartTimerAnim(time, time, true, TimerType);
                else
                    ZooRouletteTimer.Inst.StartTimerAnim(time, time, false, TimerType);
            }          
            InfoSetup = true;
        }
        //------------------------------ Timer Start -----------------------------------------------------------

        //------------------------------ Seat User ---------------------------------------------------------
        public void SEAT_USER(JSONObject data)
        {
            string id = data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
            string online_users_count = data.GetField("online_users_count").ToString().Trim(Config.Inst.trim_char_arry);
            ZooRoulette_Online_User_Manager.Inst.txtOnlineUser.text = "("+online_users_count+")";

            bool is_Bot = bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry));

            if (id != GS.Inst._userData.Id)
            {
                bool fl = false;
                for (int i = 0; i < ZooRoulette_PlayerManager.Inst.Player_Bot_List.Count; i++)
                {
                    if (ZooRoulette_PlayerManager.Inst.Player_Bot_List[i]._Status == ZooRoulette_Player.Status.Null && !is_Bot && !fl)
                    {
                        fl = true;
                        ZooRoulette_PlayerManager.Inst.Player_Bot_List[i].SEAT(data);
                    }
                }
            }
        }
        //------------------------------ Seat User ----------------------------------------------------------- 

        //------------------------------ BET INFO -----------------------------------------------------------
        public void BET_INFO(JSONObject data)
        {
            if (InfoSetup)
                ZooRoulette_PlayerManager.Inst.PLAYER_CHAAL(data);
        }
        //------------------------------ BET INFO ----------------------------------------------------------- 

        //------------ Winning history set -----------------
        public void WINNER_DECLARE(JSONObject data)
        {
            UnityEngine.Debug.Log("WINNER_DECLARE: " + data);

            saveManager.ResetSaveData();
            string win_card = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
            resultSymbolIndex = SplitStringToInt(win_card);
           
            winRoulette = PlaceBetButtonList.Find(item => item.id == resultSymbolIndex);

            winRoulette.animWin.gameObject.SetActive(true);
            winRoulette.animWin.Rebind();
            winRoulette.animWin.Play("WinRouletteEffect");
            Invoke(nameof(WaitCallOff), 5f);

            if (resultSymbolIndex >= 1 && resultSymbolIndex <= 4 && (int.Parse(winRoulette.txtTotalChip.text) > 0))
            {
                winRouletteBeatBirds = PlaceBetButtonList.Find(item => item.id == 9); // here, 9 means beast x2 upar win animation karvu
                winRouletteBeatBirds.animWin.gameObject.SetActive(true);
                winRouletteBeatBirds.animWin.Rebind();
                winRouletteBeatBirds.animWin.Play("WinRouletteEffect");
                Invoke(nameof(WaitCallOffBeastBirds), 5f);
            }
            else if (resultSymbolIndex >= 5 && resultSymbolIndex <= 8 && (int.Parse(winRoulette.txtTotalChip.text) > 0))
            {
                winRouletteBeatBirds = PlaceBetButtonList.Find(item => item.id == 10); // here, 10 means birds x2 upar win animation karvu
                winRouletteBeatBirds.animWin.gameObject.SetActive(true);
                winRouletteBeatBirds.animWin.Rebind();
                winRouletteBeatBirds.animWin.Play("WinRouletteEffect");
                Invoke(nameof(WaitCallOffBeastBirds), 5f);
            }
          

            int totalCounter = data.GetField("last_win_cards").Count;
            if (totalCounter > 0)
            {
                int currentCard = totalCounter - 1;
                int getIndex = SplitStringToInt(data.GetField("last_win_cards")[currentCard].ToString().Trim(Config.Inst.trim_char_arry));
                _historyManager.SET_HISTORY_CARD_DATA((getIndex - 1), true);
            }
            _UIManager.WinAnimRaysEffect(resultSymbolIndex);
        }

        public void WaitCallOff()
        {
            winRoulette.animWin.gameObject.SetActive(false);
        }
        public void WaitCallOffBeastBirds()
        {
            winRouletteBeatBirds.animWin.gameObject.SetActive(false);
        }
        double OnlineUserValue;
        public List<double> onlinePlayerValues; 
        //------------ Win Coin Move to winner user -----------------
        public void WINNER_TO_MOVE_CHIP(JSONObject data)
        {
            print("WINNER_TO_MOVE_CHIP:  " + data);

            isLossClip = true;
            OnlineUserValue = 0;
            onlinePlayerValues.Clear();

            if (TargetList != null)
            {          
                TargetList.Clear(); 
            }
          
            var usersBets = data.GetField("users_bets");
            var userBetsKeys = usersBets.keys;         
            int userBetsCount = userBetsKeys.Count;
           
            for (int i = 0; i < userBetsCount; i++)
            {               
                string uid = userBetsKeys[i].ToString().Trim(Config.Inst.trim_char_arry);               
                double value = double.Parse(usersBets.GetField(uid).ToString().Trim(Config.Inst.trim_char_arry));
               

                if (GS.Inst._userData.Id.Equals(uid))
                {
                    ZooRoulette_PlayerManager.Inst.WinOrLose_Chips = value;
                    ZooRoulette_PlayerManager.Inst.Played_Chips = true;

                    if (value != 0)
                    {
                        _UIManager.TextParticleAnimation(value);
                    }
                }
                else
                {
                    onlinePlayerValues.Add(value);
                    print("OnlineUserValue:  " + value);
                    OnlineUserValue += value;                   
                    print("GS.Inst._userData.Id Not available in user list " + GS.Inst._userData.Id);
                }
            }
            if (OnlineUserValue != 0)
            {
                _UIManager.OnliePlayerTextParticleAnimation(OnlineUserValue);
            }
           
            foreach (var roulette in PlaceBetButtonList)
            {
                int totalChips = roulette._combineChipList.Count;
                int remainChipsIndex = totalChips;

                if (resultSymbolIndex == 14) // 14 means take all vari condition, aa condition ma coins ne destroy kari deva
                {
                    AnimateChips(roulette, totalChips, remainChipsIndex, false, true);
                }
                else
                {
                    if (roulette.id == winRoulette.id)
                    {
                        UnityEngine.Debug.Log("USER WIN");
                        AnimateChips(roulette, totalChips, remainChipsIndex, true);
                    }
                    else
                    {
                        AnimateChips(roulette, totalChips, remainChipsIndex, false);
                    }
                }
            }            
        }
        private int nonUserChipAnimationLimit = 5;
        private void AnimateChips(ZooRoulette_Symbol roulette, int totalChips, int remainChipsIndex, bool isWin, bool isDestroyAll = false)
        {
            int nonUserChipAnimations = 0;
            if (isDestroyAll)
            {
                for (int j = 0; j < remainChipsIndex; j++)
                {
                    ZooRoulette_ChipManager chip = roulette._combineChipList[j];
                    Destroy(chip.gameObject);
                }
            }
            else
            {
                for (int j = 0; j < remainChipsIndex; j++)
                {
                    ZooRoulette_ChipManager chip = roulette._combineChipList[j];
                    chip.imgChip.DOFade(1, 0);
                    if (isWin)
                    {
                        if (chip.userChipStatus == UserStatus.MYUSER)
                            chip.WIN_MOVE_ANIM();
                        else
                        {
                            if (nonUserChipAnimations < nonUserChipAnimationLimit)
                            {
                                chip.LOSS_MOVE_ANIM();
                                nonUserChipAnimations++;
                            }
                            else
                            {
                                Destroy(chip.gameObject);
                            }
                        }
                    }
                    else
                    {
                        if (nonUserChipAnimations < nonUserChipAnimationLimit)
                        {
                            chip.LOSS_MOVE_ANIM();
                            nonUserChipAnimations++;
                        }
                        else
                        {
                            Destroy(chip.gameObject);
                        }
                    }
                }
            }
        }

        public float minLoss, maxLoss;

        //------------------------------ Leave User ---------------------------------------------------------
        public void LEAVE_USER(JSONObject data)
        {
            ZooRoulette_EventManager.USER_LEAVE(data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry));
        }
        //------------------------------ Leave User ----------------------------------------------------------- 

        public void ChooseOtherChipMethod()
        {
            int rnIndex = UnityEngine.Random.Range(0, PlaceBetButtonList.Count);
            ZooRoulette_Symbol botRoulette = PlaceBetButtonList[rnIndex];
            int betVal = betAmountList[UnityEngine.Random.Range(0, betAmountList.Count)].intValue;
            botRoulette.OnClickOtherItem(betVal);
        }

        public void StartNewGame(string id,bool rejoin=false,float timer=0) // we recieved like "1|10"
        {          
            isLossClip = false;
            _UIManager.rebetTarget = 0;
            StopSideChek();
            _UIManager.btnRebet.interactable = false;
            resultSymbolIndex = SplitStringToInt(id); // Get Win Card number

            SetConfigDataForAnimation(rejoin, timer);
        }
        private void SetConfigDataForAnimation(bool rejoin = false, float timer = 0)
        {
            if (rejoin)   // Spining animation karva mate aeni speed manage karel chhe, jyare rejoin kariye
            {
                animationSpeed = (timer - 10); // 18 - 10 = 8 second remaining chhe

                if (animationSpeed < 6)
                    rotationCount = 2; // default 5 rakhel chhee
            }
            else
            {
                animationSpeed = 13;
                rotationCount = 4;
            }

            stopAnimAtSymbol = FindStopIndexOfAnimation();
            SelectCurve();

            tempSymbolList.Clear();

            for (int i = 0; i < rotationCount; i++)
            {
                AddTempListData();
            }

            AddSelectedIndex();
            StartCoroutine(PlayLoopAnimation());
        }
        private int FindStopIndexOfAnimation()
        {
            int rng = Random.Range(0, SpiningSymbolList.Count);
            if (resultSymbolIndex == SpiningSymbolList[rng].GetComponent<ZooRoulette_Symbol>().id)
            {
                return rng;
            }
            else
            {
                return FindStopIndexOfAnimation();
            }
        }

        private void SelectCurve()
        {
            speedCurve = curves[0];
        }

        private void AddTempListData()
        {
            for (int i = 0; i < SpiningSymbolList.Count; i++)
            {
                tempSymbolList.Add(SpiningSymbolList[i]);
            }
        }

        private void AddSelectedIndex()
        {
            for (int i = 0; i < SpiningSymbolList.Count; i++)
            {
                if (i == stopAnimAtSymbol + 1)
                {
                    break;
                }
                else
                {
                    tempSymbolList.Add(SpiningSymbolList[i]);
                }
            }
        }

        public float fltSoundDuration;
        public float loopCloseTime = 1f;
        public Vector3 scaleVector;
        public Ease setEase;
        public float fltDuration;

        public Vector3 winScaleVector;
        public Ease winSetEase;
        public float fltWinDuration;
        float pitchUp;
        bool IspitchUp;
        private IEnumerator PlayLoopAnimation()
        {
            float i = 0;
            int newindex = 0;
            int currentIndex = 0;

            while (i < loopCloseTime)
            {
                i += Time.deltaTime / animationSpeed;
                newindex = (int)Mathf.Lerp(0, tempSymbolList.Count - 1, speedCurve.Evaluate(i));             

                if (newindex == tempSymbolList.Count - 1)
                {
                }
                else
                {
                    if (newindex != currentIndex)
                    {
                        currentIndex = newindex;

                        float speedValue = speedCurve.Evaluate(i);

                       // print("speedValue: " + speedValue);
                        float pitch = Mathf.Lerp(1f, 1.3f, speedValue); // Adjust these values as needed for desired pitch range
                        //float pitch = 1+speedValue; // Adjust these values as needed for desired pitch range


                        if (pitch >= 1.29f)
                        {
                            pitchUp -=0.02f;
                           // print("pitchUp: " + pitchUp);
                            Zoo_Roulette_Sound.Inst.PlayReel(pitchUp);
                        }
                        else
                        {
                            pitchUp = pitch;
                           // print("pitch: " + pitch);                           
                            Zoo_Roulette_Sound.Inst.PlayReel(pitch);
                        }
                      
                        // UnityEngine.Debug.Log("Next Symbol Index: " + currentIndex);
                    }
                    tempSymbolList[newindex].AnimatedItem(scaleVector, fltDuration, setEase);
                }
                yield return null;
            }

            winItem = tempSymbolList[newindex];
            Zoo_Roulette_Sound.Inst.StopReel();
            winItem.WinItemAnimated(winScaleVector, fltWinDuration, winSetEase);
            goWinItemParticle = Instantiate(_UIManager._goItemWinParticle, Vector3.zero, Quaternion.identity);
            goWinItemParticle.transform.SetParent(winItem.transform);
            goWinItemParticle.transform.localPosition = Vector3.zero;
            goWinItemParticle.transform.localScale = Vector3.one;
        }

        public void WinInfo(int winIndex)
        {
            UnityEngine.Debug.Log("WIN:" + winIndex);
            var occurrences = tempSymbolList.Where(number => number.id == winIndex).ToList();
            winItem = occurrences[Random.Range(0, occurrences.Count)];
            Zoo_Roulette_Sound.Inst.StopReel();
            winItem.WinItemAnimated(winScaleVector, fltWinDuration, winSetEase);
            goWinItemParticle = Instantiate(_UIManager._goItemWinParticle, Vector3.zero, Quaternion.identity);
            goWinItemParticle.transform.SetParent(winItem.transform);
            goWinItemParticle.transform.localPosition = Vector3.zero;
            goWinItemParticle.transform.localScale = Vector3.one;
        }


        public float hitTime = 0f;
        public void ResetGame()
        {
            UnityEngine.Debug.Log("RESET GAME START");
            _UIManager.raysWinObject.SetActive(false);
            _UIManager.isWaitCond = false;

            if (goWinItemParticle != null)
            {
                Destroy(goWinItemParticle);
            }

            if(winItem !=null)
            {
                winItem.RevertAnimatedItem(0.1f, Ease.Linear);
            }

            for (int i = 0; i < PlaceBetButtonList.Count; i++)
            {
                PlaceBetButtonList[i].ResetItemValue();
            }
                      
            _timerStatus = TimerStatus.START;

            _userWinChip = 0;
            _UIManager.totalCoins = 0;
            _UIManager.txtTotalBet.text = "0";
            ZooRoulette_PlayerManager.Inst.Played_Chips = false;
            ZooRoulette_PlayerManager.Inst.WinOrLose_Chips = 0f;
            _UIManager.isRebet = false;
            saveManager.ResetSaveData();
            UnityEngine.Debug.Log("RESET GAME END");
        }
        public AudioSource Coin_Audio_Source;

        public void MY_User_Chaal_Animation(string Chaa_Amount, string side)
        {
            _UIManager.isWaitCond = true;
            //UnityEngine.Debug.Log("MY USER CHAl: "+ side + "|"+ Chaa_Amount);
            ZooRoulette_Symbol sy = null;
            for (int i = 0; i < PlaceBetButtonList.Count; i++)
            {
                string st = PlaceBetButtonList[i].name;
                if (st.Contains(side))
                {
                    sy = PlaceBetButtonList[i];
                    break;
                }
            }

            // UnityEngine.Debug.Log("CURRENT BET ITEM: " + sy.name);
            Vector3 target;
            if (sy.id.Equals(9) || sy.id.Equals(10))
            {
                target = getRandomPoint(sy.goMoveTarget, 2);
            }
            else
            {
                target = getRandomPoint(sy.goMoveTarget, 0);
            }
            GameObject _Coin = Instantiate(cloneCoinPrefab);
            ZooRoulette_ChipManager _chipManager = _Coin.transform.GetComponent<ZooRoulette_ChipManager>();
            _chipManager.userChipStatus = UserStatus.MYUSER;
            if (PlayerPrefs.GetInt("sound").Equals(1))
                Coin_Audio_Source.PlayOneShot(Zoo_Roulette_Sound.Inst.SFX[3]);
            _chipManager.SET_COIN(Chaa_Amount);
            _Coin.transform.SetParent(sy.gameObject.transform, false);
            _Coin.name = "MY_USER_COIN_" + Chaa_Amount;
            _Coin.transform.position = goMyUserChal.transform.position;
            _chipManager.Move_Anim(target, 0.3f);
            sy.UserChipCalculate(true, _chipManager);

            if (!saveManager._userBetDataClass.betPlaceIndexLst.Contains(sy.id))
            {
                saveManager._userBetDataClass.betPlaceIndexLst.Add(sy.id);
            }

            _UIManager.rebetTarget++;
            UnityEngine.Debug.Log("NEW ROUND CURRENT BET ITEM: " + isNewRound + "|" + _UIManager.isRebet + ">>>>" + _UIManager.rebetTarget);

            if (!_UIManager.isRebet && !isNewRound)
                saveManager.MyAlreadyUserBetStatus(sy.id, _chipManager.val);

            saveManager.CheckAlreadyUserBetStatus(sy.id, _chipManager.val, UserStatus.MYUSER);
            saveManager.CheckMaxBetReached();

            if (saveManager.addUserDataLst.Count == _UIManager.rebetTarget)
            {
                _UIManager.WaitToCallResetRebet();
                StopSideChek();
                _UIManager.isWaitCond = false;
            }

            My_User_ChalAnim.DOScale(MyAnimScale, fltChalAnimTimer).SetEase(Ease.Linear).OnComplete(() =>
            ResetTweeCall(0)
            ).SetAutoKill();
        }

        public void Real_User_Chaal_Animation(string Chaa_Amount, string side)
        {
            // UnityEngine.Debug.Log("OTHER USER CHAl: " + side + "|" + Chaa_Amount);

            ZooRoulette_Symbol sy = null;
            for (int i = 0; i < PlaceBetButtonList.Count; i++)
            {
                string st = PlaceBetButtonList[i].name;
                if (st.Contains(side))
                {
                    sy = PlaceBetButtonList[i];
                    break;
                }
            }

            //UnityEngine.Debug.Log("CURRENT BET ITEM: " + sy.name);
            Vector3 target;
            if (sy.id.Equals(9) || sy.id.Equals(10))
            {
                target = getRandomPoint(sy.goMoveTarget, 2);
            }
            else
            {
                target = getRandomPoint(sy.goMoveTarget, 0);
            }
            GameObject _Coin = Instantiate(cloneCoinPrefab);
            ZooRoulette_ChipManager _chipManager = _Coin.transform.GetComponent<ZooRoulette_ChipManager>();
            _chipManager.userChipStatus = UserStatus.OTHERUSER;
            if (PlayerPrefs.GetInt("sound").Equals(1))
                Coin_Audio_Source.PlayOneShot(Zoo_Roulette_Sound.Inst.SFX[3]);
            _chipManager.SET_COIN(Chaa_Amount);
            _Coin.transform.SetParent(sy.gameObject.transform, false);
            _Coin.name = "OTHER_USER_COIN_" + Chaa_Amount;
            _Coin.transform.position = goOtherUserChal.transform.position;
            _chipManager.Move_Anim(target, 0.6f);
            sy.OtherUserChipCalculate(true, _chipManager);
            saveManager.CheckAlreadyUserBetStatus(sy.id, _chipManager.val, UserStatus.OTHERUSER);
            Chaal_Anim.DOScale(RealUserAnimScale, fltChalAnimTimer).SetEase(Ease.Linear).OnComplete(() =>
            ResetTweeCall(1)
            ).SetAutoKill();
        }

        public void Save_User_Bet_Animation(string Chaa_Amount, GameObject TargetOBJ, GameObject Coin_MoveOBJ, int type,
            UserStatus status)
        {                     
            Vector3 target = getRandomPoint(Coin_MoveOBJ, type);
            GameObject _Coin = Instantiate(cloneCoinPrefab) as GameObject;
            ZooRoulette_ChipManager _chipManager = _Coin.transform.GetComponent<ZooRoulette_ChipManager>();
            _chipManager.SET_COIN(Chaa_Amount);
            _Coin.transform.SetParent(Coin_MoveOBJ.transform, false);
            _chipManager.Place_Anim(target);
            _chipManager.userChipStatus = status;
            ZooRoulette_Symbol scr = TargetOBJ.GetComponent<ZooRoulette_Symbol>();
            scr._combineChipList.Add(_chipManager);
        }

        public void MyUserSideChipTotal(JSONObject data)
        {
            if (data.HasField("card_details"))
            {
                UnityEngine.Debug.Log("card_details Key exists.");

                int cardDetailCount = data.GetField("card_details").Count;
                UnityEngine.Debug.Log("card_details counter: " + cardDetailCount);

                if(cardDetailCount > 0)
                {
                    string ary = data.GetField("card_details").ToString();
                    string[] totalCardDetails = ary.Split(',').ToArray();
                    
                    for (int i = 0; i < cardDetailCount; i++)
                    {
                        string[] split_XCard = totalCardDetails[i].Split(':');
                        int.TryParse(ZooRoulette_EventManager.RemoveSpecialCharacters(split_XCard[0]), out int id);
                    
                        int amt = 0;
                        int.TryParse(ZooRoulette_EventManager.RemoveSpecialCharacters(split_XCard[1]), out amt);

                        for (int j = 0; j < PlaceBetButtonList.Count; j++)
                        {
                            ZooRoulette_Symbol sc = PlaceBetButtonList[j];
                                    if (id == sc.id)
                                    {
                                        sc._totalSymbolMyChip += amt;
                                        sc.txtTotalMyChip.text = sc._totalSymbolMyChip.ToString();
                                        break;
                                    }
                            }
                        }
                }
            }
            else
            {
                UnityEngine.Debug.Log("card_details Key does not exist.");
            }
        }

        public void AllBetSideCheck()
        {
            for (int i = 0; i < saveManager._userBetDataClass.betDataClassLst.Count; i++)
            {
                BetDataClass bdc = saveManager._userBetDataClass.betDataClassLst[i];

                for (int j = 0; j < PlaceBetButtonList.Count; j++)
                {
                    ZooRoulette_Symbol sc = PlaceBetButtonList[j];

                    if (bdc.side == sc.id)
                    {
                        for (int k = 0; k < bdc.userStatusBetClassLst.Count; k++)
                        {
                            UserStatusBetClass usbc = bdc.userStatusBetClassLst[k];
                            for (int x = 0; x < usbc.betValueLst.Count; x++)
                            {
                                BetValue bv = usbc.betValueLst[x];
                                int betVal = bv.betValue;
                                int totalBetCounter = bv.totalBetValue / betVal;

                                for (int y = 0; y < totalBetCounter; y++)
                                {
                                    if (sc.id.Equals(9) || sc.id.Equals(10))
                                    {
                                        Save_User_Bet_Animation(betVal.ToString(), sc.gameObject, sc.goMoveTarget, 2,
                                       usbc.userStatus);                                       
                                    }
                                    else
                                    {
                                        Save_User_Bet_Animation(betVal.ToString(), sc.gameObject, sc.goMoveTarget, 0,
                                        usbc.userStatus);
                                    }
                                   
                                }
                            }
                        }
                    }
                }
            }
        }
        public static int SplitStringToInt(string str)
        {
            string[] st = str.Split('|');           
            int.TryParse(st[0], out int result);
            return result;
        }
        public void StopSideChek()
        {
            StopCoroutine(MyAllBetSideCheck());
        }
        public IEnumerator MyAllBetSideCheck()
        {
            int totalRebet = saveManager.addUserDataLst.Count;

            for (int x = 0; x < totalRebet; x++)
            {
                string rebetData = saveManager.addUserDataLst[x];
                _UIManager.isWaitCond = false;
                string[] st = rebetData.Split(':');
                string betSide = st[0];
                int.TryParse(st[1], out int result);
                RE_USER_SEND_BET(betSide, result);
                yield return new WaitUntil(() => _UIManager.isWaitCond);
            }
        }

        public void ResetTweeCall(int a)
        {
            if (a == 1)
                Chaal_Anim.DOScale(RealIdleUserAnimScale, fltChalAnimTimer).SetEase(Ease.Linear);
            else
                My_User_ChalAnim.DOScale(MyIdleAnimScale, fltChalAnimTimer).SetEase(Ease.Linear);
        }

        public Vector2 MyIdleAnimScale;
        public Vector2 MyAnimScale;

        public Vector2 RealIdleUserAnimScale;
        public Vector2 RealUserAnimScale;
        public float fltChalAnimTimer = 0.25f;
        public int loopType = 1;
        public LoopType animType;

        public void Win_Chip_Animation(GameObject chipTarget, GameObject moveTarget, int type)
        {
            UnityEngine.Debug.Log("TARGET: " + chipTarget.name + "{}" + moveTarget.name);
            Vector3 target = getRandomPoint(chipTarget, type);
            ZooRoulette_ChipManager _chipManager = moveTarget.transform.GetComponent<ZooRoulette_ChipManager>();
            _chipManager.transform.SetParent(chipTarget.transform, false);
            _chipManager.Move_Anim(target, 0.6f);
        }

        public void Idle_Chip_Animation(GameObject chipTarget, GameObject moveTarget, int type)
        {
            UnityEngine.Debug.Log("TARGET: " + chipTarget.name + "{}" + moveTarget.name);
            ZooRoulette_ChipManager _chipManager = moveTarget.transform.GetComponent<ZooRoulette_ChipManager>();
            _chipManager.transform.SetParent(chipTarget.transform, false);
            _chipManager.Move_Anim(chipTarget.transform.position, 0.6f);
        }

        private List<GameObject> TargetList = new();


        void Kill_Win_Coins()
        {
            _UIManager.txtTotalBet.text = "0";
            ZooRoulette_EventManager.PFB_COIN_KILL();
            ZooRoulette_PlayerManager.Inst.Played_Chips = false;
        }

        //------------ Win Coin Move to winner user -----------------

        public void RouletteItemMethod(int index, int betValue, GameObject target = null, GameObject moveTarget = null, UserStatus userStatus = UserStatus.MYUSER)
        {
            UnityEngine.Debug.Log("OnClickRouletteItem(): " + index);
        }



        //------------ Random Box Point Positions -----------------
        public Vector3 getRandomPoint(GameObject TypeObj, int type)
        {
            Vector3 pos = Vector3.zero;
            switch (type)
            {
                case 0:
                    pos = GetRandomPointInBox(TypeObj.transform, fltBetPlacePosX, fltBetPlacePosY/*, Vector3.up * 2*/);
                    break;
                case 1:
                    pos = GetRandomPointInBox(TypeObj.transform, 1.1f, 1.1f/*, Vector3.up * 2*/);
                    break;
                case 2:
                    pos = GetRandomPointInBox(TypeObj.transform, 0.6f, 0.13f/*, Vector3.up * 2*/);
                    break;
                default:
                    break;
            }
            return pos;
        }

        public Vector3 GetRandomPointInBox(Transform box, float hightBounds, float widthBounds/*, Vector3 offset = new Vector3()*/)
        {
            return box.position + new Vector3(Random.Range(hightBounds * -1, hightBounds), Random.Range(widthBounds * -1, widthBounds), 0f)/* + offset*/;
        }

        public Vector3 GetRandomPointInBox(Transform box, float hightBounds, float widthBounds, Vector3 offset = new Vector3())
        {
            return box.transform.position + new Vector3(Random.Range(hightBounds * -1, hightBounds), Random.Range(widthBounds * -1, widthBounds), 0f) + offset;
        }

        public void CurrentBetAnimetd(ZooRoulette_BetManager bm)
        {
            _selectedBetItem = bm;

            for (int i = 0; i < betAmountList.Count; i++)
            {
                if (_selectedBetItem != betAmountList[i])
                {
                    betAmountList[i].ResetAnimatedItem(fltDefaultBetPosY, fltMoveResetDuration, easeMove);
                }
                else
                {
                    betAmountList[i].AnimatedItem(fltAnimetdBetPosY, fltMoveDuration, easeMove);
                }
            }
        }

        public bool isLossClip = false;
    }

    [System.Serializable]
    public enum UserStatus
    {
        NONE,
        MYUSER,
        OTHERUSER
    }

    [System.Serializable]
    public enum TimerType
    {
        INIT_TIMER,
        BETTING_TIMER,
        DRAWING_TIMER
    }
}