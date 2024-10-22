using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FR_BetSelection : MonoBehaviour
{
    public static FR_BetSelection Inst;
    public FISH_PEF_BETLIST PFB_BETLIST_DATA;
    public RectTransform BET_Parent;
    internal List<GameObject> OLD_BetList = new List<GameObject>();
    [SerializeField] Text Txt_OnlinePlayers;
    public TextMeshProUGUI Txt_header_text;
    public Animator Head_Text_Anim;

    public FishBetList _fishBetList;

    private void Start()
    {
        Inst = this;
    }
    public void BTN_FISH_RUSH()
    {
        Debug.Log("BTN_TEEN_PATTI");
        Clear_Old_BETList();
        PreeLoader.Inst.Show();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.FISH_GAME_BETLIST());
    }

    public void BTN_OPEN_BET_SCREEN()
    {
        GS.Inst.PrivateTable = false;
        Head_Text_Anim.enabled = false;
        Txt_header_text.text = "SELECT TABLE";
        Txt_header_text.color = Color.white;
        GS.Inst.iTwin_Open(this.gameObject);
    }

    public void BTN_CLOSE_BET_SCREEN()
    {
        transform.localScale = Vector3.zero;
        BET_Parent.parent.parent.GetComponent<ScrollRect>().enabled = false;
    }

    public void BTN_PRIVATE_TB()
    {
        TP_PrivateTable.Inst.OPEN_PRIVATE_TB_SC();
    }

    public void SET_BETLIST_DATA(JSONObject data)
    {
        Debug.Log("RESPONSE FISH RUSH BET LIST DATA: "+ data);

        _fishBetList = JsonUtility.FromJson<FishBetList>(data.ToString());
        int active_P_Total = 0;
        PreeLoader.Inst.Stop_Loader();
        BET_Parent.parent.parent.GetComponent<ScrollRect>().enabled = false;

        for (int i = 0; i < _fishBetList.List.Count; i++)
        {
            ListItem fItem = _fishBetList.List[i];
            FISH_PEF_BETLIST cell = Instantiate(PFB_BETLIST_DATA);
            cell.transform.SetParent(BET_Parent, false);
            cell.SET_BET_DATA_LIST(fItem);
            OLD_BetList.Add(cell.gameObject);
           
            if (fItem.active_player != 0)
                active_P_Total = active_P_Total + fItem.active_player;
        }

        //ttest
        Txt_OnlinePlayers.text = "Online " + active_P_Total;
        Invoke(nameof(Scroll_EN), 0.5f);
    }

    void Scroll_EN()
    {
        BET_Parent.anchoredPosition = new Vector2(BET_Parent.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        BET_Parent.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }

    public void Clear_Old_BETList()
    {
        if (OLD_BetList.Count > 0)
        {
            for (int i = 0; i < OLD_BetList.Count; i++)
            {
                Destroy(OLD_BetList[i]);
            }
            OLD_BetList.Clear();
        }
    }
}

[System.Serializable]
public struct ListItem
{
    public string name;
    public int max_seat;
    public int point_value;
    public int min_entry;
    public int active_player;
    public bool is_free;
    public string bet_id;
}

[System.Serializable]
public class FishBetList
{
    public int total_active_player;
    public List<ListItem> List = new List<ListItem>();
}
