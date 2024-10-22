using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Upi_Selection : MonoBehaviour
{
    public static Shop_Upi_Selection Inst;
    [SerializeField] GameObject Selected_Glow;
    [SerializeField] Text Txt_Upi_Header;
    public string MyBetSelected;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        Txt_Upi_Header = this.transform.GetChild(0).GetComponent<Text>();
        Selected_Glow = this.transform.GetChild(1).gameObject;
        MyBetSelected = this.name;
        if (MyBetSelected.Equals("BTN_UPI-11"))
            Selected_Glow.SetActive(true);
        else
            Selected_Glow.SetActive(false);

        this.GetComponent<Button>().onClick.AddListener(ON_Box_Click);
    }
    public void ON_Box_Click()
    {
        Comen_Event_Setup.Selected_Shop_UPI(MyBetSelected);
    }

    private void OnEnable()
    {
        Comen_Event_Setup._Shop_UPI += IM_SELECTED;
    }

    private void OnDisable()
    {
        Comen_Event_Setup._Shop_UPI -= IM_NOT_SELECTED;
    }

    public void IM_SELECTED(string name)
    {
        if (name.Equals(MyBetSelected))
        {
            Selected_Glow.SetActive(true);
        }
        else
        {
            Selected_Glow.SetActive(false);
        }
    }

    public void IM_NOT_SELECTED(string action)
    {
        Selected_Glow.SetActive(false);
    }
}
