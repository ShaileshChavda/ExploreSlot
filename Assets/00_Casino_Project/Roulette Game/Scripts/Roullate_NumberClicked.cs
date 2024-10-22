using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roullate_NumberClicked : MonoBehaviour
{
    public static Roullate_NumberClicked Inst;
    public string MyBetSelected;
    [SerializeField] GameObject Glow;
    public bool HaveChips;
    [SerializeField] Animator Glow_Anim;
    bool beted;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
        HaveChips = false;
        MyBetSelected = this.name;
        Glow = this.transform.GetChild(0).gameObject;
        Glow.transform.localScale = Vector3.zero;
        Glow_Anim = Glow.transform.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Roullate_EventSetup._Roullate_BetSelect_SEND += IM_SELECTED;
        Roullate_EventSetup._RESET_ALL_GLOW += RESET_GLOW;
    }

    private void OnDisable()
    {
        Roullate_EventSetup._Roullate_BetSelect_SEND -= IM_NOT_SELECTED;
        Roullate_EventSetup._RESET_ALL_GLOW -= RESET_GLOW;
    }

    public void IM_SELECTED(string name)
    {
        if (name.Equals(MyBetSelected))
        {
            if (!beted && Roullate_Manager.Inst.Total_Bet_Pos_Count<16)
            {
                Roullate_Manager.Inst.Total_Bet_Pos_Count++;
                beted = true;
            }
            if (beted)
            {
                Glow.transform.localScale = Vector3.one;
                string lastCharacters = name.Substring(name.Length - 2);
                if (lastCharacters.Equals("_b"))
                    Roullate_Manager.Inst.USER_SEND_BET(name.Remove(name.Length - 2, 2));
                else
                    Roullate_Manager.Inst.USER_SEND_BET(name);
            }
            else
            {
                if (Roullate_UI_Manager.Inst.Wait_For_NewRound.transform.localScale.x <= 0)
                    Alert_MSG.Inst.MSG("Players cannot bet more then the maximum betting area");
            }
        }
        else
        {
            Glow.transform.localScale = Vector3.zero;
        }
    }

    public void IM_NOT_SELECTED(string action)
    {
        Glow.transform.localScale = Vector3.zero;
    }

    public void RESET_GLOW()
    {
        beted = false;
        Glow.transform.localScale = Vector3.zero;
    }

    public void PLAY_GLOW_ANIM()
    {
        Glow_Anim.Play("Glow_HightLight_Anim", 0);
    }
}
