using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mines_Manager : MonoBehaviour
{
    public static Mines_Manager Inst;
    public string GameID;
    public int Selected_Bet_Amount;
    public GameObject Selected_Bet_Ring;
    public GameObject BTN_SPIN_Disable,BTN_CLAIM_Disable,BTN_CLEAR_Disable;
    public Button Btn_Left_TRX, Btn_Right_TRX;
    public List<Sprite> Chips_Sprite_List;
    public string Round_Ticket_ID;

    //TreasureBox
    public List<Mines_PFB_TreasureBox_X> List_PFB_TRS_BOX;
    public RectTransform DataParent;
    public List<Sprite> List_TRS_BOX_SP;
    public JSONObject TRS_data_Config = new JSONObject();
    [SerializeField]public int Mines = 0;
    [SerializeField]public int TRS_Glow_Index;
    public Image Progress_filler;
    public float Next_Move = 0;

    //Coin Animation
    public Mines_PFB_Coins _PFB_Coins; 
    public GameObject Coin_Source,Coin_Destination;

    //SCratch Box
    public List<Mines_PFB_Scratch_Box> List_Scratch_Box;
    public List<int> NotActive_List_Scratch_Box_Index;
    public List<int> Bomb_List_Scratch_Box_Index;
    public Sprite Scratch_Box_Default_SP, Scratch_Box_Crack_SP;
    public int NotActive_Count = 0;
    public bool First_bet;
    public int Card_Scratch_Count = 0;
    public Sprite Default_Diamond_SP;
    public string GameState;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.MINES_GAME_INFO());
    }

    //------------- Join Old Game ----------------------------------------------------------------
    public void JOIN_OLD_GAME(JSONObject data)
    {
        Card_Scratch_Count = 0;
        Mines_UI_Manager.Inst.SET_SCREEN_DETAILS();
        GameState = data.GetField("game_state").ToString().Trim(Config.Inst.trim_char_arry);
        Mines = int.Parse(data.GetField("mines").ToString().Trim(Config.Inst.trim_char_arry));
        TRS_data_Config = data.GetField("mines_config");
        Card_Scratch_Count = data.GetField("positions").Count;
        Mines_UI_Manager.Inst.Txt_Mines.text = Mines.ToString();
        switch (GameState)
        {
            case "init_state":
                BTN_SPIN_Disable.SetActive(false);
                BTN_CLEAR_Disable.SetActive(false);
                BTN_CLAIM_Disable.SetActive(true);
                Btn_Left_TRX.interactable = true;
                Btn_Right_TRX.interactable = true;
                SET_TREASURE_BOX_DATA_INIT();
                break;
            case "spin_start":
                Round_Ticket_ID = data.GetField("ticket_id").ToString().Trim(Config.Inst.trim_char_arry);
                if (Round_Ticket_ID != "" && data.GetField("next_win_amount").ToString().Trim(Config.Inst.trim_char_arry) != "-1")
                {
                    BTN_SPIN_Disable.SetActive(true);
                    BTN_CLEAR_Disable.SetActive(true);
                    Btn_Left_TRX.interactable = false;
                    Btn_Right_TRX.interactable = false;
                }
                else
                {
                    if (data.GetField("next_win_amount").ToString().Trim(Config.Inst.trim_char_arry) != "-1")
                    {
                        BTN_SPIN_Disable.SetActive(false);
                        BTN_CLEAR_Disable.SetActive(false);
                        Btn_Left_TRX.interactable = true;
                        Btn_Right_TRX.interactable = true;
                    }
                    else
                    {
                        BTN_SPIN_Disable.SetActive(true);
                        BTN_CLEAR_Disable.SetActive(true);
                        Btn_Left_TRX.interactable = false;
                        Btn_Right_TRX.interactable = false;
                    }
                }
                Mines_UI_Manager.Inst.Txt_BET.text=data.GetField("total_bet_value").ToString().Trim(Config.Inst.trim_char_arry);

                if (data.GetField("next_win_amount").ToString().Trim(Config.Inst.trim_char_arry) != "-1")
                    Mines_UI_Manager.Inst.Txt_Next_Win_Amount.text = data.GetField("next_win_amount").ToString().Trim(Config.Inst.trim_char_arry);
                else
                    Mines_UI_Manager.Inst.Txt_Next_Win_Amount.text = "0";

                Mines_UI_Manager.Inst.Txt_Claim_Amount.text=data.GetField("curr_win_amount").ToString().Trim(Config.Inst.trim_char_arry);

                if (Mines_UI_Manager.Inst.Txt_Claim_Amount.text != "0")
                    BTN_CLAIM_Disable.SetActive(false);
                else
                    BTN_CLAIM_Disable.SetActive(true);

                if (data.GetField("next_win_amount").ToString().Trim(Config.Inst.trim_char_arry).Equals("-1"))
                    REJOIN_Last_All_Card_Open(data.GetField("positions"));
                else
                    Rejoin_Box_Scratch(data.GetField("positions"));

                SET_TREASURE_BOX_DATA_SPIN(data.GetField("mines_win_x"));
                break;
            case "game_timer_start":
                break;
            case "winner_declare_done":
                break;
            case "finish_state":
                break;
        }
        GS.Inst.Rejoin = false;
    }
    //------------- Join Old Game ----------------------------------------------------------------

    //------------- Treasure box logic -----------------------------------------------------------
    public void SET_TREASURE_BOX_DATA_SPIN(JSONObject data)
    {
        int index = 0;
        for (int i = 0; i < data.Count; i++)
        {
            string[] TRS_X = data[i].ToString().Trim(Config.Inst.trim_char_arry).Split("|");
            if (TRS_X[1] != "0")
            {
                index++;
                List_PFB_TRS_BOX[i].Txt_No.text = index.ToString();
                List_PFB_TRS_BOX[i].Txt_X_Value.text = TRS_X[1];
                List_PFB_TRS_BOX[i].My_Box_Avilable(true);
                if (index.Equals(1))
                    Mines_UI_Manager.Inst.Txt_X_Treasure.text = TRS_X[1] + "X Treasure";
            }
            else
            {
                List_PFB_TRS_BOX[i].Txt_No.text = "0";
                List_PFB_TRS_BOX[i].Txt_X_Value.text = "0";
                List_PFB_TRS_BOX[i].My_Box_Avilable(false);
                Progress_filler.fillAmount = Progress_filler.fillAmount + 0.044f;
            }
        }
        if (Card_Scratch_Count > 0) { 
            TRS_Glow_Index = Card_Scratch_Count;
            Mines = Mines + TRS_Glow_Index;
            REJOIN_TRS_Glow_Update();
            REJOIN_TRS_MOVE(Mines);
        }
        else
        {
            REJOIN_TRS_MOVE(Mines);
        }
    }

    public void SET_TREASURE_BOX_DATA_INIT()
    {
        int index = 0;
        for (int i = 0; i < TRS_data_Config.GetField(Mines.ToString()).Count; i++)
        {
            string[] TRS_X = TRS_data_Config.GetField(Mines.ToString())[i].ToString().Trim(Config.Inst.trim_char_arry).Split("|");
            if (TRS_X[1] != "0")
            {
                index++;
                List_PFB_TRS_BOX[i].Txt_No.text = index.ToString();
                List_PFB_TRS_BOX[i].Txt_X_Value.text = TRS_X[1];
                List_PFB_TRS_BOX[i].My_Box_Avilable(true);
                if (index.Equals(1))
                    Mines_UI_Manager.Inst.Txt_X_Treasure.text = TRS_X[1] + "X Treasure";
            }
            else
            {
                List_PFB_TRS_BOX[i].Txt_No.text = "0";
                List_PFB_TRS_BOX[i].Txt_X_Value.text = "0";
                List_PFB_TRS_BOX[i].My_Box_Avilable(false);
            }
        }
    }

    //----------------------- TRS GLOW -------------------------
    public void REJOIN_TRS_Glow_Update()
    {
        int temp_index = 0;
        for (int j = 0; j < List_PFB_TRS_BOX.Count; j++)
        {
            if (List_PFB_TRS_BOX[j].TRS_Active && TRS_Glow_Index > temp_index)
            {
                temp_index++;
                List_PFB_TRS_BOX[j].Active_Glow(true);
                Mines_UI_Manager.Inst.Txt_X_Treasure.text = List_PFB_TRS_BOX[j].Txt_X_Value.text + "X Treasure";
            }
        }
    }
    public void TRS_Glow_Update()
    {
        if (Mines < 25)
        {
            int temp_index = 0;
            for (int j = 0; j < List_PFB_TRS_BOX.Count; j++)
            {
                if (List_PFB_TRS_BOX[j].TRS_Active && TRS_Glow_Index > temp_index)
                {
                    temp_index++;
                    List_PFB_TRS_BOX[j].Active_Glow(true);
                    Mines_UI_Manager.Inst.Txt_X_Treasure.text = List_PFB_TRS_BOX[j].Txt_X_Value.text + "X Treasure";
                }
            }
            Mines++;
        }
        if(Mines<21)
            TreasurBox_Move_Anim('p', Mines);
    }
    public void TRS_GLOW_RESET()
    {
        for (int i = 0; i < List_PFB_TRS_BOX.Count; i++)
        {
            List_PFB_TRS_BOX[i].Active_Glow(false);
        }
    }
    //----------------------- TRS GLOW -------------------------


    //----------------------- TRS PLUS MINUS -------------------------
    bool ScrollMove = false;
    public void TRS_BOX_PLUS_MINUS(int pm)
    {
        Mines = int.Parse(Mines_UI_Manager.Inst.Txt_Mines.text);
        Mines_SoundManager.Inst.PlaySFX(0);
        if (!ScrollMove)
        {
            int index = 0;
            if (pm.Equals(1))
            {
                if (Mines <= TRS_data_Config.GetField(Mines.ToString()).Count)
                {
                    Mines++;
                    Progress_filler.fillAmount = Progress_filler.fillAmount + 0.044f;
                    Mines_UI_Manager.Inst.Txt_Mines.text = Mines.ToString();
                    for (int i = 0; i < TRS_data_Config.GetField(Mines.ToString()).Count; i++)
                    {
                        string[] TRS_X = TRS_data_Config.GetField(Mines.ToString())[i].ToString().Trim(Config.Inst.trim_char_arry).Split("|");
                        if (TRS_X[1] != "0")
                        {
                            index++;
                            List_PFB_TRS_BOX[i].Txt_No.text = index.ToString();
                            List_PFB_TRS_BOX[i].Txt_X_Value.text = TRS_X[1];
                            List_PFB_TRS_BOX[i].My_Box_Avilable(true);
                            if (index.Equals(1))
                                Mines_UI_Manager.Inst.Txt_X_Treasure.text = TRS_X[1] + "X Treasure";
                        }
                        else
                        {
                            List_PFB_TRS_BOX[i].Txt_No.text = "0";
                            List_PFB_TRS_BOX[i].Txt_X_Value.text = "0";
                            List_PFB_TRS_BOX[i].My_Box_Avilable(false);
                        }
                    }
                    TreasurBox_Move_Anim('p', Mines);
                }
            }
            else
            {
                if (Mines > 2)
                {
                    Mines--;
                    Progress_filler.fillAmount = Progress_filler.fillAmount - 0.044f;
                    Mines_UI_Manager.Inst.Txt_Mines.text = Mines.ToString();
                    for (int i = 0; i < TRS_data_Config.GetField(Mines.ToString()).Count; i++)
                    {
                        string[] TRS_X = TRS_data_Config.GetField(Mines.ToString())[i].ToString().Trim(Config.Inst.trim_char_arry).Split("|");
                        if (TRS_X[1] != "0")
                        {
                            index++;
                            List_PFB_TRS_BOX[i].Txt_No.text = index.ToString();
                            List_PFB_TRS_BOX[i].Txt_X_Value.text = TRS_X[1];
                            List_PFB_TRS_BOX[i].My_Box_Avilable(true);
                            if (index.Equals(1))
                                Mines_UI_Manager.Inst.Txt_X_Treasure.text = TRS_X[1] + "X Treasure";
                        }
                        else
                        {
                            List_PFB_TRS_BOX[i].Txt_No.text = "0";
                            List_PFB_TRS_BOX[i].Txt_X_Value.text = "0";
                            List_PFB_TRS_BOX[i].My_Box_Avilable(false);
                        }
                    }
                    TreasurBox_Move_Anim('m', Mines);
                }
                else
                {
                    TreasurBox_Move_Anim('m', Mines);
                    Mines_UI_Manager.Inst.Txt_X_Treasure.text = List_PFB_TRS_BOX[0].Txt_X_Value.text + "X Treasure";
                }
            }
        }
    }
    //----------------------- TRS PLUS MINUS -------------------------
    
   //----------------------- TRS MOVE -------------------------
    private void Update()
    {
        if (ScrollMove)
        {
            DataParent.localPosition = Vector3.Lerp(DataParent.localPosition, new Vector3(Next_Move, DataParent.localPosition.y, 0), Time.deltaTime*7f);
        }
    }
    int first = 0;
    public void TreasurBox_Move_Anim(char pm, int _mines)
    {
        DataParent.localPosition = new Vector3(Next_Move, DataParent.localPosition.y, 0);
        if (pm.Equals('p'))
        {
            if (_mines < 22)
            {
                if (first == 0)
                {
                    first = 1;
                    Next_Move = DataParent.localPosition.x-40f;
                }
                else
                    Next_Move = DataParent.localPosition.x-80f;
            }
        }
        else
        {
            if (_mines == 2)
            {
                first = 0;
                Next_Move = 0;
            }
            if (_mines == 3)
                Next_Move = DataParent.localPosition.x+40f;
            if (_mines > 3 && _mines < 21)
                Next_Move = DataParent.localPosition.x+80f;
        }
        ScrollMove = true;
        Invoke("Move_False", 0.5f);
    }
    void Move_False()
    {
        ScrollMove = false;
    }

    public void REJOIN_TRS_MOVE(int mines)
    {
        int Go_index= mines;
        Go_index = Go_index - 2;
        Debug.Log("GO INDEX :" + Go_index);
        switch (Go_index)
        {
            case 0:
                Next_Move = 1;
                break;
            case 1:
                Next_Move =- 40f;
                break;
            case 2:
                Next_Move =- 120f;
                break;
            case 3:
                Next_Move =- 200f;
                break;
            case 4:
                Next_Move =- 280f;
                break;
            case 5:
                Next_Move =- 360f;
                break;
            case 6:
                Next_Move =- 440f;
                break;
            case 7:
                Next_Move =- 520f;
                break;
            case 8:
                Next_Move =- 600f;
                break;
            case 9:
                Next_Move =- 680f;
                break;
            case 10:
                Next_Move =- 760f;
                break;
            case 11:
                Next_Move =- 840f;
                break;
            case 12:
                Next_Move =- 920f;
                break;
            case 13:
                Next_Move =- 1000f;
                break;
            case 14:
                Next_Move =- 1080f;
                break;
            case 15:
                Next_Move =- 1160f;
                break;
            case 16:
                Next_Move =- 1240f;
                break;
            case 17:
                Next_Move =- 1320f;
                break;
            case 18:
                Next_Move =- 1400f;
                break;
            case 19:
                Next_Move =- 1480f;
                break;
            case 20:
                Next_Move =- 1560f;
                break;
            case 21:
                Next_Move =- 1640f;
                break;
            case 22:
                Next_Move =- 1720f;
                break;
            case 23:
                Next_Move =- 1800f;
                break;
            case 24:
                Next_Move =- 1880f;
                break;
            case 25:
                Next_Move =- 1960f;
                break;
            default:
                Next_Move = 1;
                break;
        }
        DataParent.localPosition = new Vector3(Next_Move, DataParent.localPosition.y, 0);
    }
    //----------------------- TRS MOVE -----------------------------


   //--------------------- CLICK SPIN ------------------------------
    public void CLICK_SPIN_START()
    {
        Mines_SoundManager.Inst.PlaySFX(0);
        if (Mines_UI_Manager.Inst.Txt_BET.text != "0")
        {
            Mines_UI_Manager.Inst.BadLuck_SC.SetActive(false);
            SocketHandler.Inst.SendData(SocketEventManager.Inst.MINES_START_SPIN());
        }
        else
        {
            Alert_MSG.Inst.MSG("Please bet first!");
        }
    }
    //--------------------- CLICK SPIN ------------------------------


    public void BTN_CLEAR()
    {
        Mines_SoundManager.Inst.PlaySFX(0);
        Mines_UI_Manager.Inst.Txt_BET.text = "0";
        Mines_UI_Manager.Inst.SET_SCREEN_DETAILS();
    }

    public void BTN_BOX_CRATCH(int BoxNo)
    {
        if(GameState!= "stop_game")
            SocketHandler.Inst.SendData(SocketEventManager.Inst.MINES_CARD_CRASH(Round_Ticket_ID,BoxNo));
    }

    public void CLICK_BET()
    {
        Mines_SoundManager.Inst.PlaySFX(0);
        if (!BTN_SPIN_Disable.activeSelf)
        {
            Btn_Left_TRX.interactable = true;
            Btn_Right_TRX.interactable = true;
            if (First_bet)
            {
                First_bet = false;
                Selected_Bet_Amount = int.Parse(Mines_UI_Manager.Inst.Txt_BET.text) + Selected_Bet_Amount;
            }
            if (Selected_Bet_Amount <= float.Parse(Mines_UI_Manager.Inst.Txt_Player_Chips.text))
            {
                BTN_SPIN_Disable.SetActive(false);
                BTN_CLEAR_Disable.SetActive(false);
                Mines_UI_Manager.Inst.Txt_BET.text = (int.Parse(Mines_UI_Manager.Inst.Txt_BET.text) + Selected_Bet_Amount).ToString();
                Mines_PFB_Coins cell = Instantiate(_PFB_Coins, GameObject.Find("BG_MAIN").transform) as Mines_PFB_Coins;
                cell.Move_Anim_Coin();
                Mines_UI_Manager.Inst.BET_CHIPS_UPDATE();
            }
            else
            {
                Alert_MSG.Inst.MSG("You do not have sufficient balance.!");
            }
        }
        else
        {
            Alert_MSG.Inst.MSG("Please scratch cards.");
        }
    }

    public void SET_CRATCH_DATA(JSONObject data)
    {
            int position = int.Parse(data.GetField("position").ToString().Trim(Config.Inst.trim_char_arry)) - 1;
            string next_win_amount = data.GetField("next_win_amount").ToString().Trim(Config.Inst.trim_char_arry);
            if (data.GetField("card").ToString().Trim(Config.Inst.trim_char_arry) != "bomb")
            {
                if (next_win_amount != "-1")
                    Mines_UI_Manager.Inst.Txt_Next_Win_Amount.text = data.GetField("next_win_amount").ToString().Trim(Config.Inst.trim_char_arry);
                else
                    Mines_UI_Manager.Inst.Txt_Next_Win_Amount.text = "0";

                StartCoroutine(Mines_UI_Manager.Inst.Claim_Amount_Update(float.Parse(data.GetField("curr_win_amount").ToString().Trim(Config.Inst.trim_char_arry))));
                TRS_Glow_Index++;
                TRS_Glow_Update();
            }
            else
            {
                if (data.GetField("next_win_amount").ToString().Trim(Config.Inst.trim_char_arry) != "-1")
                {
                    Mines_UI_Manager.Inst.Txt_Claim_Amount.text = "0";
                    Btn_Left_TRX.interactable = true;
                    Btn_Right_TRX.interactable = true;
                }
                else
                {
                    Mines_UI_Manager.Inst.Txt_Next_Win_Amount.text = "0";
                }

            }
            if (Mines_UI_Manager.Inst.Txt_Claim_Amount.text != "0")
                BTN_CLAIM_Disable.SetActive(false);
            else
                BTN_CLAIM_Disable.SetActive(true);

            if (data.GetField("card").ToString().Trim(Config.Inst.trim_char_arry).Equals("daimond") && next_win_amount != "-1")
                List_Scratch_Box[position].Diamond_Scratch();
            else if (data.GetField("card").ToString().Trim(Config.Inst.trim_char_arry).Equals("bomb"))
                List_Scratch_Box[position].Bomb_Scratch(data.GetField("card_details"));
            else
                List_Scratch_Box[position].Minus_1_Scratch(data.GetField("card").ToString().Trim(Config.Inst.trim_char_arry), data.GetField("card_details"));

    }


    public void BTN_CLAIM_AMOUNT()
    {
        Mines_SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.MINES_CLAIM_WIN(Round_Ticket_ID));
    }

    public void Last_All_Card_Open(JSONObject data,string reson)
    {
        First_bet = false;
        TRS_Glow_Index = 0;
      
        for (int i = 0; i < data.Count; i++)
        {
            string[] Card_Box = data[i].ToString().Trim(Config.Inst.trim_char_arry).Split("|");
            List_Scratch_Box[int.Parse(Card_Box[0])-1].Open_All_Scratch(Card_Box[1],bool.Parse(Card_Box[2]));
        }
        if (reson.Equals("bomb"))
        {
            Mines_UI_Manager.Inst.Txt_Claim_Amount.text = "0";
            BTN_CLAIM_Disable.SetActive(true);
        }
        else
            BTN_CLAIM_Disable.SetActive(false);

        BTN_SPIN_Disable.SetActive(false);
        BTN_CLEAR_Disable.SetActive(false);
        Btn_Left_TRX.interactable = true;
        Btn_Right_TRX.interactable = true;
        Mines = int.Parse(Mines_UI_Manager.Inst.Txt_Mines.text);
        Mines_UI_Manager.Inst.Txt_Next_Win_Amount.text = "0";
        SET_TREASURE_BOX_DATA_INIT();
        if (Mines_UI_Manager.Inst.Txt_BET.text != "0")
            First_bet = true;
    }

    public void REJOIN_Last_All_Card_Open(JSONObject data)
    {
        First_bet = false;
        TRS_Glow_Index = 0;
        for (int i = 0; i < data.Count; i++)
        {
            string[] Card_Box = data[i].ToString().Trim(Config.Inst.trim_char_arry).Split("|");
            List_Scratch_Box[int.Parse(Card_Box[0]) - 1].Open_All_Scratch(Card_Box[1], bool.Parse(Card_Box[2]));
        }
        Mines = int.Parse(Mines_UI_Manager.Inst.Txt_Mines.text);
        Mines_UI_Manager.Inst.Txt_Next_Win_Amount.text = "0";
        if (Mines_UI_Manager.Inst.Txt_BET.text != "0")
            First_bet = true;
    }

    public void RESET_SCRATCH_BOX()
    {
        for (int i = 0; i < List_Scratch_Box.Count; i++)
        {
            List_Scratch_Box[i].Reset_BOX();
        }
    }

    public void Rejoin_Box_Scratch(JSONObject data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            string[] Box = data[i].ToString().Trim(Config.Inst.trim_char_arry).Split("|");
            for (int j = 0; j < List_Scratch_Box.Count; j++)
            {
                if ((int.Parse(Box[0]) - 1).Equals(j))
                {
                    List_Scratch_Box[j].Diamond_Scratch();
                }
            }
        }
    }
}
