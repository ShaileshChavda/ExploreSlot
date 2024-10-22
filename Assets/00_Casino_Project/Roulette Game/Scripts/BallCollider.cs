using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollider : MonoBehaviour
{
    public static Action<GameObject, GameObject> TargetObject;
    GameObject targetObject;
    GameObject nrObject;

    Animator anim;
    private void OnEnable()
    {
        TargetObject += ReferanceObject;
    }
    private void OnDisable()
    {
        TargetObject -= ReferanceObject;
    }

    void ReferanceObject(GameObject obj, GameObject obj2)
    {
        targetObject = obj;
        nrObject = obj2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == nrObject)
        {
            Debug.Log("OnCollisionEnter2D ==> " + collision.gameObject.name);
            CollidewithNearObject();
        }
    }

    void CollidewithNearObject()
    {
        RouletteSpinWheel.StopBall?.Invoke();
        targetObject.SetActive(false);
        nrObject.SetActive(false);
        transform.DOLocalMove(Vector3.up * -63, 0.3f).OnComplete(() =>
        {
            transform.DOLocalMove(new Vector3(-22, -47, 0), 0.25f).OnComplete(() =>
            {
                transform.DOLocalMove(new Vector3(-6.7f, -63, 0), 0.25f).OnComplete(() =>
                {
                    targetObject.SetActive(true);
                    transform.DOLocalMove(new Vector3(-12f, -47, 0), 0.3f).OnComplete(() =>
                    {
                        nrObject.SetActive(true);
                        DOTween.KillAll();
                    });
                });
            });
        });
    }
}
