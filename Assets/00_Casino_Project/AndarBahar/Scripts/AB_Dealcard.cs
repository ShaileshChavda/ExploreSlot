using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AB_Dealcard : MonoBehaviour
{
    public static AB_Dealcard Inst;
    [SerializeField] GameObject Andar_Card_View, Bahar_Card_View;
    [SerializeField] GameObject PFB_Card;
    public List<GameObject> ThrowCardList;
    public GameObject JocketCard;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    public void Card_Throw_Anim_Start()
    {
        StartCoroutine(dealingCardAnimation());
    }

    public void REJOIN_Card_Throw_Anim_Start(int Remain_Card)
    {
        StartCoroutine(REJOIN_dealingCardAnimation(Remain_Card));
    }

    IEnumerator dealingCardAnimation()
    {
        int under = 0;
        float Andar_x = 0;
        float Bahar_x = 0;
        for (int i = 0; i < AB_Manager.Inst.ThrowCardJsonList.Count; i++)
        {
            string tcard = AB_Manager.Inst.ThrowCardJsonList[i].ToString().Trim(Config.Inst.trim_char_arry);
            GameObject cardRect = Instantiate(PFB_Card) as GameObject;
            cardRect.name = "dealCardPrefab";
            Vector3 target;
            if (under.Equals(0))
            {
                under = 1;
                cardRect.transform.SetParent(Andar_Card_View.transform, false);
                cardRect.transform.position = new Vector3(cardRect.transform.position.x + Andar_x, cardRect.transform.position.y, cardRect.transform.position.z);
                Andar_x = Andar_x + 0.3f;
            }
            else
            {
                under = 0;
                cardRect.transform.SetParent(Bahar_Card_View.transform, false);
                cardRect.transform.position = new Vector3(cardRect.transform.position.x + Bahar_x, cardRect.transform.position.y, cardRect.transform.position.z);
                Bahar_x = Bahar_x + 0.3f;
            }
            yield return new WaitForSeconds(.1f);
            AB_SoundManager.Inst.PlaySFX(4);
            iTween.RotateBy(cardRect.gameObject, iTween.Hash("y", 0.5f, "time", 0.45f, "easetype", iTween.EaseType.linear));
            yield return new WaitForSeconds(.35f);

            for (int j = 0; j < AB_Manager.Inst.Card_List.Count; j++)
            {
                if (AB_Manager.Inst.Card_List[j].name.ToUpper().Equals(tcard))
                    cardRect.GetComponent<Image>().sprite = AB_Manager.Inst.Card_List[j];
            }

            yield return new WaitForSeconds(.2f);
            if (cardRect.gameObject != null)
            {
                ThrowCardList.Add(cardRect);
            }
        }
    }

    IEnumerator REJOIN_dealingCardAnimation(int Remain_Card)
    {
        int under = 0;
        float Andar_x = 0;
        float Bahar_x = 0;
        for (int i = 0; i < AB_Manager.Inst.ThrowCardJsonList.Count - Remain_Card; i++)
        {
            string tcard = AB_Manager.Inst.ThrowCardJsonList[i].ToString().Trim(Config.Inst.trim_char_arry);
            GameObject cardRect = Instantiate(PFB_Card) as GameObject;
            cardRect.name = "dealCardPrefab";
            Vector3 target;
            if (under.Equals(0))
            {
                under = 1;
                cardRect.transform.SetParent(Andar_Card_View.transform, false);
                cardRect.transform.position = new Vector3(cardRect.transform.position.x + Andar_x, cardRect.transform.position.y, cardRect.transform.position.z);
                Andar_x = Andar_x + 0.3f;
            }
            else
            {
                under = 0;
                cardRect.transform.SetParent(Bahar_Card_View.transform, false);
                cardRect.transform.position = new Vector3(cardRect.transform.position.x + Bahar_x, cardRect.transform.position.y, cardRect.transform.position.z);
                Bahar_x = Bahar_x + 0.3f;
            }
            iTween.RotateBy(cardRect.gameObject, iTween.Hash("y", 0.5f, "time", 0.1f, "easetype", iTween.EaseType.linear));
            for (int j = 0; j < AB_Manager.Inst.Card_List.Count; j++)
            {
                if (AB_Manager.Inst.Card_List[j].name.ToUpper().Equals(tcard))
                    cardRect.GetComponent<Image>().sprite = AB_Manager.Inst.Card_List[j];
            }
            if (cardRect.gameObject != null)
            {
                ThrowCardList.Add(cardRect);
            }
        }
        int Remain_Count = AB_Manager.Inst.ThrowCardJsonList.Count - Remain_Card;
        for (int i = Remain_Count; i < AB_Manager.Inst.ThrowCardJsonList.Count; i++)
        {
            string tcard = AB_Manager.Inst.ThrowCardJsonList[i].ToString().Trim(Config.Inst.trim_char_arry);
            GameObject cardRect = Instantiate(PFB_Card) as GameObject;
            cardRect.name = "dealCardPrefab";
            Vector3 target;
            if (under.Equals(0))
            {
                under = 1;
                cardRect.transform.SetParent(Andar_Card_View.transform, false);
                cardRect.transform.position = new Vector3(cardRect.transform.position.x + Andar_x, cardRect.transform.position.y, cardRect.transform.position.z);
                Andar_x = Andar_x + 0.3f;
            }
            else
            {
                under = 0;
                cardRect.transform.SetParent(Bahar_Card_View.transform, false);
                cardRect.transform.position = new Vector3(cardRect.transform.position.x + Bahar_x, cardRect.transform.position.y, cardRect.transform.position.z);
                Bahar_x = Bahar_x + 0.3f;
            }
            yield return new WaitForSeconds(.1f);
            AB_SoundManager.Inst.PlaySFX(4);
            iTween.RotateBy(cardRect.gameObject, iTween.Hash("y", 0.5f, "time", 0.45f, "easetype", iTween.EaseType.linear));
            yield return new WaitForSeconds(.35f);

            for (int j = 0; j < AB_Manager.Inst.Card_List.Count; j++)
            {
                if (AB_Manager.Inst.Card_List[j].name.ToUpper().Equals(tcard))
                    cardRect.GetComponent<Image>().sprite = AB_Manager.Inst.Card_List[j];
            }

            yield return new WaitForSeconds(.2f);
            if (cardRect.gameObject != null)
            {
                ThrowCardList.Add(cardRect);
            }
        }
    }

    public void REJOIN_DEAL_CARD()
    {
        int under = 0;
        float Andar_x = 0;
        float Bahar_x = 0;
        for (int i = 0; i < AB_Manager.Inst.ThrowCardJsonList.Count; i++)
        {
            string tcard = AB_Manager.Inst.ThrowCardJsonList[i].ToString().Trim(Config.Inst.trim_char_arry);
            GameObject cardRect = Instantiate(PFB_Card) as GameObject;
            cardRect.name = "dealCardPrefab";
            Vector3 target;
            if (under.Equals(0))
            {
                under = 1;
                cardRect.transform.SetParent(Andar_Card_View.transform, false);
                cardRect.transform.position = new Vector3(cardRect.transform.position.x + Andar_x, cardRect.transform.position.y, cardRect.transform.position.z);
                Andar_x = Andar_x + 0.3f;
            }
            else
            {
                under = 0;
                cardRect.transform.SetParent(Bahar_Card_View.transform, false);
                cardRect.transform.position = new Vector3(cardRect.transform.position.x + Bahar_x, cardRect.transform.position.y, cardRect.transform.position.z);
                Bahar_x = Bahar_x + 0.3f;
            }
            iTween.RotateBy(cardRect.gameObject, iTween.Hash("y", 0.5f, "time", 0.2f, "easetype", iTween.EaseType.linear));

            for (int j = 0; j < AB_Manager.Inst.Card_List.Count; j++)
            {
                if (AB_Manager.Inst.Card_List[j].name.ToUpper().Equals(tcard))
                    cardRect.GetComponent<Image>().sprite = AB_Manager.Inst.Card_List[j];
            }
            if (cardRect.gameObject != null)
            {
                ThrowCardList.Add(cardRect);
            }
        }
    }


    public IEnumerator Spin_JockerCard(string Joker_Name)
    {
        yield return new WaitForSeconds(2f);
        JocketCard.transform.localScale = Vector3.one;
        AB_SoundManager.Inst.PlaySFX(4);
        iTween.RotateBy(JocketCard.gameObject, iTween.Hash("y", 0.5f, "time", 0.2f, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(.2f);
        for (int j = 0; j < AB_Manager.Inst.Card_List.Count; j++)
        {
            if (AB_Manager.Inst.Card_List[j].name.ToUpper().Equals(Joker_Name))
                JocketCard.GetComponent<Image>().sprite = AB_Manager.Inst.Card_List[j];
        }
        string[] split_XCard = Joker_Name.Split('-');

        if (split_XCard[1].Equals("11"))
            AB_Full_History_Manager.Inst.Txt_Joker_text.text = "J";
        else if (split_XCard[1].Equals("12"))
            AB_Full_History_Manager.Inst.Txt_Joker_text.text = "Q";
        else if (split_XCard[1].Equals("13"))
            AB_Full_History_Manager.Inst.Txt_Joker_text.text = "K";
        else if (split_XCard[1].Equals("1"))
            AB_Full_History_Manager.Inst.Txt_Joker_text.text = "A";
        else
            AB_Full_History_Manager.Inst.Txt_Joker_text.text = split_XCard[1];
    }
    public void REJOIN_Spin_JockerCard(string Joker_Name)
    {
        JocketCard.transform.localScale = Vector3.one;
        iTween.RotateBy(JocketCard.gameObject, iTween.Hash("y", 0.5f, "time", 0.2f, "easetype", iTween.EaseType.linear));
        for (int j = 0; j < AB_Manager.Inst.Card_List.Count; j++)
        {
            if (AB_Manager.Inst.Card_List[j].name.ToUpper().Equals(Joker_Name))
                JocketCard.GetComponent<Image>().sprite = AB_Manager.Inst.Card_List[j];
        }
        string[] split_XCard = Joker_Name.Split('-');
        if (split_XCard[1].Equals("11"))
            AB_Full_History_Manager.Inst.Txt_Joker_text.text = "J";
        else if (split_XCard[1].Equals("12"))
            AB_Full_History_Manager.Inst.Txt_Joker_text.text = "Q";
        else if (split_XCard[1].Equals("13"))
            AB_Full_History_Manager.Inst.Txt_Joker_text.text = "K";
        else if (split_XCard[1].Equals("1"))
            AB_Full_History_Manager.Inst.Txt_Joker_text.text = "A";
        else
            AB_Full_History_Manager.Inst.Txt_Joker_text.text = split_XCard[1];
    }

    public void Reset_Cards()
    {
        JocketCard.transform.GetComponent<Image>().sprite = AB_PlayerManager.Inst.BackCard;
        Vector3 newRotation = new Vector3(-20f, 180, 0);
        JocketCard.transform.eulerAngles = newRotation;
        JocketCard.transform.localScale = Vector3.zero;
        Destroyee_Old_ThrowCards();
    }
    public void Destroyee_Old_ThrowCards()
    {
        if (ThrowCardList.Count > 0)
        {
            for (int i = 0; i < ThrowCardList.Count; i++)
            {
                Destroy(ThrowCardList[i]);
            }
        }
    }
}
