using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spinner_Hendler : MonoBehaviour
{
    public static Spinner_Hendler Inst;
    public string CurrectSpinType;
    public List<AnimationCurve> animationCurves;
    public RectTransform Spin_Rect;
    private bool spinning;
    private float anglePerItem;
    private int randomTime;
    private int itemNumber;
    int SpinIndex;

    [Header("Silver Spinner")]
    [SerializeField] List<Text> Silver_Spin_Text_List;
    public GameObject Silver_Spinner_OBJ;

    [Header("Gold Spinner")]
    [SerializeField] List<Text> Gold_Spin_Text_List;
    public GameObject Gold_Spinner_OBJ;

    [Header("Diamond Spinner")]
    [SerializeField] List<Text> Diamond_Spin_Text_List;
    public GameObject Diamond_Spinner_OBJ;

    public Text TxtSilver_Point, TxtGold_Point, TxtDiamond_Point;
    public Image SpinBG, SilverLeftButtonBG, GoldLeftButtonBG, DiamondLeftButtonBG;
    public Sprite Blue_Box_SP, Pink_Box_SP;
    public List<Sprite> Spin_BG_Sprite_List;
    public Text Txt_Win_Claim,TxtAvailablePoints;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        spinning = false;
        anglePerItem = 360 / 12f;
    }
    public void StartSpinner(int time, int index)
    {
        Debug.Log("index :::"+index);
        randomTime = time;
        itemNumber = index;
        float maxAngle = GetMAXANGLE();
        StartCoroutine(SpinTheWheel(15 * randomTime, maxAngle));
    }
    IEnumerator SpinTheWheel(float time, float maxAngle)
    {
        spinning = true;
        float timer = 0.0f;
        float startAngle = Spin_Rect.eulerAngles.z;
        maxAngle = maxAngle - startAngle;
        int animationCurveNumber = 0;
        while (timer < time)
        {
            float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
            Spin_Rect.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += (Time.maximumDeltaTime / 1.5f);
            yield return 0;
        }
        Spin_Rect.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
        spinning = false;
        OPEN_SPIN_WIN();
    }

    float GetMAXANGLE()
    {
        return 360 * -randomTime + (itemNumber * anglePerItem);
    }

    public void SET_SPINNER_DATA(JSONObject data)
    {
        TxtAvailablePoints.text = data.GetField("spinner_points").ToString().Trim(Config.Inst.trim_char_arry);
        switch (data.GetField("type").ToString().Trim(Config.Inst.trim_char_arry))
        {
            case "silver":
                for (int i = 0; i < data.GetField("spin_slot").Count; i++)
                {
                    Silver_Spin_Text_List[i].text = data.GetField("spin_slot")[i].ToString().Trim(Config.Inst.trim_char_arry);
                }
                break;
            case "gold":
                for (int i = 0; i < data.GetField("spin_slot").Count; i++)
                {
                    Gold_Spin_Text_List[i].text = data.GetField("spin_slot")[i].ToString().Trim(Config.Inst.trim_char_arry);
                }
                break;
            case "diamond":
                for (int i = 0; i < data.GetField("spin_slot").Count; i++)
                {
                    Diamond_Spin_Text_List[i].text = data.GetField("spin_slot")[i].ToString().Trim(Config.Inst.trim_char_arry);
                }
                break;
            default:
                for (int i = 0; i < data.GetField("spin_slot").Count; i++)
                {
                    Silver_Spin_Text_List[i].text = data.GetField("spin_slot")[i].ToString().Trim(Config.Inst.trim_char_arry);
                }
                break;
        }
    }
    public void Spinner_START_DATA(JSONObject data)
    {
        int index = int.Parse(data.GetField("win_index").ToString().Trim(Config.Inst.trim_char_arry));
        Txt_Win_Claim.text = data.GetField("win_nu").ToString().Trim(Config.Inst.trim_char_arry);
        TxtAvailablePoints.text = data.GetField("user_points").ToString().Trim(Config.Inst.trim_char_arry);
        StartSpinner(5, index);
    }
    public void OPEN_SPINNER()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(this.gameObject);
        CurrectSpinType = "silver";
        SpinBG.sprite = Spin_BG_Sprite_List[0];
        Selected_Spin_BG(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SPINNER_INFO("silver"));
    }
    public void OPEN_SPIN_WIN()
    {
        GS.Inst.iTwin_Open(GameObject.Find("Spinner_Win_POP"));
        Invoke("CLOSE_SPIN_WIN_POP", 5f);
    }
    public void CLOSE_SPIN_WIN_POP()
    {
        GS.Inst.iTwin_Close(GameObject.Find("Spinner_Win_POP"), 0.3f);
    }
    public void CLOSE_SPINNER()
    {
        SoundManager.Inst.PlaySFX(0);
        if (!spinning)
        {
            spinning = false;
            GS.Inst.iTwin_Close(this.gameObject, 0.3f);
        }
        else
        {
            Alert_MSG.Inst.MSG("Let finish spin first");
        }
    }
    public void OPEN_Silver_SP()
    {
        SoundManager.Inst.PlaySFX(0);
        if (!spinning)
        {
            CurrectSpinType = "silver";
            SpinBG.sprite = Spin_BG_Sprite_List[0];
            Selected_Spin_BG(0);
            SocketHandler.Inst.SendData(SocketEventManager.Inst.SPINNER_INFO("silver"));
        }
        else
        {
            Alert_MSG.Inst.MSG("Let finish spin first");
        }
    }
    public void OPEN_Gold_SP()
    {
        SoundManager.Inst.PlaySFX(0);
        if (!spinning)
        {
            CurrectSpinType = "gold";
            SpinBG.sprite = Spin_BG_Sprite_List[1];
            Selected_Spin_BG(1);
            SocketHandler.Inst.SendData(SocketEventManager.Inst.SPINNER_INFO("gold"));
        }
        else
        {
            Alert_MSG.Inst.MSG("Let finish spin first");
        }
    }
    public void OPEN_Diamond_SP()
    {
        SoundManager.Inst.PlaySFX(0);
        if (!spinning)
        {
            CurrectSpinType = "diamond";
            SpinBG.sprite = Spin_BG_Sprite_List[2];
            Selected_Spin_BG(2);
            SocketHandler.Inst.SendData(SocketEventManager.Inst.SPINNER_INFO("diamond"));
        }
        else
        {
            Alert_MSG.Inst.MSG("Let finish spin first");
        }
    }
    public void BTN_CENTER_SPIN()
    {
        SoundManager.Inst.PlaySFX(0);
        if (!spinning)
        {
            SocketHandler.Inst.SendData(SocketEventManager.Inst.SPINNER_START(CurrectSpinType));
        }
        else
        {
            Alert_MSG.Inst.MSG("Let finish spin first");
        }
    }

    public void Open_Spin_RULE()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Open(GameObject.Find("SpinnerRules"));
    }
    public void Close_Spin_RULE()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(GameObject.Find("SpinnerRules"),0.3f);
    }

    public void Selected_Spin_BG(int index)
    {
        SilverLeftButtonBG.sprite = Pink_Box_SP;
        GoldLeftButtonBG.sprite = Pink_Box_SP;
        DiamondLeftButtonBG.sprite = Pink_Box_SP;

        Silver_Spinner_OBJ.transform.localScale = Vector3.zero;
        Gold_Spinner_OBJ.transform.localScale = Vector3.zero;
        Diamond_Spinner_OBJ.transform.localScale = Vector3.zero;

        switch (index)
        {
            case 0:
                SilverLeftButtonBG.sprite = Blue_Box_SP;
                Silver_Spinner_OBJ.transform.localScale = Vector3.one;
                break;
            case 1:
                GoldLeftButtonBG.sprite = Blue_Box_SP;
                Gold_Spinner_OBJ.transform.localScale = Vector3.one;
                break;
            case 2:
                DiamondLeftButtonBG.sprite = Blue_Box_SP;
                Diamond_Spinner_OBJ.transform.localScale = Vector3.one;
                break;
            default:
                SilverLeftButtonBG.sprite = Blue_Box_SP;
                Silver_Spinner_OBJ.transform.localScale = Vector3.one;
                break;
        }
    }
}
