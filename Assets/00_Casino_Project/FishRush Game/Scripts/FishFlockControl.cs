using UnityEngine;
using System.Collections;

public class FishFlockControl : MonoBehaviour
{

    public float DistanceToLeader = 50;

    Swim _swimLeader;
    FishControl _fishControl;
    Transform _trLeader;


    FishFlockLeaderControl _fol;
    private Swim _swim;
    private Transform _tr;
    private Vector3 DirectionRndForce;

    float CohesionDistance;
    Vector3 DirectionToLeader;

    float ElapseRotate;
    float TimeRotate = 1;
    float SpeedRotate = 180;
    float AngleActiveRotate;
    bool IsRotating;

    Vector3 RndForceRotate;
    float RotateSpdWithDirection;

    float ElapseIntervalRotate;
    float TimeIntervalRotate;


    void OnEnable()
    {
        _swim = GetComponent<Swim>();
        _fishControl = GetComponent<FishControl>();
        _tr = transform;
        transform.SetParent(FishManage.Instance.trContainer, false);
        _fishControl.OnDeath += onDeath;

        IsRotating = true;

        TimeRotate = 1;
        SpeedRotate = 180;

        RndForceRotate = Random.insideUnitCircle.normalized * 96;
        TimeIntervalRotate = Random.Range(0.1f, 2.5f);
        RotateSpdWithDirection = (Random.Range(0, 2) == 0 ? -1f : 1f) * SpeedRotate;
    }

    void onDeath()
    {
        this.enabled = false;
    }


    void Update()
    {
        if (_trLeader == null)
            return;
        DirectionToLeader = _trLeader.position - _tr.position;
        DirectionToLeader.z = 0;

        _tr.right = _trLeader.right * 384f + (DirectionToLeader.magnitude / CohesionDistance * DirectionToLeader) + DirectionRndForce;

        if (IsRotating)
        {
            AngleActiveRotate += RotateSpdWithDirection * Time.deltaTime;
            DirectionRndForce = Quaternion.Euler(0, 0, AngleActiveRotate) * RndForceRotate;
            ElapseRotate += Time.deltaTime;

            if (ElapseRotate > TimeRotate)
            {
                IsRotating = false;
                ElapseRotate = 0;
            }
        }
        else
        {
            ElapseIntervalRotate += Time.deltaTime;
            if (ElapseIntervalRotate > TimeIntervalRotate)
            {
                IsRotating = true;
                TimeIntervalRotate = Random.Range(0.1f, 2.5f);
                RotateSpdWithDirection = (Random.Range(0, 2) == 0 ? -1f : 1f) * SpeedRotate;
                ElapseIntervalRotate = 0;
            }
        }
    }
    void OnBecameInvisible()
    {
        if (_fol != null)
            _fol.destroyOnList(transform);
    }

    public void SetLeader(Transform _leader)
    {
        _trLeader = _leader;
        _swimLeader = _leader.GetComponent<Swim>();
        _fol = _leader.GetComponent<FishFlockLeaderControl>();
        CohesionDistance = DistanceToLeader + _swimLeader.BoundCircleRadius + _swim.BoundCircleRadius;
    }
}
