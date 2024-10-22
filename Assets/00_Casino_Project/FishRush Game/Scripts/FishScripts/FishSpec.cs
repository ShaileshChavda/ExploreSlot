using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishSpec : MonoBehaviour
{
    public Image imgSelected;
    public Sprite[] sptSelected;
    public GameObject[] goModeAry;
    // Start is called before the first frame update
    void OnEnable()
    {
        onClickMode(0);
    }

    public void onClickMode(int val)
    {
        for (int i = 0; i < goModeAry.Length; i++)
        {
            goModeAry[i].SetActive(false);
        }

        imgSelected.sprite = sptSelected[val];
        goModeAry[val].SetActive(true);
    }
}
