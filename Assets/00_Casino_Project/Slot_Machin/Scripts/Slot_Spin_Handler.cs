using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Spin_Handler : MonoBehaviour
{
    public static Slot_Spin_Handler Inst;
    [Header("<<< SPECIAL LIST >>>")]
    public List<Sprite> All_Item_Sprite;
    public int Last_Spiner;

    [Header("<<< SPINER RESULT DATA >>>")]
    public List<Image> Item_Images_0;
    public List<Image> Item_Images_1;
    public List<Image> Item_Images_2;
    public List<Image> Item_Images_3;
    public List<Image> Item_Images_4;
    public List<Image> Item_Images_5;
    public List<Image> Item_Images_6;
    public List<Image> Item_Images_7;
    public List<Image> Item_Images_8;
    public List<Image> Item_Images_9;
    public List<Image> Item_Images_10;
    public List<Image> Item_Images_11;
    public List<Image> Item_Images_12;
    public List<Image> Item_Images_13;
    public List<Image> Item_Images_14;
    public List<List<Image>> All_Item_Images_List;

    // Start is called before the first frame update
    void Awake()
    {
        Last_Spiner = 1;
        Inst = this;
        _Create_Item_Image_List();
    }

    public void SET_SPIN_DATA(JSONObject data)
    {
            for (int i = 0; i < data.GetField("win_line_data").Count; i++)
            {
                for (int j = 0; j < All_Item_Sprite.Count; j++)
                {
                    if (data.GetField("win_line_data")[i].ToString().Trim(Config.Inst.trim_char_arry).Equals(All_Item_Sprite[j].name))
                    {
                       for (int p = 0; p < All_Item_Images_List[i].Count; p++)
                           All_Item_Images_List[i][p].sprite = All_Item_Sprite[j];
                               
                    }
                }
            }
    }

    void _Create_Item_Image_List()
    {
        All_Item_Images_List = new List<List<Image>>();
        All_Item_Images_List.Insert(0,Item_Images_0);
        All_Item_Images_List.Insert(1,Item_Images_1);
        All_Item_Images_List.Insert(2,Item_Images_2);
        All_Item_Images_List.Insert(3,Item_Images_3);
        All_Item_Images_List.Insert(4,Item_Images_4);
        All_Item_Images_List.Insert(5,Item_Images_5);
        All_Item_Images_List.Insert(6,Item_Images_6);
        All_Item_Images_List.Insert(7,Item_Images_7);
        All_Item_Images_List.Insert(8,Item_Images_8);
        All_Item_Images_List.Insert(9,Item_Images_9);
        All_Item_Images_List.Insert(10,Item_Images_10);
        All_Item_Images_List.Insert(11,Item_Images_11);
        All_Item_Images_List.Insert(12,Item_Images_12);
        All_Item_Images_List.Insert(13,Item_Images_13);
        All_Item_Images_List.Insert(14,Item_Images_14);
    }
}
