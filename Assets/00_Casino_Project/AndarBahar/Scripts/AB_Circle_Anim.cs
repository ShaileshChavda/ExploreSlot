using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AB_Circle_Anim : MonoBehaviour
{
    public static AB_Circle_Anim Inst;
    [SerializeField] Image preloader;
    RectTransform _rectLoader;

    internal bool isLoaded = false;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        _rectLoader = preloader.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!preloader.enabled)
            return;
        _rectLoader.Rotate(Vector3.forward, -5);
    }

    public void Show()
    {
        preloader.enabled = true;
        transform.localScale = Vector3.one;
        Invoke("Stop_Loader", 3f);
    }

    public void Stop_Loader()
    {
        preloader.enabled = false;
        transform.localScale = Vector3.zero;
        CancelInvoke("Stop_Loader");
    }
}
