using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CrashController : MonoBehaviour
{
    public static CrashController Instance;
    [Header("Common Settings")]
    [SerializeField] private TextMeshProUGUI otherUserCashoutText;
    [SerializeField] private TextMeshProUGUI valueText, receivableAmountText;
    [SerializeField] private Image valueBg;

    [SerializeField] private ParticleSystem coinFlowParticle;
    [SerializeField] private TextMeshProUGUI winningsText;
    [SerializeField] public TextMeshProUGUI betAmountText;
    [SerializeField] private RectTransform toastRect;
    [SerializeField] private CanvasGroup toastCanvasGroup;
    [SerializeField] private TextMeshProUGUI toastText;
    [SerializeField] private float gameCrashAt, currentVal = 0;
    private bool isCashOut = false;
    private float currencyValue;
    public int betAmount = 0;
    private bool isBetting = false;


    [Header("Path Settings")]
    private List<Vector3> points = new List<Vector3>();
    [SerializeField] private UILineRenderer LineRenderer;
    [SerializeField] private Transform pathParent;


    [Header("Rocket Settings")]
    [SerializeField] private RectTransform rocketRect;
    [SerializeField] private bool isCrashed = false, isFlying = false;
    [SerializeField] public Transform rocket;
    public ParticleSystem rocketTrail, rocketBlast, starParticle;
    private Vector3 rocketTrailDefaultPos;

    private Vector2 rocketInitialPos;

    [Header("Graph Settings")]
    [SerializeField] private GameObject historyPrefab;
    [SerializeField] private Transform historyContent;
    [SerializeField] private ScrollRect historyScrollrect;
    [SerializeField] private RectTransform horizontalGraph;
    [SerializeField] private RectTransform verticalGraph;
    [SerializeField] private bool isMoveGraph = false;
    [SerializeField] private float graphMoveSpeed = 1f;
    private Vector2 vGraphInitPos, hGraphInitPos;

    [Header("Game Start In")]
    [SerializeField] public GameObject gameStartIn;
    [SerializeField] public TextMeshProUGUI gameStartInText;
    [SerializeField] private Image timerProgress;


    [Header("Flee Condition")]
    [SerializeField] private TextMeshProUGUI fleeConditionValueText;
    [SerializeField] private float fleeConditionValue = 0f;
    [SerializeField] private float fleeValueIncrement = 1f;

    [Header("On Change Profit Loss Condition")]
    [SerializeField] private TextMeshProUGUI onProfitConditionText;
    [SerializeField] private TextMeshProUGUI onLossConditionText;
    [SerializeField] private TextMeshProUGUI ProfitProgressText;
    [SerializeField] private TextMeshProUGUI LossProgressText;
    [SerializeField] private Slider profitSlider, lossSlider;
    public int profitPercent = 0, lossPercent = 0;
    public float setMaxProfit = 0f, setMaxLoss = 0f;

    [Header("Game Mode")]
    public Sprite activeMode;
    public Sprite inactiveMode;
    public bool isManualMode = true;
    public Button autoModeButton;
    public Button manualModeButton;
    public GameObject[] disableObjectInManualMode;
    public GameObject[] disableObjectsInAutoMode;

    [Header("Game Mode/Manual")]
    public Sprite guessPressed;
    public Sprite guessNormal;
    public Image guessButtonImage;
    private bool isGuess = false;

    [Header("Game Mode/Auto")]
    public TextMeshProUGUI clickToStartText;
    private bool isClickToStart = false;

    public GameObject cashOutButton;

    [Header("User Profile")]
    public Text coins_user;
    public Text name;
    public RawImage profile;

    private void Awake()
    {
        Instance = this;
       // Application.runInBackground = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform item in pathParent)
        {
            points.Add(item.transform.localPosition);
        }
        //rocketInitialPos = pathParent.GetChild(0).localPosition;
        rocketInitialPos = rocketRect.transform.localPosition;
        rocketRect.transform.localPosition = rocketInitialPos;
        vGraphInitPos = verticalGraph.anchoredPosition;
        hGraphInitPos = horizontalGraph.anchoredPosition;
        rocketTrailDefaultPos = rocketTrail.transform.localPosition;
        valueText.text = "1.00x";
        // StartCoroutine(GameStartInCoroutine(10, 1f));


        Flee_UpdateUI();

        /*if (GlobalGameManager.instance)
        {
            coins_user.text = GlobalGameManager.instance.GetPlayerPersonalDetails().gamePoints[0];
            name.text = GlobalGameManager.instance.GetPlayerPersonalDetails().userName;           
            currencyValue = float.Parse(GlobalGameManager.instance.GetPlayerPersonalDetails().gamePoints[0]);
        }*/

    }
       
    public void SetTotalWalletAmountFromServer(double value)
    {
        currencyValue = (float)value; // This is a main wallet amount of the game
    }
    public void SetBetAmountFromServer(int user_total_bet_amount)
    {
        betAmount = user_total_bet_amount;
        betAmountText.text = user_total_bet_amount.ToString();
    }
    public void SetAutoMode(bool auto_claim)
    {
        isClickToStart = auto_claim;
    }
   
    public void SetUserInfoConfigFromServer(float flee_condition=0,int profit_on_stop = 0, float profit_win_amount = 0, int loss_on_stop = 0, float profit_loss_amount = 0,bool auto_remove=false,string mode="",bool isFromCashOut=false)
    {
        fleeConditionValueText.text = flee_condition.ToString();
        fleeConditionValue = flee_condition;

        profitSlider.value = profit_on_stop;       
        OnChangeProfitCondition();

        lossSlider.value = loss_on_stop;
        OnChangeLossCondition();                  

        if (mode.Equals("auto") && !auto_remove)
        {
            Debug.Log("Auto mode activated");
            GameMode(true);// true means game auto mode ma chhe   
            isClickToStart = false;
            ClickToStartStopButtonClick();
        }
        if (mode.Equals("auto") && auto_remove && isFromCashOut)
        {
            MakeToast("Auto mode stopped on your condition");
            Debug.Log("Manual mode activated");
            GameMode(false);// false means game auto mode ma nathi   
            isClickToStart = true;
            ClickToStartStopButtonClick();
        }
    }
    public void GameCrashAt(float crash_reward)
    {
        //gameCrashAt = crash_reward;
        valueText.text = crash_reward.ToString("F2") + "x";
    }
    public double GetCurrentTotalSecond()
    {
        DateTime currentDate = DateTime.Now;
        TimeSpan elapsedSpan = new TimeSpan(currentDate.Ticks);
        return elapsedSpan.TotalSeconds;      
    }
    public double currentSecond;

    bool ispaused = false;
    // Update is called once per frame
    void Update()
    {
        currentSecond = GetCurrentTotalSecond();
        //Debug.Log("currentSecond: " + currentSecond);

        coins_user.text = currencyValue.ToString("F2");        
        if (isCrashed == false && isFlying)
        {
            currentVal = 1f + (float)(currentSecond - startTime) * 0.08f;
            

            if (currentVal > gameCrashAt)
            {
                bool isAlreadyClaimed = true;
                isCrashed = true;
                currentVal = gameCrashAt;
                if (isBetting)
                {
                    if (isCashOut == false)
                    {
                        isAlreadyClaimed = false;
                        isCashOut = true;
                        ShowLossAmount(betAmount);
                       
                    }
                    isBetting = false;
                }
                cashOutButton.SetActive(false);
                CrashRocket(isAlreadyClaimed);
            }
            if (isBetting)
            {               
                if (!isCashOut && fleeConditionValue >= 1.01f && currentVal >= fleeConditionValue)
                {                   
                    isBetting = false;
                    isCashOut = true;
                    cashOutButton.SetActive(false);
                    //CashOut(getFleeConditionTime(fleeConditionValue)); // Jyare client side thi flee condtion check karvi hoe tyare aa line use karvi
                }
            }

            valueText.text = currentVal.ToString("F2") + "x";
            receivableAmountText.text = (currentVal * (float)betAmount).ToString("F2");
            // if (isMoveGraph)            
            {
                if (currentSecond - startTime > 10)
                {
                    Vector2 vPos = verticalGraph.anchoredPosition;
                    Vector2 hPos = horizontalGraph.anchoredPosition;
                    vPos.y = (float)(currentSecond - startTime-10) * -24f;//graphMoveSpeed;
                    hPos.x = (float)(currentSecond - startTime-10) * -87.3f;//graphMoveSpeed;

                    verticalGraph.anchoredPosition = vPos;
                    horizontalGraph.anchoredPosition = hPos;
                }

                rocketTrail.transform.localPosition = rocketTrailDefaultPos + (Vector3) UnityEngine.Random.insideUnitCircle * 0.2f;
            }
        }
    }

    public void AutoModeBtnEnable(bool value)
    {
        autoModeButton.interactable = value;
        if (value)
        {
            autoModeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            autoModeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0.3f);
        }
    }
    public void START_SCREEN(bool value)
    {
        gameStartIn.SetActive(value);
    }
    public void GameStartTimer(int seconds)
    {
        StartCoroutine(GameStartInCoroutine(seconds));
    }   
   
    IEnumerator GameStartInCoroutine(int seconds = 10, float delay = 1f)
    {
        //yield return new WaitForSeconds(delay);
        ClearLinePoints();
        gameStartIn.SetActive(true);
        AutoModeBtnEnable(true);
        cashOutButton.SetActive(false);
        valueBg.color = Crash_HistoryManager.Inst.histColorGreen;     
        valueText.text = "1.00x";
        if (fleeConditionValue <= 0 )
        {            
            sparkleAnimFlee.SetActive(true);
            if (isClickToStart)
            {
                sparkleAnimFlee.SetActive(false);
            }
        }
        if (isManualMode)
        {
            betAmount = 0;
            betAmountText.text = "0";
            isGuess = false;          
            guessButtonImage.sprite = guessNormal;
        }
        //gameStartInText.transform.parent.gameObject.SetActive(true);
        Debug.Log("GameStartInCoroutine "+seconds);
        DOTween.To(x => timerProgress.fillAmount = x, 1f, 0, seconds).SetEase(Ease.Linear);
        
        for (int s = 0; s < seconds; s++)
        { 
            yield return new WaitForSeconds(1f);          
        }
        //yield return new WaitForEndOfFrame();
        //ResetGame();
       
        // gameStartInText.transform.parent.gameObject.SetActive(false);
        //yield return new WaitForEndOfFrame();
        StartNewGame();
    }
    public void CashInOnClickBetPlace()
    {
        if (isManualMode)
        {
            if (betAmount > 0)// added for remove bet button
            {
                GuessButtonClick();
            }
        }
    }
    public void CashInEvent()
    {
        
        CheckIsBettings();

        if (betAmount > 0 && (isGuess || isClickToStart))
        {
            Crash_UI_Manager.Inst.BlockUIFull.SetActive(true);
            if (isManualMode)
            {
                Debug.Log("Cash in event call");
                Crash_Manager.Inst.USER_SEND_BET(betAmount, fleeConditionValue, 0, 0,"manual");
            }
            else
            {
                Crash_Manager.Inst.USER_SEND_BET(betAmount, fleeConditionValue, profitPercent, lossPercent,"auto");
            }
        }
        else
        {
            betAmount = 0;
            betAmountText.text = "0";
            Crash_UI_Manager.Inst.BlockUIFull.SetActive(true);
        }       
    }
    public bool cashOutBtnStatus;
    public void StartNewGame()
    {
        Debug.Log("StartNewGame()");
        Crash_SoundManager.Inst.PlaySFX(0);
        isCashOut = false;
        cashOutButton.SetActive(false);

        isBetting = false;

        if (cashOutBtnStatus)
        {
            cashOutButton.SetActive(true);
            cashOutBtnStatus = false;
        }
        if (isManualMode)
        {   
            if (isGuess && betAmount > 0)
            {            
                isBetting = true;
            }
            else if (!isGuess && betAmount > 0)
            {               
                betAmount = 0;
                betAmountText.text = "0";
            }         
        }
        else
        {
            if (isClickToStart && betAmount > 0)
                isBetting = true;
        }

        CheckIsBettings();

       /* if (isBetting)
        {
            if (isManualMode)
            {
                if (isGuess)
                {
                    if (betAmount > 0)
                    {
                        cashOutButton.SetActive(true);
                    }
                }

            }
            else
            {
                if (isClickToStart)
                {
                    if (betAmount > 0)
                    {
                         cashOutButton.SetActive(true);
                    }
                }
            }
        }*/

        Debug.Log("FlyRocket-after completing local time");

        //FlyRocket();
    }

    public void ResetGame()
    {
        Debug.Log("RESET GAME");
        if (DOTween.IsTweening("_planeMoving"))
        {
            DOTween.Kill("_planeMoving");
        }

        currentVal = 0f;
        ClearLinePoints();
        rocketRect.transform.localPosition = rocketInitialPos;

        verticalGraph.anchoredPosition = vGraphInitPos;
        horizontalGraph.anchoredPosition = hGraphInitPos;

        //rocket.gameObject.SetActive(true);
        //gameCrashAt = Random.Range(1.01f, 5.80f);
        //gameCrashAt = 2.12f;// (14*0.08f)+1;//Random.Range(1.01f, 5.80f);
        rocket.gameObject.SetActive(false);
        gameCrashAt = 0;
        isMoveGraph = false;
        isCrashed = false;
        isFlying = false;

    }
    double startTime;
    int start;
    public TextMeshProUGUI messText;
    public void FlyRocket(float crash_reward=0.0f,float sec=0.0f,bool isNewGame=false,float planeCrashTime=0.0f)
    {
        isFlying = false;
        currentSecond = GetCurrentTotalSecond();
        
        Debug.Log(string.Format("Fly Rocket: crash_reward {0} ,Second {1} , isNewGame {2}, planeCrashTime {3}", crash_reward, sec, isNewGame, planeCrashTime));
        messText.text = string.Format("crash_reward {0} ,Second {1} , isNewGame {2}, planeCrashTime {3}", crash_reward, sec, isNewGame, planeCrashTime);

        float orgSec = sec;
        Crash_UI_Manager.Inst.BlockUIFull.SetActive(true);
        gameStartIn.SetActive(false);
        sparkleAnimFlee.SetActive(false);
        sparkleAnimGuess.SetActive(false);      
        rocket.gameObject.SetActive(true);

        gameCrashAt = crash_reward; // important
       

        if (!isNewGame)
        {
            startTime = currentSecond - sec;
        }
        else
        {
            startTime = currentSecond;
        }

        isFlying = true;

        AddLinePoint(rocketRect.localPosition);
        float pointDist = 5f;
        Vector2 lastpos = rocketRect.localPosition;
        Vector3 _origPos = rocket.localPosition;

        List<Vector3> newPath = new List<Vector3>();
        for (int i = 0; i < points.Count; i++)
        {
            newPath.Add(points[i]);
        }
       
        if (!isNewGame)
        {
          
            start = (int)(sec);
            Debug.Log("Start Point:" + start);
            if (start < points.Count - 1 && start > 0)
            {
                newPath.Clear();
                for (int i = start; i < points.Count; i++)
                {
                    newPath.Add(points[i]);
                }
            }
            else
            {               
                sec = 10f;
               // startTime -= orgSec;
            }
        }
        else
        {
            sec = 0.0f;
        }

        if (DOTween.IsTweening("_planeMoving")) {
            DOTween.Kill("_planeMoving");
        }

        if (!isNewGame)
        {          
            List<Vector3> oldPath = new List<Vector3>();
            for (int i = 0; i < start; i++)
            {
                if (i < points.Count)
                {
                    oldPath.Add(points[i]);
                }
            }
            if (oldPath.Count > 0)
            {
                int l = oldPath.Count % 3;
                for (int j = 0; j < l; j++)
                {

                    oldPath.Add(oldPath[oldPath.Count-1]);
                }               
                rocket.localPosition = oldPath[oldPath.Count-1];
                CreateCurve(oldPath);
            }
        }
      
        rocket.DOLocalPath(newPath.ToArray(),10.0f-sec, PathType.CatmullRom).SetEase(Ease.Linear).SetId("_planeMoving")
            .OnUpdate(() =>
            {  
                if (Vector2.Distance(rocketRect.localPosition, lastpos) > pointDist)
                {
                    lastpos = rocketRect.localPosition;
                    //if(isNewGame)
                        AddLinePoint(lastpos);
                }

                Vector3 moveDirection = rocket.localPosition - _origPos;
                if (moveDirection != Vector3.zero)
                {
                    float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                    rocket.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
                _origPos = rocket.localPosition;

            })
            .OnComplete(() =>
            {
               /* if (!isNewGame)
                {
                    if(orgSec > 10)
                    {
                        graphMoveTime = Time.time - (orgSec-10);
                    }
                    else
                    {
                        graphMoveTime = Time.time;
                    }
                }
                else
                {
                    graphMoveTime = Time.time;
                }
               
                isMoveGraph = true;*/
            });

        if (starParticle.gameObject.activeSelf == false)
        {
            starParticle.gameObject.SetActive(true);
        }
        ParticleSystem.EmissionModule rocketTrailEm = rocketTrail.emission;
        rocketTrailEm.enabled = true;
        ParticleSystem.EmissionModule starParticleEm = starParticle.emission;
        starParticleEm.enabled = true;

    }

    Vector3 BezierPathCalculation(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float tt = t * t;
        float ttt = t * tt;
        float u = 1.0f - t;
        float uu = u * u;
        float uuu = u * uu;

        Vector3 B = new Vector3();
        B = uuu * p0;
        B += 3.0f * uu * t * p1;
        B += 3.0f * u * tt * p2;
        B += ttt * p3;

        return B;
    }
    private List<Vector3> pathPoints=new List<Vector3>();
    private int segments;
    private int pointCount;
    public void CreateCurve(List<Vector3> controlPoints)
    {
        segments = controlPoints.Count / 3;
        pointCount = 100;
        pathPoints.Clear();

        for (int s = 0; s < controlPoints.Count - 3; s += 3)
        {
            Vector3 p0 = controlPoints[s];
            Vector3 p1 = controlPoints[s + 1];
            Vector3 p2 = controlPoints[s + 2];
            Vector3 p3 = controlPoints[s + 3];
           
            if (s == 0)
            {
                pathPoints.Add(BezierPathCalculation(p0, p1, p2, p3, 0.0f));
            }

            for (int p = 0; p < (pointCount / segments); p++)
            {
                float t = (1.0f / (pointCount / segments)) * p;
                Vector3 point1 = new Vector3();
                point1 = BezierPathCalculation(p0, p1, p2, p3, t);
                AddLinePoint(point1);
                pathPoints.Add(point1);
            }
        }
    }
    float graphMoveTime;
    public void CrashRocket( bool isClaimed=false)
    {

        Debug.Log("CrashRocket...");
        Crash_SoundManager.Inst.PlaySFX(1);       
        isCrashed = true;
        isMoveGraph = false;
        isFlying = false;
        if (DOTween.IsTweening("_planeMoving"))
        {
            DOTween.Kill("_planeMoving");
        }

        rocket.gameObject.SetActive(false);
        valueBg.color = Color.red;

        //AddToHistorybar(gameCrashAt,isClaimed);
       // StartCoroutine(GameStartInCoroutine(5, 2f));

        ParticleSystem.EmissionModule rocketTrailEm = rocketTrail.emission;
        rocketTrailEm.enabled = false;
        ParticleSystem.EmissionModule starParticleEm = starParticle.emission;
        starParticleEm.enabled = false;
        rocketBlast.transform.localPosition = rocket.transform.localPosition;
        rocketBlast.Play();
    }
    public void CrashRocketOfOtherUsers(float crash_reward=0)
    {
        Debug.Log("CRASH_USER_CASH_OUT: "+ crash_reward);
    }


    public void AddLinePoint(Vector2 point)
    {
        var pointlist = new List<Vector2>(LineRenderer.Points);
        pointlist.Add(point);
        LineRenderer.Points = pointlist.ToArray();
    }

    public void ClearLinePoints()
    {
        LineRenderer.Points = new Vector2[0];
    }


    public void AddToHistorybar(float crashAt,bool isClaim)
    {
        GameObject gm = Instantiate(historyPrefab, historyContent);
        gm.GetComponentInChildren<TextMeshProUGUI>().text = crashAt.ToString("F2");
        if (crashAt > 2.0f)
        {
            gm.GetComponent<Image>().color = Color.green;
            gm.transform.Find("red").gameObject.SetActive(true);
        }
        else
        {
            //gm.GetComponent<Image>().color = Color.black;
            gm.transform.Find("green").gameObject.SetActive(true);
        }
        
        gm.SetActive(true);
        GridLayoutGroup g = historyContent.GetComponent<GridLayoutGroup>();
        historyContent.DOLocalMoveX(-((historyContent.childCount + 5) * (g.cellSize.x + g.spacing.x) + g.padding.left + g.padding.right), 0f);
        //historyScrollrect.ScrollToBottom();
    }
    ///=============================Flee Condition UI Logic==============================================================================================
    public void Flee_Increment()
    {
        fleeConditionValue += fleeValueIncrement;
        Debug.Log(fleeConditionValue);
        Flee_UpdateUI();
    }
    public void Flee_Decrement()
    {
        fleeConditionValue -= fleeValueIncrement;
        if (fleeConditionValue < 1.01f)
        {
            fleeConditionValue = 0f;
        }
        Flee_UpdateUI();
    }

    public void FleeKeyboardInput(string _input)
    {

        switch (_input)
        {
            case "back":
                if (fleeConditionValueText.text.Length > 0)
                {
                    fleeConditionValueText.text = fleeConditionValueText.text.Substring(0, fleeConditionValueText.text.Length - 1);
                }
                break;
            case "done":
                fleeConditionValue = 0f;
                if (fleeConditionValueText.text.Trim() != "" && fleeConditionValueText.text.Trim() != "." && fleeConditionValueText.text.Trim() != "-")
                {
                    fleeConditionValue = float.Parse(fleeConditionValueText.text);
                    if(fleeConditionValue < 1.01f)
                    {
                        fleeConditionValueText.text = "0.00";
                        MakeToast("Number must be set between 1.01 and 5.8");
                    }
                }
                break;
            case ".":

                if (fleeConditionValueText.text.Length >= 5)
                    return;

                fleeConditionValueText.text = fleeConditionValueText.text.Replace("-", "");
                if (!fleeConditionValueText.text.Contains('.'))
                {
                    fleeConditionValueText.text += _input;
                }
                break;
            default:
                if (fleeConditionValueText.text.Length >= 5)
                    return;

                fleeConditionValueText.text = fleeConditionValueText.text.Replace("-", "");
                fleeConditionValueText.text += _input;
                break;
        }
    }

    private void Flee_UpdateUI()
    {
        if (fleeConditionValue < 1.01f)
        {
            fleeConditionValueText.text = "-";
        }
        else
        {
            fleeConditionValueText.text = fleeConditionValue.ToString("F2").Replace(".00", "");
        }
    }


    ///=============================End Flee Condition Logic=============================================================================================

    ///==============================Profit Loss Condition Logic===================================================================================================================
    public void OnChangeProfitCondition()
    {
        profitPercent = (int)profitSlider.value;
        UpdateProfitLossPercentUI();
        OnSetCondition();
    }
    public void OnChangeLossCondition()
    {
        lossPercent = (int)lossSlider.value;
        UpdateProfitLossPercentUI();
        OnSetCondition();
    }

    public void UpProfitCondition()
    {
        if (profitPercent < 100)
        {
            profitPercent++;
        }
        profitSlider.value = profitPercent;
        UpdateProfitLossPercentUI();
        OnSetCondition();
    }
    public void DownProfitCondition()
    {
        if (profitPercent > 0)
        {
            profitPercent--;
        }
        profitSlider.value = profitPercent;
        UpdateProfitLossPercentUI();
        OnSetCondition();
    }
    public void UpLossCondition()
    {
        if (lossPercent < 100)
        {
            lossPercent++;
        }
        lossSlider.value = lossPercent;
        UpdateProfitLossPercentUI();
        OnSetCondition();
    }
    public void DownLossCondition()
    {
        if (lossPercent > 0)
        {
            lossPercent--;
        }
        lossSlider.value = lossPercent;
        UpdateProfitLossPercentUI();
        OnSetCondition();
    }

    public void OnSetCondition()
    {
        if (lossPercent > 0f)
            setMaxLoss = currencyValue - ((currencyValue * lossPercent) / 100f);
        else
            setMaxLoss = 0f;

        if (profitPercent > 0f)
            setMaxProfit = (currencyValue * (profitPercent + 100f)) / 100f;
        else
            setMaxProfit = 0f;

        Debug.Log("Loos:" + setMaxLoss + " Profit:" + setMaxProfit);
    }

    private void UpdateProfitLossPercentUI()
    {
        ProfitProgressText.text = string.Format("{0}%", profitPercent);
        onProfitConditionText.text = string.Format("{0}%", profitPercent);

        LossProgressText.text = string.Format("{0}%", lossPercent);
        onLossConditionText.text = string.Format("{0}%", lossPercent);
    }

    ///==============================EndProfit Loss Condition Logic====================================================================================================================

    ///==============================Game Mode=====================================================================================================================
    public GameObject sparkleAnimGuess, sparkleAnimFlee;
    public void GameMode(bool isAuto)   /// Guess button
    {
       
        if (isAuto)
        {           
            isManualMode = false;
            for (int i = 0; i < disableObjectsInAutoMode.Length; i++)
            {
                disableObjectsInAutoMode[i].SetActive(false);
            }
            for (int i = 0; i < disableObjectInManualMode.Length; i++)
            {
                disableObjectInManualMode[i].SetActive(true);
            }

            if (isGuess)
                GuessButtonClick();
        }
        else
        {
            isManualMode = true;           
            for (int i = 0; i < disableObjectInManualMode.Length; i++)
            {
                disableObjectInManualMode[i].SetActive(false);
            }
            for (int i = 0; i < disableObjectsInAutoMode.Length; i++)
            {
                disableObjectsInAutoMode[i].SetActive(true);
            }

            if (isClickToStart)
            {
                ClickToStartStopButtonClick();
            }

        }
        UpdateGameModeButtons();
    }
    private void UpdateGameModeButtons()
    {
        if (isManualMode)
        {
            autoModeButton.image.sprite = inactiveMode;
            manualModeButton.image.sprite = activeMode;
        }
        else
        {
            autoModeButton.image.sprite = activeMode;
            manualModeButton.image.sprite = inactiveMode;
        }
    }
    ///==============================End Game Mode=================================================================================================================
    ///==============================Cash Out Logic================================================================================================================
    public void CashOutButtonClick()
    {
        Crash_SoundManager.Inst.PlaySFX(3); // cashout sound
        Debug.Log("CashOutButtonClick");      
        //CashOut((Time.time - (float)startTime));        
        Debug.Log("Plane position time: "+ (float)(currentSecond - startTime));
        CashOut((float)(currentSecond - startTime));
    }
    public void CashOut(float time)
    {
        isCashOut = true;
        cashOutButton.SetActive(false);
        Crash_Manager.Inst.CRASH_CASH_OUT(time);
    }
    public float getFleeConditionTime(float fleeConditionValue)
    {
        return ((fleeConditionValue - 1f) / 0.00008f) / 1000f;
    }
    public void CalculateWinnings(bool isGameCrash = false,float fleeConditionValue=0f)
    {
        if (isGameCrash)
        {
            ShowLossAmount(betAmount);
        }
        cashOutButton.SetActive(false);
    }
   
    public void ShowWinningAmount(float total_win_amount)
    {  
        cashOutButton.SetActive(false);
        winningsText.alpha = 0f;
        winningsText.color = Color.white;
        winningsText.text = string.Format("+{0:F2}", total_win_amount);
        winningsText.transform.localPosition = Vector3.zero;
        Sequence s = DOTween.Sequence();
        s.Append(winningsText.transform.DOLocalMoveY(10, 1f));
        s.Join(winningsText.DOFade(1f, 0.5f));
        s.Append(winningsText.DOFade(0f, 0.5f));
        coinFlowParticle.Play();
    }
    public void ShowLossAmount(float amount)
    {      
        cashOutButton.SetActive(false);
        winningsText.color = Color.red;
        winningsText.text = string.Format("-{0:F2}", amount);
        winningsText.alpha = 0f;
        winningsText.transform.localPosition = Vector3.zero;
        Sequence s = DOTween.Sequence();
        s.Append(winningsText.transform.DOLocalMoveY(-10, 1f));
        s.Join(winningsText.DOFade(1f, 0.5f));
        s.Append(winningsText.DOFade(0f, 0.5f));
    }
    bool isOtherTextDestroyed = true;
    public void ShowOtherPlayerCashout(string name, float amount)
    {
        if (!isFlying)
            return;

        //Debug.Log("name: "+name +" amount  "+amount);
        //Debug.Log(string.Format("{0}_{1:F2}x", name, amount));

        if (!isOtherTextDestroyed)        
            return;
        
        isOtherTextDestroyed = false;
        Invoke(nameof(enableOtherText), 0.5f);
        GameObject otherText = Instantiate(otherUserCashoutText.gameObject,rocket);
        otherText.gameObject.SetActive(true);
        otherText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}_{1:F2}x", name,amount);
        otherText.GetComponent<TextMeshProUGUI>().alpha = 0f;
        otherText.transform.localPosition = Vector3.zero;
        otherText.transform.SetParent(rocket.parent);
        OtherPlayerCashoutAnimation(otherText.GetComponent<TextMeshProUGUI>());
    }

    private void enableOtherText()
    {
        isOtherTextDestroyed = true;
    }

    public void OtherPlayerCashoutAnimation(TextMeshProUGUI otherText)
    {
        Sequence s = DOTween.Sequence();
        Vector3 pos = otherText.transform.position;
        pos.y -= 2f;
        pos.x -= 1f;
        otherText.transform.rotation = Quaternion.identity;
        s.Append(otherText.transform.DOMove(pos, 1f).SetEase(Ease.InOutSine));
        s.Join(otherText.DOFade(1f, 0.5f));
        s.Append(otherText.DOFade(0f, 0.5f));
        s.OnComplete(() =>
        {            
            Destroy(otherText.gameObject);
        });
    }
    public void CheckIsBettings()
    {
        /*if (currencyValue < 10 && !isManualMode)
        {
            isBetting = false;
            return;
        }*/

        if (betAmount <= 0)
        {
            isBetting = false;
            return;
        }

        if (isManualMode)
        {
            if (isGuess == false)
            {
                isBetting = false;
                return;
            }
        }
        else // Auto mode
        {
            if (isClickToStart == false)
            {
                isBetting = false;
                return;
            }
          
            /*  if (profitPercent > 0f && currencyValue >= setMaxProfit)
              {
                  isBetting = false;
                  MakeToast("Stop On Profit Condition");
                  OnClearButtonClick(false);
                  return;
              }
              if (lossPercent > 0f && (currencyValue-betAmount) <= setMaxLoss)
              {
                  isBetting = false;
                  MakeToast("Stop On Loss Condition");
                  OnClearButtonClick(false);
                  return;
              }*/
        }
    }
    public void StopAutoModeWhenConditionCompleted()
    {
        if (!isManualMode) // when auto mode active
        {            
            isBetting = false;
            MakeToast("Stop On Profit/Stop Condition");
            OnClearButtonClick(false);            
        }
    }
    ///==============================End Cash Out Logic============================================================================================================

    ///==============================Manual Mode Logic================================================================================================================

    public void GuessButtonClick(bool checkForBet=true)
    {
        if (betAmount <= 0f && checkForBet) {
            MakeToast("Please Increase Bet Amount");
            return;
        }
        Debug.Log("GuessButtonClick...");
        sparkleAnimGuess.SetActive(false);
        Crash_UI_Manager.Inst.BlockUIFull.SetActive(true);

        isGuess = true;
        guessButtonImage.sprite = guessPressed;     
       
    }
    ///==============================End Manual Mode Logic============================================================================================================
    ///==============================Auto Mode Logic================================================================================================================

    public void ClickToStartStopButtonClick() 
    {
        if(betAmount <= 0f && isClickToStart)
        {           
            isClickToStart = false;
            betAmount = 0;
            betAmountText.text = betAmount.ToString();
            Crash_UI_Manager.Inst.BlockUIAutoMode.SetActive(false);
            clickToStartText.text = "Click To Start";
            return;
        }
        if (betAmount <= 0f)
        {
            MakeToast("Please Increase Bet Amount");
            return;
        }

        isClickToStart = !isClickToStart; // first time button upar click karso etle isClickToStart = true thae jase ane else vari condition ma jase.
        Debug.Log("isClickToStart: "+ isClickToStart);
        if (!isClickToStart) // isClickToStart = false hoe means auto mode chalu karvano baki chhe
        {
            betAmount = 0;
            betAmountText.text = betAmount.ToString();
            Crash_UI_Manager.Inst.BlockUIAutoMode.SetActive(false);
            clickToStartText.text = "Click To Start";
        }
        else
        {
            isManualMode = false;
            Crash_UI_Manager.Inst.BlockUIFull.SetActive(false);
            Crash_UI_Manager.Inst.BlockUIAutoMode.SetActive(true);
            OnSetCondition();
            clickToStartText.text = "Click To Stop";
        }
    }
    ///==============================End Auto Mode Logic============================================================================================================

    ///==============================Clear Button Logic================================================================================================================

    public void OnClearButtonClick(bool checkForBet = true)
    {     
        betAmount = 0;
        betAmountText.text = "0";

        if (isClickToStart)
        {
            ClickToStartStopButtonClick();
        }

        if (isGuess)
            GuessButtonClick(checkForBet);
    }
    ///==============================End Clear Button Logic============================================================================================================

    ///==============================Place Button Logic================================================================================================================

    public void OnPlaceButtonClick(int amount)
    {
        Debug.Log("Place Bet: " + amount);

        /* betAmount += amount;  // Old logic jyare bet button upar click karta hta tyare
         if (currencyValue > betAmount)
         {
             betAmountText.text = betAmount.ToString();
             sparkleAnimGuess.SetActive(true);
             sparkleAnimFlee.SetActive(false);
         }
         else
         {
             betAmount -= amount;
             MakeToast("Insufficient balance.");
         }*/

        
        if (currencyValue >= amount)
        {
            if (isManualMode)
            {
                currencyValue -= amount;
            }
            betAmount += amount;
            betAmountText.text = betAmount.ToString();
            sparkleAnimGuess.SetActive(true);
            sparkleAnimFlee.SetActive(false);
            Debug.Log("Bet Amount: "+betAmount);           
        }       
        else
        {           
            MakeToast("Insufficient balance.");
        }
    }
    public void SetChips(int amount)
    {
        betAmount += amount;
        betAmountText.text = betAmount.ToString();
    }
    ///==============================End Place Button Logic============================================================================================================

    ///===============================Toast Logic=========================================================================================================================
    public void MakeToast(string msg)
    {
        toastRect.gameObject.SetActive(false);
        toastCanvasGroup.alpha = 0f;
        toastText.text = msg;
        toastRect.gameObject.SetActive(true);
        Sequence toast = DOTween.Sequence();
        toast.Append(toastCanvasGroup.DOFade(1f, 0.5f));
        toast.Join(toastCanvasGroup.DOFade(0f, 0.5f).SetDelay(2f));
        toast.OnUpdate(() =>
        {
            toastRect.sizeDelta = new Vector2(toastText.rectTransform.sizeDelta.x+20f, toastRect.sizeDelta.y);
        })
        .OnComplete(() =>
        {
            toastRect.gameObject.SetActive(false);
        });
    }

    ///===============================End Toast Logic=========================================================================================================================

}

public static class ScrollRectExtensions
{
    public static void ScrollToTop(this ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 1);
    }
    public static void ScrollToBottom(this ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }
}

