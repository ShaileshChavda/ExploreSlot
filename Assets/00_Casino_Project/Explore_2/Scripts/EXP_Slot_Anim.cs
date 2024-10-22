using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EXP_Slot_Anim : MonoBehaviour
{
    public static EXP_Slot_Anim Inst;
    [SerializeField] GameObject Anim_First_OBJ, Anim_Second_OBJ;
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
            EXP_Spin_Hendler.Inst.Last_Spiner = 2;
        }
        else
        {
            Anim_First_OBJ.SetActive(true);
            Anim_Second_OBJ.SetActive(false);
            EXP_Spin_Hendler.Inst.Last_Spiner = 1;
        }
    }
}
