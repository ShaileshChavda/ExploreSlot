using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class FishUIManager : MonoBehaviour
{
    public static FishUIManager Instance;
    [SerializeField] private bool isBotStatus;
    public float fltPlayerOnTimer;
    public PlayerManager[] goPlayerAry;
    [HideInInspector]
    public PlayerSwapnClass[] _playerSwapnClasse;
    public Transform trCoinParant;

    public int[] intBetAry;

    public Image imgAttack;
    public Sprite[] sptAttackAry;

    public Image imgFocus;
    public Sprite[] sptFocusAry;

    public Image imgAcce;
    public Sprite[] sptAcceAry;

    private PlayerManager _player;

    public GameObject goSettingPopUp;
    public GameObject goFishSpecPopUp;

    public float countdownTime = 60f;
    
    [SerializeField]
    private float timer;


    [SerializeField]
    private FishGameInfo _fishGameInfo;

    [SerializeField]
    private FishKillInfo _fishKillInfo;

    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.FISH_GAME_INFO("6633724c781e640bd305b61c"));
    }
    private void OnEnable()
    {
        _player = goPlayerAry[0];
        timer = countdownTime;
    }
    void Start()
    {
        if(isBotStatus)
        StartCoroutine(IEPlayerOn());
    }

    public void SEND_FISH_KILL_EVENT(int fishID,int fKey,string pKey)
    {
        Debug.Log("SEDN FISH ID: "+fishID);
        SocketHandler.Inst.SendData(
            SocketEventManager.Inst.FISH_GAME_KILL
            (_player.CurrBetVal.ToString(), fishID, fKey, pKey));
    }

    public void FISH_KILL_RESPONSE(JSONObject data)
    {
        Debug.Log("RESPONSE FISH-KILL : " + data);
        _fishKillInfo = JsonUtility.FromJson<FishKillInfo>(data.ToString());

        if(_fishKillInfo.kill)
        {
            bool isKillFish = false;

            int pIndex = 0;
            for (int i = 0; i < goPlayerAry.Length; i++)
            {
                if(goPlayerAry[i].PlayerID.ToString().Equals(_fishKillInfo.player_key))
                {
                    pIndex = i;
                }
            }

            FishControl fc = null;

            for (int i = 0; i < _fcList.Count; i++)
            {
                if(_fcList[i].ID == _fishKillInfo.fish_key)
                {
                    isKillFish = true;
                    fc = _fcList[i];
                }
            }

            Debug.Log("<color=yellow> FISH KILL:</color> "+ isKillFish);

            if(isKillFish)
            fc.Die(pIndex);
        }
    }

    public void JOIN_OLD_GAME(JSONObject data)
    {
        UnityEngine.Debug.Log("REJOIN DATA: " + GS.Inst.Rejoin+ "|DATA|"+ data);
        // Parse JSON using JsonUtility
        _fishGameInfo = JsonUtility.FromJson<FishGameInfo>(data.ToString());
        Array.Clear(intBetAry, 0, 0);
        UnityEngine.Debug.Log("JOIN OLD GAME |TOTAL CREDIT|" + _fishGameInfo.total_wallet);
        _player.UpdateScore((float)_fishGameInfo.total_wallet);
        _player.GetUserDetails(_fishGameInfo.user_info._id, _fishGameInfo.user_info.user_name, _fishGameInfo.user_info.profile_url);
        intBetAry = _fishGameInfo.bet_info.bet_slots.ToArray();
    }

    IEnumerator IEPlayerOn()            
    {
        for (int i = 0; i < goPlayerAry.Length; i++)
        {
            goPlayerAry[i].gameObject.SetActive(true);
            goPlayerAry[i].GunManager.trCoinUI.gameObject.SetActive(true);
            yield return new WaitForSeconds(fltPlayerOnTimer);
        }
    }

    public GameObject goAutoLeavePopup;
    public TextMeshProUGUI txtTimer;

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    SetCoinPrefab();
        //}

        timer -= Time.deltaTime;

        if (timer <= 30f)
        {
            goAutoLeavePopup.gameObject.SetActive(true);
            int val = Mathf.RoundToInt(timer);
            txtTimer.text = val.ToString();

            if (val == 0f)
            {
                Debug.Log("GameQuit");
                Application.Quit();
            }
        }

        // Check for player actions here and reset the timer if any action is taken
        if (HasPlayerAction())
        {
            ResetTimer();
        }
    }

    // Check for player actions (e.g., input from keyboard, mouse clicks)
    bool HasPlayerAction()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0)) // Change conditions as needed
        {
            return true;
        }
        return false;
    }

    // Reset the timer when an action is taken
    public void ResetTimer()
    {
        goAutoLeavePopup.SetActive(false);
        timer = countdownTime;
    }

    void ResizeCamera()
    {

    }

    void SetCoinPrefab()
    {
        GameObject goCoin = Instantiate(Resources.Load("coinEff"), Vector3.zero, Quaternion.identity) as GameObject;
        goCoin.transform.SetParent(trCoinParant);
        Transform trCoinUI = _player.GunManager.trCoinUI.GetChild(0).transform;

        coinEffControl coinEff = goCoin.GetComponent<coinEffControl>();
        //StartCoroutine(coinEff.EmMoveToTarget(trCoinUI));
        coinEff.MoveToTarget(trCoinUI);
    }

    public int ChooseBet(int betVal)
    {
        for (int i = 0; i <intBetAry.Length; i++)
        {
            if(betVal == intBetAry[i])
            {

            }
        }

        return betVal;
    }

    public int ChooseBetIndex(int index)
    {
        int betVal= intBetAry[index];
        return betVal;
    }

    public void BtnAttack()
    {
        _player.GunManager.OnAttack();
        SetAttackSprite();
    }

    public void BtnAutoFoucs()
    {
        _player.GunManager.OnAutoFocus();
        SetFocusSprite();
    }

    public void BtnAcceration()
    {
        _player.GunManager.OnAccelaration();
        SetAcceSprite();
    }

    public void SetAttackSprite()
    {
        if(_player.GunManager.isAttack)
        {
            imgAttack.sprite = sptAttackAry[1];
        }
        else
        {
            imgAttack.sprite = sptAttackAry[0];
        }
    }

    public void SetFocusSprite()
    {
        if (_player.GunManager.isAutoFocus)
        {
            imgFocus.sprite = sptFocusAry[1];
        }
        else
        {
            imgFocus.sprite = sptFocusAry[0];
        }
    }

    public void SetAcceSprite()
    {
        if (_player.GunManager.intAccelaration == 1)
        {
            imgAcce.sprite = sptAcceAry[0];
        }
        else if (_player.GunManager.intAccelaration == 2)
        {
            imgAcce.sprite = sptAcceAry[1];
        }
        else
        {
            imgAcce.sprite = sptAcceAry[2];
        }
    }

    public Transform trGoGameSetting;
    public Transform trBackBtn;
    public Transform trSettingBtn;
    public Transform trFishSpeBtn;
    public void onClickGameSetting()
    {
        if(trGoGameSetting.eulerAngles.Equals(new Vector3(0, 0, 180)))
        {
            trGoGameSetting.eulerAngles = new Vector3(0, 0, 0);
            trFishSpeBtn.DOLocalMove(new Vector3(-350, 0, 0), fltBtnMoveTime).SetEase(Ease.Linear);
            trSettingBtn.DOLocalMove(new Vector3(-350, 0, 0), fltBtnMoveTime).SetEase(Ease.Linear);
            trBackBtn.DOLocalMove(new Vector3(-350, 0, 0), fltBtnMoveTime).SetEase(Ease.Linear);
        }
        else
        {
            trGoGameSetting.eulerAngles = new Vector3(0, 0, 180);
            trFishSpeBtn.DOLocalMove(new Vector3(275, 200, 0), fltBtnMoveTime).SetEase(Ease.Linear);
            trSettingBtn.DOLocalMove(new Vector3(475, 0, 0), fltBtnMoveTime).SetEase(Ease.Linear);
            trBackBtn.DOLocalMove(new Vector3(275,-200, 0), fltBtnMoveTime).SetEase(Ease.Linear);
        }
    }

    public float fltBtnMoveTime;
    public void onClickSetting()
    {
        goSettingPopUp.SetActive(true);
    }

    public void onClickFishSpec()
    {
        goFishSpecPopUp.SetActive(true);
    }

    public void onClickHome()
    {
        Debug.Log("HOME");
        Application.Quit();
    }

    public List<FishControl> _fcList;
}

[System.Serializable]
public class PlayerSwapnClass
{
    public bool isActive = false;
    public Transform trPlayerPos;
    public Transform trPlayer;
}
