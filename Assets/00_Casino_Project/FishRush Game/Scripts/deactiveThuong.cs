using UnityEngine;
using System.Collections;

public class deactiveThuong : MonoBehaviour {
    void OnEnable()
    {
        Invoke("deactive", 0.8f);
    }

    void deactive()
    {
        gameObject.SetActive(false);
    }
}
