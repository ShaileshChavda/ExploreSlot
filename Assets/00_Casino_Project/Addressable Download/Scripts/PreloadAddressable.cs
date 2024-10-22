using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class PreloadAddressable : MonoBehaviour
{
    public bool isLoadScene;
    //public string NextScene;
    private AsyncOperationHandle asyncHandle;
    [SerializeField] private AssetReference assetRef;
    [SerializeField] private string assetKey = "";
    private long downloadSize;
    [SerializeField] private Text sizeTxt, progressText;  
    [SerializeField] private GameObject downloadBtn, progressBarGo;
    [SerializeField] private Image progressBar;
    public UnityEvent<float> ProgressEvent;
    public UnityEvent<bool> CompletionEvent;
    public string GameName;
    
    public void Start()
    {
        //Caching.ClearCache();  
        progressBarGo.SetActive(false);
        Debug.Log(Application.streamingAssetsPath);
        if (DownloadFlagManager.Instance.IsInDownloading(assetKey))// && CheckInternetConnection.Instance.IsConnected())
        { 
            DownloadScene();
        }
        else
        {
            GetDownloadSize();
        }       
    }

     public void GetDownloadSize()
     {
         if (assetRef.RuntimeKeyIsValid() )
         {
             Addressables.GetDownloadSizeAsync(assetKey).Completed += GetDownloadedSize;
            //Addressables.GetDownloadSizeAsync(assetRef.RuntimeKey).Completed += GetDownloadedSize;
         }        
     }
     void GetDownloadedSize(AsyncOperationHandle<long> obj)
     {
         if (obj.IsValid())
         {
             downloadSize = obj.Result;
             if (downloadSize == 0)
             {               
                downloadBtn.gameObject.SetActive(false);
                progressBarGo.SetActive(false);
                progressText.text = "Completed";
                progressBar.fillAmount = 1;
                if (sizeTxt != null)
                {
                    sizeTxt.text = "";
                }
             }
             else
             {
                if (sizeTxt != null)
                {
                    sizeTxt.text = SizeToString(downloadSize);
                }
             }
         }
     }
    public void DownloadScene()
    {
        if (CheckInternetConnection.Instance.IsConnected()) {
            DownloadFlagManager.Instance.AddAssetKey(assetKey);
            StartCoroutine(StatusCheck());
        }
        else
        {
            Alert_MSG.Inst.MSG("Please check internet connection..!");
        }
    }
    public void LoadScene()
    {
        SoundManager.Inst.PlaySFX(0);

        if(isLoadScene)
        {
            SceneManager.LoadScene(GameName);
        }
        else
        {
            SoundManager.Inst.StopBG();
            PreeLoader.Inst.Show();
            Addressables.LoadSceneAsync(assetRef, LoadSceneMode.Single, true);
        }
    }
   
    private string SizeToString(long size)
    {        
        string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
        if (size == 0)
            return "0" + suf[0];
        long bytes = Math.Abs(size);
        int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        double num = Math.Round(bytes / Math.Pow(1024, place), 1);
        return (Math.Sign(size) * num).ToString() + suf[place];
    }


   
    IEnumerator StatusCheck()
    {
        asyncHandle = Addressables.DownloadDependenciesAsync(assetKey, false);
       // asyncHandle = Addressables.DownloadDependenciesAsync(assetRef.RuntimeKey, false);
        float progress = 0;

        while (asyncHandle.Status == AsyncOperationStatus.None)
        {
            progressBarGo.SetActive(true);
            float percentageComplete = asyncHandle.GetDownloadStatus().Percent;                
            progressText.text = "Downloading.."+ (percentageComplete*100).ToString("f2")+"%";           
            if (percentageComplete >= progress * 1) // Report at most every 10% or so
            {
                progress = percentageComplete; // More accurate %
                progressBar.fillAmount = progress;
               // ProgressEvent.Invoke(progress);
            }
            yield return null;
        }
        if (CheckInternetConnection.Instance.IsConnected())
        {
            CompletionEvent.Invoke(asyncHandle.Status == AsyncOperationStatus.Succeeded);
            if (asyncHandle.Status == AsyncOperationStatus.Succeeded)
            {
                progressBarGo.SetActive(false);
                PreeLoader.Inst.Stop_Loader();
                Alert_MSG.Inst.MSG(GameName+" downloaded");
                //CommenMSG.Inst.MSG("Success..!", "Game is successfully downloaded..!");
            }
        }
        else
        {
            Alert_MSG.Inst.MSG("Please check internet connection..!");
            progressText.text = "Download";
            progressBar.fillAmount = 0;
        }
        // Addressables.Release(asyncHandle); //Release the operation handle       
    }
    public void CompletedDownload()
    { 
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Dashboard")
        {
            downloadBtn.gameObject.SetActive(false);
            progressBarGo.SetActive(false);
            progressText.text = "Completed " + assetKey;
            progressBar.fillAmount = 1;
            if (sizeTxt != null)
            {
                sizeTxt.text = "";
            }
        }
        DownloadFlagManager.Instance.RemoveAssetKey(assetKey);       
    } 
}
