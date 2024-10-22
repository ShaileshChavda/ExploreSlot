using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishFollowLeaderBonusControl : MonoBehaviour
{

    List<Transform> _fish;
    public int countFish = 1;
    public GameObject _FishPre;
    public float Distan = 0.5f;
    Swim _swim;

    public void FollowStart()
    {
        _swim = GetComponent<Swim>();

        _fish = new List<Transform>();
        for (int i = 0; i < countFish; i++)
        {
            Transform _tr = Instantiate(_FishPre).transform;
            _tr.position = Vector3.zero;
            _tr.position = transform.position - transform.right * Distan * i;
            _tr.right = transform.right;
            _tr.GetComponent<FishFollowBonusControl>().SetTarget(transform, Vector3.Magnitude(transform.right * Distan * i));
            _tr.GetComponent<Swim>().Speed = _swim.Speed;
            _fish.Add(_tr);
        }
    }

    public void destroyOnList(Transform _trs)
    {
        _fish.Remove(_trs);

        if (_fish.Count == 0)
        {
            Destroy(gameObject);
        }
    }
}
