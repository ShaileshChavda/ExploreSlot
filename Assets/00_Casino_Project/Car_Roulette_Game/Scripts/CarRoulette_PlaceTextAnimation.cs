namespace CarRoulette_Game
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;
    using DG.Tweening;
    using System.Diagnostics;
    using System.Collections.Specialized;
    using UnityEngine.EventSystems;
    using TMPro;
    using UnityEngine.UI;

    public class CarRoulette_PlaceTextAnimation : MonoBehaviour
    {
        public float _animDuration = 1f;
        private void OnEnable()
        {
            Vector3 originalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            transform.DOScale(Vector3.one, _animDuration).SetEase(Ease.Linear).
                OnComplete(() => transform.localScale = originalScale);
        }       
    }
}
