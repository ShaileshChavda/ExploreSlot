using System;
using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine.UI;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;


public enum SocketState
{
    None,
    Close,
    Connecting,
    Open,
    Running,
    Error
}

public class SocketHandler : MonoBehaviour
{
    public static SocketHandler Inst;
    public SocketIOComponent socket;
    public SocketState _socketState;
    internal bool isCanSendRLR, AppPause;
    public int pingMissCounter = 0;

    public static event Action<JSONObject> OnSocketResponse;
    public static event Action OnSocketOpenCustomCall;


    public bool isSocketClosed, isPongReceived;
    bool SocketClosed, socketopen;
    bool no_Connection;

    private void Awake()
    {
        Inst = this;
        isCanSendRLR = false;
        AppPause = false;
        no_Connection = false;
    }

    internal void Create()
    {
        //check internet First
        if (!NetworkCheck.IsInternetConnection())
        {
            Invoke("Create", 1f);
            return;
        }
        _socketState = SocketState.None;

        socket.url = Config.Inst.SocketServerURL;
        //socket.SetUpWebSocket();
        socket.SetUpWS();
        Invoke("Init", 1);
    }

    void Init()
    {
        socket.On("open", TestOpen);
        socket.On("error", TestError);
        socket.On("close", TestClose);
        socket.On("res", TestResponse);
        socket.On("hb", OnReceivePong);
        StartCoroutine(CheckPingTimeOut());
        Invoke("Connect", 1);
        InvokeRepeating("checkandreconnectsocket", 1, 2);
        InvokeRepeating("CheckInternetAndCloseSocket", 1, 2);
    }

    public void TestOpen(SocketIOEvent e)
    {
        Debug.Log("<Color=red><b>_______SocketOpen________</b></Color>");
        _socketState = SocketState.Open;
        socketopen = true;
        no_Connection = false;
    }

    public void TestError(SocketIOEvent e)
    {
        Debug.Log("<Color=red><b>____________SocketError__________</b></Color>");
        _socketState = SocketState.Error;
        no_Connection = true;
        isSocketClosed = true;
    }

    public void TestClose(SocketIOEvent e)
    {
        Debug.Log("<Color=red><b>____________SocketClose__________</b></Color>");
        no_Connection = true;
        isCanSendRLR = true;
        isSocketClosed = true;
        _socketState = SocketState.Close;
    }

    public void SendData(JSONObject obj)
    {
        // Debug.Log("Send evt >>>>");
        print("<color=yellow>SEND : </color> : " + obj + "Time => " + DateTime.Now);
        //JSONObject newData = new JSONObject ();
        //string tempstr = AESEncrypt (obj.Print (), ENCKEY, ENCIV);
        //newData.AddField ("data", tempstr);
        socket.Emit("req", obj);
    }

    public void TestResponse(SocketIOEvent e)
    {
        _socketState = SocketState.Running;

        JSONObject obj = new JSONObject(e.data.ToString().Trim(new char[] { '"' }));
        var en = obj.GetField("en").ToString().Trim(new char[] { '"' });
        if (en != "hb")
        {
            if (en.Equals("SEVEN_UP_BET_INFO") || en.Equals("ZOO_GAME_BET_INFO") || en.Equals("ZOO_GAME_JOINED_USER_LISTS") )
            {

            }
            else
            {
                Debug.Log("<Color=green><b>_____RECEIVE___</b></Color>" + obj + "<Color=red><b>____TIME_____</b></Color>" + DateTime.Now);
            }

        }
        if (OnSocketResponse != null)
        {
            OnSocketResponse(obj);
        }
    }
  
   
    public void OnReceivePong(SocketIOEvent e)
    {
        //Debug.Log("HB Recv....");
        _socketState = SocketState.Running;
        isPongReceived = true;
    }

    IEnumerator CheckPingTimeOut()
    {
        yield return new WaitForSecondsRealtime(5);
        NT:
        if (IsConnected())
        {
            isPongReceived = false;
            socket.Emit("hb", new JSONObject());
            float wait = 3;
            int c = 2;
            yield return new WaitForSecondsRealtime(wait);
            if (isPongReceived)
            {
                pingMissCounter = 0;
                no_Connection = true;
            }
            else
            {
                pingMissCounter++;
                if (pingMissCounter > c)
                {
                    no_Connection = false;
                    pingMissCounter = 0;
                    CloseSocket();
                }
            }
            goto NT;
        }
        else
        {
            yield return new WaitForSecondsRealtime(1);
            goto NT;
        }
    }

