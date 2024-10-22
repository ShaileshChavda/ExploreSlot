using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HR_LuckyPlayer_Hendler : MonoBehaviour
{
    public static HR_LuckyPlayer_Hendler Inst;
    [SerializeField] Image IMG_Header;
    [SerializeField] Sprite SP_LuckyPlayer, SP_GamePlayer;
    [SerializeField] HR_PFB_LuckyPlayer pfb_luckyplayer;
    [SerializeField] HR_PFB_GamePlayer pfb_gameplayer;
    public RectTransform DataParent, DataParent2;
    public List<GameObject> CellList;

    [Header("GAME WINNER")]
    [SerializeField] TextMeshProUGUI Txt_WinnerName;
    [SerializeField] TextMeshProUGUI Txt_WinChips;
    [SerializeField] TextMeshProUGUI Txt_TotalWinRound;
    [SerializeField] IMGLoader Img_Winner_DP;

    [Header("LUCKY PLAYER")]
    [SerializeField] TextMeshProUGUI Txt_LuckyName;
    [SerializeField] TextMeshProUGUI Txt_LuckyChips;
    [SerializeField] TextMeshProUGUI Txt_Lucky_TotalWinRound;
    [SerializeField] IMGLoader Img_Lucky_DP;

    [SerializeField] TextMeshProUGUI Txt_Total_Online_User;
    [SerializeField] TextMeshProUGUI Txt_Page_No;
    int Total_Page,currentPage;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        currentPage = 0;
    }

    public void SET_LIST_DATA(JSONObject data)
    {
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_DATA();

        for (int i = 0; i < data.GetField("user_list").Count; i++)
        {
            HR_PFB_LuckyPlayer cell = Instantiate(pfb_luckyplayer, DataParent) as HR_PFB_LuckyPlayer;
            CellList.Add(cell.gameObject);
            cell.SET_LIST_DATA(data.GetField("user_list")[i]);
        }
        DataParent.anchoredPosition = new Vector2(DataParent.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }

    public void SET_LIST_GAME_PLAYER_DATA(JSONObject data)
    {
        DataParent2.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Txt_Total_Online_User.text = data.GetField("total").ToString().Trim(Config.Inst.trim_char_arry);
        Total_Page = int.Parse(data.GetField("total_page").ToString().Trim(Config.Inst.trim_char_arry));
        Txt_Page_No.text = currentPage+"/"+ data.GetField("total_page").ToString().Trim(Config.Inst.trim_char_arry);
        Clear_OLD_DATA();
        SET_WINNER_AND_LUCKY_INFO(data.GetField("winner_info"), data.GetField("lucky_info"));
        for (int i = 0; i < data.GetField("user_joins").Count; i++)
        {
            HR_PFB_GamePlayer cell = Instantiate(pfb_gameplayer, DataParent2) as HR_PFB_GamePlayer;
            CellList.Add(cell.gameObject);
            cell.SET_LIST_DATA(data.GetField("user_joins")[i]);
        }
        DataParent2.anchoredPosition = new Vector2(DataParent2.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent2.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }

    public void Clear_OLD_DATA()
    {
        if (CellList.Count > 0)
        {
            for (int i = 0; i < CellList.Count; i++)
            {
                if (CellList[i] != null)
                    Destroy(CellList[i]);
            }
            CellList.Clear();
        }
    }
    public void SET_WINNER_AND_LUCKY_INFO(JSONObject Windata,JSONObject Luckydata)
    {
        Txt_WinnerName.text= Windata.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_WinChips.text = Windata.GetField("win_amount").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_TotalWinRound.text = Windata.GetField("round").ToString().Trim(Config.Inst.trim_char_arry);

        Img_Winner_DP.LoadIMG(Windata.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false, false);

        Txt_LuckyName.text = Luckydata.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_LuckyChips.text = Luckydata.GetField("win_amount").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Lucky_TotalWinRound.text = Luckydata.GetField("round").ToString().Trim(Config.Inst.trim_char_arry);

        Img_Lucky_DP.LoadIMG(Luckydata.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false, false);
    }
    public void BTN_Lucky_Player()
    {
        DataParent2.parent.parent.transform.localScale = Vector3.zero;
        DataParent.parent.parent.transform.localScale = Vector3.one;
        IMG_Header.sprite = SP_LuckyPlayer;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.HORSE_RACING_LUCKY_USERS_HISTORY());
    }
    public void BTN_Game_Player()
    {
        currentPage = 0;
        DataParent.parent.parent.transform.localScale = Vector3.zero;
        DataParent2.parent.parent.transform.localScale = Vector3.one;
        IMG_Header.sprite = SP_GamePlayer;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.HORSE_RACING_ONLINE_USERS_HISTORY(currentPage));
    }
    public void OPEN_LuckyPlayer()
    {
        SoundManager.Inst.PlaySFX(0);
        BTN_Lucky_Player();
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void CLOSE_LuckyPlayer()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }

    public void Btn_Next_Current_Page(string action)
    {
        if (Total_Page>1)
        {
            if (action.Equals("pr"))
            {
                if (currentPage > 0)
                {
                    currentPage--;
                    Txt_Page_No.text = currentPage + "/" + Total_Page;
                    SocketHandler.Inst.SendData(SocketEventManager.Inst.HORSE_RACING_ONLINE_USERS_HISTORY(currentPage));
                }
            }
            else
            {
                if (currentPage < Total_Page)
                {
                    currentPage++;
                    Txt_Page_No.text = currentPage + "/" + Total_Page;
                    SocketHandler.Inst.SendData(SocketEventManager.Inst.HORSE_RACING_ONLINE_USERS_HISTORY(currentPage));
                }
            }
        }
    }
}
