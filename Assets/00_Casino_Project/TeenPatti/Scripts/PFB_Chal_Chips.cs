using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_Chal_Chips : MonoBehaviour
{
    public static PFB_Chal_Chips Inst;
    [SerializeField] Text TxtChaal_chips;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void Chaal_Anim(string chalAmount)
    {
        TxtChaal_chips.text = chalAmount;
    }
}
