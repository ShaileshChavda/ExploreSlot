using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class SevenUpDown_DiceAnimation : MonoBehaviour
{
    public static SevenUpDown_DiceAnimation instance;
    public SkeletonGraphic dice1, dice2;
    public Sprite[] diceSprite;
    public Image diceImg1, diceImg2;
    public RectTransform diceAnim;
    public TextMeshProUGUI txt_DiceTotal;
    public void Awake()
    {
        instance = this;
    }

   /* void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           StartCoroutine(DiceAnimate(2,3,5));
        }
    }*/
   public void StartDiceAnim(int no1, int no2, int win_card_number)
    {
        StartCoroutine(DiceAnimate(no1,no2, win_card_number));
    }
    IEnumerator DiceAnimate(int no1, int no2, int win_card_number)
    {
        diceAnim.DOScale(1.3f, 0.3f).SetEase(Ease.OutExpo);
        diceAnim.DOLocalMove(new Vector2(-6, 78), 0.3f);

        yield return new WaitForSeconds(0.3f);

        SevenUpDown_SoundManager.Inst.PlaySFX(3);

        diceImg1.gameObject.SetActive(false);
        diceImg2.gameObject.SetActive(false);
     
        dice1.gameObject.SetActive(true);
        dice1.gameObject.GetComponent<SkeletonGraphic>().startingAnimation = "animation"+ Random.Range(1, 6);
        dice2.gameObject.SetActive(true);
        dice2.gameObject.GetComponent<SkeletonGraphic>().startingAnimation = "animation"+ Random.Range(1, 6);

        yield return new WaitForSeconds(1.8f);
        SevenUpDown_SoundManager.Inst.PlaySFX(4);

        txt_DiceTotal.text = win_card_number.ToString();

        if (no1 > 0)
        {
            no1= no1 - 1;
        }
        if (no2 > 0)
        {
            no2 = no2 - 1;
        }
        diceImg1.sprite = diceSprite[no1];
        diceImg2.sprite = diceSprite[no2];
        diceImg1.gameObject.SetActive(true);
        diceImg2.gameObject.SetActive(true);
        dice1.gameObject.SetActive(false);
        dice2.gameObject.SetActive(false);       

        yield return new WaitForSeconds(1.8f);   
        
        diceAnim.DOScale(0.7f, 0.2f).SetEase(Ease.OutCubic);
        diceAnim.DOLocalMove(new Vector2(140, 240), 0.2f);
        txt_DiceTotal.text = "";
        StopCoroutine("DiceAnimate");
    }
}
