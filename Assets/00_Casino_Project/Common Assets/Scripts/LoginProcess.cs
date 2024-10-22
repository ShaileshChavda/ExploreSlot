
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginProcess : MonoBehaviour
{
    public static LoginProcess Inst;
    internal string UserName;
    internal string UserPassword;
    internal string AT;
    internal string Permissions;

    private void Awake()
    {            
        Inst = this;
        SocketHandler.OnSocketOpenCustomCall += OnGetCallFromSocketOpen;
    }

    internal void OnGetCallFromSocketOpen()
    {
        Debug.Log("AGAIN_CALL >>>>>>>>>>>>>>>>>>>>>>>");
        if (PlayerPrefs.HasKey("Last_Login_User") && PlayerPrefs.GetString("Last_Login_User") != "null" && PlayerPrefs.GetString("Last_Login_User") != "")
        {
            if (PlayerPrefs.GetString("Last_Login_User") == "guest")
            {
                PreeLoader.Inst.Show();
                SocketHandler.Inst.SendData(SocketEventManager.Inst.PLAY_AS_GUEST(""));
            }
            else if (PlayerPrefs.GetString("Last_Login_User") == "mobile_number")
            {
                PreeLoader.Inst.Show();
                SocketHandler.Inst.SendData(SocketEventManager.Inst.PLAY_AS_MOBILE_NUMBER(PlayerPrefs.GetString("mobile"),""));
            }
            else
            {
                SceneManager.LoadSceneAsync("Login");
            }
        }
        else
        {
            SceneManager.LoadSceneAsync("Login");
        }
    }
}
