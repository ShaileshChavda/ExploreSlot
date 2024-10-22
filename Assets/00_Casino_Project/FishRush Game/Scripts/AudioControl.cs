using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour
{

    public static bool _isSound = true;
    public static bool _isMusic = true;
    public static AudioControl Instance;
    public static int _indexMusic=1;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void openbg()
    {
        if (_isMusic)
        {
            _indexMusic = Random.Range(1, 5);
           // MasterAudio.PlaySound("bg" + _indexMusic);
        }
    }

    public void Offbg()
    {
       // MasterAudio.StopAllOfSound("bg" + _indexMusic);
    }

    public void shoot()
    {
        if (_isSound)
        {
           // MasterAudio.PlaySound("shot");
        }
    }

    public void clickButton()
    {
        if (_isSound)
        {
           // MasterAudio.PlaySound("button");
        }
    }

    public void nguyentu()
    {
        if (_isSound)
        {
           // MasterAudio.PlaySound("nguyentu");
        }
    }

    public void boom()
    {
        if (_isSound)
        {
            //MasterAudio.PlaySound("bom");
        }
    }

    public void coin()
    {
        if (_isSound)
        {
            //MasterAudio.PlaySound("coin");
        }
    }
}
