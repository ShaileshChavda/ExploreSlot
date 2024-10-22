using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_RANK : MonoBehaviour
{
    public static PFB_RANK Inst;
    [SerializeField] IMGLoader UserPic;
    [SerializeField] Image IMG_VIP,IMG_CROWN,Vip_Ring;
    [SerializeField] Text TxtName,TxtChips,TxtRankNo;
    [SerializeField] Image RankLable_BG;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_RANK_DATA(JSONObject data,int RankNo)
    {
        TxtRankNo.text = RankNo.ToString();
        int vipLevel = int.Parse(data.GetField("level").ToString().Trim(Config.Inst.trim_char_arry));
        TxtName.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
        TxtChips.text = float.Parse(data.GetField("chips").ToString().Trim(Config.Inst.trim_char_arry)).ToString("n2");
        UserPic.LoadIMG(data.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry),false, false);
        Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[vipLevel];
        IMG_VIP.sprite = GS.Inst.VIP_LEVEL_LIST[vipLevel];

        switch (RankNo)
        {
            case 1:
                TxtRankNo.text = "";
                RankLable_BG.sprite = Ranking.Inst.Rank_BG[0];
                IMG_CROWN.sprite = Ranking.Inst.Rank_Crown[0];
                break;
            case 2:
                TxtName.color = Color.gray;
                TxtChips.color = Color.gray;
                TxtRankNo.text = "";
                RankLable_BG.sprite = Ranking.Inst.Rank_BG[1];
                IMG_CROWN.sprite = Ranking.Inst.Rank_Crown[1];
                break;
            case 3:
                TxtRankNo.text = "";
                RankLable_BG.sprite = Ranking.Inst.Rank_BG[2];
                IMG_CROWN.sprite = Ranking.Inst.Rank_Crown[2];
                break;
            default:
                RankLable_BG.sprite = Ranking.Inst.Rank_BG[3];
                IMG_CROWN.enabled = false;
                break;
        }
    }
}
