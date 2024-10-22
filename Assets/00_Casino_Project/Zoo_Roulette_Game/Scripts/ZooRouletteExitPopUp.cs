namespace ZooRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ZooRouletteExitPopUp : MonoBehaviour
    {
        public static ZooRouletteExitPopUp Inst;
        // Start is called before the first frame update
        void Start()
        {
            Inst = this;
        }

        public void Open_Popup()
        {
            //Roullate_SoundManager.Inst.PlaySFX(38);
            GS.Inst.iTwin_Open(this.gameObject);
        }
        public void Close_Popup()
        {
            //Roullate_SoundManager.Inst.PlaySFX(38);
            GS.Inst.iTwin_Close(this.gameObject, 0.3f);
        }
        public void BTN_YES()
        {
            //Roullate_SoundManager.Inst.PlaySFX(38);
            ZooRoulette_EventManager.Inst.CLEAR_EVENT_DATA();
            SocketHandler.Inst.SendData(SocketEventManager.Inst.ZOO_ROULETTE_CLOSE_GAME());
            SceneManager.LoadScene(2);
        }
    }
}