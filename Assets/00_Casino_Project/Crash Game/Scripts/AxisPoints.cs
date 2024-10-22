using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxisPoints : MonoBehaviour
{
    public Vector2 direction;
    public float placeDistance;
    public Text textPrefabe;
    public float startFrom;
    public float seed;
    public string formate="{0}";
    public float maxPoint;
    public RectTransform barRect;
    public Transform startPos;
    // Start is called before the first frame update
    void Start()
    {
        float val = startFrom;
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < maxPoint; i++)
        {
            textPrefabe.text = string.Format(formate, val);
            GameObject newPoint = Instantiate(textPrefabe.gameObject, transform);
            newPoint.SetActive(true);
            newPoint.transform.localPosition = startPos.localPosition + pos;
            pos += (Vector3)direction.normalized * placeDistance;
            val += seed;
            barRect.sizeDelta += direction * placeDistance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
