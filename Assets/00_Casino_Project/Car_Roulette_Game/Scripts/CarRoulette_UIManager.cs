namespace CarRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;
    using TMPro;
    using UnityEngine.UI;

    public class CarRoulette_UIManager : MonoBehaviour
    {
        public static CarRoulette_UIManager _instance;

        public TextMeshProUGUI txtGameID;
        public TextMeshProUGUI txtTotalBet;
        public TextMeshProUGUI txtBetTimer;

        public int totalCoins;
        public int beforeUserChip = 0;

        public GameObject Wait_For_NewRound;

        public int _timer = 0;
        public TimerStatus _timerStatus;

        public List<Image> _imgBetChangeList;

        public int _placeTextIndex = 0;
        public List<GameObject> _goPlaceTextList;

        public float _placeTextAnimTime = 1f;
        public Vector3 _v3DefalutPlaceTextSize = new Vector3(1.25f, 1.25f, 1);

        public CarRoulette_GameManager _carRoulette_Game;
        public List<Symbol_CarRoulette> _chooseBetRouletteList;

        public GameObject _goItemWinParticle;
        public Transform goTextParticle;
        public Transform onlinePlayerGoTextParticle;

        public Animator Start_Bet_Anim, Stop_Bet_Anim;

        public Transform trNextPrevBtn;
        public Image nextPrevSpt;
        public Sprite[] sptNextPrevAry;
        public Transform trMainBetPos;

        private void Awake()
        {
            _instance = this;
        }

        public float fltBetMovePosX;
        public float fltBetMoveTimer;
        public bool isUserList = false;
        Vector3 startPosGoTextParticle;
        Vector3 OnlinePlayerStartPosGoTextParticle;
        // Start is called before the first frame update
        void Start()
        {
            isUserList = false;
            _carRoulette_Game = FindObjectOfType<CarRoulette_GameManager>();
            _placeTextIndex = 0;
            nextPrevSpt.sprite = sptNextPrevAry[0];
            trNextPrevBtn.DOLocalMoveX(fltBetMovePosX, fltBetMoveTimer).SetEase(Ease.Linear).SetLoops(-2, LoopType.Yoyo);
            isNext = true;
            startPosGoTextParticle = goTextParticle.transform.localPosition;
            OnlinePlayerStartPosGoTextParticle = onlinePlayerGoTextParticle.transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void BTN_GROUP_LIST()
        {
            isUserList = true;
            Car_Roulette_Sound.Inst.BtnSFX(1);
            Car_Roullate_Online_User_Manager.Inst.BTN_OPEN();
            SocketHandler.Inst.SendData(SocketEventManager.Inst.CAR_JOINED_USER_LISTS());
        }

        public float fltIdleBetPos;
        public float fltMainBetMovePosX;
        public float fltMainBetMoveTimer;

        public bool isNext = false;

        public void BtnNextPrevBet()
        {
            Car_Roulette_Sound.Inst.BtnSFX(1);
            Debug.Log("NEXT-PREV: " + isNext);
            if (isNext)
            {
                trMainBetPos.DOLocalMoveX(fltMainBetMovePosX, fltMainBetMoveTimer).SetEase(Ease.Linear);
                nextPrevSpt.sprite = sptNextPrevAry[1];
                isNext = false;
            }
            else
            {
                trMainBetPos.DOLocalMoveX(fltIdleBetPos, fltMainBetMoveTimer).SetEase(Ease.Linear);
                nextPrevSpt.sprite = sptNextPrevAry[0];
                isNext = true;
            }
        }

        public Vector3 v3TimerScale;

        public void PlaceTextAnimation(int index, float timer)
        {
            _goPlaceTextList[_placeTextIndex].SetActive(true);
            _goPlaceTextList[_placeTextIndex].transform.DOScale(v3TimerScale, timer).SetEase(Ease.OutExpo).
                OnComplete(() => { ResetPlaceTextAnimation(_placeTextIndex); });
        }

        public void ResetPlaceTextAnimation(int index)
        {
            _goPlaceTextList[index].transform.localScale = _v3DefalutPlaceTextSize;
            _goPlaceTextList[index].SetActive(false);

            if (_placeTextIndex < 2)
                _placeTextIndex++;
            else
            {
                _placeTextIndex = 0;
            }
        }


        public IEnumerator WaitCallResetTextAnim(float timer, int index)
        {
            yield return new WaitForSeconds(timer);
            _goPlaceTextList[_placeTextIndex].transform.localScale = _v3DefalutPlaceTextSize;

            if (index == 0 || index == 4)
                _goPlaceTextList[_placeTextIndex].SetActive(false);

            _placeTextIndex++;
        }

        public void TextParticleAnimation(double val)
        {
            UnityEngine.Debug.Log("PLAYER WIN OR LOOS: " + val);

            goTextParticle.gameObject.SetActive(true);
            if (val < 0)
            {
                goTextParticle.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 230, 255, 255);
                goTextParticle.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + val.ToString();
                Car_Roulette_Sound.Inst.PlaySFX_Others(6);
            }
            else
            {
                goTextParticle.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                goTextParticle.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + val.ToString();
                Car_Roulette_Sound.Inst.PlaySFX_Others(5);
            }

            goTextParticle.DOLocalMoveY(100f, 2f).SetEase(Ease.InOutElastic).OnComplete(() =>
            {
                goTextParticle.gameObject.SetActive(false);
                goTextParticle.transform.localPosition = startPosGoTextParticle;
            });
        }
        public void OnliePlayerTextParticleAnimation(double val)
        {
            UnityEngine.Debug.Log("OnliePlayer PLAYER WIN OR LOOS: " + val);

            onlinePlayerGoTextParticle.gameObject.SetActive(true);
            if (val < 0)
            {
                onlinePlayerGoTextParticle.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 230, 255, 255);
                onlinePlayerGoTextParticle.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + val.ToString();
            }
            else
            {
                onlinePlayerGoTextParticle.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                onlinePlayerGoTextParticle.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + val.ToString();
            }

            onlinePlayerGoTextParticle.DOLocalMoveY(2f, 2f).SetEase(Ease.InOutElastic).OnComplete(() =>
            {
                onlinePlayerGoTextParticle.gameObject.SetActive(false);
                onlinePlayerGoTextParticle.transform.localPosition = OnlinePlayerStartPosGoTextParticle;
            });
        }

        public void Wait_Next_Round_POP(bool action)
        {
            UnityEngine.Debug.Log("Wait_Next_Round_POP: " + action);

            if (action)
                GS.Inst.iTwin_Open(Wait_For_NewRound);
            else
                GS.Inst.iTwin_Close(Wait_For_NewRound, 0.2f);
        }

        public void NEW_ROUND_START_STOP(bool action)
        {
            //UnityEngine.Debug.Log("ROUND STATUS: "+action);
            if (action)
            {
                Car_Roulette_Sound.Inst.PlayBG(0);
                Start_Bet_Anim.enabled = true;
                Start_Bet_Anim.Play("PlaceBetAnim");
            }
            else
            {
                Stop_Bet_Anim.enabled = true;
                Stop_Bet_Anim.Play("StopBetAnim");
            }
            Invoke(nameof(AfterOffRound), 3.1f);
        }

        public void AfterOffRound()
        {
            Start_Bet_Anim.Rebind();
            Stop_Bet_Anim.Rebind();

            Start_Bet_Anim.enabled = false;
            Stop_Bet_Anim.enabled = false;
        }

        public void onClickRebet()
        {
            if (isRebet)
                return;

            if (_carRoulette_Game.saveManager.addUserDataLst.Count == 0)
                return;

            UnityEngine.Debug.Log("RE-BET");
            rebetTarget = 0;
            isWaitCond = false;
            isRebet = true;
            _carRoulette_Game.isNewRound = false;
            btnRebet.interactable = false;

            if (isTestSend)
                StartCoroutine(SendDataRepeatedly());
            else
                _carRoulette_Game.StartCoroutine(nameof(_carRoulette_Game.MyAllBetSideCheck));
        }

        IEnumerator SendDataRepeatedly()
        {
            string betID = "5";
            for (int i = 0; i < 10; i++)
            {
                isWaitCond = false;
                int bet = 100; // Assuming bet can be any random value
                _carRoulette_Game.RE_USER_SEND_BET(betID, bet);
                yield return new WaitUntil(() => isWaitCond);
            }
        }
        public void CheckRebetStatus()
        {
            Debug.Log("CheckRebetStatus: " + isRebet + "||" + _carRoulette_Game.saveManager.addUserDataLst.Count);
            if (_carRoulette_Game.saveManager.addUserDataLst.Count > 0)
            {
                btnRebet.interactable = true;
            }
        }

        public void WaitToCallResetRebet()
        {
            isRebet = false;
        }

        public bool isTestSend;
        public float reBetWaitTime = 0.5f;
        public Button btnRebet;
        public bool isRebet = false;

        public bool isWaitCond = false;
        public int rebetTarget = 0;
        public GameObject popupMaxBetArea;

        public void CheckMaxinumBetArea()
        {
            Debug.Log("MAXIMUM BETTING AREA");
            popupMaxBetArea.SetActive(true);
        }
    }

    public enum TimerStatus
    {
        START,
        IDLE,
        BETTING,
        DRAWING
    }
}