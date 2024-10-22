using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DT_RatePercent : MonoBehaviour
{
    public static DT_RatePercent instance;
    public Text rateText;
    public Image BaseImg;
    public Sprite[] FillImage;
    public void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetRatePercentAndProgressBar();
    }
    public void SetRatePercentAndProgressBar()
    {
        rateText.text = "Rate: "+Random.Range(20,100).ToString()+"%";
        BaseImg.sprite = FillImage[Random.Range(1, 4)];
    }
    public void SetBaseProgressBar()
    {
        BaseImg.sprite = FillImage[0];
    }
}
