using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DontDestroyLobby : MonoBehaviour
{
    public static DontDestroyLobby instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
           Destroy(gameObject);    
        }
    }
    public void MainScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
