using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_SUPPORT : MonoBehaviour
{
    public static PFB_SUPPORT Inst;
    public Sprite BG_TELEGRAM, BG_WATSAPP;
    public Image My_BG;
    public Text Txt_Title, Txt_Link;
    string MyLink;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        this.GetComponent<Button>().onClick.AddListener(ON_Box_Click);
    }

    public void ON_Box_Click()
    {
        Application.OpenURL(MyLink);
    }

    public void SET_DATA(JSONObject data)
    {
        Txt_Title.text= data.GetField("title").ToString().Trim(Config.Inst.trim_char_arry);
        Txt_Link.text= data.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry);
        MyLink = data.GetField("msg").ToString().Trim(Config.Inst.trim_char_arry);

        if (data.GetField("redirect").ToString().Trim(Config.Inst.trim_char_arry).Equals("gmail"))
            My_BG.sprite = BG_TELEGRAM;
        else
            My_BG.sprite = BG_WATSAPP;
    }

    public void First_DATA()
    {
        Txt_Title.text = "Luck and Skill";
        Txt_Link.text = "Telegram Personal Support";
        MyLink = "https://t.me/Luck_and_skill";
        My_BG.sprite = BG_TELEGRAM;
    }
    public void First_DATA2()
    {
        Txt_Title.text = "Luck and Skill";
        Txt_Link.text = "Telegram Group";
        MyLink = "https://t.me/+s8WRknqYAZA1NDg1";
        My_BG.sprite = BG_TELEGRAM;
    }
}
