using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TP_PrivateTable : MonoBehaviour
{
    public static TP_PrivateTable Inst;
    public InputField Input_Room_Code;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void OPEN_PRIVATE_TB_SC()
    {
        GS.Inst.PrivateTable = true;
        GS.Inst.iTwin_Open(this.gameObject);
    }
    public void CLOSE_PRIVATE_TB_SC()
    {
        TP_BetSelection.Inst.Txt_header_text.text = "SELECT TABLE";
        TP_BetSelection.Inst.Txt_header_text.color = Color.white;
        TP_BetSelection.Inst.Head_Text_Anim.enabled = false;
        GS.Inst.PrivateTable = false;
        GS.Inst.iTwin_Close(this.gameObject,0.3f);
    }

    public void Btn_Table_Create()
    {
        TP_BetSelection.Inst.Txt_header_text.text = "SELECT PRIVATE TABLE";
        TP_BetSelection.Inst.Head_Text_Anim.enabled = true;
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }

    public void Btn_Join_Table()
    {
        if (Input_Room_Code.text != "" && Input_Room_Code.text != " ")
        {
            PreeLoader.Inst.Show();
            SocketHandler.Inst.SendData(SocketEventManager.Inst.TEENPATTI_JoinPrivateTable(Input_Room_Code.text));
        }
    }
}
