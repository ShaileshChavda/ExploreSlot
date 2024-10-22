namespace CarRoulette_Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CarRouletteSaveManager : MonoBehaviour
    {
        public bool isTest = false;
        public UserBetDataClass _userBetDataClass = new UserBetDataClass();

        private void OnEnable()
        {
            if (!isTest)
            {
                Debug.Log("LOAD USER SAVE BET DATA");
                _userBetDataClass = CarRoulette_DataConfig.LoadUserBetData();
                addUserDataLst.Clear();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _userBetDataClass.myUserBetCounter = 0;

                for (int i = 0; i < _userBetDataClass.betDataClassLst.Count; i++)
                {
                    for (int j = 0; j < _userBetDataClass.betDataClassLst[i].userStatusBetClassLst.Count; j++)
                    {
                        if (_userBetDataClass.betDataClassLst[i].userStatusBetClassLst[j].userStatus == UserStatus.MYUSER)
                        {
                            _userBetDataClass.myUserBetCounter++;
                        }
                    }
                }
            }
        }

        public void CheckMaxBetReached()
        {
            _userBetDataClass.myUserBetCounter = 0;

            for (int i = 0; i < _userBetDataClass.betDataClassLst.Count; i++)
            {
                for (int j = 0; j < _userBetDataClass.betDataClassLst[i].userStatusBetClassLst.Count; j++)
                {
                    if (_userBetDataClass.betDataClassLst[i].userStatusBetClassLst[j].userStatus == UserStatus.MYUSER)
                    {
                        _userBetDataClass.myUserBetCounter++;
                    }
                }
            }
        }

        public void CheckAlreadyUserBetStatus(int side, int amt, UserStatus addStatus)
        {
            BetDataClass currClass = _userBetDataClass.betDataClassLst.Find(num => num.side == side);

            if (currClass == null)
            {
                _userBetDataClass.betDataClassLst.Add(new BetDataClass());
                int ubdcCounter = _userBetDataClass.betDataClassLst.Count;
                int currUserClassIndex = ubdcCounter - 1;
                BetDataClass usbd = _userBetDataClass.betDataClassLst[currUserClassIndex];
                usbd.side = side;

                usbd.userStatusBetClassLst.Add(new UserStatusBetClass());
                int usbcCounter = usbd.userStatusBetClassLst.Count;
                int currStatsBetClassIndex = usbcCounter - 1;
                UserStatusBetClass usbc = usbd.userStatusBetClassLst[currStatsBetClassIndex];
                usbc.userStatus = addStatus;

                usbc.betValueLst.Add(new BetValue());
                int bvCounter = usbc.betValueLst.Count;
                int currBetClassIndex = bvCounter - 1;

                BetValue bv = usbc.betValueLst[currBetClassIndex];
                bv.betValue = amt;
                bv.totalBetValue += amt;
            }
            else
            {
                UserStatusBetClass usbd = currClass.userStatusBetClassLst.Find(num => num.userStatus == addStatus);

                if (usbd == null)
                {
                    currClass.userStatusBetClassLst.Add(new UserStatusBetClass());
                    int usbcCounter = currClass.userStatusBetClassLst.Count;
                    int currStatsBetClassIndex = usbcCounter - 1;
                    UserStatusBetClass usbc = currClass.userStatusBetClassLst[currStatsBetClassIndex];
                    usbc.userStatus = addStatus;

                    usbc.betValueLst.Add(new BetValue());
                    int bvCounter = usbc.betValueLst.Count;
                    int currBetClassIndex = bvCounter - 1;

                    BetValue bv = usbc.betValueLst[currBetClassIndex];
                    bv.betValue = amt;
                    bv.totalBetValue += amt;
                }
                else
                {
                    BetValue currBetVal = usbd.betValueLst.Find(num => num.betValue == amt);

                    if (currBetVal == null)
                    {
                        usbd.betValueLst.Add(new BetValue());
                        int bvCounter = usbd.betValueLst.Count;
                        int currBetClassIndex = bvCounter - 1;

                        BetValue bv = usbd.betValueLst[currBetClassIndex];
                        bv.betValue = amt;
                        bv.totalBetValue += amt;
                    }
                    else
                    {
                        currBetVal.betValue = amt;
                        currBetVal.totalBetValue += amt;
                    }
                }
            }

            CarRoulette_DataConfig.SaveUserBetData(_userBetDataClass);
        }

        public List<string> addUserDataLst;
        public void MyAlreadyUserBetStatus(int side, int amt)
        {
            string stSideData = side + ":" + amt;
            addUserDataLst.Add(stSideData);
        }

        public void ResetSaveData()
        {
            _userBetDataClass = new UserBetDataClass();
            CarRoulette_DataConfig.SaveUserBetData(_userBetDataClass);
        }

        public void ResetSaveRebetData()
        {
            addUserDataLst.Clear();
        }
    }

    [System.Serializable]
    public class UserBetDataClass
    {
        public int myUserBetCounter = 0;
        public List<int> betPlaceIndexLst = new List<int>();
        public List<BetDataClass> betDataClassLst = new List<BetDataClass>();
        public UserBetDataClass()
        {

        }
        public UserBetDataClass(int myUserBetCounter, List<int> betPlaceIndexLst, List<BetDataClass> betDataClassLst)
        {
            this.myUserBetCounter = myUserBetCounter;
            this.betPlaceIndexLst = betPlaceIndexLst;
            this.betDataClassLst = betDataClassLst;
        }
    }

    [System.Serializable]
    public class BetDataClass
    {
        public int side = 1;
        public List<UserStatusBetClass> userStatusBetClassLst = new List<UserStatusBetClass>();
    }

    [System.Serializable]
    public class UserStatusBetClass
    {
        public UserStatus userStatus;
        public List<BetValue> betValueLst = new List<BetValue>();
    }

    [System.Serializable]
    public class BetValue
    {
        //public bool isRebet = false;
        public int betValue = 0;
        public int totalBetValue = 0;
    }

    [System.Serializable]
    public class CurrentRoundStatus
    {
        public bool isRebet = false;
        public int round = 0;
    }
}