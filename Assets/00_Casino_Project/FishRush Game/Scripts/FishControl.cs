using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;

public class FishControl : MonoBehaviour
{
    [SerializeField]
    private int _fishID;
    public int ID;
    public bool isDead = false;
    [SerializeField] private bool isOutScreen = false;
    public bool IsOutScreen => isOutScreen;
    public Color hitColor;
    public SpriteRenderer sp;
    public string AnimationName;
    public string AnimationNameDie;
    Animator _ani;


    Swim _swim;
    [SerializeField]
    private int _hp;
    public int minHP, maxHP;

    GameObject _checkCollsion;
    bool _checkInvisible;
    // Event triggered when the enemy dies
    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    public int _gold;
    public GameObject goTarget;
    public Transform trContainer;
    public bool isLockTarget = false;

    [SerializeField]
    private Animator _dieAnim;

    [SerializeField]
    private GameObject _skDie;

    public bool isHitColor = false;

    Collider2D _collider2D;

    [SerializeField]
    private SkeletonAnimation _saAnim;

    [SerializeField]
    private float animationSpeed;

    [SerializeField]
    private FishKillInfo _fishKillInfo;

    void OnEnable()
    {
        ID = GetInstanceID();
        FishUIManager.Instance._fcList.Add(this);
        isDead =false;
        _checkInvisible = false;
        sp = GetComponent<SpriteRenderer>();
        _ani = GetComponent<Animator>();

        animationSpeed = Random.Range(0f, 1f);
        _ani.Play(AnimationName, 0, animationSpeed);

        _collider2D = GetComponent<Collider2D>();
        _collider2D.isTrigger = true;

        goTarget = transform.GetChild(0).gameObject;
        goTarget.SetActive(false);
        _swim = GetComponent<Swim>();

        _hp = Random.Range(minHP, maxHP);

        Invoke(nameof(WaitCallInvoke),7f);
    }

    public void FishRunAnim()
    {
        // Play the animation
        _saAnim.AnimationState.SetAnimation(0,"Idle", true).TimeScale = animationSpeed;
    }

    public void WaitCallInvoke()
    {
        isOutScreen = true;
    }

