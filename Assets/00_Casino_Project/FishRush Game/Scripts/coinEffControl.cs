using UnityEngine;
using System.Collections;
using DG.Tweening;

public class coinEffControl : MonoBehaviour
{
    Camera uiCam;
    public Transform trCoinPos;
    public float fltMoveTime = 1f;

    void Start()
    {
        //LeanTween.move(gameObject, new Vector2(3.4f, 3.4f), 1).setOnComplete(() =>
        //{
        //    Destroy(gameObject);
        //});
        //AudioControl.Instance.coin();
    }

    public void MoveToTarget(Transform tr)
    {
        trCoinPos = tr;
        transform.DOMove(trCoinPos.position, fltMoveTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    public void InitCoinEffect(Vector3 _trs, Transform tr)
    {
        trCoinPos = tr;
        uiCam = GameObject.Find("CamUI").GetComponent<Camera>();
        Vector3 a = _trs;
        a = Camera.main.WorldToScreenPoint(a);
        a = uiCam.ScreenToWorldPoint(a);
        transform.position = a;
        
        Vector3 v3Pos = a - Vector3.up * 0.5f;

        //Vector3 v3Pos = new Vector3(a.x,(a.y - Vector3.up.y *0.025f),a.z);

        transform.DOMove(v3Pos, fltMoveTime).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            EmMoveToTarget();
        });

        //_txtScore.text = "+" + gold;
        //uiCam = GameObject.Find("CamUI").GetComponent<Camera>();
        //Vector3 a = _trs;
        //a = Camera.main.WorldToScreenPoint(a);
        //a = uiCam.ScreenToWorldPoint(a);
        //transform.position = a;
        ////LeanTween.move(gameObject, a + Vector3.up * 0.1f, 0.8f).setOnComplete(() =>
        ////{
        ////    Destroy(gameObject);
        ////    UiTextSpawmControl.Instance.PushGold(gold * GunControl.BonusCoin);
        ////});

        //Vector3 v3Pos = a + Vector3.up * 0.3f;
        //transform.DOMove(v3Pos, fltMoveTime).SetEase(Ease.Linear).OnComplete(() =>
        //{
        //    Destroy(gameObject);
        //});
    }

    [HideInInspector]
    public Transform[] trCoinAry;
    public void EmMoveToTarget()
    {
        for (int i = 0; i < trCoinAry.Length; i++)
        {
            trCoinAry[i].GetComponent<MoveToTarget>().MoveOnTarget(i, trCoinPos.position);
        }
    }
}