    internal void Connect()
    {
        if (!IsConnected())
        {
            Debug.Log("<Color=yellow><b>_____Socket Connect_______</b></Color>" + _socketState);
            socket.Connect();
        }

    }

    void checkandreconnectsocket()
    {
        if (isSocketClosed)
        {
            isSocketClosed = false;
            StartCoroutine(CheckAndReConnectSocket());
        }

    }


    IEnumerator CheckAndReConnectSocket()
    {
        //Debug.Log("RECONECT.....");
        NT:
        if (_socketState == SocketState.Close || _socketState == SocketState.Error)
        {
            //Debug.Log("RECONECT...11111..");
            if (!Config.Inst._serverConfig.m_Mode)
            {
                //Debug.Log("RECONECT..22222...");
                if (NetworkCheck.IsInternetConnection())
                {
                    //Debug.Log("RECONECT..3333...");
                    isCanSendRLR = true;
                    _socketState = SocketState.Connecting;
                    Connect();
                }
                else
                {
                    // user offline
                    yield return new WaitForSecondsRealtime(2);
                    goto NT;

                }
            }
            else
            {
                yield return new WaitForSecondsRealtime(1);
                goto NT;
            }

        }

    }


    internal void CheckInternetAndCloseSocket()
    {
        if (!NetworkCheck.IsInternetConnection())
        {
            socket.sid = "";
            socket.Close();
        }
    }

    internal bool IsConnected()
    {
        bool val = false;
        switch (_socketState)
        {
            case SocketState.Open:
            case SocketState.Running:
                val = true;
                break;
        }
        return val;
    }


    internal void CloseSocket()
    {
        //Debug.LogError("In The Socket Close From Miss Counter");
        if (IsConnected())
        {
            socket.sid = "";
            socket.Close();
        }
    }

    // Start is called before the first frame update
    void Update()
    {
        switch (_socketState)
        {
            case SocketState.Open:
                break;
            case SocketState.Running:
                break;
            case SocketState.Connecting:
                break;
            case SocketState.Error:
                break;
            case SocketState.Close:
                break;
        }
        if (socketopen)
        {
            socketopen = false;
            if (isCanSendRLR)
            {
                isCanSendRLR = false;
                //if (AppPause)
                //    App_RLR();
                //else
                    OnSocketOpenCustomCall();
            }
            else
                OnSocketOpenCustomCall();
        }
        if (SocketClosed)
        {
            SocketClosed = false;
        }
    }

    public void INTERNET_OK()
    {
        no_Connection = false;
        PreeLoader.Inst.Show();
        Invoke("LOST_AGAIN", 5);
    }

    void LOST_AGAIN()
    {
        if (_socketState == SocketState.Close || _socketState == SocketState.Error)
        {
            no_Connection = true;
        }
        PreeLoader.Inst.Stop_Loader();
    }

    void OnApplicationPause(bool focus)
    {
        if (!focus && SceneManager.GetActiveScene().name != "Dashboard" && SceneManager.GetActiveScene().name != "Splace" && SceneManager.GetActiveScene().name != "Login")// && SceneManager.GetActiveScene().name != "Crash")
        {
            if (Shop.Inst.transform.localScale.x <= 0)
            {
                PreeLoader.Inst.Show();
                AppPause = true;
                socket.sid = "";
                socket.Close();

                if (SceneManager.GetActiveScene().name.Equals("Roulette"))
                    Roullate_EventSetup.Inst.CLEAR_EVENT_DATA();
                else if (SceneManager.GetActiveScene().name.Equals("DragonTiger"))
                    DT_EventSetup.Inst.CLEAR_EVENT_DATA();
                else if (SceneManager.GetActiveScene().name.Equals("Crash"))
                    Crash_EventSetup.Inst.CLEAR_EVENT_DATA();
                else if (SceneManager.GetActiveScene().name.Equals("CarRouletteScene"))
                    CarRoulette_Game.CarRoulette_EventManager.Inst.CLEAR_EVENT_DATA();
                else if (SceneManager.GetActiveScene().name.Equals("HorsRacing"))
                    HR_EventSetup.Inst.CLEAR_EVENT_DATA();
                else if (SceneManager.GetActiveScene().name.Equals("7UpDown"))
                    SevenUpDown_EventSetup.Inst.CLEAR_EVENT_DATA();
                else if (SceneManager.GetActiveScene().name.Equals("AndarBahar"))
                    AB_EventSetup.Inst.CLEAR_EVENT_DATA();
                else if (SceneManager.GetActiveScene().name.Equals("ZooRoulette"))
                    ZooRoulette_Game.ZooRoulette_EventManager.Inst.CLEAR_EVENT_DATA();
                else if (SceneManager.GetActiveScene().name.Equals("Mines"))
                    Mines_EventSetup.Inst.CLEAR_EVENT_DATA();

            }
        }
    }

