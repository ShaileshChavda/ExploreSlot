using UnityEngine;
using System.Collections;

public class BoomControl : MonoBehaviour
{
    public GameObject eff;
    Vector3 _posMouse;
    void Update()
    {
        _posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(_posMouse.x, _posMouse.y, -6);

        //if (Input.GetMouseButtonUp(0))
        //{
        //    RaycastHit2D[] fish = Physics2D.CircleCastAll(new Vector3(transform.position.x, transform.position.y, 0), 2, Vector3.zero);
        //    AudioControl.Instance.boom();
        //    for (int i = 0; i < fish.Length; i++)
        //    {
        //        if (fish[i].collider.tag == "fish")
        //            fish[i].collider.gameObject.GetComponent<FishControl>().hitDame(1000, gameObject);
        //    }
        //    GameObject boom = (GameObject)Instantiate(eff, transform.position, Quaternion.identity);
        //    Destroy(boom, 1.5f);
        //    Destroy(gameObject);
        //}
    }
}
