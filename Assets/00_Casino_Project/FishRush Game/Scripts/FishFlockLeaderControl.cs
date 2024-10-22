using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishFlockLeaderControl : MonoBehaviour
{

    List<Transform> _fish;
    int countFish = 1;
    public GameObject _FishPre;

    public int minCount;
    public int maxCount;

    public void FlockStart()
    {
        _fish = new List<Transform>();
        countFish = Random.Range(minCount, maxCount);
        for (int i = 0; i < countFish; i++)
        {
            Transform _tr = Instantiate(_FishPre).transform;
            _tr.GetComponent<FishFlockControl>().SetLeader(transform);
            _fish.Add(_tr);
            _tr.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * 0.8f;
           // FishManage.Instance._FishMange.Add(_tr);
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