    public void Reload_Game()
    {
        PreeLoader.Inst.Show();
        AppPause = true;
        socket.sid = "";
        socket.Close();

        if (SceneManager.GetActiveScene().name.Equals("Roulette"))
            Roullate_EventSetup.Inst.CLEAR_EVENT_DATA();
        else if (SceneManager.GetActiveScene().name.Equals("DragonTiger"))
            DT_EventSetup.Inst.CLEAR_EVENT_DATA();
        else if (SceneManager.GetActiveScene().name.Equals("CarRouletteScene"))
            CarRoulette_Game.CarRoulette_EventManager.Inst.CLEAR_EVENT_DATA();
        else if (SceneManager.GetActiveScene().name.Equals("Crash"))
            Crash_EventSetup.Inst.CLEAR_EVENT_DATA();
        else if (SceneManager.GetActiveScene().name.Equals("HorsRacing"))
            HR_EventSetup.Inst.CLEAR_EVENT_DATA();
        else if (SceneManager.GetActiveScene().name.Equals("7UpDown"))
            SevenUpDown_EventSetup.Inst.CLEAR_EVENT_DATA();
        else if (SceneManager.GetActiveScene().name.Equals("AndarBahar"))
            AB_EventSetup.Inst.CLEAR_EVENT_DATA();
        else if (SceneManager.GetActiveScene().name.Equals("ZooRoulette"))
            ZooRoulette_Game.ZooRoulette_EventManager.Inst.CLEAR_EVENT_DATA();
        else if (SceneManager.GetActiveScene().name.Equals("Mines"))
            Mines_EventSetup.Inst.CLEAR_EVENT_DATA();
    }
    //public void App_RLR()
    //{
    //    AppPause = false;
    //    if (PlayerPrefs.HasKey("Last_Login_User") && PlayerPrefs.GetString("Last_Login_User") != "null")
    //    {
    //        if (PlayerPrefs.GetString("Last_Login_User") == "guest")
    //        {
    //            PreeLoader.Inst.Show();
    //            SendData(SocketEventManager.Inst.PLAY_AS_GUEST());
    //        }
    //        if (PlayerPrefs.GetString("Last_Login_User") == "mobile")
    //        {
    //            PreeLoader.Inst.Show();
    //            SendData(SocketEventManager.Inst.PLAY_AS_MOBILE_NUMBER(PlayerPrefs.GetString("mobile"), "", ""));
    //        }
    //    }
    //    else
    //    {
    //        SceneManager.LoadSceneAsync("Login");
    //    }
    //}

    //if (SceneManager.GetActiveScene().name.Equals("Roullate"))
    //    {
    //        PreeLoader.Inst.Show();
    //        //UB_EvenetSystem.Inst.BET_SYSTEM_CLEAR();
    //        //UB_EvenetSystem.Inst.EVENT_SYSTEM_CLEAR();
    //        SocketHandler.Inst.SendData(SocketEventManager.Inst.ROULETTE_GAME_INFO());
    //        //SocketHandler.Inst.socket.sid = "";
    //        //SocketHandler.Inst.socket.Close();                
    //    }
    //    else if (SceneManager.GetActiveScene().name.Equals("DragonTiger"))
    //    {
    //        PreeLoader.Inst.Show();
    //        //UB_EvenetSystem.Inst.BET_SYSTEM_CLEAR();
    //        //UB_EvenetSystem.Inst.EVENT_SYSTEM_CLEAR();
    //        SocketHandler.Inst.SendData(SocketEventManager.Inst.DRAGON_TIGER_GAME_INFO());
    //    }
}
