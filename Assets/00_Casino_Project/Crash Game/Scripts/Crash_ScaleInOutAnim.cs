using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash_ScaleInOutAnim : MonoBehaviour
{

    void Start()
    {        
        transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.5f).SetLoops(-1, LoopType.Yoyo);       
    }
}
