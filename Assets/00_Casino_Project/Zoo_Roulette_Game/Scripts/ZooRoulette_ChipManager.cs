namespace ZooRoulette_Game
{
    using DG.Tweening;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class ZooRoulette_ChipManager : MonoBehaviour
    {
        public static ZooRoulette_ChipManager Inst;
        public int val;
        public float duration;
        public Image imgChip;
        public UserStatus userChipStatus;

        void Awake()
        {
            Inst = this;
        }

        void Start()
        {
        }

        private void OnEnable()
        {
            ZooRoulette_EventManager._COIN_KILL_PREFAB += IM_KILL;
            ZooRoulette_EventManager._COIN_WIN_MOVE_PREFAB += WIN_MOVE_ANIM;
        }

        private void OnDisable()
        {
            ZooRoulette_EventManager._COIN_KILL_PREFAB -= IM_KILL;
            ZooRoulette_EventManager._COIN_WIN_MOVE_PREFAB -= WIN_MOVE_ANIM;
        }

        public void SET_COIN(string Coin)
        {
            for (int i = 0; i < ZooRoulette_EventManager.Inst._chipSptList.Count; i++)
            {
                if (ZooRoulette_EventManager.Inst._chipSptList[i].name.Equals(Coin))
                {
                    val = int.Parse(Coin);
                    imgChip.sprite = ZooRoulette_EventManager.Inst._chipSptList[i];
                }
            }
        }
        public void Move_Anim(Vector3 target, float moveTime = 1f)
        {
            this.transform.DOScale(idelChipScale, duration).SetEase(Ease.Linear);
            this.transform.DOMove(target, moveTime).SetEase(Ease.Linear).
                 OnComplete(() =>
                 {
                     StartCoroutine(DelayedComplete());
                 });
        }

        public void Place_Anim(Vector3 target)
        {
            this.transform.position = target;
            this.transform.localScale = new Vector3(idelChipScale, idelChipScale, 1f);
        }

        public float idelChipScale;
        public Vector3 animChipScale;

        //public void ComplateChipMove()
        //{
        //    this.transform.DOScale(animChipScale, 0.15f).SetEase(Ease.OutBack);
        //}

        public void ComplateChipMove()
        {
            this.transform.DOScale(animChipScale, resetTimer).SetEase(Ease.Linear).OnComplete(() =>
            {
                this.transform.DOScale(idelChipScale, resetTimer).SetEase(Ease.Linear).OnComplete(() =>
                {
                    imgChip.DOFade(0, 0.5f).SetDelay(0.2f);
                });
            });
        }
       
        public float resetTimer = 0f;
        public float delayTime;
        private IEnumerator DelayedComplete()
        {
            yield return new WaitForSeconds(delayTime); // Adjust the delay duration as needed
                                                        // This code will be executed after the delay
                                                        //Debug.Log("Tween complete after delay");
            ComplateChipMove();
        }

        public void IM_KILL()
        {
            Destroy(this.gameObject);
        }
        public void WIN_MOVE_ANIM()
        {
            GameObject g = ZooRoulette_GameManager.instance.winRoulette.gameObject;
            Vector3 target = ZooRoulette_GameManager.instance.getRandomPoint(g, 0);
            iTween.MoveTo(this.gameObject, iTween.Hash("position", target, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
            StartCoroutine(Win_User_Delay());
        }

        public void LOSS_MOVE_ANIM()
        {
            GameObject g = ZooRoulette_GameManager.instance.winRoulette.gameObject;
            Vector3 target = ZooRoulette_GameManager.instance.getRandomPoint(g, 0);
            iTween.MoveTo(this.gameObject, iTween.Hash("position", target, "time", 0.5f, "easetype", iTween.EaseType.easeOutExpo));
            StartCoroutine(Loss_User_Delay());
        }

        IEnumerator Win_User_Delay()
        {
            float rn = UnityEngine.Random.Range(1f, 1.3f);
            yield return new WaitForSeconds(rn);
            Win_Player_Coin_Move();
        }

        IEnumerator Loss_User_Delay()
        {
            float rn = UnityEngine.Random.Range(ZooRoulette_GameManager.instance.minLoss,
                ZooRoulette_GameManager.instance.maxLoss);
            yield return new WaitForSeconds(rn);
            Loss_Player_Coin_Move();
        }

        public void Win_Player_Coin_Move()
        {
            GameObject g = ZooRoulette_GameManager.instance.goMyUserChal;
            Vector3 Pos = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
            this.transform.DOMove(Pos, 1f).SetEase(Ease.Linear).
                 OnComplete(() => { Destroy(gameObject); });
        }

        public void Loss_Player_Coin_Move()
        {
            if (ZooRoulette_GameManager.instance.isLossClip)
            {
                Zoo_Roulette_Sound.Inst.PlayMixCoin();
                ZooRoulette_GameManager.instance.isLossClip = false;
            }

            // Debug.Log("Loss_Player_Coin_Move");
            GameObject g = ZooRoulette_GameManager.instance.goOtherUserChal;
            Vector3 Pos = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
            //this.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.OutExpo);
            this.transform.DOMove(Pos, 0.7f).SetEase(Ease.Linear).
                 OnComplete(() =>
                 {
                 //if(CarRoulette_GameManager.instance.isLossClip)
                 //{
                 //    Car_Roulette_Sound.Inst.PlayMixCoin();
                 //    CarRoulette_GameManager.instance.isLossClip = false;
                 //}
                 Destroy(gameObject, 0.1f);
                 });
        }
    }
}