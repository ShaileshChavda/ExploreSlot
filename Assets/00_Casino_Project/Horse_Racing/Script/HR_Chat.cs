using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HR_Chat : MonoBehaviour
{
    public static HR_Chat Inst;
    [SerializeField] GameObject Chat_Source_OBJ, Chat_Destination_OBJ;
    [SerializeField] HR_PFB_Text_Chat pfb_text;
    [SerializeField] HR_PFB_Emoji_Chat pfb_emoji;
    [SerializeField] HR_PFB_Sticker pfb_sticker;
    [SerializeField] GameObject Text_Chat_View, Emoji_Chat_View;

    [SerializeField] Image IMG_Chat_Header;
    [SerializeField] Sprite SP_Text_Chat, SP_Emoji_Chat;

    [SerializeField] Transform Emoji_Movie_Area;
    public RectTransform DataParent_Text, DataParent_Emoji;
    public List<GameObject> CellList;

    public TextMeshProUGUI Txt_Chat_Counter;
    [SerializeField] List<string> Text_Msg_List;
    [SerializeField]public List<Sprite> Emoji_sprite_List;
    [SerializeField]public List<Sprite> Sardar_sprite_List;

    [Header("My User Chat data")]
    [SerializeField] GameObject Text_Chat_BOX;
    [SerializeField] TextMeshProUGUI Txt_MyChat;
    [SerializeField] Image MyChat_Emoji_IMG;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_TEXT_CHAT_DATA()
    {
        DataParent_Text.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_PFB();
        for (int i = 0; i < Text_Msg_List.Count; i++)
        {
            HR_PFB_Text_Chat cell = Instantiate(pfb_text, DataParent_Text) as HR_PFB_Text_Chat;
            CellList.Add(cell.gameObject);
            cell.SET_DATA(Text_Msg_List[i]);
        }
        DataParent_Text.anchoredPosition = new Vector2(DataParent_Text.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent_Text.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }

    public void SET_EMOJI_CHAT_DATA()
    {
        DataParent_Emoji.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_PFB();
        for (int i = 0; i < Sardar_sprite_List.Count; i++)
        {
            HR_PFB_Emoji_Chat cell = Instantiate(pfb_emoji, DataParent_Emoji) as HR_PFB_Emoji_Chat;
            CellList.Add(cell.gameObject);
            cell.SET_DATA(Sardar_sprite_List[i],i);
        }
        DataParent_Emoji.anchoredPosition = new Vector2(DataParent_Emoji.GetComponent<RectTransform>().anchoredPosition.x, 0f);
        DataParent_Emoji.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }

    public void Clear_OLD_PFB()
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
    public void Open_Chat()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        IMG_Chat_Header.sprite = SP_Text_Chat;
        Text_Chat_View.transform.localScale = Vector3.one;
        Emoji_Chat_View.transform.localScale = Vector3.zero;
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Chat_Destination_OBJ.transform.position, "time", 0.3f, "easetype", iTween.EaseType.easeOutExpo));
        SET_TEXT_CHAT_DATA();
    }

    public void Close_Chat()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Chat_Source_OBJ.transform.position, "time", 0.3f, "easetype", iTween.EaseType.easeInExpo));
    }

    public void BTN_Text_Chat()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        IMG_Chat_Header.sprite = SP_Text_Chat;
        Text_Chat_View.transform.localScale = Vector3.one;
        Emoji_Chat_View.transform.localScale = Vector3.zero;
        SET_TEXT_CHAT_DATA();
    }

    public void BTN_Emoji_Chat()
    {
        HR_SoundManager.Inst.PlaySFX(0);
        IMG_Chat_Header.sprite = SP_Emoji_Chat;
        Emoji_Chat_View.transform.localScale = Vector3.one;
        Text_Chat_View.transform.localScale = Vector3.zero;
        SET_EMOJI_CHAT_DATA();
    }

    public void SET_AND_MOVE_STICKER(JSONObject data)
    {
        bool send = bool.Parse(data.GetField("send").ToString().Trim(Config.Inst.trim_char_arry));
        string s_id = data.GetField("s_id").ToString().Trim(Config.Inst.trim_char_arry);
        string r_id = data.GetField("r_id").ToString().Trim(Config.Inst.trim_char_arry);
        string IMG = data.GetField("image").ToString().Trim(Config.Inst.trim_char_arry);
        string _Text = data.GetField("text").ToString().Trim(Config.Inst.trim_char_arry);

        if (send)
        {
            HR_PFB_Sticker cell = Instantiate(pfb_sticker, Emoji_Movie_Area) as HR_PFB_Sticker;
            cell.SET_Sticker(int.Parse(IMG));
            Vector3 target = new Vector3();
            if (s_id != GS.Inst._userData.Id)
                cell.transform.position = HR_PlayerManager.Inst.GetPlayer_UsingID_CHAT(s_id).transform.position;
            else
                cell.transform.position = HR_Manager.Inst.MyUser_Chal_Pos.transform.position;

            if (r_id != GS.Inst._userData.Id)
                target = HR_PlayerManager.Inst.GetPlayer_UsingID_CHAT(r_id).transform.position;
            else
                target = HR_Manager.Inst.MyUser_Chal_Pos.transform.position;
            iTween.MoveTo(cell.gameObject, iTween.Hash("position", target, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
        }
        else
        {
            if(s_id==GS.Inst._userData.Id && r_id == GS.Inst._userData.Id)
            {
                if (_Text != "")
                {
                    Text_Chat_BOX.transform.localScale = Vector3.one;
                    Txt_MyChat.text = _Text;
                }
                else
                {
                    MyChat_Emoji_IMG.transform.localScale = Vector3.one;
                    MyChat_Emoji_IMG.sprite = Sardar_sprite_List[int.Parse(IMG)];
                }
                Invoke(nameof(Close_MyChat_Box), 5f);
            }
            else
            {
                HR_PlayerManager.Inst.GetPlayer_UsingID(s_id).SET_CHAT_BOX(_Text,IMG);
            }
        }
    }
    void Close_MyChat_Box()
    {
        MyChat_Emoji_IMG.transform.localScale = Vector3.zero;
        Text_Chat_BOX.transform.localScale = Vector3.zero;
    }
}
