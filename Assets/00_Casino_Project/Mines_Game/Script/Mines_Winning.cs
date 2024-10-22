using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mines_Winning : MonoBehaviour
{
    public static Mines_Winning Inst;
    public TextMeshProUGUI Txt_Winner_Chips;
    public List<GameObject> Winner_OBJ;
    public RectTransform _sun;
    public GameObject Simple_Win_Coin;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    private void Update()
    {
        if(this.transform.localScale.x>0)
            _sun.Rotate(Vector3.forward, -0.5f);
    }
    public void SET_WIN_DATA(JSONObject data)
    {
        Mines_SoundManager.Inst.PlaySFX(3);
        Mines_Manager.Inst.Btn_Left_TRX.interactable = true;
        Mines_Manager.Inst.Btn_Right_TRX.interactable = true;
        Disable_Current_Win();
        string winType = data.GetField("win_type").ToString().Trim(Config.Inst.trim_char_arry);
        float win_amount = float.Parse(data.GetField("win_amount").ToString().Trim(Config.Inst.trim_char_arry));
        if (winType.Equals(""))
        {
            Simple_Win_Coin.SetActive(true);
            StartCoroutine(Mines_UI_Manager.Inst._Win_Amount_Update(win_amount));
        }
        else
        {
            if (winType.Equals("Big Win"))
            {
                Winner_OBJ[0].SetActive(true);
            }
            else if (winType.Equals("Super Big Win"))
            {
                Winner_OBJ[1].SetActive(true);
            }
            else if (winType.Equals("Mega Win"))
            {
                Winner_OBJ[2].SetActive(true);
            }
            else if (winType.Equals("Super Mega Win"))
            {
                Winner_OBJ[3].SetActive(true);
            }
            else if (winType.Equals("Epic Win"))
            {
                Winner_OBJ[4].SetActive(true);
            }
            GS.Inst.iTwin_Open(this.gameObject);
            StartCoroutine(Mines_UI_Manager.Inst._Winning_SC_Amount_Update(win_amount));
        }
        Mines_Manager.Inst.Last_All_Card_Open(data.GetField("card_details"), "");
        Mines_Manager.Inst.TRS_GLOW_RESET();
    }
 
    public void Reset_Win()
    {
        Simple_Win_Coin.SetActive(false);
        Disable_Current_Win();
    }
    public void Disable_Current_Win()
    {
        for (int i = 0; i < Winner_OBJ.Count; i++)
        {
            Winner_OBJ[i].SetActive(false);
        }
    }
}
