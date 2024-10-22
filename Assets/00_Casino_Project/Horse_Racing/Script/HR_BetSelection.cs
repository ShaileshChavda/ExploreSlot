using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HR_BetSelection : MonoBehaviour
{
    public static HR_BetSelection Inst;
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
            HR_Manager.Inst.Selected_Bet_Amount = MyValue;
            HR_Manager.Inst.Selected_Bet_Ring.transform.position = this.transform.position;
            HR_Manager.Inst.Selected_Bet_Tick.transform.position = this.transform.position;
        }
        else
        {
            this.GetComponent<Image>().color = Color.gray;
        }
    }
    public void ON_Box_Click()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        HR_EventSetup.SelectedBET_DT(MyBetSelected);
    }

    private void OnEnable()
    {
        HR_EventSetup._DT_BetSelect += IM_SELECTED;
    }

    private void OnDisable()
    {
        HR_EventSetup._DT_BetSelect -= IM_NOT_SELECTED;
    }

    public void IM_SELECTED(string name)
    {
        if (name.Equals(MyBetSelected))
        {
            HR_SoundManager.Inst.PlaySFX(0);
            Selected = true;
            this.GetComponent<Image>().color = Color.white;
            HR_Manager.Inst.Selected_Bet_Amount = MyValue;
            HR_Manager.Inst.Selected_Bet_Ring.transform.position = this.transform.position;
            HR_Manager.Inst.Selected_Bet_Tick.transform.position = this.transform.position;
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
}
