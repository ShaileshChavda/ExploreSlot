using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mines_Bet_Selection : MonoBehaviour
{
    public static Mines_Bet_Selection Inst;
    public string MyBetSelected;
    public bool Selected = false;
    [SerializeField] int MyValue;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        this.GetComponent<Button>().onClick.AddListener(ON_Box_Click);
        MyBetSelected = this.name;

        if (MyBetSelected.Equals("Coin_10"))
        {
            Selected = true;
            this.GetComponent<Image>().color = Color.white;
            Mines_Manager.Inst.Selected_Bet_Amount = MyValue;
            Mines_Manager.Inst.Selected_Bet_Ring.transform.position = this.transform.position;
        }
        else
        {
            this.GetComponent<Image>().color = Color.gray;
        }
    }
    public void ON_Box_Click()
    {
        Mines_SoundManager.Inst.PlaySFX(0);
        Mines_EventSetup.SelectedBET_MS(MyBetSelected);
    }

    private void OnEnable()
    {
        Mines_EventSetup._MS_BetSelect += IM_SELECTED;
    }

    private void OnDisable()
    {
        Mines_EventSetup._MS_BetSelect -= IM_NOT_SELECTED;
    }

    public void IM_SELECTED(string name)
    {
        if (name.Equals(MyBetSelected))
        {
            Mines_SoundManager.Inst.PlaySFX(0);
            Selected = true;
            this.GetComponent<Image>().color = Color.white;
            Mines_Manager.Inst.Selected_Bet_Amount = MyValue;
            Mines_Manager.Inst.Selected_Bet_Ring.transform.position = this.transform.position;
            Mines_Manager.Inst.CLICK_BET();
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
