using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_PFB_WinCoinMove : MonoBehaviour
{
    public static Slot_PFB_WinCoinMove Inst;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }
    public void Move(Vector3 obj)
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", obj, "time", 0.3f, "easetype", iTween.EaseType.easeOutExpo));
        Invoke(nameof(Kill), 0.2f);
    }
    void Kill()
    {
        Destroy(this.gameObject);
    }
}
