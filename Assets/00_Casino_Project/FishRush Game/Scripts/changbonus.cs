using UnityEngine;
using System.Collections;

public class changbonus : MonoBehaviour
{

    public GameObject[] chil;

    void OnEnable()
    {
      //  chil[Random.Range(0, chil.Length)].SetActive(true);
        chil[0].SetActive(true);
    }
}
