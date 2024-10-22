using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EXP_Big_Win : MonoBehaviour
{
    public static EXP_Big_Win Inst;
    [SerializeField] TextMeshProUGUI TxtPrice;
    [SerializeField] Image IMG_Result;
    [SerializeField] Image IMG_Big_Win_Jackpot;
    [SerializeField] Sprite SP_Big_Win, SP_Jackpot, SP_FreeSpin, SP_777;
    [SerializeField] GameObject BigWin_OBJ;
    [SerializeField] GameObject BIG_Coin_Partical_OBJ, Jackpot_Coin_Partical_OBJ, FreeSpin_Coin_Partical_OBJ, Simple_Coin_Partical_OBJ;
    public List<Slot_Big_Items> BigList_Items;
    int FreeSpincount;
    float Total_Win;
    [SerializeField] GameObject WinCoin_Target;
    [SerializeField] Slot_PFB_WinCoinMove PFB_Win_Coin;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Open_BigWin_SC(JSONObject data)
    {
        bool bigwin = bool.Parse(data.GetField("best_reward").GetField("big_winner").ToString().Trim(Config.Inst.trim_char_arry));
        bool FreeSpinwin = bool.Parse(data.GetField("best_reward").GetField("free_spin_winner").ToString().Trim(Config.Inst.trim_char_arry));
        bool JackPotwin = bool.Parse(data.GetField("best_reward").GetField("jackpot_winner").ToString().Trim(Config.Inst.trim_char_arry));
        bool Three_Seven_Win = bool.Parse(data.GetField("best_reward").GetField("three_seven_winner").ToString().Trim(Config.Inst.trim_char_arry));
        bool FreeSpin = bool.Parse(data.GetField("free_spin_animation").ToString().Trim(Config.Inst.trim_char_arry));
        Total_Win = 0f;
        if (bigwin || FreeSpinwin || JackPotwin || Three_Seven_Win)
            BigWin_OBJ.SetActive(true);
        else
            BigWin_OBJ.SetActive(false);

        if (data.GetField("win_lines_infos").Count > 0)
        {
            for (int i = 0; i < data.GetField("win_lines_infos").Count; i++)
            {
                BigList_Items[i].SET_Item_Data(data.GetField("win_lines_infos")[i], JackPotwin);
                Total_Win = Total_Win + float.Parse(data.GetField("win_lines_infos")[i].GetField("win_amount").ToString().Trim(Config.Inst.trim_char_arry));
            }
        }

        if (bigwin)
        {
            GS.Inst.iTwin_Open(this.gameObject);
            IMG_Big_Win_Jackpot.transform.localScale = Vector3.one;
            IMG_Big_Win_Jackpot.sprite = SP_Big_Win;
            WIN_PLUS_ANIM();
            BIG_Coin_Partical_OBJ.SetActive(true);
            EXP_SoundManager.Inst.PlaySFX(3);
            Invoke(nameof(Close_BigWin_SC), 3f);
        }
        else if (JackPotwin)
        {
            GS.Inst.iTwin_Open(this.gameObject);
            IMG_Big_Win_Jackpot.transform.localScale = Vector3.one;
            IMG_Big_Win_Jackpot.sprite = SP_Jackpot;
            WIN_PLUS_ANIM();
            Jackpot_Coin_Partical_OBJ.SetActive(true);
            int JackPotWin = int.Parse(data.GetField("jackpot_data").GetField("jackpot_win").ToString().Trim(Config.Inst.trim_char_arry));
            EXP_SoundManager.Inst.PlaySFX(3);
            Invoke(nameof(Close_BigWin_SC), 3f);
        }
        else if (FreeSpinwin)
        {
            FreeSpincount = int.Parse(data.GetField("free_spin_data").GetField("free_spin_count").ToString().Trim(Config.Inst.trim_char_arry));
            EXP_Free_Spin.Inst.OPEN_FREE_SPIN(FreeSpincount);
            if (FreeSpin)
            {
                GS.Inst.iTwin_Open(this.gameObject);
                WIN_PLUS_ANIM();
                IMG_Big_Win_Jackpot.transform.localScale = Vector3.one;
                IMG_Big_Win_Jackpot.sprite = SP_FreeSpin;
                FreeSpin_Coin_Partical_OBJ.SetActive(true);
                EXP_SoundManager.Inst.PlaySFX(3);
            }
            else
            {

                if (data.GetField("win_lines_infos").Count > 0)
                {
                    GS.Inst.iTwin_Open(this.gameObject);
                    IMG_Big_Win_Jackpot.transform.localScale = Vector3.zero;
                    Simple_Coin_Partical_OBJ.SetActive(true);
                    WIN_PLUS_ANIM();
                    EXP_SoundManager.Inst.PlaySFX(2);
                    Invoke(nameof(Close_BigWin_SC), 3f);
                }
                else
                    Close_BigWin_SC();
            }
        }
        else if (Three_Seven_Win)
        {
            GS.Inst.iTwin_Open(this.gameObject);
            IMG_Big_Win_Jackpot.transform.localScale = Vector3.one;
            IMG_Big_Win_Jackpot.sprite = SP_777;
            WIN_PLUS_ANIM();
            BIG_Coin_Partical_OBJ.SetActive(true);
            EXP_SoundManager.Inst.PlaySFX(3);
            Invoke(nameof(Close_BigWin_SC), 3f);
        }
        else
        {
            EXP_Free_Spin.Inst.CLOSE_FREE_SPIN();
            if (data.GetField("win_lines_infos").Count > 0)
            {
                GS.Inst.iTwin_Open(this.gameObject);
                IMG_Big_Win_Jackpot.transform.localScale = Vector3.zero;
                Simple_Coin_Partical_OBJ.SetActive(true);
                WIN_PLUS_ANIM();
                EXP_SoundManager.Inst.PlaySFX(2);
                Invoke(nameof(Close_BigWin_SC), 3f);
            }
            EXP_Manager.Inst.CLICK_ACTION = true;
        }
    }
    public void Close_BigWin_SC()
    {
        BIG_Coin_Partical_OBJ.SetActive(false);
        Jackpot_Coin_Partical_OBJ.SetActive(false);
        FreeSpin_Coin_Partical_OBJ.SetActive(false);
        Simple_Coin_Partical_OBJ.SetActive(false);
        EXP_Manager.Inst.CLICK_ACTION = true;
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
        HIDE_Big_item_List();
    }

    public void HIDE_Big_item_List()
    {
        for (int i = 0; i < BigList_Items.Count; i++)
        {
            BigList_Items[i].transform.localScale = Vector3.zero;
        }
    }

    public void WIN_PLUS_ANIM()
    {
        //StartCoroutine(Move_Anim());
        //EXP_UI_Manager.Inst.TxtPlusMinus.text = "+" + Total_Win;
        //EXP_UI_Manager.Inst.Win_Plus_Minus_Anim.Play("WinPlusMinus_Anim", 0);
    }

    IEnumerator Move_Anim()
    {
        for (int i = 0; i < 10; i++)
        {
            Slot_PFB_WinCoinMove cell2 = Instantiate(PFB_Win_Coin, this.GetComponent<RectTransform>()) as Slot_PFB_WinCoinMove;
            cell2.Move(WinCoin_Target.transform.position);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
