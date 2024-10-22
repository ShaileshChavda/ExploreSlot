using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roullate_PFB_Coins : MonoBehaviour
{
    public static Roullate_PFB_Coins Inst { get; set; }
    public Image MyCoin_IMG;
    public List<float> Time_go;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    private void OnEnable()
    {
        Roullate_EventSetup._ROULLATE_PFB_COIN_KILL += IM_KILL;
        Roullate_EventSetup._ROULLATE_PFB_COIN_WIN_MOVE += WIN_MOVE_ANIM;
    }

    private void OnDisable()
    {
        Roullate_EventSetup._ROULLATE_PFB_COIN_KILL -= IM_KILL;
        Roullate_EventSetup._ROULLATE_PFB_COIN_WIN_MOVE -= WIN_MOVE_ANIM;
    }

    public void SET_COIN(string Coin)
    {
        for (int i = 0; i < Roullate_PlayerManager.Inst.Chips_Sprite_List.Count; i++)
        {
            if (Roullate_PlayerManager.Inst.Chips_Sprite_List[i].name.Equals(Coin))
                MyCoin_IMG.sprite = Roullate_PlayerManager.Inst.Chips_Sprite_List[i];
        }
    }
    public void Move_Anim(Vector3 target)
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", target, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
    }


    public void IM_KILL()
    {
        Destroy(this.gameObject);
    }
    public void WIN_MOVE_ANIM()
    {
        StartCoroutine(GO_To_Player());
        //Coin_Show();
        //iTween.MoveTo(this.gameObject, iTween.Hash("position", Roullate_UI_Manager.Inst._TotalVBet_Chip_Icon.transform.position, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
    }
    IEnumerator GO_To_Player()
    {
        Coin_Show();
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Roullate_UI_Manager.Inst._TotalVBet_Chip_Icon.transform.position, "time", 0.7f, "easetype", iTween.EaseType.easeOutExpo));
        yield return new WaitForSeconds(0.75f);
        iTween.Stop(this.gameObject);
        if (Roullate_Manager.Inst.TargetList.Count > 0)
        {
            yield return new WaitForSeconds(Random.Range(0.5f,1f));
            GameObject g = Roullate_Manager.Inst.TargetList[Random.Range(0, Roullate_Manager.Inst.TargetList.Count)];
            Vector3 Pos = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
            iTween.ScaleTo(this.gameObject, iTween.Hash("scale", Vector3.zero, "speed", 3f, "easetype", "linear"));
            iTween.MoveTo(this.gameObject, iTween.Hash("position", Pos, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
            Roullate_PlayerManager.Inst.Play_DiductionAnimation();
        }
    }
    void Coin_Show()
    {
        this.gameObject.transform.localScale = Vector3.one;
    }
}
