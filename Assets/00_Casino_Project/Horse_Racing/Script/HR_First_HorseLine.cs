using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_First_HorseLine : MonoBehaviour
{
    public static HR_First_HorseLine Inst;
    [SerializeField] List<GameObject> Winner_Leble;
    public GameObject First_Horse_No;
    int Horse_No;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        Horse_No = 0;
        //First_Horse_No = HR_Manager.Inst.All_Hore_Animation[0].gameObject;
        //transform.localPosition = new Vector2(First_Horse_No.transform.localPosition.x + 53f, transform.localPosition.y);
    }
    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    int no = int.Parse(collision.gameObject.name.Substring(5));
    //    for (int i = 0; i < Winner_Leble.Count; i++)
    //    {
    //        if (i == (no - 1))
    //        {
    //            Winner_Leble[no - 1].SetActive(true);
    //            First_Horse_No = HR_Manager.Inst.All_Hore_Animation[no - 1].gameObject;
    //        }
    //        else
    //            Winner_Leble[i].SetActive(false);
    //    }
    //}
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Horse"))
        {
            int no = int.Parse(collision.gameObject.name.Substring(5));
            for (int i = 0; i < Winner_Leble.Count; i++)
            {
                if (i == (no - 1))
                {
                    Winner_Leble[no - 1].SetActive(true);
                    Horse_No = no - 1;
                    First_Horse_No = HR_Manager.Inst.All_Hore_Animation[no - 1].gameObject;
                }
                else
                    Winner_Leble[i].SetActive(false);
            }
            for (int i = 0; i < Winner_Leble.Count; i++)
            {
                if (i != Horse_No)
                    Winner_Leble[i].SetActive(false);
            }
        }
    }


    private void Update()
    {
        if(HR_All_Horse_Move.Inst.Run_Move)
            transform.localPosition = new Vector2(First_Horse_No.transform.localPosition.x + 50f, transform.localPosition.y);
    }
    public void RESET_HORSE_WIN_LABLE()
    {
        transform.localPosition = new Vector2(First_Horse_No.transform.localPosition.x+50f, transform.localPosition.y);
        //FIRST_LINE_LABLE();
    }

    public void FIRST_LINE_LABLE()
    {
        int no = Random.Range(0, 5);
        for (int i = 0; i < Winner_Leble.Count; i++)
        {
            if (i == no)
            {
                Winner_Leble[i].SetActive(true);
                First_Horse_No = HR_Manager.Inst.All_Hore_Animation[i].gameObject;
            }
            else
                Winner_Leble[i].SetActive(false);
        }
    }
}
