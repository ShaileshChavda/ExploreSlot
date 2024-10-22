using UnityEngine;
using System.Collections;

public class FishFollowBonusControl : MonoBehaviour
{

    public float DistanceToLeader = 0.2f;

    public Swim _swimLeader;
    public FishFollowLeaderBonusControl _fol;
    Vector3 Direction;
    Vector3 PriDirectionUnNor;
    public int i;

    Swim _swim;
    FishControl _fishControl;

    void OnEnable()
    {
        _swim = GetComponent<Swim>();
        _swimLeader = null;
        transform.position = Vector3.zero;
        transform.right = Vector3.right;
        _fishControl = GetComponent<FishControl>();
        _fishControl.OnDeath += onDeath;
    }

    void onDeath()
    {
        this.enabled = false;
        _swimLeader.EventRotate -= Handlel_TargetFishRotateStasrt;
    }

    public void Handlel_TargetFishRotateStasrt(float angle)
    {
        StartCoroutine(_Rotate(angle));
    }

    IEnumerator _Rotate(float angle)
    {
        float waitTime = DistanceToLeader / _swim.Speed;
        float elapse = 0;
        while (elapse < waitTime)
        {
            elapse += Time.deltaTime;
            yield return 0;
        }
        _swim.Rotate(angle);
    }
    void OnBecameInvisible()
    {
        _swimLeader.EventRotate -= Handlel_TargetFishRotateStasrt;
        if (_fol != null)
            _fol.destroyOnList(transform);
    }

    public void SetTarget(Transform leader, float distan)
    {
        _swimLeader = leader.GetComponent<Swim>();
        _fol = leader.GetComponent<FishFollowLeaderBonusControl>();
        DistanceToLeader = Vector3.Distance(transform.position, leader.position);
        _swimLeader.EventRotate += Handlel_TargetFishRotateStasrt;
    }

    void OnDisable()
    {
        _swimLeader.EventRotate -= Handlel_TargetFishRotateStasrt;
        if (_fol != null)
            _fol.destroyOnList(transform);
    }

    void OnDestroy()
    {
        _swimLeader.EventRotate -= Handlel_TargetFishRotateStasrt;
        if (_fol != null)
            _fol.destroyOnList(transform);
    }
}
