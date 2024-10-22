using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
public class CheckInternetConnection : MonoBehaviour
{
    public static CheckInternetConnection Instance;
    public bool isGoogleConnected;   
    
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
    public bool IsConnected()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {          
            return false;
        }
        else
        {
            return true;
        }       
    }
   
    public void Start()
    {
        StartCoroutine(CheckInternetConnectionStatus(isConnectedNet =>
        {
            if (isConnectedNet)
            {
                isGoogleConnected = true;
                Debug.Log("Internet Available!");
            }
            else
            {
                isGoogleConnected = false;
                Debug.Log("Internet Not Available");
            }
        }));
    }

    IEnumerator CheckInternetConnectionStatus(Action<bool> action)
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            Debug.Log("Error");
            action(false);
        }
        else
        {
            Debug.Log("Success");
            action(true);
        }
    }
}
