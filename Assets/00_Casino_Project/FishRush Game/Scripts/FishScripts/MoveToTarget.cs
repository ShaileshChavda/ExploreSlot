using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    private float fltMoveTime;
    public void MoveOnTarget(int id,Vector3 target)
    {
        transform.DOMove(target, fltMoveTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            if(id == 5)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        });
    }
}
