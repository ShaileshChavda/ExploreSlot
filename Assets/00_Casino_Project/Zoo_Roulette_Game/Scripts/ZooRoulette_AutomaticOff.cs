namespace ZooRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ZooRoulette_AutomaticOff : MonoBehaviour
    {
        public bool isAutoOff = false;
        public float autoTimer = 2f;

        private void OnEnable()
        {
            if (isAutoOff)
            {
                Invoke(nameof(WaitCallAuto), autoTimer);
            }
        }

        public void WaitCallAuto()
        {
            gameObject.SetActive(false);
        }
    }
}
