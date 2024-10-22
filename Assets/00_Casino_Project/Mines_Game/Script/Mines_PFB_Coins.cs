using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mines_PFB_Coins : MonoBehaviour
{
    public static Mines_PFB_Coins Inst { get; set; }
    public Image MyCoin_IMG;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    public void SET_COIN()
    {
        for (int i = 0; i < Mines_Manager.Inst.Chips_Sprite_List.Count; i++)
        {
            if (Mines_Manager.Inst.Chips_Sprite_List[i].name.Equals(Mines_Manager.Inst.Selected_Bet_Amount.ToString()))
                MyCoin_IMG.sprite = Mines_Manager.Inst.Chips_Sprite_List[i];
        }
    }

    public void Move_Anim_Coin()
    {
        SET_COIN();
        this.gameObject.transform.position = Mines_Manager.Inst.Coin_Source.transform.position;
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Mines_Manager.Inst.Coin_Destination.transform.position, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
        Invoke("IM_KILL", 0.9f);
    }

    public void IM_KILL()
    {
        Destroy(this.gameObject);
    }
 
}
