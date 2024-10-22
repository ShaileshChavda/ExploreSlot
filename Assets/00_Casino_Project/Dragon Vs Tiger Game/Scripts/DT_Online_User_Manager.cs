using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DT_Online_User_Manager : MonoBehaviour
{
    public static DT_Online_User_Manager Inst;
    public DT_PFB_OnlineUser PFB_Online_User;
    public RectTransform DataParent;
    internal List<DT_PFB_OnlineUser> UserCellList;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        UserCellList = new List<DT_PFB_OnlineUser>();
    }

    public void SET_ONLINE_USER_LIST(JSONObject data)
    {
        StartCoroutine(SET_USER_LIST(data));
    }

    public IEnumerator SET_USER_LIST(JSONObject data)
    {
        int j = 0;
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_cells();
        for (int i = 0; i < data.GetField("user_joins").Count; i++)
        {
            if (data.GetField("user_joins")[i].GetField("_id").ToString().Trim(Config.Inst.trim_char_arry) != GS.Inst._userData.Id)
            {
                j++;
                DT_PFB_OnlineUser cell = Instantiate(PFB_Online_User);
                cell.transform.SetParent(DataParent, false);
                string Name = data.GetField("user_joins")[i].GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
                string chips = data.GetField("user_joins")[i].GetField("wallet").ToString().Trim(Config.Inst.trim_char_arry);
                string PicURL = data.GetField("user_joins")[i].GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry);
                int vip = 0;
                if (data.GetField("user_joins")[i].GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry) != "null")
                    vip = int.Parse(data.GetField("user_joins")[i].GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry));
                cell.SET_USER_DATA(j, Name, chips, PicURL, vip);
                UserCellList.Add(cell);
            }
        }
        yield return new WaitForSeconds(0.1f);
        DataParent.anchoredPosition = new Vector2(DataParent.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }

    internal void Clear_OLD_cells()
    {
        for (int i = 0; i < UserCellList.Count; i++)
        {
            if (UserCellList[i].gameObject != null)
            {
                Destroy(UserCellList[i].gameObject);
            }
        }
        UserCellList.Clear();
    }

    public void BTN_OPEN()
    {
        GS.Inst.iTwin_Open(this.gameObject);
    }

    public void BTN_CLOSE()
    {
        GS.Inst.iTwin_Close(this.gameObject,0.3f);
    }
}
