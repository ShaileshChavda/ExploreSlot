using UnityEngine;
using System.Collections;

public class WaveQuadControl : MonoBehaviour
{

    public float speed;
    Renderer ren;

    void Start()
    {
        ren = GetComponent<MeshRenderer>();
        ren.material.mainTextureOffset = new Vector2(0, 0);
        ren.sortingOrder = 1;
        ren.sortingLayerName = "bg";
    }

    void Update()
    {
        ren.material.mainTextureOffset = new Vector2(Mathf.Repeat(Time.timeSinceLevelLoad * speed / 20, 1), 0);
    }
}
