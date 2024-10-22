using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishSwimBonus4 : MonoBehaviour
{

    Swim _swim;

    List<Transform> _fish;
    public int countFish = 1;
    public GameObject _FishPre;
    public float Distan = 0.5f;

    public void StartBonus4(float index, float direction)
    {
        _swim = GetComponent<Swim>();
        _swim.Speed = ((Mathf.PI * index) / 5);
        _swim.RotateSpeed = 38;
        _fish = new List<Transform>();
        for (int i = 0; i < countFish; i++)
        {
            Transform _tr = Instantiate(_FishPre).transform;
            _tr.position = Vector3.zero;
            _tr.position = transform.position - transform.right * Distan * i;
            _tr.right = transform.right;
            _tr.GetComponent<FishFollowBonusControl>().SetTarget(transform, Vector3.Magnitude(transform.right * Distan * i));
            _tr.GetComponent<Swim>().Speed = _swim.Speed;
            _tr.GetComponent<Swim>().RotateSpeed = 38;
            if (direction == 0)
                _tr.gameObject.AddComponent<SwinRotationBonus>().a = 270;
            else
                _tr.gameObject.AddComponent<SwinRotationBonus>().a = 90;
            _fish.Add(_tr);
        }
        Destroy(gameObject);
    }

    void Update()
    {
        // transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0, 0, 270), 180 * Time.deltaTime / 5);
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
