using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DT_Circle_Anim : MonoBehaviour
{
    public static DT_Circle_Anim Inst;
   // [SerializeField] Image preloader;
   // RectTransform _rectLoader;
    public SkeletonGraphic playerWinEffectSkeleton;
   // internal bool isLoaded = false;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
       // _rectLoader = preloader.GetComponent<RectTransform>();
    }

    // Update is called once per frame
   /* void Update()
    {
        if (!preloader.enabled)
            return;
        _rectLoader.Rotate(Vector3.forward, -5);
    }*/

    public void Show()
    {
        transform.localScale = Vector3.one;
        playerWinEffectSkeleton.AnimationState.SetAnimation(0, "Animation", true);
       // preloader.enabled = true;
        Invoke(nameof(Stop_Loader), 2.3f);
    }

    public void Stop_Loader()
    {
       // preloader.enabled = false;
        transform.localScale = Vector3.zero;
        CancelInvoke(nameof(Stop_Loader));
    }
}
