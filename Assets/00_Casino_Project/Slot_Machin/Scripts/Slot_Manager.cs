using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Manager : MonoBehaviour
{
    public static Slot_Manager Inst;

    [Header("Splace data")]
    bool splace = false;
    [SerializeField] GameObject OBJ_Splace;
    [SerializeField]public Animator[] All_Slot_Animator;
    [SerializeField]public Animator[] All_Slot_Animator_2;

    public string GameID;
    public bool Spin_Started = false;

    public int Selected_Lines;
    public float Selected_Bet;

    public bool CLICK_ACTION = true;
    private float startTime = 0f;
    private float timerHold = 0f;
    public float holdTime = 1f; // how long you need to hold to trigger the effect
    float timer;
    private bool held = true;
    private bool held2 = true;

    public GameObject Spin_Button, Auto_Spin_Button;
    public double TotalWon_Chips;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
        TotalWon_Chips = 0;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SLOT_GAME_INFO());
        if (!splace)
        {
            OBJ_Splace.SetActive(true);
            Invoke(nameof(Splace), 1.5f);
        }
        else
            OBJ_Splace.SetActive(false);
    }

    public void Update()
    {
        if (CLICK_ACTION && !Auto_Spin_Button.activeSelf && Slot_FreeSpin.Inst.transform.localScale.x <= 0 && Slot_BigWin.Inst.transform.localScale.x <= 0 && Shop.Inst.transform.localScale.x<=0 && Slot_LuckyPlayer.Inst.transform.localScale.x <= 0)
        {
            if (Input.GetMouseButtonDown(0) && held)
            {
                startTime = Time.time;
                timer = startTime;
                held = false;
            }
            if (Input.GetMouseButtonUp(0) && held2)
            {
                held2 = false;
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && timer<(startTime + holdTime))
                    BTN_SPIN();
            }
            if (Input.GetKey(KeyCode.Mouse0) && held == false)
            {
                timer += Time.deltaTime;
                // Once the timer float has added on the required holdTime, changes the bool (for a single trigger), and calls the function
                if (timer > (startTime + holdTime))
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hit.collider != null)
                    {
                        held = true;
                        Spin_Button.SetActive(false);
                        Auto_Spin_Button.SetActive(true);
                        SocketHandler.Inst.SendData(SocketEventManager.Inst.SLOT_START_SPIN(true));
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                held = true;
                held2 = true;
            }
      
        }
    }

    public void JOIN_OLD_GAME(JSONObject data)
    {
        GameID = data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
        Slot_UI_Manager.Inst.TxtGameID.text = GameID;
        Slot_UI_Manager.Inst.Txt_Jackpot.text = data.GetField("collected_jackpot_amount").ToString().Trim(Config.Inst.trim_char_arry);
        Slot_UI_Manager.Inst.Txt_LuckyPlayerName.text = data.GetField("luck_user_name").ToString().Trim(Config.Inst.trim_char_arry);
        Slot_UI_Manager.Inst.Txt_UserName.text = GameID;
        Slot_UI_Manager.Inst.SET_BET_LIST_DATA(data.GetField("bet_list"));
        Slot_UI_Manager.Inst.SET_USER_DATA(data);
    }

    void Splace()
    {
        OBJ_Splace.SetActive(false);
    }

    public void BTN_SPIN()
    {
        if (!Spin_Started && CLICK_ACTION && Slot_FreeSpin.Inst.transform.localScale.x <= 0 && Slot_BigWin.Inst.transform.localScale.x <= 0 && Shop.Inst.transform.localScale.x <= 0 && Slot_LuckyPlayer.Inst.transform.localScale.x <= 0)
        {
            Slot_SoundManager.Inst.PlaySFX(0);
            Spin_Started = true;
            SocketHandler.Inst.SendData(SocketEventManager.Inst.SLOT_START_SPIN(false));
        }
    }

    public void BTN_AutoSpin_Stop()
    {
        if (CLICK_ACTION && Slot_FreeSpin.Inst.transform.localScale.x<= 0 && Slot_BigWin.Inst.transform.localScale.x <= 0 && Shop.Inst.transform.localScale.x <= 0 && Slot_LuckyPlayer.Inst.transform.localScale.x<=0)
        {
            Slot_SoundManager.Inst.PlaySFX(0);
            Spin_Button.SetActive(true);
            Auto_Spin_Button.SetActive(false);
            SocketHandler.Inst.SendData(SocketEventManager.Inst.SLOT_STOP_SPIN());
        }
    }

    public IEnumerator SpinSlot_Anim(JSONObject data)
    {
        Slot_UI_Manager.Inst.Hide_All_Lines();
        if (Slot_Spin_Handler.Inst.Last_Spiner.Equals(1))
        {
            for (int i = 0; i < All_Slot_Animator.Length; i++)
            {
                All_Slot_Animator[i].Play("SlotAnim", 0, 0f);
                yield return new WaitForSeconds(0.18f);
            }
        }
        else
        {
            for (int i = 0; i < All_Slot_Animator_2.Length; i++)
            {
                All_Slot_Animator_2[i].Play("SlotAnim", 0, 0f);
                yield return new WaitForSeconds(0.18f);
            }
        }
        //Slot_UI_Manager.Inst.Change_Blur_Image();
        Slot_Spin_Handler.Inst.SET_SPIN_DATA(data);
        yield return new WaitForSeconds(1f);
        Slot_UI_Manager.Inst.Show_Win_Line(data);
        yield return new WaitForSeconds(0.1f);
        Slot_BigWin.Inst.Open_BigWin_SC(data);
        Spin_Started = false;
    }
}
