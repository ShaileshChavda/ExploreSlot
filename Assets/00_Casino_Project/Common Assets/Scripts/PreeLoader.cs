using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreeLoader : MonoBehaviour
{
    public static PreeLoader Inst;
    [SerializeField] Image preloader;
    RectTransform _rectLoader;

    internal bool isLoaded = false;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        preloader = transform.GetChild(0).GetComponent<Image>();
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
        Invoke("Stop_Loader", 10f);
    }

    public void Stop_Loader()
    {
        preloader.enabled = false;
        transform.localScale = Vector3.zero;
        CancelInvoke("Stop_Loader");
    }
}
