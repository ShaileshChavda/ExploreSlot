namespace ZooRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class ZooRoulette_Player : MonoBehaviour
    {
        public static ZooRoulette_Player Inst { get; set; }
        public TextMeshProUGUI Txt_UserName, TxtChips, TxtPlusMinus;
        public Image Vip_Ring;
        public string ID;
        public float MyCoins;
        public double WinOrLose_Chips;
        public bool Played_Chips;
        public Status _Status = Status.Null;
        public bool Is_Bot = false;
        [SerializeField] Animator Chaal_Anim, Win_Plus_Minus_Anim;
        [SerializeField] IMGLoader User_PIC;
        // Start is called before the first frame update
        void Awake()
        {
            Inst = this;
            //Debug.Log("PLAYER: Other");
            Txt_UserName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TxtChips = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            Vip_Ring = transform.GetChild(3).GetComponent<Image>();
            TxtPlusMinus = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
            Win_Plus_Minus_Anim = transform.GetChild(4).GetComponent<Animator>();
            User_PIC = this.GetComponent<IMGLoader>();
            Chaal_Anim = this.GetComponent<Animator>();
        }
        private void OnEnable()
        {
            ZooRoulette_EventManager._USER_CHAL += CHAAL;
            ZooRoulette_EventManager._USER_LEAVE += LEAVE;
        }

        private void OnDisable()
        {
            ZooRoulette_EventManager._USER_CHAL -= CHAAL;
            ZooRoulette_EventManager._USER_LEAVE -= LEAVE;
        }

        public void CHAAL(string id)
        {
            if (id.Equals(ID))
            {
                ZooRoulette_PlayerManager.Inst.Chal_Player = this;
            }
        }

        public enum Status
        {
            Play,
            Null,
        }

        public void SET_PLAYER_DATA(JSONObject data)
        {
            ID = data.GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
            Txt_UserName.text = data.GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);
            MyCoins = float.Parse(data.GetField("wallet").ToString().Trim(Config.Inst.trim_char_arry));
            TxtChips.text = MyCoins.ToString("n2");
            Is_Bot = bool.Parse(data.GetField("is_robot").ToString().Trim(Config.Inst.trim_char_arry));
            User_PIC.LoadIMG(data.GetField("profile_url").ToString().Trim(Config.Inst.trim_char_arry), false, false);
            Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[int.Parse(data.GetField("vip_level").ToString().Trim(Config.Inst.trim_char_arry))];
            _Status = Status.Play;
        }

        public void Update_Win_Loss_Chips()
        {
            if (WinOrLose_Chips < 0)
            {
                TxtPlusMinus.color = Color.red;
                TxtPlusMinus.text = "-" + WinOrLose_Chips.ToString().Replace("-", "");
            }
            else
            {
                TxtPlusMinus.color = Color.green;
                TxtPlusMinus.text = "+" + WinOrLose_Chips.ToString();
            }

            if (Played_Chips)
            {
                Played_Chips = false;
                if (WinOrLose_Chips > 0)
                {
                    if (TxtChips.text != "" && TxtChips.text != " ")
                        TxtChips.text = (double.Parse(TxtChips.text) + WinOrLose_Chips).ToString();
                }
                Win_Plus_Minus_Anim.Play("WinPlusMinus_Anim", 0);
            }
        }

        //----------- Card Chal Animation----------------
        public void Chaal_Animation(string Chaa_Amount, string side)
        {
            // Debug.Log("CHAL ANIM: "+ Chaa_Amount +"|"+ side);
            ZooRoulette_Symbol sy = null;
            for (int i = 0; i < ZooRoulette_GameManager.instance.PlaceBetButtonList.Count; i++)
            {
                string st = ZooRoulette_GameManager.instance.PlaceBetButtonList[i].name;
                if (st.Contains(side))
                {
                    sy = ZooRoulette_GameManager.instance.PlaceBetButtonList[i];
                    break;
                }
            }

            //UnityEngine.Debug.Log("CURRENT BET ITEM: " + sy.name);
            Vector3 target;
            if (sy.id.Equals(9) || sy.id.Equals(10))
            {
                target = ZooRoulette_GameManager.instance.getRandomPoint(sy.goMoveTarget, 2);
            }
            else
            {
                target = ZooRoulette_GameManager.instance.getRandomPoint(sy.goMoveTarget, 0);
            }
          
            GameObject _Coin = Instantiate(ZooRoulette_GameManager.instance.cloneCoinPrefab);
            ZooRoulette_ChipManager _chipManager = _Coin.transform.GetComponent<ZooRoulette_ChipManager>();
            _chipManager.userChipStatus = UserStatus.OTHERUSER;
            if (PlayerPrefs.GetInt("sound").Equals(1))
                ZooRoulette_GameManager.instance.Coin_Audio_Source.PlayOneShot(Zoo_Roulette_Sound.Inst.SFX[3]);
            _chipManager.SET_COIN(Chaa_Amount);
            _Coin.transform.SetParent(sy.gameObject.transform, false);
            _Coin.name = "CHAL_USER_COIN_" + Chaa_Amount;
            _Coin.transform.position = transform.position;
            _chipManager.Move_Anim(target, 0.6f);
            sy.OtherUserChipCalculate(true, _chipManager);
            ZooRoulette_GameManager.instance.saveManager.CheckAlreadyUserBetStatus(sy.id, _chipManager.val, UserStatus.OTHERUSER);
        }
        IEnumerator Coin_Hide(GameObject coin)
        {
            yield return new WaitForSeconds(1.1f);
            coin.transform.localScale = Vector3.zero;
        }
        //----------- Card Chal Animation----------------

        public void LEAVE(string _id)
        {
            if (_id.Equals(ID))
            {
                _Status = Status.Null;
                ID = "";
                Txt_UserName.text = "";
                TxtChips.text = "";
                MyCoins = 0;
                Is_Bot = false;
                User_PIC.icon.sprite = ZooRoulette_PlayerManager.Inst.EmptySeat_Sprite;
                Vip_Ring.sprite = GS.Inst.VIP_RING_LIST[0];
            }
        }

        public void SEAT(JSONObject data)
        {
            // if (_Status == Status.Null)
            SET_PLAYER_DATA(data);
        }
    }
}