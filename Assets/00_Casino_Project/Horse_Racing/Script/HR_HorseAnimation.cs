using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HR_HorseAnimation : MonoBehaviour
{
    public static HR_HorseAnimation Inst;
    public Animator Horse_Anim, Dust_Partical_Anim;
    public int MyNo;
    public GameObject Dust_Partical;
    public List<GameObject> Positions;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        if (!HR_All_Horse_Move.Inst.Run_Move)
            Horse_Idel_Start();
    }
    public void Horse_Idel_Start()
    {
        Horse_Anim.Play("IdelAnim",0,Random.Range(0.10f,0.50f));
    }
    public void Horse_Run_Start()
    {
        Horse_Anim.Play("RunAnim");
        Dust_Partical.SetActive(true);
    }
    public void RESET_HORSE()
    {
        Dust_Partical.SetActive(false);
        Horse_Anim.StopPlayback();
        transform.localPosition = new Vector2(0, transform.localPosition.y);
        Horse_Idel_Start();
    }
    public void Move_New_Position(int Index)
    {
        if(HR_All_Horse_Move.Inst.Run_Move)
            iTween.MoveTo(this.gameObject, iTween.Hash("position", new Vector3(Positions[Index].transform.position.x, transform.position.y, transform.position.z), "time", 3f, "easetype", iTween.EaseType.linear));
    }
    public void Move_Win_Position(int Index)
    {
         iTween.MoveTo(this.gameObject, iTween.Hash("position", new Vector3(Positions[Index].transform.position.x, transform.position.y, transform.position.z), "time", 1.5f, "easetype", iTween.EaseType.linear));
    }
}
