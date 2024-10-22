using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using DG.Tweening;

public class DT_UI_Manager : MonoBehaviour
{
    public static DT_UI_Manager Inst;
    public Text TxtGameID;
    public Text Txt_Total_Dragon_PlayCoin, Txt_Total_Tie_PlayCoin, Txt_Total_Tiger_PlayCoin;
    public GameObject Dragon_FirCard, Tiger_FireCard, DragonBox_Light, TigerBox_Light, TieBox_Light, Dragon_LightCard, Tiger_LightCard;
    public Animator Start_Bet_Anim, Stop_Bet_Anim;
    public SkeletonGraphic Dragon_ROR, Tiger_ROR;
    public GameObject BetttingBG;
    public GameObject VS_screen_Parent;
    public SkeletonGraphic VS_SCREEN_SkeletonAnim;

    public Sprite[] sptNextPrevAry;
    public Transform trMainBetPos;
    public Image nextPrevSpt;
    public float fltIdleBetPos;
    public float fltMainBetMovePosX;
    public float fltMainBetMoveTimer;
    public bool isNext = false;

    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        nextPrevSpt.transform.DOLocalMoveX(303,2).SetLoops(-1);
    }

    public void Exit_Game()
    {
        DT_SoundManager.Inst.PlaySFX(0);
        DT_Exit_Popup.Inst.Open_Popup();
    }

    public void BTN_GROUP_LIST()
    {
        DT_SoundManager.Inst.PlaySFX(0);
        DT_Online_User_Manager.Inst.BTN_OPEN();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.DRAGON_TIGER_JOINED_USER_LISTS());
    }
    public void NEW_ROUND_START_STOP(bool action, string screen)
    {
        if (screen.Equals("sb"))
            Start_Bet_Anim.Play("StartBet_Anim");
        else
            Stop_Bet_Anim.Play("StartBet_Anim");
    }

 
    public void START_VS_SCREEN(bool action)
    {
        if (action)
        {          
            VS_screen_Parent.gameObject.SetActive(action);
            VS_SCREEN_SkeletonAnim.gameObject.SetActive(action);
            VS_SCREEN_SkeletonAnim.AnimationState.SetAnimation(0, "VS Animation", false);
            Invoke(nameof(DisableVSBlackPatch),2);
        }
        //GS.Inst.iTwin_Open(GameObject.Find("VS_SCREEN"));
        else
        {
            VS_screen_Parent.gameObject.SetActive(action);
            VS_SCREEN_SkeletonAnim.gameObject.SetActive(action);
            //VS_SCREEN.AnimationState.SetAnimation(0, "animation", false);
        }
        // GS.Inst.iTwin_Close(GameObject.Find("VS_SCREEN"), 0.1f);
    }
    void DisableVSBlackPatch()
    {
        VS_screen_Parent.gameObject.SetActive(false);
    }
    public void SET_PLAYED_TOTAL_CHIPS(JSONObject data)
    {
        if (data.HasField("dragon"))
            Txt_Total_Dragon_PlayCoin.text = DT_PlayerManager.Inst._User_TotalBet_Dragon+"/"+data.GetField("dragon").ToString().Trim(Config.Inst.trim_char_arry);
        if (data.HasField("tie"))
            Txt_Total_Tie_PlayCoin.text = DT_PlayerManager.Inst._User_TotalBet_Tie+"/"+data.GetField("tie").ToString().Trim(Config.Inst.trim_char_arry);
        if (data.HasField("tiger"))
            Txt_Total_Tiger_PlayCoin.text = DT_PlayerManager.Inst._User_TotalBet_Tiger+"/"+data.GetField("tiger").ToString().Trim(Config.Inst.trim_char_arry);
    }

    public void Active_Light_Anim(string DT)
    {
        if (DT.Equals("dragon"))
        {
            //TigerBox_Light.SetActive(false);
            Tiger_LightCard.SetActive(false);
            //DragonBox_Light.SetActive(true);
            Dragon_LightCard.SetActive(true);
        }
        else if (DT.Equals("tiger"))
        {
            //DragonBox_Light.SetActive(false);
            Dragon_LightCard.SetActive(false);
            //TigerBox_Light.SetActive(true);
            Tiger_LightCard.SetActive(true);
        }
        else
        {
            //DragonBox_Light.SetActive(false);
            Dragon_LightCard.SetActive(false);
            //TigerBox_Light.SetActive(false);
            Tiger_LightCard.SetActive(false);
            //TieBox_Light.SetActive(true);
        }      
    }
    public void Active_Fir_Anim(bool action)
    {
        if (action)
        {
            Dragon_FirCard.SetActive(true);
            Tiger_FireCard.SetActive(true);
        }
        else
        {
            Dragon_FirCard.SetActive(false);
            Tiger_FireCard.SetActive(false);
        }
    }
    public void RoarAnimtion(string DT)
    {
        StartCoroutine(start_ROR(DT));
    }
    IEnumerator start_ROR(string DT)
    {
        //yield return new WaitForSeconds(0.5f);
        if (DT.Equals("dragon"))
        {
            DT_SoundManager.Inst.PlaySFX_Others(3);
            Dragon_ROR.gameObject.SetActive(true);
            Dragon_ROR.AnimationState.SetAnimation(0, "Roar", false);
        }
        else if (DT.Equals("tiger"))
        {
            DT_SoundManager.Inst.PlaySFX_Others(4);
            Tiger_ROR.gameObject.SetActive(true);
            Tiger_ROR.AnimationState.SetAnimation(0, "Roar", false);
        }

        yield return new WaitForSeconds(2f);
        Dragon_ROR.gameObject.SetActive(false);
        Tiger_ROR.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        DT_Timer.Inst.FREE_TIME_Coroutine();       
    }
       


    public void BtnNextPrevBet()
    {
        //DT_SoundManager.Inst.(1);
        Debug.Log("NEXT-PREV: " + isNext);
        if (isNext)
        {
            trMainBetPos.DOLocalMoveX(fltIdleBetPos, fltMainBetMoveTimer).SetEase(Ease.Linear);
            nextPrevSpt.sprite = sptNextPrevAry[0];
            isNext = false;            
        }
        else
        {
            trMainBetPos.DOLocalMoveX(fltMainBetMovePosX, fltMainBetMoveTimer).SetEase(Ease.Linear);
            nextPrevSpt.sprite = sptNextPrevAry[1];
            isNext = true;
        }
    }
    public void RESET_UI_NEXT_ROUDN()
    {
        StartCoroutine(RESET_UI_NEXT_ROUDN_Corutine());
    }
    IEnumerator RESET_UI_NEXT_ROUDN_Corutine()
    {
        yield return new WaitForSeconds(1.5f);
        Txt_Total_Dragon_PlayCoin.text = "";
        Txt_Total_Tie_PlayCoin.text = "";
        Txt_Total_Tiger_PlayCoin.text = "";
        DT_PlayerManager.Inst._User_TotalBet_Dragon = 0;
        DT_PlayerManager.Inst._User_TotalBet_Tie = 0;
        DT_PlayerManager.Inst._User_TotalBet_Tiger = 0;
        DT_Manager.Inst.RESET_CARD();
        Dragon_FirCard.SetActive(false);
        Tiger_FireCard.SetActive(false);
        DragonBox_Light.SetActive(false);
        TigerBox_Light.SetActive(false);
        TieBox_Light.SetActive(false);
        Tiger_LightCard.SetActive(false);
        Dragon_LightCard.SetActive(false);
    }
}
