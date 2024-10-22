using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TP_BetSelection : MonoBehaviour
{
    public static TP_BetSelection Inst;
    public PFB_BETLIST PFB_BETLIST_DATA;
    public RectTransform BET_Parent;
    internal List<GameObject> OLD_BetList = new List<GameObject>();
    [SerializeField] Text Txt_OnlinePlayers;
    public TextMeshProUGUI Txt_header_text;
    public Animator Head_Text_Anim;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void BTN_TEEN_PATTI()
    {
        Debug.Log("BTN_TEEN_PATTI");
        Clear_Old_BETList();
        PreeLoader.Inst.Show();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_BetList());
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
        int active_P_Total = 0;
        PreeLoader.Inst.Stop_Loader();
        BET_Parent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        for (int i = 0; i < data.GetField("List").Count; i++)
        {
            PFB_BETLIST cell = Instantiate(PFB_BETLIST_DATA);
            cell.transform.SetParent(BET_Parent, false);
            cell.SET_BET_DATA_LIST(data.GetField("List")[i]);
            OLD_BetList.Add(cell.gameObject);
            if (data.GetField("List")[i].GetField("active_player").ToString().Trim(Config.Inst.trim_char_arry) != "0")
                active_P_Total = active_P_Total + int.Parse(data.GetField("List")[i].GetField("active_player").ToString().Trim(Config.Inst.trim_char_arry));
        }
        //ttest
        Txt_OnlinePlayers.text = "Online "+active_P_Total;
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
