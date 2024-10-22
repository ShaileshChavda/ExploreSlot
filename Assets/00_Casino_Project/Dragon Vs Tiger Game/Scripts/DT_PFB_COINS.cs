using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DT_PFB_COINS : MonoBehaviour
{
    public static DT_PFB_COINS Inst;
    public Image MyCoin_IMG;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    private void OnEnable()
    {
        DT_EventSetup._DT_PFB_COIN_KILL += IM_KILL;
        DT_EventSetup._DT_PFB_COIN_WIN_MOVE += WIN_MOVE_ANIM;
    }

    private void OnDisable()
    {
        DT_EventSetup._DT_PFB_COIN_KILL -= IM_KILL;
        DT_EventSetup._DT_PFB_COIN_WIN_MOVE -= WIN_MOVE_ANIM;
    }

    public void SET_COIN(string Coin)
    {
        for (int i = 0; i < DT_PlayerManager.Inst.Chips_Sprite_List.Count; i++)
        {
            if (DT_PlayerManager.Inst.Chips_Sprite_List[i].name.Equals(Coin))
                MyCoin_IMG.sprite = DT_PlayerManager.Inst.Chips_Sprite_List[i];
        }
    }
    public void Move_Anim(Vector3 target)
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash("scale", Vector3.one, "speed", 4.5f, "easetype", "linear"));
        iTween.MoveTo(this.gameObject, iTween.Hash("position",target, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
    }

    public void IM_KILL()
    {
        Destroy(this.gameObject);
    }
    public void WIN_MOVE_ANIM()
    {
        if (DT_Manager.Inst.Win_Side.Equals("dragon"))
        {
            GameObject g = DT_PlayerManager.Inst.Dragon_Coin_Local_OBJ;
            Vector3 Pos = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
            iTween.MoveTo(this.gameObject, iTween.Hash("position", Pos, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
        }
        else if (DT_Manager.Inst.Win_Side.Equals("tiger"))
        {
            GameObject g = DT_PlayerManager.Inst.Tiger_Coin_Local_OBJ;
            Vector3 Pos = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
            iTween.MoveTo(this.gameObject, iTween.Hash("position", Pos, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
        }
        else
        {
            GameObject g = DT_PlayerManager.Inst.Tie_Coin_Local_OBJ;
            Vector3 Pos = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
            iTween.MoveTo(this.gameObject, iTween.Hash("position", Pos, "time",0.5f, "easetype", iTween.EaseType.easeOutExpo));
        }
        if (DT_Manager.Inst.TargetList.Count > 0)
            StartCoroutine(Win_User_Delay());
            
    }

    IEnumerator Win_User_Delay()
    {
        yield return new WaitForSeconds(0.7f);
        Invoke("Win_Player_Coin_Move", Random.Range(0.3f, 0.5f));
    }

    void Win_Player_Coin_Move()
    {
        GameObject g = DT_Manager.Inst.TargetList[Random.Range(0, DT_Manager.Inst.TargetList.Count)];
        Vector3 Pos = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Pos, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
        DT_PlayerManager.Inst.Play_DiductionAnimation();
    }
}
