namespace CarRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;
    using DG.Tweening;
    using System.Linq;

    public class CarRoulette_GameManager : MonoBehaviour
    {
        public static CarRoulette_GameManager instance;

        public CarRouletteSaveManager saveManager;

        public bool isNewRound = false;
        public UserStatus _userStatus;
        public CarRoulette_UIManager _carRoulette_UIManager;

        public int currSideID;
        public int currBetVal;


        public float loopCloseTime = 1f;
        public CarRoulette_HistoryManager _historyManager;

        [SerializeField] Transform Chaal_Anim, My_User_ChalAnim;
        public Vector3 scaleVector;
        public Ease setEase;
        public float fltDuration;

        public Vector3 winScaleVector;
        public Ease winSetEase;
        public float fltWinDuration;

        public List<CarRoulette_BetManager> _betList = new List<CarRoulette_BetManager>();

        [Header("---- Gameplay Settings ----")]

        public GameObject cloneCoinPrefab;
        public GameObject goMyUserChal, goOtherUserChal;

        public List<Symbol_CarRoulette> symbolList;
        public List<Symbol_CarRoulette> tempSymbolList = new List<Symbol_CarRoulette>();
        public int resultSymbolIndex; // We will get Result id of car symbol at here

        // Set Animation Variable that you want dynamic
        public float animationSpeed;
        public float symbolActiveSpeed;
        public int rotationCount;
        private int stopAnimAtSymbol;
        public AnimationCurve speedCurve;
        public List<AnimationCurve> curves;

        public Symbol_CarRoulette winRoulette;

        GameObject goWinItemParticle;
        public int _userWinChip = 0;
        public bool isUserWin = false;

        public Symbol_CarRoulette winItem;
        public List<Transform> trAllChipList;


        //public int Selected_Bet_Amount;

        [Header("***** BET UI *****")]
        public CarRoulette_BetManager _selectedBetItem;
        public float fltMoveDuration = 1f;
        public float fltMoveResetDuration = 1f;
        public float fltDefaultBetPosY;
        public float fltAnimetdBetPosY;
        public Ease easeMove;

        public float fltBetPlacePosX;
        public float fltBetPlacePosY;

        [SerializeField] bool InfoSetup;
        public string GameID;

        void Awake()
        {
            instance = this;
            isLossClip = false;
            SocketHandler.Inst.SendData(SocketEventManager.Inst.CAR_GAME_INFO());
            _historyManager = GameObject.Find("HistoryPanel").GetComponent<CarRoulette_HistoryManager>();
        }


        //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE --------------------------------------

        void Start()
        {
            _carRoulette_UIManager = FindObjectOfType<CarRoulette_UIManager>();
            CurrentBetAnimetd(_betList[0]);
        }

        //------------------------------ WHEN USER COMES IN TABLE JOIN OLD TABLE ------------------------------------
        public void JOIN_OLD_GAME(JSONObject data)
        {
            UnityEngine.Debug.Log("REJOIN DATA: " + GS.Inst.Rejoin);

            UnityEngine.Debug.Log("CAR_GAME_GAME_INFO: " + data);

            for (int i = 0; i < saveManager._userBetDataClass.betPlaceIndexLst.Count; i++)
            {
                for (int j = 0; j < _carRoulette_UIManager._chooseBetRouletteList.Count; j++)
                {
                    if (saveManager._userBetDataClass.betPlaceIndexLst[i] ==
                         _carRoulette_UIManager._chooseBetRouletteList[j].id)
                    {
                        _carRoulette_UIManager._chooseBetRouletteList[j].isUserBet = true;
                        break;
                    }
                }
            }

            _carRoulette_UIManager._timerStatus = TimerStatus.START;
            InfoSetup = false;
            string GameState = data.GetField("game_state").ToString().Trim(Config.Inst.trim_char_arry);
            GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
            string winCARD = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
            UnityEngine.Debug.Log("JOIN OLD GAME |GAME STATE|" + GameState + "|GAME ID|" + GameID + "|WIN CARD|" + winCARD);

            _historyManager.SET_HISTORY(data);

            _carRoulette_UIManager.txtGameID.text = GameID;
            //_carRoulette_UIManager.txtTotalBet.text = data.GetField("disp_user_total_bet").ToString().Trim(Config.Inst.trim_char_arry);

            _carRoulette_UIManager.txtTotalBet.text = data.GetField("disp_user_total_bet").ToString().Trim(Config.Inst.trim_char_arry);

            UnityEngine.Debug.Log("CAR INFO: " + data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry) + "|TOTAL BET|" + data.GetField("disp_user_total_bet").ToString().Trim(Config.Inst.trim_char_arry));

            CarRoulette_PlayerManager.Inst.SET_PLAYER_DATA(data);

            float time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
            switch (GameState)
            {
                case "game_timer_start":
                    UnityEngine.Debug.Log("1: " + time);
                    _carRoulette_UIManager._timerStatus = TimerStatus.BETTING;
                    CarRouletteTimer.Inst.Txt_Timer_Status.text = "Betting";
                    isLossClip = false;
                    isNewRound = true;
                    _carRoulette_UIManager.CheckRebetStatus();
                    START_MAIN_TIMER(data, "M", 0);
                    UserRejoinMethod(data, false);
                    break;
                case "start_spin":
                    UnityEngine.Debug.Log("2: " + time);
                    _carRoulette_UIManager._timerStatus = TimerStatus.DRAWING;
                    CarRouletteTimer.Inst.Txt_Timer_Status.text = "Idle";
                    //START_MAIN_TIMER(data, "N",1);
                    //if (time > 8)
                    //{
                    //    START_SPIN(data);
                    //}
                    //else
                    //{
                    //}
                    UserRejoinMethod(data, true);
                    _carRoulette_UIManager.Wait_Next_Round_POP(true);
                    break;
                case "winner_declare_done":
                    UnityEngine.Debug.Log("3: " + time);
                    _carRoulette_UIManager._timerStatus = TimerStatus.IDLE;
                    CarRouletteTimer.Inst.Txt_Timer_Status.text = "Idle";
                    UserRejoinMethod(data, true);
                    saveManager.ResetSaveData();
                    _carRoulette_UIManager.Wait_Next_Round_POP(true);
                    break;
                case "finish_state":
                    UnityEngine.Debug.Log("4: " + time);
                    _carRoulette_UIManager._timerStatus = TimerStatus.IDLE;
                    CarRouletteTimer.Inst.Txt_Timer_Status.text = "Idle";
                    saveManager.ResetSaveData();
                    _carRoulette_UIManager.Wait_Next_Round_POP(true);
                    break;
            }
            GS.Inst.Rejoin = false;
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
                        int.TryParse(CarRoulette_EventManager.RemoveSpecialCharacters(split_XCard[0]), out int id);
                    
                        int amt = 0;
                        int.TryParse(CarRoulette_EventManager.RemoveSpecialCharacters(split_XCard[1]), out amt);

                        for (int j = 0; j < _carRoulette_UIManager._chooseBetRouletteList.Count; j++)
                        {
                            Symbol_CarRoulette sc = _carRoulette_UIManager._chooseBetRouletteList[j];

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

        public void UserRejoinMethod(JSONObject data, bool winner)
        {
            MyUserSideChipTotal(data);
            AllBetSideCheck();

            int betCard = data.GetField("dis_total_bet_on_cards").Count;
            UnityEngine.Debug.Log("USER BET ON CARD: " + betCard);
            string strBet = data.GetField("dis_total_bet_on_cards").ToString();
            UnityEngine.Debug.Log("dis_total_bet_on_cards: " + strBet);
            string[] disBetList = strBet.Split(',').ToArray();

            for (int i = 0; i < betCard; i++)
            {
                string[] split_XCard = disBetList[i].Split(':');
                int.TryParse(CarRoulette_EventManager.RemoveSpecialCharacters(split_XCard[0]), out int id);
                var betItem = _carRoulette_UIManager._chooseBetRouletteList.Find(number => number.id == id);
                int amt = 0;
                int.TryParse(CarRoulette_EventManager.RemoveSpecialCharacters(split_XCard[1]), out amt);
                betItem._totalSymbolChip = amt;
                betItem.txtTotalChip.text = amt.ToString();
            }

            if (winner)
            {
                int win = int.Parse(data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry));
                WinInfo(win);
            }
        }

        //------------ User Bet Send -----------------
        public void USER_SEND_BET(string Bet)
        {
            SocketHandler.Inst.SendData(SocketEventManager.Inst.CAR_ROULETTE_PLACE_BET(Bet));
        }
        //------------ User Bet Send -----------------

        //------------ User Bet Send -----------------
        public void RE_USER_SEND_BET(string Bet, int betVal)
        {
            SocketHandler.Inst.SendData(SocketEventManager.Inst.CAR_ROULETTE_PLACE_RE_BET(Bet, betVal));
        }
        //------------ User Bet Send -----------------

        //------------------------------ SPIN -----------------------------------------------------------
        public void INIT_GAME(JSONObject data)
        {
            UnityEngine.Debug.Log("INIT GAME");
            ResetGame();
            CarRouletteTimer.Inst.Txt_Timer_Status.text = "Idle";
            _carRoulette_UIManager._timerStatus = TimerStatus.IDLE;
            UnityEngine.Debug.Log("INIT GAME: " + data);
            START_MAIN_TIMER(data, "N", 2);
            _carRoulette_UIManager.Wait_Next_Round_POP(true);
            CarRoulette_PlayerManager.Inst.REFRESH_PLAYER_DATA(data);
        }
        //------------------------------ SPIN -----------------------------------------------------------


        public void PLACING_BET(JSONObject data)
        {
            CarRoulette_EventManager.PFB_COIN_KILL();
            _carRoulette_UIManager.Wait_Next_Round_POP(false);
            _carRoulette_UIManager.NEW_ROUND_START_STOP(true);
            GameID = data.GetField("game_id").ToString().Trim(Config.Inst.trim_char_arry);
            _carRoulette_UIManager.txtGameID.text = GameID;
            _carRoulette_UIManager.txtTotalBet.text = "0";
            CarRouletteTimer.Inst.Txt_Timer_Status.text = "Betting";
            UnityEngine.Debug.Log("START BETTING");
            isNewRound = true;
            _carRoulette_UIManager.CheckRebetStatus();
            _carRoulette_UIManager._timerStatus = TimerStatus.BETTING;
            START_MAIN_TIMER(data, "M", 0);
        }

        //------------------------------ SPIN -----------------------------------------------------------
        public void START_SPIN(JSONObject data)
        {
            UnityEngine.Debug.Log("START SPIN TO WIN: " + data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry));
            CarRouletteTimer.Inst.Txt_Timer_Status.text = "Drawing";
            _carRoulette_UIManager._timerStatus = TimerStatus.DRAWING;
            StartNewGame(data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry));
            START_MAIN_TIMER(data, "M", 0);
        }
        //------------------------------ SPIN -----------------------------------------------------------

        //------------------------------ Timer Start -----------------------------------------------------------
        public void START_MAIN_TIMER(JSONObject data, string TimerType, int startIndex)
        {
            //UnityEngine.Debug.Log("TimerType: " + TimerType);

            float time = 0;

            if (TimerType.Equals("N"))
            {
                if (startIndex == 2)
                {
                    UnityEngine.Debug.Log("INIT GAME IDLE TIMER: " + data.GetField("game_start_time").ToString().Trim(Config.Inst.trim_char_arry));
                    time = float.Parse(data.GetField("game_start_time").ToString().Trim(Config.Inst.trim_char_arry));
                }
                else
                {
                    time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
                }
            }
            else if (TimerType.Equals("M"))
            {
                time = float.Parse(data.GetField("timer").ToString().Trim(Config.Inst.trim_char_arry));
            }

            //UnityEngine.Debug.Log("TIMER START >>>>>>>>" +_carRoulette_UIManager._timerStatus +"-"+TimerType + "-" + time);

            if (TimerType.Equals("M"))
            {
                if (time < 15)
                    CarRouletteTimer.Inst.StartTimerAnim(time, time, true, TimerType);
                else
                    CarRouletteTimer.Inst.StartTimerAnim(time, time, false, TimerType);
            }
            else
            {
                CarRouletteTimer.Inst.StartTimerAnim(time, time, false, TimerType);
            }
            InfoSetup = true;
        }
        //------------------------------ Timer Start -----------------------------------------------------------

        //------------------------------ Seat User ---------------------------------------------------------
        public void SEAT_USER(JSONObject data)
        {
            string id = data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
            bool is_Bot = bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry));
            if (id != GS.Inst._userData.Id)
            {
                bool fl = false;
                for (int i = 0; i < DT_PlayerManager.Inst.Player_Bot_List.Count; i++)
                {
                    if (DT_PlayerManager.Inst.Player_Bot_List[i]._Status == DT_Player.Status.Null && !is_Bot && !fl)
                    {
                        fl = true;
                        DT_PlayerManager.Inst.Player_Bot_List[i].SEAT(data);
                    }
                }
            }
        }
        //------------------------------ Seat User ----------------------------------------------------------- 

        //------------------------------ BET INFO -----------------------------------------------------------
        public void BET_INFO(JSONObject data)
        {
            if (InfoSetup)
                CarRoulette_PlayerManager.Inst.PLAYER_CHAAL(data);
        }
        //------------------------------ BET INFO ----------------------------------------------------------- 

        //------------ Winning history set -----------------
        public void WINNER_DECLARE(JSONObject data)
        {
            saveManager.ResetSaveData();
            UnityEngine.Debug.Log("WINNER_DECLARE: " + data);
            string win_card = data.GetField("win_card").ToString().Trim(Config.Inst.trim_char_arry);
            UnityEngine.Debug.Log("WIN CARD: " + win_card);
            resultSymbolIndex = int.Parse(win_card);
            winRoulette = _carRoulette_UIManager._chooseBetRouletteList.Find(item => item.id == resultSymbolIndex);

            winRoulette.animWin.gameObject.SetActive(true);
            winRoulette.animWin.Rebind();
            winRoulette.animWin.Play("WinRouletteEffect");
            Invoke("WaitCallOff", 3f);

            int totalCounter = data.GetField("last_win_cards").Count;
            UnityEngine.Debug.Log("CURRENT BET ITEM: " + winRoulette.name + "Current history Card: " + totalCounter);

            if (totalCounter > 0)
            {
                int currentCard = totalCounter - 1;
                int getIndex = int.Parse(data.GetField("last_win_cards")[currentCard].ToString().Trim(Config.Inst.trim_char_arry));
                _historyManager.SET_HISTORY_CARD_DATA((getIndex - 1), true);
            }
        }

        public void WaitCallOff()
        {
            winRoulette.animWin.gameObject.SetActive(false);
        }
        double OnlineUserValue;

        //------------ Win Coin Move to winner user -----------------
        public void WINNER_TO_MOVE_CHIP(JSONObject data)
        {
            UnityEngine.Debug.Log($"WINNER_TO_MOVE_CHIP: {data} | CCCC | {data.GetField("users_bets").keys.Count}");

            isLossClip = true;

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
                print("value: " + value);
                if (GS.Inst._userData.Id.Equals(uid))
                {
                    CarRoulette_PlayerManager.Inst.WinOrLose_Chips = value;
                    CarRoulette_PlayerManager.Inst.Played_Chips = true;

                    if (value != 0)
                    {
                        _carRoulette_UIManager.TextParticleAnimation(value);
                    }
                }
                else
                {
                    OnlineUserValue += value;
                    print("GS.Inst._userData.Id Not available in user list " + GS.Inst._userData.Id);
                }
            }
            if (OnlineUserValue != 0)
            {
                _carRoulette_UIManager.OnliePlayerTextParticleAnimation(OnlineUserValue);
            }
           
                foreach (var roulette in _carRoulette_UIManager._chooseBetRouletteList)
                {
                    int totalChips = roulette._combineChipList.Count;
                    int remainChipsIndex = totalChips;



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
        public int nonUserChipAnimationLimit = 5;
        private void AnimateChips(Symbol_CarRoulette roulette, int totalChips, int remainChipsIndex, bool isWin)
        {
            int nonUserChipAnimations = 0;           
            
            for (int j = 0; j < remainChipsIndex; j++)
            {
                CarRoulette_ChipManager chip = roulette._combineChipList[j];               
                //chip.imgChip.DOFade(1, 0);
               
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


        public float minLoss, maxLoss;

        //------------------------------ Leave User ---------------------------------------------------------
        public void LEAVE_USER(JSONObject data)
        {
            CarRoulette_EventManager.USER_LEAVE(data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry));
        }
        //------------------------------ Leave User ----------------------------------------------------------- 

        public void ChooseOtherChipMethod()
        {
            int rnIndex = UnityEngine.Random.Range(0, _carRoulette_UIManager._chooseBetRouletteList.Count);
            Symbol_CarRoulette botRoulette = _carRoulette_UIManager._chooseBetRouletteList[rnIndex];
            int betVal = _betList[UnityEngine.Random.Range(0, _betList.Count)].intValue;
            botRoulette.OnClickOtherItem(betVal);
        }

        public void StartNewGame(string id)
        {
            isLossClip = false;
            _carRoulette_UIManager.rebetTarget = 0;
            StopSideChek();
            _carRoulette_UIManager.btnRebet.interactable = false;
            resultSymbolIndex = int.Parse(id);
            //UnityEngine.Debug.Log("WIN INDEX: "+ resultSymbolIndex);
            SetConfigDataForAnimation();
        }
        private void SetConfigDataForAnimation()
        {
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
            int rng = Random.Range(0, symbolList.Count);
            if (resultSymbolIndex == symbolList[rng].GetComponent<Symbol_CarRoulette>().id)
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
            for (int i = 0; i < symbolList.Count; i++)
            {
                tempSymbolList.Add(symbolList[i]);
            }
        }

        private void AddSelectedIndex()
        {
            for (int i = 0; i < symbolList.Count; i++)
            {
                if (i == stopAnimAtSymbol + 1)
                {
                    break;
                }
                else
                {
                    tempSymbolList.Add(symbolList[i]);
                }
            }
        }
        public float fltSoundDuration;
        private IEnumerator PlayLoopAnimation()
        {
            float i = 0;
            int newindex = 0;
            int currentIndex = 0;

            while (i < loopCloseTime)
            {
                i += Time.deltaTime / animationSpeed;
                newindex = (int)Mathf.Lerp(0, tempSymbolList.Count - 1, speedCurve.Evaluate(i));
                //UnityEngine.Debug.Log("WIN ITEM: "+ speedCurve.Evaluate(i).ToString());
                // Check if the next index has changed

                if (newindex == tempSymbolList.Count - 1)
                {
                }
                else
                {
                    if (newindex != currentIndex)
                    {
                        currentIndex = newindex;
                        Car_Roulette_Sound.Inst.PlayReel();
                        // UnityEngine.Debug.Log("Next Symbol Index: " + currentIndex);
                    }
                    tempSymbolList[newindex].AnimatedItem(scaleVector, fltDuration, setEase);
                }
                yield return null;
            }

            winItem = tempSymbolList[newindex];
            Car_Roulette_Sound.Inst.StopReel();
            winItem.WinItemAnimated(winScaleVector, fltWinDuration, winSetEase);
            goWinItemParticle = Instantiate(_carRoulette_UIManager._goItemWinParticle, Vector3.zero, Quaternion.identity);
            goWinItemParticle.transform.SetParent(winItem.transform);
            goWinItemParticle.transform.localPosition = Vector3.zero;
            goWinItemParticle.transform.localScale = Vector3.one;
        }

        public void WinInfo(int winIndex)
        {
            UnityEngine.Debug.Log("WIN:" + winIndex);
            var occurrences = tempSymbolList.Where(number => number.id == winIndex).ToList();
            winItem = occurrences[Random.Range(0, occurrences.Count)];
            Car_Roulette_Sound.Inst.StopReel();
            winItem.WinItemAnimated(winScaleVector, fltWinDuration, winSetEase);
            goWinItemParticle = Instantiate(_carRoulette_UIManager._goItemWinParticle, Vector3.zero, Quaternion.identity);
            goWinItemParticle.transform.SetParent(winItem.transform);
            goWinItemParticle.transform.localPosition = Vector3.zero;
            goWinItemParticle.transform.localScale = Vector3.one;
        }

        public float hitTime = 0f;
        public void ResetGame()
        {
            _carRoulette_UIManager.isWaitCond = false;
            UnityEngine.Debug.Log("RESET GAME START");

            if (goWinItemParticle != null)
            {
                Destroy(goWinItemParticle);
            }
            winItem.RevertAnimatedItem(0.1f, Ease.Linear);

            for (int i = 0; i < _carRoulette_UIManager._chooseBetRouletteList.Count; i++)
            {
                _carRoulette_UIManager._chooseBetRouletteList[i].ResetItemValue();
            }

            _carRoulette_UIManager._timer = 5;
            _carRoulette_UIManager._timerStatus = TimerStatus.START;

            _userWinChip = 0;
            _carRoulette_UIManager.totalCoins = 0;
            _carRoulette_UIManager.txtTotalBet.text = "0";
            CarRoulette_PlayerManager.Inst.Played_Chips = false;
            CarRoulette_PlayerManager.Inst.WinOrLose_Chips = 0f;
            _carRoulette_UIManager.isRebet = false;
            saveManager.ResetSaveData();
            UnityEngine.Debug.Log("RESET GAME END");
        }
        public AudioSource Coin_Audio_Source;

        public void MY_User_Chaal_Animation(string Chaa_Amount, string side)
        {
            _carRoulette_UIManager.isWaitCond = true;
            //UnityEngine.Debug.Log("MY USER CHAl: "+ side + "|"+ Chaa_Amount);
            Symbol_CarRoulette sy = null;
            for (int i = 0; i < _carRoulette_UIManager._chooseBetRouletteList.Count; i++)
            {
                string st = _carRoulette_UIManager._chooseBetRouletteList[i].name;
                if (st.Contains(side))
                {
                    sy = _carRoulette_UIManager._chooseBetRouletteList[i];
                    break;
                }
            }

            // UnityEngine.Debug.Log("CURRENT BET ITEM: " + sy.name);
            Vector3 target = getRandomPoint(sy.goMoveTarget, 0);
            GameObject _Coin = Instantiate(cloneCoinPrefab);
            CarRoulette_ChipManager _chipManager = _Coin.transform.GetComponent<CarRoulette_ChipManager>();
            _chipManager.userChipStatus = UserStatus.MYUSER;
            if (PlayerPrefs.GetInt("sound").Equals(1))
                Coin_Audio_Source.PlayOneShot(Car_Roulette_Sound.Inst.SFX[3]);
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

            _carRoulette_UIManager.rebetTarget++;
            UnityEngine.Debug.Log("NEW ROUND CURRENT BET ITEM: " + isNewRound + "|" + _carRoulette_UIManager.isRebet + ">>>>" + _carRoulette_UIManager.rebetTarget);

            if (!_carRoulette_UIManager.isRebet && !isNewRound)
                saveManager.MyAlreadyUserBetStatus(sy.id, _chipManager.val);

            saveManager.CheckAlreadyUserBetStatus(sy.id, _chipManager.val, UserStatus.MYUSER);
            saveManager.CheckMaxBetReached();

            if (saveManager.addUserDataLst.Count == _carRoulette_UIManager.rebetTarget)
            {
                _carRoulette_UIManager.WaitToCallResetRebet();
                StopSideChek();
                _carRoulette_UIManager.isWaitCond = false;
            }

            My_User_ChalAnim.DOScale(MyAnimScale, fltChalAnimTimer).SetEase(Ease.Linear).OnComplete(() =>
            ResetTweeCall(0)
            ).SetAutoKill();
        }

        public void Real_User_Chaal_Animation(string Chaa_Amount, string side)
        {
            // UnityEngine.Debug.Log("OTHER USER CHAl: " + side + "|" + Chaa_Amount);

            Symbol_CarRoulette sy = null;
            for (int i = 0; i < _carRoulette_UIManager._chooseBetRouletteList.Count; i++)
            {
                string st = _carRoulette_UIManager._chooseBetRouletteList[i].name;
                if (st.Contains(side))
                {
                    sy = _carRoulette_UIManager._chooseBetRouletteList[i];
                    break;
                }
            }

            //UnityEngine.Debug.Log("CURRENT BET ITEM: " + sy.name);
            Vector3 target = getRandomPoint(sy.goMoveTarget, 0);
            GameObject _Coin = Instantiate(cloneCoinPrefab);
            CarRoulette_ChipManager _chipManager = _Coin.transform.GetComponent<CarRoulette_ChipManager>();
            _chipManager.userChipStatus = UserStatus.OTHERUSER;
            if (PlayerPrefs.GetInt("sound").Equals(1))
                Coin_Audio_Source.PlayOneShot(Car_Roulette_Sound.Inst.SFX[3]);
            _chipManager.SET_COIN(Chaa_Amount);
            _Coin.transform.SetParent(sy.gameObject.transform, false);
            _Coin.name = "OTHER_USER_COIN_" + Chaa_Amount;
            _Coin.transform.position = goOtherUserChal.transform.position;
            _chipManager.Move_Anim(target);
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
            CarRoulette_ChipManager _chipManager = _Coin.transform.GetComponent<CarRoulette_ChipManager>();
            _chipManager.SET_COIN(Chaa_Amount);
            _Coin.transform.SetParent(Coin_MoveOBJ.transform, false);
            _chipManager.Place_Anim(target);
            _chipManager.userChipStatus = status;
            Symbol_CarRoulette scr = TargetOBJ.GetComponent<Symbol_CarRoulette>();
            scr._combineChipList.Add(_chipManager);
        }

        public void AllBetSideCheck()
        {
            for (int i = 0; i < saveManager._userBetDataClass.betDataClassLst.Count; i++)
            {
                BetDataClass bdc = saveManager._userBetDataClass.betDataClassLst[i];

                for (int j = 0; j < _carRoulette_UIManager._chooseBetRouletteList.Count; j++)
                {
                    Symbol_CarRoulette sc = _carRoulette_UIManager._chooseBetRouletteList[j];

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
                                    Save_User_Bet_Animation(betVal.ToString(), sc.gameObject, sc.goMoveTarget, 0,
                                        usbc.userStatus);
                                }
                            }
                        }
                    }
                }
            }
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
                UnityEngine.Debug.Log("BetValue: " + x + "-->" + rebetData + "::" + _carRoulette_UIManager.isWaitCond);
                _carRoulette_UIManager.isWaitCond = false;
                string[] st = rebetData.Split(':');
                string betSide = st[0];
                int.TryParse(st[1], out int result);
                UnityEngine.Debug.Log("UPLOAD RESULT: " + betSide + ":" + result);
                RE_USER_SEND_BET(betSide, result);
                yield return new WaitUntil(() => _carRoulette_UIManager.isWaitCond);
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
            CarRoulette_ChipManager _chipManager = moveTarget.transform.GetComponent<CarRoulette_ChipManager>();
            _chipManager.transform.SetParent(chipTarget.transform, false);
            _chipManager.Move_Anim(target);
        }

        public void Idle_Chip_Animation(GameObject chipTarget, GameObject moveTarget, int type)
        {
            UnityEngine.Debug.Log("TARGET: " + chipTarget.name + "{}" + moveTarget.name);
            CarRoulette_ChipManager _chipManager = moveTarget.transform.GetComponent<CarRoulette_ChipManager>();
            _chipManager.transform.SetParent(chipTarget.transform, false);
            _chipManager.Move_Anim(chipTarget.transform.position);
        }

        public List<GameObject> TargetList = new();


        void Kill_Win_Coins()
        {
            _carRoulette_UIManager.txtTotalBet.text = "0";
            CarRoulette_EventManager.PFB_COIN_KILL();
            CarRoulette_PlayerManager.Inst.Played_Chips = false;
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

        public void CurrentBetAnimetd(CarRoulette_BetManager bm)
        {
            _selectedBetItem = bm;

            for (int i = 0; i < _betList.Count; i++)
            {
                if (_selectedBetItem != _betList[i])
                {
                    _betList[i].ResetAnimatedItem(fltDefaultBetPosY, fltMoveResetDuration, easeMove);
                }
                else
                {
                    _betList[i].AnimatedItem(fltAnimetdBetPosY, fltMoveDuration, easeMove);
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