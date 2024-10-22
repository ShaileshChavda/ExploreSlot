using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mines_UI_Manager : MonoBehaviour
{
    public static Mines_UI_Manager Inst;
    //--------Player Data----------------
    public Text Txt_GameID,Txt_Player_Name,Txt_Player_Chips;

    //--------TreasurBox Data----------------
    public Text Txt_X_Treasure,Txt_Mines; 
    
    //--------BET Data----------------
    public Text Txt_BET;
    
    //--------NEXT WINNING AMOUNT Data----------------
    public Text Txt_Next_Win_Amount;
    
    //--------Claim Data----------------
    public Text Txt_Claim_Amount;
    
    //--------Win amount----------------
    public TextMeshProUGUI Txt_Win_screen_Amount, Txt_Win_Box_Amount;

    //--------- screen ----------
    public GameObject BadLuck_SC;

    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        //Txt_Claim_Amount.text = "5000";
        //StartCoroutine(Next_Win_Amount_Update());
    }

    public void SET_SCREEN_DETAILS()
    {
        Txt_Player_Name.text = GS.Inst._userData.Name;
        Txt_Player_Chips.text = GS.Inst._userData.Chips.ToString("n2");
        //Txt_GameID.text = data.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry);
        //Txt_Player_Name.text = data.GetField("user_info").GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        //Txt_Player_Chips.text = data.GetField("user_info").GetField("wallet").ToString().Trim(Config.Inst.trim_char_arry);
    }

    public void BET_CHIPS_UPDATE()
    {
        Txt_Player_Chips.text = (float.Parse(Txt_Player_Chips.text) - Mines_Manager.Inst.Selected_Bet_Amount).ToString("n2");
    }

    public IEnumerator Claim_Amount_Update(float targetAmount)
    {
        float targetValue = float.Parse(Txt_Claim_Amount.text) + (targetAmount-float.Parse(Txt_Claim_Amount.text));
        float currentValue = float.Parse(Txt_Claim_Amount.text);
        var rate = Mathf.Abs(targetValue - currentValue) / 2;
        while (currentValue != targetValue)
        {
            currentValue = Mathf.MoveTowards(currentValue, targetValue, rate * 0.1f/*Time.deltaTime*/);
            Txt_Claim_Amount.text = currentValue.ToString("n2");
            yield return null;
        }
    }

    public IEnumerator _Win_Amount_Update(float amount)
    {
        //if(Txt_Next_Win_Amount.text!="0")
           //Mines_Manager.Inst.Last_Show_Cards("C");
        float jackpot_amount = amount;
        float targetValue = amount;
        float currentValue = 1;
        var rate = Mathf.Abs(targetValue - currentValue) / 2;
        while (currentValue != targetValue)
        {
            currentValue = Mathf.MoveTowards(currentValue, targetValue, rate * Time.deltaTime);
            Txt_Win_Box_Amount.text = currentValue.ToString("n2");
            Txt_Claim_Amount.text = (jackpot_amount - currentValue).ToString("n2");
            yield return null;
        }
        Txt_Claim_Amount.text = "0";
        Txt_Next_Win_Amount.text = "0";
        Txt_Win_Box_Amount.text = "0";
        Mines_Manager.Inst.BTN_SPIN_Disable.SetActive(false);
        Mines_Manager.Inst.BTN_CLEAR_Disable.SetActive(false);
        Mines_Manager.Inst.BTN_CLAIM_Disable.SetActive(true);
        yield return new WaitForSeconds(1f);
        Txt_Win_Box_Amount.text = "";
    }

    public IEnumerator _Winning_SC_Amount_Update(float amount)
    {
        //if (Txt_Next_Win_Amount.text != "0")
            //ines_Manager.Inst.Last_Show_Cards("C");
        //float jackpot_amount = float.Parse(Txt_Claim_Amount.text);
        float targetValue = amount;
        float currentValue = 1;
        var rate = Mathf.Abs(targetValue - currentValue) / 2;
        while (currentValue != targetValue)
        {
            currentValue = Mathf.MoveTowards(currentValue, targetValue, rate * Time.deltaTime);
            Txt_Win_screen_Amount.text = currentValue.ToString("n2");
            //Txt_Claim_Amount.text = (jackpot_amount - (int)currentValue).ToString("N0");
            yield return null;
        }
        Txt_Claim_Amount.text = "0";
        Txt_Next_Win_Amount.text = "0";
        Txt_Win_Box_Amount.text = "0";
        Mines_Manager.Inst.BTN_SPIN_Disable.SetActive(false);
        Mines_Manager.Inst.BTN_CLEAR_Disable.SetActive(false);
        Mines_Manager.Inst.BTN_CLAIM_Disable.SetActive(true);
        yield return new WaitForSeconds(1f);
        Txt_Win_Box_Amount.text = "";
        Invoke("Close_Win_SC", 3f);
    }
    public void Close_Win_SC()
    {
        Mines_Winning.Inst.Reset_Win();
        Mines_Winning.Inst.transform.localScale = Vector3.zero;
    }
}
