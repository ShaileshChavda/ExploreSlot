using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    public static Ranking Inst;
    public PFB_RANK _PFB_RANK;
    public RectTransform DataParent;
    public List<GameObject> CellList;
    public List<Sprite> Rank_BG, Rank_Crown;

    [SerializeField] Text TxtUserName,TxtUserChips, TxtRankNo;
    [SerializeField] Image IMG_VIP;

    [Header("RANK LNG")]
    [SerializeField] Text TxtHeader;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;

    }
    public void OPEN_RANKING()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.CHIPS_RANK_LISTS());
        GS.Inst.iTwin_Open(this.gameObject);
        LNG_SETUP();
    }
    public void CLOSE_RANKING()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }
    public void SET_RANK_LIST(JSONObject data)
    {
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_RANK();
        for (int i = 0; i < data.GetField("rank_lists").Count; i++)
        {
            if (bool.Parse(data.GetField("user_in_list").ToString().Trim(Config.Inst.trim_char_arry))) {
                string id = data.GetField("rank_lists")[i].GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
                if (id.Equals(GS.Inst._userData.Id))
                {
                    TxtRankNo.text = (i + 1).ToString();
                    int vipLevel = int.Parse(data.GetField("rank_lists")[i].GetField("level").ToString().Trim(Config.Inst.trim_char_arry));
                    TxtUserName.text = data.GetField("rank_lists")[i].GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
                    TxtUserChips.text = float.Parse(data.GetField("rank_lists")[i].GetField("chips").ToString().Trim(Config.Inst.trim_char_arry)).ToString("n2");
                    IMG_VIP.sprite = GS.Inst.VIP_LEVEL_LIST[vipLevel];
                }
            }
            PFB_RANK cell = Instantiate(_PFB_RANK) as PFB_RANK;
            CellList.Add(cell.gameObject);
            cell.transform.SetParent(DataParent, false);
            cell.SET_RANK_DATA(data.GetField("rank_lists")[i],i+1);
        }

        if (!bool.Parse(data.GetField("user_in_list").ToString().Trim(Config.Inst.trim_char_arry)))
        {
                TxtRankNo.text = "100+";
                int vipLevel = int.Parse(data.GetField("user_info").GetField("level").ToString().Trim(Config.Inst.trim_char_arry));
                TxtUserName.text = data.GetField("user_info").GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
                TxtUserChips.text = float.Parse(data.GetField("user_info").GetField("chips").ToString().Trim(Config.Inst.trim_char_arry)).ToString("n2");
                IMG_VIP.sprite = GS.Inst.VIP_LEVEL_LIST[vipLevel];
        }
        DataParent.anchoredPosition = new Vector2(DataParent.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }

    public void Clear_OLD_RANK()
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

    void LNG_SETUP()
    {
        switch (PlayerPrefs.GetInt("LNG"))
        {
            case 0:
                //english
                TxtHeader.text = "Rank";
                break;
            case 1:
                //Nepali
                TxtHeader.text = "श्रेणी";
                break;
            case 2:
                //urdu
                TxtHeader.text = "رینک";
                break;
            case 3:
                //Bangali
                TxtHeader.text = "পদমর্যাদা";
                break;
            case 4:
                //Marathi
                TxtHeader.text = "रँक";
                break;
            case 5:
                //Telugu
                TxtHeader.text = "ర్యాంక్";
                break;
            default:
                break;
        }
    }
}
