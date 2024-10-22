using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class FishManage : MonoBehaviour
{
    public enum stastus
    {
        normal,
        bonus
    }

    public GameObject Normal;
    public GameObject Bonus;

    public GameObject wave;
    public GameObject effwave;

    public stastus _stt;

    public static FishManage Instance;

    public TextMeshProUGUI txtAutoFocus;
    public TextMeshProUGUI txtAttack;
    public TextMeshProUGUI txtAccel;


    public List<Transform> _FishMange;
    public List<Transform> _CaMapManage;
    public List<Transform> _MucManager;

    public Transform trContainer;
    bool _checkTimeBonus;
    public int intCredit;

    
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Instance = this;
        _FishMange = new List<Transform>();
        _CaMapManage = new List<Transform>();
        _MucManager = new List<Transform>();
        _checkTimeBonus = false;

        //txtAttack.text = "ATTACK: " + _gun.isAttack;
        //txtAutoFocus.text = "FOCUS: " + _gun.isAutoFocus;
        //txtAccel.text = "ACCE: X" + _gun.intAccelaration;
    }
    void Update()
    {
        if (_checkTimeBonus && Bonus.activeInHierarchy)
        {
            if (_FishMange.Count == 0)
            {
                Normal.SetActive(true);
                Bonus.SetActive(false);
                _checkTimeBonus = false;
                _FishMange.Clear();
            }
        }
    }

    public void ChangeToBonus()
    {
        Normal.SetActive(false);
        Bonus.SetActive(false);

        Instantiate(wave, new Vector2(8, 0), Quaternion.identity);
        _stt = stastus.bonus;
        Invoke("activeeffwave", 0.2f);
    }
    void activeeffwave()
    {
        effwave.SetActive(true);
    }

    public void BonusTime()
    {
        Normal.SetActive(false);
        Bonus.SetActive(true);
        Invoke("encheck", 2);
    }

    void encheck()
    {
        _checkTimeBonus = true;
    }
}