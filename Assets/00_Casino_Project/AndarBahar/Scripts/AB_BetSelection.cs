using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AB_BetSelection : MonoBehaviour
{
    public static AB_BetSelection Inst;
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
            AB_Manager.Inst.Selected_Bet_Amount = MyValue;
            AB_Manager.Inst.Selected_Bet_Ring.transform.position = this.transform.position;
        }
        else
        {
            this.GetComponent<Image>().color = Color.gray;
        }
    }
    public void ON_Box_Click()
    {
        AB_SoundManager.Inst.PlaySFX(0);
        AB_EventSetup.SelectedBET_DT(MyBetSelected);
    }

    private void OnEnable()
    {
        AB_EventSetup._AB_BetSelect += IM_SELECTED;
    }

    private void OnDisable()
    {
        AB_EventSetup._AB_BetSelect -= IM_NOT_SELECTED;
    }

    public void IM_SELECTED(string name)
    {
        if (name.Equals(MyBetSelected))
        {
            AB_SoundManager.Inst.PlaySFX(0);
            Selected = true;
            this.GetComponent<Image>().color = Color.white;
            AB_Manager.Inst.Selected_Bet_Amount = MyValue;
            AB_Manager.Inst.Selected_Bet_Ring.transform.position = this.transform.position;
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

}
