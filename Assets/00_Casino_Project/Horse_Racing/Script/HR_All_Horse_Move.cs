using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_All_Horse_Move : MonoBehaviour
{
    public static HR_All_Horse_Move Inst;
    public bool Run_Move;
    public float MySpeed;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Run_Move)
            transform.localPosition = new Vector2(transform.localPosition.x + MySpeed, transform.localPosition.y);

        if (HR_GroundManager.Inst.Ground_Reset_Start)
            transform.position = new Vector2(HR_GroundManager.Inst.Horse_Defoult_Pos.transform.position.x, transform.position.y);
    }
    public void SET_REJOIN_POSITION(float Xpos)
    {
        transform.localPosition = new Vector2(Xpos, transform.localPosition.y);
    }
}
