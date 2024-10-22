using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HR_GroundManager : MonoBehaviour
{
    public static HR_GroundManager Inst;
    public GameObject Pfb_Ground;
    public float LastGroundPos;
    public bool Ground_Anim;
    public bool Ground_Reset_Start;
    public GameObject WinLine_OBJ,WinLine_End_Pos, WinLine_Start_Pos,Horse_Defoult_Pos, Horse_Defoult_Pos_Ideal;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        Reset_Ground_Anim();
        //for (int i = 0; i < 21; i++)
        //{
        //    GameObject _Coin = Instantiate(Pfb_Ground) as GameObject;
        //    _Coin.transform.SetParent(this.transform, false);
        //    //LastGroundPos = LastGroundPos+_Coin.transform.localPosition.x + 636f;
        //    LastGroundPos = LastGroundPos + _Coin.transform.localPosition.x + 711f;
        //    _Coin.transform.localPosition = new Vector2(LastGroundPos, _Coin.transform.localPosition.y);
        //}
    }

    void Update()
    {
        if (Ground_Anim)
            transform.localPosition = new Vector2(transform.localPosition.x - 5f, 0);
    }

    public void Reset_Ground_Anim()
    {
        Ground_Anim = false;
        transform.localPosition = new Vector2(-170f, 0);
    }

    public void Anim_for_idel()
    {
        Ground_Reset_Start = true;
        iTween.MoveTo(this.gameObject, iTween.Hash("position", new Vector3(0, transform.position.y, transform.position.z), "time", 1.2f, "easetype", iTween.EaseType.linear));
        Invoke(nameof(stop_reset_ground), 1.3f);
    }
    void stop_reset_ground()
    {
        Ground_Reset_Start = false;
    }

    public void Start_Ground_Anim()
    {
        Ground_Anim = true;
    }
    public void Stop_Ground_Animation()
    {
        Ground_Anim = false;
    }

    public void Win_Line(bool action)
    {
        Vector3 Pos;
        if (action)
            Pos = new Vector3(WinLine_End_Pos.transform.position.x, WinLine_End_Pos.transform.position.y, WinLine_End_Pos.transform.position.z);
        else
            Pos = new Vector3(WinLine_Start_Pos.transform.position.x, WinLine_Start_Pos.transform.position.y, WinLine_Start_Pos.transform.position.z);

        iTween.MoveTo(WinLine_OBJ, iTween.Hash("position", Pos, "time", 0.2f, "easetype", iTween.EaseType.linear));
        Invoke(nameof(Stop_Ground_Animation), 0.2f);
    }

    public void Win_Line_Idel()
    {
        Vector3 Pos = new Vector3(WinLine_Start_Pos.transform.position.x, WinLine_Start_Pos.transform.position.y, WinLine_Start_Pos.transform.position.z);
        iTween.MoveTo(WinLine_OBJ, iTween.Hash("position", Pos, "time", 0.2f, "easetype", iTween.EaseType.linear));
    }
}
