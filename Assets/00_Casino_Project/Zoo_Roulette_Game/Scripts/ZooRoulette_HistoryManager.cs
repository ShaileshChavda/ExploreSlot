namespace ZooRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class ZooRoulette_HistoryManager : MonoBehaviour
    {
        public int clampHistoryEntry = 0;
        public Transform trParant = null;
        public GameObject historyPrefabListItem;        
        private List<GameObject> goHistoryPrefabList = new List<GameObject>();

        public void CloneHistoryEntry(int itemId, bool isNew = false)
        {
            GameObject go = Instantiate(historyPrefabListItem);
            go.transform.SetParent(trParant, false);
            go.transform.SetAsFirstSibling();
            go.transform.localScale = Vector3.one;           
            go.transform.GetChild(0).GetComponent<Image>().sprite = ZooRoulette_UIManager._instance.animalSprite[itemId];
            goHistoryPrefabList.Add(go);
            //if (isNew)
            //{
            //    go.transform.localScale = go.transform.localScale + new Vector3(0.3f,0.3f,0.3f);
            //    go.transform.GetChild(0).gameObject.SetActive(true);
            //}
            ClampHistoryEntry();
        }

        public void ClampHistoryEntry()
        {
            if (goHistoryPrefabList.Count > clampHistoryEntry)
            {
                Debug.Log("CLAMP HISTORY ENTRY");
                goHistoryPrefabList.RemoveAt(clampHistoryEntry);
            }
        }

        public void SET_HISTORY(JSONObject data)
        {
            int totalHistory = data.GetField("last_win_cards").Count;
            Debug.Log("GET TOTAL HISTORY: " + totalHistory);

            for (int i = 0; i < totalHistory; i++)
            {
                int getIndex = ZooRoulette_GameManager.SplitStringToInt(data.GetField("last_win_cards")[i].ToString().Trim(Config.Inst.trim_char_arry));
                //Debug.Log("Naresh: HISTORY ID: " + getIndex);              
                SET_HISTORY_CARD_DATA((getIndex - 1));
            }
        }
        public void SET_HISTORY_CARD_DATA(int index, bool isNew = false)
        {
            CloneHistoryEntry(index, isNew);

            for (int i = 0; i < trParant.childCount; i++)
            {
                if (i == 0)
                {
                    trParant.GetChild(i).GetComponent<Image>().enabled = true;
                    trParant.GetChild(i).GetChild(0).localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    if(isNew)
                        StartCoroutine(OnOffHistoryPrefabeBg(trParant.GetChild(i).gameObject));
                }
                else
                {
                    trParant.GetChild(i).GetComponent<Image>().enabled = false;
                    trParant.GetChild(i).GetChild(0).localScale = Vector3.one;
                }
            }
        }
        private IEnumerator OnOffHistoryPrefabeBg(GameObject goObj)
        {
            for (int i = 0; i < 8; i++)
            {
                goObj.GetComponent<Image>().enabled = false;
                yield return new WaitForSeconds(0.3f);
                goObj.GetComponent<Image>().enabled = true;
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
