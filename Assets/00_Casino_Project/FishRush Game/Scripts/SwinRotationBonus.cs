using UnityEngine;
using System.Collections;

public class SwinRotationBonus : MonoBehaviour
{
    bool _check;
    public float a;

    void OnBecameVisible()
    {
        _check = true;
    }

    void Update()
    {
        if (_check == true)
            transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0, 0, a), 180 * Time.deltaTime / 5);
    }
}
