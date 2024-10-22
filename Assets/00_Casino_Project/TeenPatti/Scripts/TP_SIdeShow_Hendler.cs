using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TP_SIdeShow_Hendler : MonoBehaviour
{
    public static TP_SIdeShow_Hendler Inst;
    [SerializeField] Text Txt_Message;
    public GameObject Obj_Button_Penale;

    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    public void ClOSE_SIDE_SHOW_BOX()
    {
        this.transform.localScale = Vector3.zero;
        Obj_Button_Penale.transform.localScale = Vector3.zero;
    }

    public void HENDLE_SIDE_SHOW_REQ(JSONObject data)
    {
        CancelInvoke("close_Reject_Message");
        int Req_Seat_Index = int.Parse(data.GetField("side_show_details").GetField("request_index").ToString().Trim(Config.Inst.trim_char_arry));
        int Oppo_Seat_Index = int.Parse(data.GetField("side_show_details").GetField("opponent_index").ToString().Trim(Config.Inst.trim_char_arry));

        if (GS.Inst._userData.MySeatIndex.Equals(Req_Seat_Index))
        {
            TP_GameManager.Inst.Hide_Footer_Button();
            Obj_Button_Penale.transform.localScale = Vector3.zero;
            Txt_Message.text = data.GetField("msgs").GetField("user").ToString().Trim(Config.Inst.trim_char_arry);
        }
        else if (GS.Inst._userData.MySeatIndex.Equals(Oppo_Seat_Index))
        {
            Obj_Button_Penale.transform.localScale = Vector3.one;
            Txt_Message.text = data.GetField("msgs").GetField("opponent").ToString().Trim(Config.Inst.trim_char_arry);
        }
        else
        {
            Obj_Button_Penale.transform.localScale = Vector3.zero;
            Txt_Message.text = data.GetField("msgs").GetField("other_user").ToString().Trim(Config.Inst.trim_char_arry);
        }
        GS.Inst.iTwin_Open(this.gameObject);
    }

    public void HENDLE_SIDE_SHOW_REQ_REJOIN(JSONObject data)
    {
        int Req_Seat_Index = int.Parse(data.GetField("side_show_details").GetField("request_index").ToString().Trim(Config.Inst.trim_char_arry));
        int Oppo_Seat_Index = int.Parse(data.GetField("side_show_details").GetField("opponent_index").ToString().Trim(Config.Inst.trim_char_arry));

        if (GS.Inst._userData.MySeatIndex.Equals(Req_Seat_Index))
        {
            TP_GameManager.Inst.Hide_Footer_Button();
            Obj_Button_Penale.transform.localScale = Vector3.zero;
            Txt_Message.text = data.GetField("side_show_details").GetField("msg").GetField("user").ToString().Trim(Config.Inst.trim_char_arry);
        }
        else if (GS.Inst._userData.MySeatIndex.Equals(Oppo_Seat_Index))
        {
            Obj_Button_Penale.transform.localScale = Vector3.one;
            Txt_Message.text = data.GetField("side_show_details").GetField("msg").GetField("opponent").ToString().Trim(Config.Inst.trim_char_arry);
        }
        else
        {
            Obj_Button_Penale.transform.localScale = Vector3.zero;
            Txt_Message.text = data.GetField("side_show_details").GetField("msg").GetField("other_user").ToString().Trim(Config.Inst.trim_char_arry);
        }
        GS.Inst.iTwin_Open(this.gameObject);
    }

    public void HENDLE_SIDE_SHOW_ACCEPT(JSONObject data)
    {
       int Winner_Seat= int.Parse(data.GetField("winner").GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));
       int Looser_Seat= int.Parse(data.GetField("losser").GetField("seat_index").ToString().Trim(Config.Inst.trim_char_arry));

        TP_Player Win_p = TP_GameManager.Inst.GetPlayer_UingSetIndex(Winner_Seat);
        if(GS.Inst._userData.MySeatIndex.Equals(Winner_Seat) || GS.Inst._userData.MySeatIndex.Equals(Looser_Seat))
            Win_p.SET_Card_SEE(data.GetField("winner"));

       Win_p.Card_Status.text = "Seen";
       Win_p.IM_Win();

       TP_Player Loss_p = TP_GameManager.Inst.GetPlayer_UingSetIndex(Looser_Seat);
       if (GS.Inst._userData.MySeatIndex.Equals(Winner_Seat) || GS.Inst._userData.MySeatIndex.Equals(Looser_Seat))
            Loss_p.SET_Card_SEE(data.GetField("losser"));

        Loss_p.Card_Status.text = "Seen";

        ClOSE_SIDE_SHOW_BOX();
        //TP_GameManager.Inst.Activity_Status_Message.transform.GetChild(0).GetComponent<Text>().text = data.GetField("winner").GetField("status").ToString().Trim(Config.Inst.trim_char_arry);
        //GS.Inst.iTwin_Open(TP_GameManager.Inst.Activity_Status_Message);

        StartCoroutine(stop_otherAction(Win_p,Loss_p));
    }

    public void HENDLE_SIDE_SHOW_REJECT(JSONObject data)
    {
        Obj_Button_Penale.transform.localScale = Vector3.zero;
        int Req_Seat_Index = int.Parse(data.GetField("side_show_details").GetField("request_index").ToString().Trim(Config.Inst.trim_char_arry));
        int Oppo_Seat_Index = int.Parse(data.GetField("side_show_details").GetField("opponent_index").ToString().Trim(Config.Inst.trim_char_arry));

        if (GS.Inst._userData.MySeatIndex.Equals(Req_Seat_Index))
            Txt_Message.text = data.GetField("msgs").GetField("user").ToString().Trim(Config.Inst.trim_char_arry);
        else if (GS.Inst._userData.MySeatIndex.Equals(Oppo_Seat_Index))
            Txt_Message.text = data.GetField("msgs").GetField("opponent").ToString().Trim(Config.Inst.trim_char_arry);
        else
            Txt_Message.text = data.GetField("msgs").GetField("other_user").ToString().Trim(Config.Inst.trim_char_arry);

        GS.Inst.iTwin_Open(this.gameObject);
        Invoke("close_Reject_Message", 1.5f);
    }

    public void close_Reject_Message()
    {
        GS.Inst.iTwin_Close(this.gameObject,0.3f);
    }

    IEnumerator stop_otherAction(TP_Player Win_P,TP_Player Loos_P)
    {
        yield return new WaitForSeconds(3);
        Win_P.Stop_Win_Anim();
        TP_GameManager.Inst.Activity_Status_Message.transform.localScale=Vector3.zero;

        if (GS.Inst._userData.MySeatIndex != Win_P.SeatIndex)
            Win_P.Show_PlayerCard_Back();

        if (GS.Inst._userData.MySeatIndex != Loos_P.SeatIndex)
            Loos_P.Show_PlayerCard_Back();
    }
}
