using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SevenUpDown_PFB_COINS : MonoBehaviour
{
    public static SevenUpDown_PFB_COINS Inst;
    public Image MyCoin_IMG;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    private void OnEnable()
    {
        SevenUpDown_EventSetup._DT_PFB_COIN_KILL += IM_KILL;
        SevenUpDown_EventSetup._DT_PFB_COIN_WIN_MOVE += WIN_MOVE_ANIM;
    }

    private void OnDisable()
    {
        SevenUpDown_EventSetup._DT_PFB_COIN_KILL -= IM_KILL;
        SevenUpDown_EventSetup._DT_PFB_COIN_WIN_MOVE -= WIN_MOVE_ANIM;
    }

    public void SET_COIN(string Coin)
    {
        for (int i = 0; i < SevenUpDown_PlayerManager.Inst.Chips_Sprite_List.Count; i++)
        {
            if (SevenUpDown_PlayerManager.Inst.Chips_Sprite_List[i].name.Equals(Coin))
                MyCoin_IMG.sprite = SevenUpDown_PlayerManager.Inst.Chips_Sprite_List[i];
        }
    }
    public void Move_Anim(Vector3 target)
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash("scale", Vector3.one, "speed", 4.5f, "easetype", "linear"));
        iTween.MoveTo(this.gameObject, iTween.Hash("position",target, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
    }

    public void IM_KILL()
    {
        Destroy(this.gameObject);
    }
    public void WIN_MOVE_ANIM()
    {
        if (SevenUpDown_Manager.Inst.Win_Side.Equals("two_six"))
        {
            GameObject g = SevenUpDown_PlayerManager.Inst.Dragon_Coin_Local_OBJ;
            Vector3 Pos = new Vector3(g.transform.position.x+ SevenUpDown_UI_Manager.Inst.Get_RNG_Position(), g.transform.position.y + SevenUpDown_UI_Manager.Inst.Get_RNG_Position(), g.transform.position.z);
            iTween.MoveTo(this.gameObject, iTween.Hash("position", Pos, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
        }
        else if (SevenUpDown_Manager.Inst.Win_Side.Equals("eight_twelve"))
        {
            GameObject g = SevenUpDown_PlayerManager.Inst.Tiger_Coin_Local_OBJ;
            Vector3 Pos = new Vector3(g.transform.position.x + SevenUpDown_UI_Manager.Inst.Get_RNG_Position(), g.transform.position.y + SevenUpDown_UI_Manager.Inst.Get_RNG_Position(), g.transform.position.z);
            iTween.MoveTo(this.gameObject, iTween.Hash("position", Pos, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
        }
        else
        {
            GameObject g = SevenUpDown_PlayerManager.Inst.Tie_Coin_Local_OBJ;
            Vector3 Pos = new Vector3(g.transform.position.x + SevenUpDown_UI_Manager.Inst.Get_RNG_Position(), g.transform.position.y + SevenUpDown_UI_Manager.Inst.Get_RNG_Position(), g.transform.position.z);
            iTween.MoveTo(this.gameObject, iTween.Hash("position", Pos, "time",0.5f, "easetype", iTween.EaseType.easeOutExpo));
        }
        if (SevenUpDown_Manager.Inst.TargetList.Count > 0)
            StartCoroutine(Win_User_Delay());
            
    }

    IEnumerator Win_User_Delay()
    {
        yield return new WaitForSeconds(0.7f);
        Invoke(nameof(Win_Player_Coin_Move), Random.Range(0.3f, 0.5f));
    }

    void Win_Player_Coin_Move()
    {
        GameObject g = SevenUpDown_Manager.Inst.TargetList[Random.Range(0, SevenUpDown_Manager.Inst.TargetList.Count)];
        Vector3 Pos = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Pos, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
        SevenUpDown_PlayerManager.Inst.Play_DiductionAnimation();
    }
}
