using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Safe : MonoBehaviour
{
    public static Safe Inst;
    public GameObject GreenSelection;
    [SerializeField] Image IMG_SAVE, IMG_TAKE,IMG_Arrow;
    [SerializeField] Sprite Sprite_LeftArrow, Sprite_RightArrow;
    [SerializeField] Text Txt_Safe_Amount, Txt_Current_Amount;
    [SerializeField] InputField Input_Amount;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void OPEN_SAFE()
    {
        SoundManager.Inst.PlaySFX(0);
        OPEN_SAVE();
        GS.Inst.iTwin_Open(this.gameObject);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SAFE_INFO());
    }
    public void CLOSE_SAFE()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }

    public void BTN_SAVE()
    {
        SoundManager.Inst.PlaySFX(0);
        OPEN_SAVE();
    }
    public void BTN_TAKEOUT()
    {
        SoundManager.Inst.PlaySFX(0);
        OPEN_TAKEOUT();
    }
    public void BTN_RESET()
    {
        SoundManager.Inst.PlaySFX(0);
        Input_Amount.text = "";
    }
    public void BTN_YES()
    {
        SoundManager.Inst.PlaySFX(0);
        //Take IN
        if (IMG_Arrow.sprite.name.Equals("left"))
            SocketHandler.Inst.SendData(SocketEventManager.Inst.SAFE_TAKE_IN(Input_Amount.text));
        else
            SocketHandler.Inst.SendData(SocketEventManager.Inst.SAFE_TAKE_OUT(Input_Amount.text));
    }

    public void OPEN_SAVE()
    {
        GreenSelection.transform.localPosition = IMG_SAVE.transform.localPosition;
        IMG_Arrow.sprite = Sprite_LeftArrow;
    }
    public void OPEN_TAKEOUT()
    {
        GreenSelection.transform.localPosition = IMG_TAKE.transform.localPosition;
        IMG_Arrow.sprite = Sprite_RightArrow;
    }

    public void SET_SAFE_DATA(JSONObject data)
    {
        Txt_Current_Amount.text =float.Parse(data.GetField("chips").ToString().Trim(Config.Inst.trim_char_arry)).ToString("n2");
        Txt_Safe_Amount.text =float.Parse(data.GetField("safe_wallet").ToString().Trim(Config.Inst.trim_char_arry)).ToString("n2");
        Input_Amount.text = "";
        DashboardManager.Inst.SET_DASHBOARD_DATA();
    }
}
