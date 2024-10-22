using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PFB_AVATAR : MonoBehaviour
{
    public static PFB_AVATAR Inst;
    string Avatar_URL;
    [SerializeField] IMGLoader Avatar_Pic;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void ClicK_AVATAR()
    {
        Profile.Inst.CLOSE_AVATAR_SC();
        SocketHandler.Inst.SendData(SocketEventManager.Inst.UPDATE_PROFILE("avatar", Avatar_URL));
    }

    public void Load_Pic(string Url)
    {
        Avatar_URL = Url;
        Avatar_Pic.LoadIMG(Url,false, false);
    }
}
