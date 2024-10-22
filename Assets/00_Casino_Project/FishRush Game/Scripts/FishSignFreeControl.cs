using UnityEngine;
using System.Collections;

public class FishSignFreeControl : MonoBehaviour
{

    public float RotateInterval = 5;
    public float RotateIntervalRndRange = 1;
    public float RotateAngleRndRange = 30;

    private Swim _swim;
    private FishControl _fishControl;

    private float TimeRotate;
    private float ElapseRotate;

    void OnEnable()
    {
        _fishControl = GetComponent<FishControl>();
        transform.SetParent(FishManage.Instance.trContainer, false);
        if (gameObject.tag == "fish")
            _fishControl.OnDeath += onDeath;
        _swim = GetComponent<Swim>();
        TimeRotate = RotateInterval + Random.Range(-RotateIntervalRndRange, RotateIntervalRndRange);
    }

    void onDeath()
    {
        this.enabled = false;
    }

    void Update()
    {
        if (ElapseRotate > TimeRotate)
        {
            _swim.Rotate(Random.Range(-RotateAngleRndRange, RotateAngleRndRange));
            ElapseRotate = 0;
            TimeRotate = RotateInterval + Random.Range(-RotateIntervalRndRange, RotateIntervalRndRange);
        }
        else
        {
            ElapseRotate += Time.deltaTime;
        }
    }
}
