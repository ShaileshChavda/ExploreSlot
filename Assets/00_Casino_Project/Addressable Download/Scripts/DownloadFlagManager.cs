using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class DownloadFlagManager : MonoBehaviour
{
    public static DownloadFlagManager Instance;
    public List<string> assetLableName = new List<string>();
   
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddAssetKey(string assetKey)
    {
        assetLableName.Add(assetKey);
    }
    public bool IsInDownloading(string assetKey)
    {
        if (assetLableName.Contains(assetKey))
        {
            return true;
        }else
        {
            return false;
        }      
    }
    public void RemoveAssetKey(string assetKey)
    {       
        assetLableName.RemoveAll(s => s.Contains(assetKey));
    }
}
