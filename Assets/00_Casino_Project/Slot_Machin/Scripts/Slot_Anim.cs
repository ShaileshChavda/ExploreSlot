using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Anim : MonoBehaviour
{
    public static Slot_Anim Inst;
    [SerializeField] GameObject Anim_First_OBJ,Anim_Second_OBJ;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    public void Stop_Anim_First()
    {
        if (!Anim_Second_OBJ.activeSelf)
        {
            Anim_First_OBJ.SetActive(false);
            Anim_Second_OBJ.SetActive(true);
            Slot_Spin_Handler.Inst.Last_Spiner = 2;
        }
        else
        {
            Anim_First_OBJ.SetActive(true);
            Anim_Second_OBJ.SetActive(false);
            Slot_Spin_Handler.Inst.Last_Spiner = 1;
        }
    }
}
