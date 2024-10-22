using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mines_PFB_Scratch_Box : MonoBehaviour
{
    public static Mines_PFB_Scratch_Box Inst;
    public Image MyBox_IMG;
    public GameObject Diamond_OBJ, Bomb_OBJ, Brack_OBJ;
    public bool Active;
    public string BoxName;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Reset_BOX()
    {
        MyBox_IMG.sprite = Mines_Manager.Inst.Scratch_Box_Default_SP;
        Diamond_OBJ.SetActive(false);
        Bomb_OBJ.SetActive(false);
        Brack_OBJ.SetActive(false);
        Active = false;
        BoxName = "";
    }

    public void Open_All_Scratch(string card, bool white)
    {
        Active = true;
        Brack_OBJ.SetActive(true);
        MyBox_IMG.sprite = Mines_Manager.Inst.Scratch_Box_Crack_SP;

        if (card.Equals("daimond"))
        {
            BoxName = "D";
            Diamond_OBJ.SetActive(true);
            if (white)
            {
                Diamond_OBJ.GetComponent<Image>().color = Color.white;
                Diamond_OBJ.GetComponent<Animator>().enabled = true;
            }
            else
            {
                Diamond_OBJ.GetComponent<Image>().color = Color.gray;
                Diamond_OBJ.GetComponent<Animator>().enabled = false;
                Diamond_OBJ.GetComponent<Image>().sprite = Mines_Manager.Inst.Default_Diamond_SP;
            }
        }
        else if (card.Equals("bomb"))
        {
            BoxName = "B";
            Bomb_OBJ.SetActive(true);

            if (white)
                Bomb_OBJ.GetComponent<Image>().color = Color.white;
            else
                Bomb_OBJ.GetComponent<Image>().color = Color.gray;
        }
    }

    public void Diamond_Scratch()
    {
        Active = true;
        Brack_OBJ.SetActive(true);
        MyBox_IMG.sprite = Mines_Manager.Inst.Scratch_Box_Crack_SP;
        Mines_SoundManager.Inst.PlaySFX(2);
        BoxName = "D";
        Diamond_OBJ.SetActive(true);
        Diamond_OBJ.GetComponent<Image>().color = Color.white;
        Diamond_OBJ.GetComponent<Animator>().enabled = true;
    }

    public void Bomb_Scratch(JSONObject data)
    {
        Active = true;
        Brack_OBJ.SetActive(true);
        MyBox_IMG.sprite = Mines_Manager.Inst.Scratch_Box_Crack_SP;

        Mines_Manager.Inst.GameState = "stop_game";
        Mines_SoundManager.Inst.PlaySFX(1);
        BoxName = "B";
        Bomb_OBJ.SetActive(true);
        Bomb_OBJ.GetComponent<Image>().color = Color.white;
        Mines_UI_Manager.Inst.BadLuck_SC.SetActive(true);
        Mines_Manager.Inst.Last_All_Card_Open(data, "bomb");
        Mines_UI_Manager.Inst.Txt_Claim_Amount.text = "0";
        Mines_Manager.Inst.TRS_GLOW_RESET();
    }

    public void Minus_1_Scratch(string card, JSONObject data)
    {
        Active = true;
        Brack_OBJ.SetActive(true);
        MyBox_IMG.sprite = Mines_Manager.Inst.Scratch_Box_Crack_SP;
        Mines_Manager.Inst.GameState = "stop_game";

        if (card.Equals("daimond"))
        {
            Mines_SoundManager.Inst.PlaySFX(2);
            BoxName = "D";
            Diamond_OBJ.SetActive(true);
            Diamond_OBJ.GetComponent<Image>().color = Color.white;
            Diamond_OBJ.GetComponent<Animator>().enabled = true;

        }
        else if (card.Equals("bomb"))
        {
            Mines_SoundManager.Inst.PlaySFX(1);
            Mines_UI_Manager.Inst.BadLuck_SC.SetActive(true);
            BoxName = "B";
            Bomb_OBJ.SetActive(true);
            Bomb_OBJ.GetComponent<Image>().color = Color.white;
        }
        Mines_Manager.Inst.Last_All_Card_Open(data, card);
       // Mines_Manager.Inst.TRS_GLOW_RESET();
    }

}
