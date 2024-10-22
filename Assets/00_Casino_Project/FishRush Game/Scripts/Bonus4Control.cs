using UnityEngine;
using System.Collections;

public class Bonus4Control : MonoBehaviour
{
    public Transform[] _prefab;
    float limitHieght;

    void OnEnable()
    {
        limitHieght = Screen.height / 200;
        int a = Random.Range(0, 2);
        if (a == 0)
        {
            for (int i = 0; i < _prefab.Length; i++)
            {
                Transform _tr = Instantiate(_prefab[i]);

                _tr.position = new Vector2(2f + i, -limitHieght - 2.5f);
                _tr.eulerAngles = new Vector3(0, 0, 90);

                _tr.GetComponent<FishSwimBonus4>().StartBonus4(2f + i, a);
            }
        }
        else
        {
            for (int i = 0; i < _prefab.Length; i++)
            {
                Transform _tr = Instantiate(_prefab[i]);

                _tr.position = new Vector2(2f + i, limitHieght + 2.5f);
                _tr.eulerAngles = new Vector3(0, 0, 270);

                _tr.GetComponent<FishSwimBonus4>().StartBonus4(2f + i, a);
            }
        }
    }
}
