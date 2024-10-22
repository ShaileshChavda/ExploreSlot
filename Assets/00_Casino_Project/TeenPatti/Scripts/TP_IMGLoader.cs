using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Networking;
public class TP_IMGLoader : MonoBehaviour
{
    public Image icon;
    internal bool isLoaded = false;

    internal void LoadIMG(Sprite spriteImage)
    {
        icon.sprite = spriteImage;
    }

    internal void LoadIMG(string url)
    {
        url = GS.Inst.CheckURLContain(url);
        StartCoroutine(Load(url));
    }
    IEnumerator Load(string url)
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
