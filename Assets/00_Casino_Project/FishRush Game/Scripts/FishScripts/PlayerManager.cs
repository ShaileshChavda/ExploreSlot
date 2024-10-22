using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private bool isPlayer = false;

    [SerializeField]
    private int p_Index;

    [SerializeField]
    private int p_ID;

    [SerializeField]
    private string _userID;

    [SerializeField]
    private string _userName;

    [SerializeField]
    private string _profileUrl;

    [Space]
    [Header("Player Credit")]
    [SerializeField]
    private float credit;

    [Space]
    [Header("Player Bet Value")]
    public int currBetIndex;
    public int currBetVal;

    [Space]
    [Space]
    [Header("Text Area")]
    public TextMeshProUGUI txtPlayerID;
    public TextMeshProUGUI txtPoint;
    public TextMeshProUGUI txtBetVal;
    public Image imgProfile;


    [Space]
    [Header("Here Animation")]
    public GameObject goHereAnim;


    [Space]
    [SerializeField]
    private GunManager g_Manager;


    [SerializeField] IMGLoader User_PIC;

    public float Credit
    {
        get { return credit; }
        set { credit = value; }
    }

    public int CurrBetIndex
    {
        get { return currBetIndex; }
        set { currBetIndex = value; }
    }

    public int CurrBetVal
    {
        get { return currBetVal; }
        set { currBetVal = value; }
    }
    public int PlayerIndex
    {
        get { return p_Index; }
        set { p_Index = value; }
    }

    public bool IsPlayer
    {
        get { return isPlayer; }
        set { isPlayer = value; }
    }

    public int PlayerID
    {
        get { return p_ID; }
        set { p_ID = value; }
    }

    public GunManager GunManager
    {
        get { return g_Manager; }
        set { g_Manager = value; }
    }

    private void OnEnable()
    {
        p_ID = GetInstanceID();

        if (!isPlayer)
            g_Manager.CallBotPlayerShoot();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(isPlayer)
        {
            StartCoroutine(HereTextAnim());
        }
        else
        {
           UpdateScore(0);
        }
        ChooseBetIndex(currBetVal);
    }

    public void GetUserDetails(string uId,string uName,string uUrl)
    {
        _userID = uId;
        _userName = uName;
        _profileUrl = uUrl;

        txtPlayerID.text = uName;

        User_PIC.LoadIMG(uUrl, false, false);
    }

    public void UpdateScore(float score)
    {
        credit += score;
        txtPoint.text = credit.ToString();
    }

    IEnumerator HereTextAnim()
    {
        goHereAnim.SetActive(true);
        yield return new WaitForSeconds(2f);
        goHereAnim.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void onClickBetPlusUI()
    {
        if(currBetIndex == FishUIManager.Instance.intBetAry.Length-1)
        {
            currBetIndex = 0;
        }
        else
        {
            currBetIndex++;
        }

        GetBetValueByIndex();
    }

    public void onClickBetMinsUI()
    {
        if (currBetIndex == 0)
        {
            currBetIndex = FishUIManager.Instance.intBetAry.Length - 1;
        }
        else
        {
            currBetIndex--;
        }

        GetBetValueByIndex();
    }

    public void GetBetValueByIndex()
    {
        currBetVal = FishUIManager.Instance.ChooseBetIndex(currBetIndex);
        txtBetVal.text = currBetVal.ToString("F2");
    }

    public void GetBetValueByBet(int betval)
    {
        currBetVal = FishUIManager.Instance.ChooseBet(betval);
        txtBetVal.text = currBetVal.ToString("F2");
    }

    public void ChooseBetIndex(int betVal)
    {
        for (int i = 0; i < FishUIManager.Instance.intBetAry.Length; i++)
        {
            if (betVal == FishUIManager.Instance.intBetAry[i])
            {
                currBetIndex = i;
            }
        }

        GetBetValueByIndex();
    }

    public bool CheckCreditStatus()
    {
        return credit >= currBetVal ? true : false;
    }
}   
