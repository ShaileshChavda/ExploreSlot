using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_PFB_JackpotCoin : MonoBehaviour
{
    public static HR_PFB_JackpotCoin Inst;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        Invoke(nameof(Kill), 1f);
    }
    public void Move_Anim(Vector3 target)
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", target, "time", 1f, "easetype", iTween.EaseType.easeOutExpo));
    }
    void Kill()
    {
        Destroy(this.gameObject);
    }
}
