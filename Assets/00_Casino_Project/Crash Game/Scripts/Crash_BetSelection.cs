using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Crash_BetSelection : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{
    public static Crash_BetSelection Inst;
    public string MyBetSelected;
    public bool Selected = false;
    [SerializeField] int MyValue;
    [SerializeField] private Button[] btn_betting;
    
    // Start is called before the first frame update
    
    public void OnPlaceBet_10()
    {
        CrashController.Instance.AutoModeBtnEnable(false);
        Crash_SoundManager.Inst.PlaySFX(2);
        DisableAllBetButton();
        btn_betting[0].transform.GetChild(0).gameObject.SetActive(true);
        CrashController.Instance.OnPlaceButtonClick(10);
    }
    public void OnPlaceBet_50()
    {
        CrashController.Instance.AutoModeBtnEnable(false);
        Crash_SoundManager.Inst.PlaySFX(2);
        DisableAllBetButton();
        btn_betting[1].transform.GetChild(0).gameObject.SetActive(true);
        CrashController.Instance.OnPlaceButtonClick(50);
    }
    public void OnPlaceBet_100()
    {
        CrashController.Instance.AutoModeBtnEnable(false);
        Crash_SoundManager.Inst.PlaySFX(2);
        DisableAllBetButton();
        btn_betting[2].transform.GetChild(0).gameObject.SetActive(true);
        CrashController.Instance.OnPlaceButtonClick(100);
    }
    public void OnPlaceBet_500()
    {
        CrashController.Instance.AutoModeBtnEnable(false);
        Crash_SoundManager.Inst.PlaySFX(2);
        DisableAllBetButton();
        btn_betting[3].transform.GetChild(0).gameObject.SetActive(true);
        CrashController.Instance.OnPlaceButtonClick(500);
    }
    public void OnPlaceBet_1000()
    {
        CrashController.Instance.AutoModeBtnEnable(false);
        Crash_SoundManager.Inst.PlaySFX(2);
        DisableAllBetButton();
        btn_betting[4].transform.GetChild(0).gameObject.SetActive(true);
        CrashController.Instance.OnPlaceButtonClick(1000);
    }
    public void OnPlaceBet_5000()
    {
        CrashController.Instance.AutoModeBtnEnable(false);
        Crash_SoundManager.Inst.PlaySFX(2);
        DisableAllBetButton();
        btn_betting[5].transform.GetChild(0).gameObject.SetActive(true);
        CrashController.Instance.OnPlaceButtonClick(5000);
    }
    public void DisableAllBetButton()
    {
        for (int i = 0; i < btn_betting.Length; i++)
        {
            btn_betting[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    void Start()
    {
        Inst = this;
        DisableAllBetButton();
        btn_betting[0].transform.GetChild(0).gameObject.SetActive(true);

        /*this.GetComponent<Button>().onClick.AddListener(ON_Box_Click);
        MyBetSelected = this.name;

        if (MyBetSelected.Equals("Coin_10"))
        {
            Selected = true;
            this.GetComponent<Image>().color = Color.white;
            Crash_Manager.Inst.Selected_Bet_Amount = MyValue;
            Crash_Manager.Inst.Selected_Bet_Ring.transform.position = this.transform.position;
            Crash_Manager.Inst.Selected_Bet_Tick.transform.position = this.transform.position;
        }
        else
        {
            this.GetComponent<Image>().color = Color.gray;
        }*/
    }
    public void ON_Box_Click()
    {
        Crash_EventSetup.SelectedBET_DT(MyBetSelected);
    }

    private void OnEnable()
    {
        Crash_EventSetup._DT_BetSelect += IM_SELECTED;
    }

    private void OnDisable()
    {
        Crash_EventSetup._DT_BetSelect -= IM_NOT_SELECTED;
    }

    public void IM_SELECTED(string name)
    {
        if (name.Equals(MyBetSelected))
        {
            Crash_SoundManager.Inst.PlaySFX(2);
            Selected = true;
            this.GetComponent<Image>().color = Color.white;
            Crash_Manager.Inst.Selected_Bet_Amount = MyValue;
            Crash_Manager.Inst.Selected_Bet_Ring.transform.position = this.transform.position;
            Crash_Manager.Inst.Selected_Bet_Tick.transform.position = this.transform.position;
            //CrashController.Instance.OnPlaceButtonClick(MyValue);
           // Crash_Manager.Inst.USER_SEND_BET(MyValue);
        }
        else
        {
            this.GetComponent<Image>().color = Color.gray;
            Selected = false;
        }
    }

    public void IM_NOT_SELECTED(string action)
    {
        //this.GetComponent<Image>().color = Color.gray;
        Selected = false;
    }

    public void IM_KILL()
    {
        Destroy(this.gameObject);
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    iTween.ScaleTo(gameObject, iTween.Hash("x", 1.2f, "y", 1.2f, "z", 1, "time", 0.2f, "easetype", iTween.EaseType.easeOutExpo));
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    if (!Selected)
    //        iTween.ScaleTo(gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
    //}
}
