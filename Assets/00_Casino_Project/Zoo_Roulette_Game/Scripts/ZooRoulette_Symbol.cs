namespace ZooRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    using TMPro;

    public class ZooRoulette_Symbol : MonoBehaviour
    {
        public bool isChooseItem = false;
        public bool isUserBet = false;
        private Image iconBg;
        [SerializeField] private GameObject iconGO;

        public int id;
        public int winMultiplierVal = 0;
        public GameObject goMoveTarget;

        [SerializeField] public int _totalSymbolChip;
        [SerializeField] public int _totalSymbolMyChip;

        public TextMeshProUGUI txtTotalChip;
        public TextMeshProUGUI txtTotalMyChip;

        public List<ZooRoulette_ChipManager> _combineChipList;

        public Animator animWin;
        public GameObject blackPatchImg;

        public void Start()
        {
            if (isChooseItem)
            {
                GetComponent<Button>().onClick.AddListener(OnClickItem);
            }
            else
            {
                iconGO = transform.GetChild(1).gameObject;
                iconBg = GetComponent<Image>();
            }            
        }
        public void OnClickItem()
        {
            if (ZooRoulette_UIManager._instance.isRebet)
                return;


            blackPatchImg.GetComponent<Image>().DOFade(0, 0.3f).SetEase(Ease.Linear).OnComplete(() => blackPatchImg.GetComponent<Image>().DOFade(0.5f, 0.3f));

            int betVal = ZooRoulette_GameManager.instance._selectedBetItem.intValue;
            Zoo_Roulette_Sound.Inst.BtnSFX(1);

            ZooRoulette_GameManager.instance.USER_SEND_BET(id.ToString());
        }
        public void UserChipCalculate(bool add, ZooRoulette_ChipManager chip)
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

        public void OtherUserChipCalculate(bool add, ZooRoulette_ChipManager chip)
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
           
            _combineChipList.Add(chip);
        }

        

        public void OnClickOtherItem(int betVal)
        {
            Zoo_Roulette_Sound.Inst.BtnSFX(1);
            ZooRoulette_GameManager.instance.RouletteItemMethod(id, betVal, gameObject, goMoveTarget, UserStatus.OTHERUSER);
        }

        [SerializeField]
        int counter = 0;
        public void AnimatedItem(Vector3 passVector, float duration, Ease passEase)
        {
            counter++;
            //symbolImage.material = matOutlineEffect;
            iconBg.sprite = ZooRoulette_UIManager._instance.yellowImage;
            iconGO.transform.DOScale(1.1f, duration).SetEase(passEase).OnComplete(() => RevertAnimatedItem1(duration, passEase));
            transform.DOScale(passVector, duration).SetEase(passEase).OnComplete(() => RevertAnimatedItem(duration, passEase));
        }
              

        public void RevertAnimatedItem(float duration, Ease paaEase)
        {
            transform.DOKill();
            //symbolImage.material = null;
            iconBg.sprite = ZooRoulette_UIManager._instance.blueImage;           
            //transform.DOScale(Vector3.one, duration).SetEase(paaEase);
        }
        public void RevertAnimatedItem1(float duration, Ease paaEase)
        {
            iconGO.transform.DOKill();
            iconGO.transform.DOScale(Vector3.one, duration).SetEase(paaEase);
        }
        public void WinItemAnimated(Vector3 passVector, float duration, Ease paaEase)
        {
            Debug.Log("FINISH DRAWING");
            transform.DOKill();
            iconBg.sprite = ZooRoulette_UIManager._instance.yellowImage; 
            Zoo_Roulette_Sound.Inst.PlaySFX_Others(7);
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
    }
}

