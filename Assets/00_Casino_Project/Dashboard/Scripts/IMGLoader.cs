using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Networking;

public class IMGLoader : MonoBehaviour
{
    public Image icon;
    RectTransform _rectLoader;
    internal bool isLoaded = false;
    public bool isCarRoulette = false;
    void Awake()
    {
        if (!isCarRoulette)
            icon = GetComponent<Image>();
    }

    //internal void LoadIMG(Sprite spriteImage)
    //{
    //    icon.sprite = spriteImage;
    //}

    internal void LoadIMG(string url, bool offline,bool pic)
    {
        if(!pic)
            url = GS.Inst.CheckURLContain(url);
        else
            url = Config.Inst.S3URL + url;
        //Debug.Log("URL >"+ url);
        StartCoroutine(Load(url, false));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Load(string url, bool offline)
    {
        if (offline)
        {
          
        }
        else
        {
        MK:
            WWW www = new WWW(url);
            yield return www;
            if (www.error != null)
            {
                goto MK;
            }

            if (www.texture != null && www.error == null)
            {
                isLoaded = true;
                Texture2D texture = www.texture;
                www.LoadImageIntoTexture(texture);

                Rect rect = new Rect(0, 0, texture.width, texture.height);
                icon.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            }
        }
    }

}