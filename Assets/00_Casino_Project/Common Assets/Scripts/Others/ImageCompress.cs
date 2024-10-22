using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCompress : MonoBehaviour
{
    public static ImageCompress Inst;

    private void Awake()
    {
        Inst = this;
    }

    public void performCompressAction(string FilePath)
    {
        Debug.Log("<color=red>_________________file path_______</color>" + FilePath);
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject curentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject CashFreeActivity = new AndroidJavaObject("com.compress.image.Compress");

        CashFreeActivity.CallStatic("setCompressImage", curentActivity, FilePath, this.name, "completedMethod", "errorMethod");
    }

    void completedMethod(string imagePath)
    {
        Debug.Log("<color=magenta>___________get success image compress path____</color>" + imagePath);
        Shop.Inst.commpressFilePath = imagePath;
    }

    void errorMethod(string imagePath)
    {
        Debug.Log("<color=red>___________get failed response image compress____</color>" + imagePath);
    }

}
