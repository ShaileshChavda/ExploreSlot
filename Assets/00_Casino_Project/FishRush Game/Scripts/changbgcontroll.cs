using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class changbgcontroll : MonoBehaviour
{
    public Sprite[] _psr;
    Image _img;
    public SpriteRenderer _bg;

    void OnEnable()
    {
        _img = GetComponent<Image>();
        _img.sprite = _psr[(PlayerPrefs.GetInt("level") + 1) % 4];
        _img.fillAmount = 0;
    }

    void Update()
    {
        _img.fillAmount += Time.smoothDeltaTime / 4.1f;
        if (_img.fillAmount == 1)
        {
            _bg.sprite = _psr[(PlayerPrefs.GetInt("level") + 1) % 4];
            gameObject.SetActive(false);
        }
    }
}
