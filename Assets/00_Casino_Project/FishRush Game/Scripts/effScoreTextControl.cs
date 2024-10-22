using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;

public class effScoreTextControl : MonoBehaviour
{
    Camera uiCam;
    public TextMeshPro _txtScore;
    public float fltMoveTime = 1f;

    public void InitEffScore(Vector3 _trs, int gold)
    {
        _txtScore.text = "+" + gold;
        uiCam = GameObject.Find("CamUI").GetComponent<Camera>();
        Vector3 a = _trs;
        a = Camera.main.WorldToScreenPoint(a);
        a = uiCam.ScreenToWorldPoint(a);
        transform.position = a;
        //LeanTween.move(gameObject, a + Vector3.up * 0.1f, 0.8f).setOnComplete(() =>
        //{
        //    Destroy(gameObject);
        //    UiTextSpawmControl.Instance.PushGold(gold * GunControl.BonusCoin);
        //});
        
        Vector3 v3Pos = a + Vector3.up * 0.3f;
        transform.DOMove(v3Pos, fltMoveTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
