using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMoveEffects : MonoBehaviour
{
    Camera uiCam;
    public void InitMoveCoin(Vector3 _trs, int gold)
    {
        uiCam = GameObject.Find("CamUI").GetComponent<Camera>();
        Vector3 a = _trs;
        a = Camera.main.WorldToScreenPoint(a);
        a = uiCam.ScreenToWorldPoint(a);
        transform.position = a;
        LeanTween.move(gameObject, a + Vector3.up * 0.1f, 0.8f).setOnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
