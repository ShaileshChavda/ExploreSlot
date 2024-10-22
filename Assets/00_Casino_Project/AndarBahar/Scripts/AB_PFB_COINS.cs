using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AB_PFB_COINS : MonoBehaviour
{
    public static AB_PFB_COINS Inst;
    public Image MyCoin_IMG;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    private void OnEnable()
    {
        AB_EventSetup._AB_PFB_COIN_KILL += IM_KILL;
        AB_EventSetup._AB_PFB_COIN_WIN_MOVE += WIN_MOVE_ANIM;
    }

    private void OnDisable()
    {
        AB_EventSetup._AB_PFB_COIN_KILL -= IM_KILL;
        AB_EventSetup._AB_PFB_COIN_WIN_MOVE -= WIN_MOVE_ANIM;
    }

    public void SET_COIN(string Coin)
    {
        for (int i = 0; i < AB_PlayerManager.Inst.Chips_Sprite_List.Count; i++)
        {
            if (AB_PlayerManager.Inst.Chips_Sprite_List[i].name.Equals(Coin))
                MyCoin_IMG.sprite = AB_PlayerManager.Inst.Chips_Sprite_List[i];
        }
    }
    public void Move_Anim(Vector3 target)
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash("scale", Vector3.one, "speed", 4.5f, "easetype", "linear"));
        iTween.MoveTo(this.gameObject, iTween.Hash("position", target, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
    }

    public void IM_KILL()
    {
        Destroy(this.gameObject);
    }
    public void WIN_MOVE_ANIM()
    {
        Invoke("Win_Player_Coin_Move", Random.Range(0.3f, 0.5f));
    }

    void Win_Player_Coin_Move()
    {
        GameObject g = AB_Manager.Inst.TargetList[Random.Range(0, AB_Manager.Inst.TargetList.Count)];
        Vector3 Pos = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Pos, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
        AB_PlayerManager.Inst.Play_DiductionAnimation();
    }
}