    public void LockTarget(bool status,bool colorChange)
    {
        isLockTarget = status;
        goTarget.SetActive(status);

        if(colorChange)
        {
            if (status)
                GetComponent<SpriteRenderer>().color = Color.black;
            else
                GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void TakeDamage(int damageAmount, int pIndex,string pId)
    {
       // Debug.Log("P INDEX: "+ pIndex);
        //_hp -= damageAmount;
        StartCoroutine(HitColorChange());
        FishUIManager.Instance.SEND_FISH_KILL_EVENT(_fishID,ID,pId);

        //if (_hp <= 0) 
        //{
        //    Die(pIndex);
        //}
    }

    public void Die(int pIndex)
    {
        // Trigger the death event
        if (OnDeath != null)
        {
            OnDeath();
        }

        _swim.enabled = false;
        LockTarget(false, false);
        GetComponent<BoxCollider2D>().enabled = false;
        GetPlayerCoin(pIndex);
        FishManage.Instance._FishMange.Remove(transform);
        // Destroy the enemy GameObject
        Destroy(gameObject);
    }

    // Method to check if the enemy is dead
    public bool IsDead()
    {
        return _hp <= 0;
    }

    public void GetPlayerCoin(int playerIndex)
    {
        GunManager currPlayer = FishUIManager.Instance.goPlayerAry[playerIndex].GunManager;

        currPlayer.TargetDestroy();

        GameObject fishAnim = Instantiate(_skDie, transform.position, transform.rotation);
        fishAnim.transform.SetParent(FishUIManager.Instance.trCoinParant);
        //fishAnim.GetComponent<Animator>().Play(AnimationNameDie, 0, 0);
        Destroy(fishAnim,0.8f);

        GameObject go = Instantiate(currPlayer.goCoin);
        go.transform.SetParent(FishUIManager.Instance.trCoinParant);
        go.transform.localScale = new Vector3(80,80,1);
        coinEffControl coinEff = go.GetComponent<coinEffControl>();
        Transform trTarget = currPlayer.trCoinUI.GetChild(0);
        coinEff.InitCoinEffect(transform.position, trTarget);


        GameObject goCoinText = Instantiate(currPlayer.goCoinText);
        goCoinText.transform.SetParent(FishUIManager.Instance.trCoinParant);
        goCoinText.transform.localScale = new Vector3(20f,20f,1f);
        effScoreTextControl coinText = goCoinText.GetComponent<effScoreTextControl>();
        currPlayer.trCoinUI.GetComponent<PlayerManager>().UpdateScore(_gold);
        coinText.InitEffScore(transform.position, _gold);
    }

    public IEnumerator HitColorChange()
    {
        if (sp.color == Color.red)
            yield break;

        sp.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        LockTarget(isLockTarget,true);
    }

    public void CollisionWithWave()
    {
        if (OnDeath != null)
        {
            OnDeath();
        }
        FishManage.Instance._FishMange.Remove(transform);
        Destroy(gameObject);
    }


    void OnBecameVisible()
    {
        if (gameObject.tag == "fish")
        {
            if (_checkInvisible) return;
            _checkInvisible = true;
            FishManage.Instance._FishMange.Add(transform);
            if (gameObject.name == "Fish12FreeSign(Clone)" || gameObject.name == "Fish11FreeSign(Clone)")
            {
                FishManage.Instance._CaMapManage.Add(transform);
            }
            else
            {
                if (gameObject.name == "Fish7Follow(Clone)" || gameObject.name == "Fish7FollowBonus(Clone)" || gameObject.name == "Fish7FreeSign(Clone)" || gameObject.name == "Fish6Follow(Clone)" || gameObject.name == "Fish6FollowBonus(Clone)" || gameObject.name == "Fish6FreeSign(Clone)")
                {
                    FishManage.Instance._MucManager.Add(transform);
                }
            }
        }
    }

    void OnDestroy()
    {
        if (gameObject.tag == "fish")
        {
            FishUIManager.Instance._fcList.Remove(this);
            FishManage.Instance._FishMange.Remove(transform);
            if (gameObject.name == "Fish12FreeSign(Clone)" || gameObject.name == "Fish11FreeSign(Clone)")
            {
                FishManage.Instance._CaMapManage.Remove(transform);
            }
            else
            {
                if (gameObject.name == "Fish7Follow(Clone)" || gameObject.name == "Fish7FollowBonus(Clone)" || gameObject.name == "Fish7FreeSign(Clone)" || gameObject.name == "Fish6Follow(Clone)" || gameObject.name == "Fish6FollowBonus(Clone)" || gameObject.name == "Fish6FreeSign(Clone)")
                {
                    FishManage.Instance._MucManager.Remove(transform);
                }
            }
        }
    }

    public void OnBecameInvisible()
    {
        if (!isOutScreen)
            return;

        if (gameObject.tag == "fish")
        {
            FishManage.Instance._FishMange.Remove(transform);
            if (gameObject.name == "Fish12FreeSign(Clone)" || gameObject.name == "Fish11FreeSign(Clone)")
            {
                FishManage.Instance._CaMapManage.Remove(transform);
            }
            else
            {
                if (gameObject.name == "Fish7Follow(Clone)" || gameObject.name == "Fish7FollowBonus(Clone)" || gameObject.name == "Fish7FreeSign(Clone)" || gameObject.name == "Fish6Follow(Clone)" || gameObject.name == "Fish6FollowBonus(Clone)" || gameObject.name == "Fish6FreeSign(Clone)")
                {
                    FishManage.Instance._MucManager.Remove(transform);
                }
            }
        }
        Destroy(gameObject);
    }
}
