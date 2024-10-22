using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class PlaySpineAnimation : MonoBehaviour
{
    public string animationName; // Name of the animation you want to play
   
    [SerializeField]
    private SkeletonAnimation skeletonAnimation;

    [SerializeField]
    private bool _loopStatus;
    public float animationSpeed;

    private void OnEnable()
    {
        FishRunAnim();
    }
    public void FishRunAnim()
    {
        // Play the animation
        skeletonAnimation.AnimationState.SetAnimation(0, animationName, _loopStatus).TimeScale = animationSpeed;
    }

    
}
