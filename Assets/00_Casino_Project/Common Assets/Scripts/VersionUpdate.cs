using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionUpdate : MonoBehaviour {
	public static VersionUpdate Inst;
    public Text Txt_Version;
    public string Version_URL;

	void Awake ()
	{
		Inst = this;
	}

	internal void Show (string Version)
	{
		transform.localScale = Vector3.one;
        Txt_Version.text ="V "+Version;
    }

	public void BTN_UPDATE()
	{
		SoundManager.Inst.PlaySFX(0);
		Application.OpenURL(Version_URL);
	}
}
