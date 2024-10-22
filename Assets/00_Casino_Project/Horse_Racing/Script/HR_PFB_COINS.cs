using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HR_PFB_COINS : MonoBehaviour
{
    public static HR_PFB_COINS Inst;
    public Image MyCoin_IMG;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    private void OnEnable()
    {
        HR_EventSetup._DT_PFB_COIN_KILL += IM_KILL;
        HR_EventSetup._DT_PFB_COIN_WIN_MOVE += WIN_MOVE_ANIM;
    }

    private void OnDisable()
    {
        HR_EventSetup._DT_PFB_COIN_KILL -= IM_KILL;
        HR_EventSetup._DT_PFB_COIN_WIN_MOVE -= WIN_MOVE_ANIM;
    }

    public void SET_COIN(string Coin)
    {
        for (int i = 0; i < HR_PlayerManager.Inst.Chips_Sprite_List.Count; i++)
        {
            if (HR_PlayerManager.Inst.Chips_Sprite_List[i].name.Equals(Coin))
                MyCoin_IMG.sprite = HR_PlayerManager.Inst.Chips_Sprite_List[i];
        }
    }
    public void Move_Anim(Vector3 target)
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash("scale", Vector3.one, "speed", 4.5f, "easetype", "linear"));
        iTween.MoveTo(this.gameObject, iTween.Hash("position", target, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
    }

    public void IM_KILL()
    {
        //Kill object
        Destroy(this.gameObject);
    }
    public void WIN_MOVE_ANIM()
    {
        Vector3 target = HR_Manager.Inst.getRandomPoint(HR_PlayerManager.Inst.Horse_Coin_Local_OBJ[HR_Manager.Inst.Win_Side - 1]);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", target, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
        StartCoroutine(Horse_face_Move_Delay());
    }
    IEnumerator Horse_face_Move_Delay()
    {
        yield return new WaitForSeconds(1f);
        Invoke("Coin_HorseFace_Anim", Random.Range(0.3f, 0.5f));
        if (HR_Manager.Inst.TargetList.Count > 0)
            StartCoroutine(Win_User_Delay());
    }
    void Coin_HorseFace_Anim()
    {
        GameObject g = HR_PlayerManager.Inst.Win_Position_OBJ;
        Vector3 Pos = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Pos, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
    }
    IEnumerator Win_User_Delay()
    {
        yield return new WaitForSeconds(1f);
        Invoke("Win_Player_Coin_Move", Random.Range(0.3f, 0.5f));
    }

    void Win_Player_Coin_Move()
    {
        GameObject g = HR_Manager.Inst.TargetList[Random.Range(0, HR_Manager.Inst.TargetList.Count)];
        Vector3 Pos = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Pos, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
        HR_PlayerManager.Inst.Play_DiductionAnimation();
    }
}
