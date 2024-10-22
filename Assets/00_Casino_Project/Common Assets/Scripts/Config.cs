using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using UnityEditor;

public enum ServerType
{
    Live_LuckAndSkill = 0,
    Live_CasinoKing = 1,
    Live_TeenPatti = 2,
    Local_Kanraj = 3,
    Live_Rummy_Kart = 4,
    Live_Rummy_Crystal = 5,
    Live_TeenPattiCashout = 6,
}

public enum ConnectionType
{
    ChooseServer = 0,
    WebSocket = 1
}

public class ServerConfig
{
    public float AV;
    public float IV;
    public bool AFDNV;
    public bool IFDNV;
    public bool m_Mode;
    public string m_Msg;
    public int removeAfter;

    public ServerConfig()
    {
        AV = 0f;
        IV = 0f;
        AFDNV = false;
        IFDNV = false;
        m_Mode = false;
        m_Msg = "";
        removeAfter = 0;
    }
}

public class Config : MonoBehaviour
{
    public static Config Inst;
    [SerializeField] ServerType _serverType;
    [SerializeField] ConnectionType _connectionType;
    internal ServerConfig _serverConfig;
    [SerializeField] internal float Version;
    string connectionTyp = "";
    internal string BaseURL = "";
    internal string S3URL = "";
    [SerializeField] internal string SocketServerURL = "";

    internal char[] trim_char_arry = new char[] { '"' };

    internal string[] allServer =
    {
        "http://43.205.49.65:3000/",//Luck And skill(Live) //com.luckandskill.casino v0.5=5
        "http://43.205.91.130:3000/",//Casino King(Live) //com.luckandskill.casino v0.5=5
        "http://13.200.108.15:3000/",//TeenPatti(Live) //com.teenpatti.latest v0.3=3
        "http://65.0.132.210:3000/",//Luck And Skill(Kanraj)  
        "http://13.232.125.42:3000/",//Rummy Kart  
        //"http://65.0.132.210:3000/",//Rummy Crystal Testing server 
        "http://3.110.196.178:3000/",//Rummy Crystal Live
        "http://3.108.218.243:3000/"//TeenPattiCashout
    };


    private void Awake()
    {
        Inst = this;
        //Player_Setting();
        BaseURL = allServer[(int)_serverType];
        if (_connectionType.Equals(ConnectionType.WebSocket))
        {
            connectionTyp = "WebSocket";
        }
        else
        {
            connectionTyp = "ChooseServer";
        }

        Version = float.Parse(Application.version.ToString());
    }
    // Start is called before the first frame update
    void Start()
    {
        _serverConfig = new ServerConfig();
        StartCoroutine(CheckHTTPCall());
    }

    internal IEnumerator CheckHTTPCall()
    {
        NT:
        if (string.IsNullOrEmpty(connectionTyp) && !NetworkCheck.IsInternetConnection())
        {
            yield return new WaitForSecondsRealtime(0.1f);
            goto NT;
        }

        if (connectionTyp.Equals("ChooseServer"))
        {
            string URL = "";

            // Create URL Based On Plateform for Choose Server
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    URL = BaseURL + "chooseServer?device_type=android&version=" + Version;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    URL = BaseURL + "chooseServer?device_type=ios&version=" + Version;
                    break;
                default:
                    URL = BaseURL + "chooseServer?device_type=android&version=" + Version;
                    break;
            }
            Debug.Log("bas url_____" + URL);
            WWW www = new WWW(URL);
            yield return www;
            if (www.error != null)
            {
                goto NT;
            }
            else
            {
                JSONObject data = new JSONObject(www.text);
                Debug.Log("config ___data_____" + data);

                string host = data.GetField("SOCKET_URL").GetField("host").ToString().Trim(new char[] { '"' });
                string port = data.GetField("SOCKET_URL").GetField("port").ToString().Trim(new char[] { '"' });
                BaseURL = data.GetField("config").GetField("BASE_URL").ToString().Trim(new char[] { '"' });
                S3URL = data.GetField("config").GetField("S3_URL").ToString().Trim(new char[] { '"' });

                _serverConfig.AV = float.Parse(data.GetField("versionInfo").GetField("new_version").ToString().Trim(BasicStuff.trim_char_arry));
                VersionUpdate.Inst.Version_URL = data.GetField("versionInfo").GetField("ver_url").ToString().Trim(BasicStuff.trim_char_arry);
                _serverConfig.m_Mode = bool.Parse(data.GetField("MM").ToString().Trim(new char[] { '"' }));

                SocketServerURL = "ws://" + host + ":" + port + "/socket.io/?EIO=4&transport=websocket";

                if (_serverConfig.m_Mode)
                    Maintenance.Inst.Start_MM_MODE();
                else
                    CheckForVersionUpdate();
            }

        }
        else
        {
            //call direct socket connection
        }
    }
    void CheckForVersionUpdate()
    {
        if (_serverConfig.AV > Version)
            VersionUpdate.Inst.Show(_serverConfig.AV.ToString());
        else
            SocketHandler.Inst.Create();

        Debug.Log("URL :" + SocketServerURL);
    }

    //void Player_Setting()
    //{
    //    switch ((int)_serverType)
    //    {
    //        case 0:
    //            PlayerSettings.companyName = "LuckAndSkill_PVT";
    //            PlayerSettings.productName = "LuckAndSkill";
    //            PlayerSettings.applicationIdentifier = "com.luckandskill.casino";
    //            PlayerSettings.bundleVersion = "0.3";
    //            PlayerSettings.Android.bundleVersionCode = 3;
    //            break;
    //        case 1:
    //            PlayerSettings.companyName = "CasinoKing_PVT";
    //            PlayerSettings.productName = "Casino King";
    //            PlayerSettings.applicationIdentifier = "com.casinoking.casino";
    //            PlayerSettings.bundleVersion = "0.4";
    //            PlayerSettings.Android.bundleVersionCode = 4;
    //            break;
    //        case 2:
    //            PlayerSettings.companyName = "TeenPattiLatest_PVT";
    //            PlayerSettings.productName = "Teen Patti Latest";
    //            //PlayerSettings.applicationIdentifier = "com.teenpattilatest.casino";
    //           // PlayerSettings.applicationIdentifier = "com.luckandskill.casino";
    //            PlayerSettings.bundleVersion = "0.1";
    //            PlayerSettings.Android.bundleVersionCode = 1;
    //            break;
    //        case 3:
    //            PlayerSettings.companyName = "LuckAndSkill_PVT";
    //            PlayerSettings.productName = "LuckAndSkill";
    //            PlayerSettings.applicationIdentifier = "com.luckandskill.casino";
    //            PlayerSettings.bundleVersion = "0.3";
    //            PlayerSettings.Android.bundleVersionCode = 3;
    //            break;
    //        default:
    //            PlayerSettings.companyName = "LuckAndSkill_PVT";
    //            PlayerSettings.productName = "LuckAndSkill";
    //            PlayerSettings.applicationIdentifier = "com.luckandskill.casino";
    //            PlayerSettings.bundleVersion = "0.3";
    //            PlayerSettings.Android.bundleVersionCode = 3;
    //            break;
    //    }
    //}
}
