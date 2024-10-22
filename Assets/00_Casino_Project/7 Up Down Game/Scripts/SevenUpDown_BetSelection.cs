using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SevenUpDown_BetSelection : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{
    public static SevenUpDown_BetSelection Inst;
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
            SevenUpDown_Manager.Inst.Selected_Bet_Amount = MyValue;
            SevenUpDown_Manager.Inst.Selected_Bet_Ring.transform.position = this.transform.position;
            SevenUpDown_Manager.Inst.Selected_Bet_Tick.transform.position = this.transform.position;
        }
        else
        {
            this.GetComponent<Image>().color = Color.gray;
        }
    }
    public void ON_Box_Click()
    {
        SevenUpDown_SoundManager.Inst.PlaySFX(0);
        SevenUpDown_EventSetup.SelectedBET_DT(MyBetSelected);
    }

    private void OnEnable()
    {
        SevenUpDown_EventSetup._DT_BetSelect += IM_SELECTED;
    }

    private void OnDisable()
    {
        SevenUpDown_EventSetup._DT_BetSelect -= IM_NOT_SELECTED;
    }

    public void IM_SELECTED(string name)
    {
        if (name.Equals(MyBetSelected))
        {
            SevenUpDown_SoundManager.Inst.PlaySFX(0);
            Selected = true;
            this.GetComponent<Image>().color = Color.white;
            SevenUpDown_Manager.Inst.Selected_Bet_Amount = MyValue;
            SevenUpDown_Manager.Inst.Selected_Bet_Ring.transform.position = this.transform.position;
            SevenUpDown_Manager.Inst.Selected_Bet_Tick.transform.position = this.transform.position;
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
