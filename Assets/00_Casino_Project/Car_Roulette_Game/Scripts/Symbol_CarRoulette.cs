namespace CarRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    using TMPro;

    public class Symbol_CarRoulette : MonoBehaviour
    {
        public bool isChooseItem = false;
        public bool isUserBet = false;
        private Image symbolImage;
        public int id;
        public int winMultiplierVal = 0;
        public Material matOutlineEffect;
        //public Transform trMy;
        public GameObject goMoveTarget;

        [SerializeField] public int _totalSymbolChip;
        [SerializeField] public int _totalSymbolMyChip;

        public TextMeshProUGUI txtTotalChip;
        public TextMeshProUGUI txtTotalMyChip;

        public List<CarRoulette_ChipManager> _otherChipList;
        public List<CarRoulette_ChipManager> _combineChipList;

        public Animator animWin;

        private void OnEnable()
        {
            // EventManager._SELECTED_BET_SEND += IM_SELECTED;
        }

        private void OnDisable()
        {
            // EventManager._SELECTED_BET_SEND -= IM_NOT_SELECTED;
        }

        public void IM_SELECTED(string name)
        {
            CarRoulette_GameManager.instance.USER_SEND_BET(name);
        }

        public void IM_NOT_SELECTED(string action)
        {
            //Glow.transform.localScale = Vector3.zero;
        }

        public void Start()
        {
            if (isChooseItem)
            {
                GetComponent<Button>().onClick.AddListener(OnClickItem);
            }
            else
            {
                symbolImage = GetComponent<Image>();
            }
        }

        public void UserChipCalculate(bool add, CarRoulette_ChipManager chip)
        {
            if (add)
            {
                isUserBet = true;

                _totalSymbolMyChip += chip.val;
            }
            else
            {
                _totalSymbolMyChip -= chip.val;
            }

            txtTotalMyChip.text = _totalSymbolMyChip.ToString();
            txtTotalChip.text = (_totalSymbolMyChip + _totalSymbolChip).ToString();
            _combineChipList.Add(chip);
        }

        public void OtherUserChipCalculate(bool add, CarRoulette_ChipManager chip)
        {
            if (add)
            {
                _totalSymbolChip += chip.val;
            }
            else
            {
                _totalSymbolChip -= chip.val;
            }

            txtTotalChip.text = (_totalSymbolMyChip + _totalSymbolChip).ToString();

            //_otherChipList.Add(chip);
            _combineChipList.Add(chip);
        }

        public void OnClickItem()
        {
            if (CarRoulette_UIManager._instance.isRebet)
                return;

            int betVal = CarRoulette_GameManager.instance._selectedBetItem.intValue;
            Car_Roulette_Sound.Inst.BtnSFX(1);
            //CarRoulette_GameManager.instance.currBetVal = betVal;
            // CarRoulette_GameManager.instance.currSideID = id;

            if (CarRoulette_GameManager.instance.saveManager._userBetDataClass.myUserBetCounter > 3)
            {
                if (!isUserBet)
                    CarRoulette_UIManager._instance.CheckMaxinumBetArea();
                else
                    CarRoulette_GameManager.instance.USER_SEND_BET(id.ToString());
            }
            else
            {
                CarRoulette_GameManager.instance.USER_SEND_BET(id.ToString());
            }
        }

        public void OnClickOtherItem(int betVal)
        {
            Car_Roulette_Sound.Inst.BtnSFX(1);
            CarRoulette_GameManager.instance.RouletteItemMethod(id, betVal, gameObject, goMoveTarget, UserStatus.OTHERUSER);
        }

        public void SetRed(float activeTime)
        {
            // symbolImage.color = Color.red;
            // StartCoroutine(OnReset(activeTime));
        }

        IEnumerator OnReset(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            symbolImage.color = Color.white;
        }

        [SerializeField]
        int counter = 0;
        public void AnimatedItem(Vector3 passVector, float duration, Ease passEase)
        {
            counter++;
            symbolImage.material = matOutlineEffect;
            transform.DOScale(passVector, duration).SetEase(passEase).OnComplete(() =>
            RevertAnimatedItem(duration, passEase));
        }

        IEnumerator PlaySpinSoundCoroutine(float duration)
        {
            // Adjust this delay based on when you want the sound to start playing during the scale change
            yield return new WaitForSeconds(duration * CarRoulette_GameManager.instance.hitTime);
            //Car_Roulette_Sound.Inst.PlayReel(duration);
            // Add your sound playing logic here
            // For example, if you are using Unity's AudioSource:
            // GetComponent<AudioSource>().Play();
        }

        public void RevertAnimatedItem(float duration, Ease paaEase)
        {
            transform.DOKill();
            symbolImage.material = null;
            transform.DOScale(Vector3.one, duration).SetEase(paaEase);
        }

        public void WinItemAnimated(Vector3 passVector, float duration, Ease paaEase)
        {
            Debug.Log("FINISH DRAWING");
            transform.DOKill();
            symbolImage.material = matOutlineEffect;
            Car_Roulette_Sound.Inst.PlaySFX_Others(7);
            transform.DOScale(passVector, duration).SetEase(paaEase).SetLoops(-1, LoopType.Yoyo);
        }

        public void ResetItemValue()
        {
            _totalSymbolChip = 0;
            _totalSymbolMyChip = 0;

            txtTotalChip.text = "00";
            txtTotalMyChip.text = "00";
            isUserBet = false;
            _combineChipList.Clear();
        }

        public int WinChipCalculate()
        {
            int val = 0;
            for (int i = 0; i < _combineChipList.Count; i++)
            {
                if (_combineChipList[i].userChipStatus == UserStatus.MYUSER)
                {
                    val += _combineChipList[i].val * winMultiplierVal;
                }
            }
            return val;
        }
    }
}

