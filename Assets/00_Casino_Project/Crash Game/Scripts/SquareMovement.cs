using UnityEngine;
using DG.Tweening;
public class SquareMovement : MonoBehaviour
{
    public Transform[] points;
    public Transform obj;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 []pointList = new Vector3[points.Length];
        for (int i = 0; i < pointList.Length; i++)
        {
            pointList[i] = points[i].localPosition;
        }
        obj.DOLocalPath(pointList, speed).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        
    }

    
}
