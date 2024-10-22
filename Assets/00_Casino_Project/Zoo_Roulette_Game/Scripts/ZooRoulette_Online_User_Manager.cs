namespace ZooRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class ZooRoulette_Online_User_Manager : MonoBehaviour
    {
        public static ZooRoulette_Online_User_Manager Inst;
        public PFB_Online_Roulate_User PFB_Online_User;
        public RectTransform DataParent;
        internal List<PFB_Online_Roulate_User> UserCellList;
        public TextMeshProUGUI txtOnlineUser;
        public float fltDelayTime;
        public float fltRepetTime;
        // Start is called before the first frame update
        void Start()
        {
            Inst = this;
            UserCellList = new List<PFB_Online_Roulate_User>();
           // InvokeRepeating(nameof(CallEverySecond), fltDelayTime, fltRepetTime);
        }

        public void CallEverySecond()
        {
            if (!ZooRoulette_UIManager._instance.isUserList)
                SocketHandler.Inst.SendData(SocketEventManager.Inst.ZOO_JOINED_USER_LISTS());
        }

        public int totalOtherUser;
        public void GET_USERLIST(JSONObject data)
        {
            totalOtherUser = 0;

            for (int i = 0; i < data.GetField("user_joins").Count; i++)
            {
                if (data.GetField("user_joins")[i].GetField("_id").ToString().Trim(Config.Inst.trim_char_arry) != GS.Inst._userData.Id)
                {
                    totalOtherUser++;
                }
            }

            txtOnlineUser.text = "("+totalOtherUser+")";
        }

        public void SET_ONLINE_USER_LIST(JSONObject data)
        {
            StartCoroutine(SET_USER_LIST(data));
        }

        public IEnumerator SET_USER_LIST(JSONObject data)
        {
            // Debug.Log("USER LIST: "+ data);
            DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
           // txtOnlineUser.text = totalOtherUser.ToString();
            Clear_OLD_cells();
            for (int i = 0; i < data.GetField("user_joins").Count; i++)
            {
                if (data.GetField("user_joins")[i].GetField("_id").ToString().Trim(Config.Inst.trim_char_arry) != GS.Inst._userData.Id)
                {
                    PFB_Online_Roulate_User cell = Instantiate(PFB_Online_User);
                    cell.transform.SetParent(DataParent, false);
                    string Name = data.GetField("user_joins")[i].GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
                    string chips = data.GetField("user_joins")[i].GetField("wallet").ToString().Trim(Config.Inst.trim_char_arry);
                    string Picurl = data.GetField("user_joins")[i].GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry);
                    int vipLevel = 0;
                    if (data.GetField("user_joins")[i].GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry) != "" && data.GetField("user_joins")[i].GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry) != "null")
                    {
                        vipLevel = int.Parse(data.GetField("user_joins")[i].GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry));
                    }
                    cell.SET_USER_DATA(Picurl, Name, chips, vipLevel);
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
            ZooRoulette_UIManager._instance.isUserList = false;
            GS.Inst.iTwin_Close(this.gameObject, 0.3f);
        }
    }
}