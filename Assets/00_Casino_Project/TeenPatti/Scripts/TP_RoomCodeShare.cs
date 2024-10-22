using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TP_RoomCodeShare : MonoBehaviour
{
    public static TP_RoomCodeShare Inst;
    public Text Txt_RoomCode,TxtPrivateTime;
    public string Table_PointValue, Table_MinEntry;
    public int Private_timer;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    public void SET_ROOM_CODE(string roomCode)
    {
        Txt_RoomCode.text = roomCode;
    }
    public void OPEN_ROOM_CODE_SCREEN()
    {
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void CLOSE_ROOM_CODE_SCREEN()
    {
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }

    public void SHARE_CODE(string btnName)
    {
        switch (btnName)
        {
            case "wt":
                //ShareController.Inst.WhatsAppShare("I want to play TeenPatti game with you! Please install from Android: " + GS.Inst.Game_Download_URL +" Point Value:"+ Table_PointValue +" Minimum Entry:"+Table_MinEntry+ " Start game and go to Private Table and enter Room code " + Txt_RoomCode.text);
                ShareController.Inst.WhatsAppShare("You are invited to play Teen Patti game [Point Value: "+ Table_PointValue + "] | [Minimum Entry: " + Table_MinEntry+ "] Room Code: " + Txt_RoomCode.text);
                break;
            case "fb":
                TP_Share.Inst.FaceBookShare("I want to play TeenPatti game with you! Please install from Android: " + GS.Inst.Game_Download_URL + " Point Value:" + Table_PointValue + " Minimum Entry:" + Table_MinEntry + " Start game and go to Private Table and enter Room code " + Txt_RoomCode.text);
                break;
            case "tl":
                //ShareController.Inst.TelegramAppShare("I want to play TeenPatti game with you! Please install from Android: " + GS.Inst.Game_Download_URL + " Point Value:" + Table_PointValue + " Minimum Entry:" + Table_MinEntry + " Start game and go to Private Table and enter Room code " + Txt_RoomCode.text);
                ShareController.Inst.TelegramAppShare("You are invited to play Teen Patti game [Point Value: " + Table_PointValue + "] | [Minimum Entry: " + Table_MinEntry + "] Room Code: " + Txt_RoomCode.text);
                break;
            case "all":
                TP_Share.Inst.Share("I want to play TeenPatti game with you! Please install from Android: " + GS.Inst.Game_Download_URL + " Point Value:" + Table_PointValue + " Minimum Entry:" + Table_MinEntry + " Start game and go to Private Table and enter Room code " + Txt_RoomCode.text);
                break;
            case "copy":
                //string msg2 = "I want to play TeenPatti game with you! Please install from Android: " + GS.Inst.Game_Download_URL + " Point Value:" + Table_PointValue + " Minimum Entry:" + Table_MinEntry + " Start game and go to Private Table and enter Room code " + Txt_RoomCode.text;
                string msg2 = "You are invited to play Teen Patti game [Point Value: " + Table_PointValue + "] | [Minimum Entry: " + Table_MinEntry + "] Room Code: " + Txt_RoomCode.text;
                UniClipboard.SetText(msg2);
                GameObject.Find("Cop_Alert").transform.localScale = Vector3.one;
                UniClipboard.GetText();
                Invoke("CL_ALT", 0.3f);
                break;
        }
    }
    void CL_ALT()
    {
        GameObject.Find("Cop_Alert").transform.localScale = Vector3.zero;
    }
    public void Start_timer()
    {
        Private_timer = Private_timer - 2;
        TxtPrivateTime.text = Private_timer.ToString();
        InvokeRepeating("_timer", 1, 1);
    }
    public void _timer()
    {
        if (Private_timer > 0)
        {
            Private_timer--;
            TxtPrivateTime.text = Private_timer.ToString();
        }
        else
        {
            TxtPrivateTime.text = Private_timer.ToString();
            CancelInvoke(nameof(Start_timer));
        }
    }
}
