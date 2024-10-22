using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HR_PFB_Emoji_Chat : MonoBehaviour
{
    public static HR_PFB_Emoji_Chat Inst;
    [SerializeField] Image IMG_Emoji;
    int _id;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_DATA(Sprite emoji,int id)
    {
        _id = id;
        IMG_Emoji.sprite = emoji;
    }

    public void BTN_SEND_EMOJI()
    {
        HR_Chat.Inst.Close_Chat();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.HORSE_RACING_CHAT(GS.Inst._userData.Id, _id.ToString(), "", false));
    }
}
