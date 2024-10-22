using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crash_Full_HistoryManager : MonoBehaviour
{
    public static Crash_Full_HistoryManager Inst;
    [SerializeField] private GameObject Full_History_Screen;
    [SerializeField] List<Crash_Hist_Annal> Annal_Box_List;  
   
    public GameObject historyPrefab;
    public GameObject listContainer;
    private List<float> finalList = new List<float>();
    void Start()
    {
        Inst = this;
    }
    public void SET_DATA(JSONObject data)
    {
        //StartCoroutine(SetDataForDots(data));        // Dont delete
        StartCoroutine(SetDataForText(data));
    }
    IEnumerator SetDataForDots(JSONObject data) // Dont delete
    {
        for (int i = 0; i < data.GetField("report").Count; i++)
        {
            yield return new WaitForSeconds(0.01f);
            Annal_Box_List[i].SET_ANNAL_Box(data.GetField("report")[i]);
        }        
    }
    IEnumerator SetDataForText(JSONObject data)
    {       
        for (int i = data.GetField("report").Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(0.01f);
            ReportList(data.GetField("report")[i]);
        }
    }
    public void ReportList(JSONObject data)
    {
        for (int i = data.GetField("result").Count-1; i >= 0 ; i--) 
        {
            string result = data.GetField("result")[i].ToString().Trim(Config.Inst.trim_char_arry);
            string[] split_XCard = result.Split('|');
            float crashAt = float.Parse(split_XCard[0]);
            finalList.Add(crashAt);
            SpawnHistoryPrefabe(crashAt);
        }
    }
    private void SpawnHistoryPrefabe(float crashAt)
    {
        GameObject listItem = Instantiate(historyPrefab) as GameObject;
        Crash_Hist_Annual_ListItem.Inst.SetTextAndColor(listItem,crashAt);
        listItem.transform.SetParent(listContainer.transform, false);       
    }
    public void Open_Full_History()
    {
        GS.Inst.iTwin_Open(Full_History_Screen);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.CRASH_RESULT_HISTORY());
    }
    public void Close_Full_History()
    {       
        GS.Inst.iTwin_Close(Full_History_Screen, 0.3f);
       // Clear_OLD_History();
        Clear_OLD_History_New();
    }

    public void Clear_OLD_History()
    {       
        if (Annal_Box_List.Count > 0)
        {           
            for (int i = 0; i < Annal_Box_List.Count; i++)
            {
                Annal_Box_List[i].Reset_ANNAL_Box(); ;
            }          
        }
    }
    public void Clear_OLD_History_New()
    {
        finalList.Clear();
        foreach (Transform child in listContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
