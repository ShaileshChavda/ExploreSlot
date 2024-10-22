using UnityEngine;
using System.Collections;

public class WaveControl : MonoBehaviour {
    public float speed = 2;

	void Update () {
        transform.Translate(Vector3.left * speed * Time.smoothDeltaTime);
	}

    void OnBecameInvisible()
    {
        FishManage.Instance.BonusTime();
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "fish")
        {
            col.GetComponent<FishControl>().CollisionWithWave();
        }
    }
}
