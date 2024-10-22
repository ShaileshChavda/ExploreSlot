namespace CarRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class CarRoulette_HistoryManager : MonoBehaviour
    {
        public int clampHistoryEntry = 0;
        public Transform trParant = null;
        public GameObject goHistoryClone;

        public List<GameObject> goHistoryPrefabList = new List<GameObject>();
        // Start is called before the first frame update
        public List<Symbol_CarRoulette> _symbol_CarRouletteList;
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void CloneHistoryEntry(GameObject item, bool isNew = false)
        {
            GameObject go = Instantiate(goHistoryClone);
            go.transform.SetParent(trParant, false);
            go.transform.SetAsFirstSibling();
            go.transform.localScale = Vector3.one;
            go.name = "WinItem_" + item.name;
            go.GetComponent<Image>().sprite = item.GetComponent<Image>().sprite;
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
                int getIndex = int.Parse(data.GetField("last_win_cards")[i].ToString().Trim(Config.Inst.trim_char_arry));
                SET_HISTORY_CARD_DATA((getIndex - 1));
            }
        }
        public void SET_HISTORY_CARD_DATA(int index, bool isNew = false)
        {
            CloneHistoryEntry(_symbol_CarRouletteList[index].gameObject, isNew);

            for (int i = 0; i < trParant.childCount; i++)
            {
                if (i == 0)
                {
                    trParant.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
                    trParant.GetChild(i).localScale = new Vector3(1.3f, 1.3f, 1.3f);
                }
                else
                {
                    trParant.GetChild(i).localScale = Vector3.one;
                    trParant.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }
}
