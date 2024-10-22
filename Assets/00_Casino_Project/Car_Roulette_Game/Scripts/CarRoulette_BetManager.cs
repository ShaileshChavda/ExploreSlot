namespace CarRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using DG.Tweening;

    public class CarRoulette_BetManager : MonoBehaviour
    {
        public int intValue;
        public Sprite sptEnable, sptDisable;
        public GameObject goFrame;
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClickBet);
        }

        public void OnClickBet()
        {
            Debug.Log("CLICK BET");
            CarRoulette_GameManager.instance.CurrentBetAnimetd(this);
        }

        public void AnimatedItem(float pos, float duration, Ease paaEase)
        {
            GetComponent<Image>().sprite = sptEnable;
            goFrame.SetActive(true);
            transform.GetComponent<RectTransform>().DOAnchorPosY(pos, duration).SetEase(paaEase);
        }

        public void ResetAnimatedItem(float pos, float duration, Ease paaEase)
        {
            GetComponent<Image>().sprite = sptDisable;
            goFrame.SetActive(false);
            transform.GetComponent<RectTransform>().DOAnchorPosY(pos, duration).SetEase(paaEase);
        }
    }
}
