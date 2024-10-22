using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    [Header("General Settings")]
    public bool _isGunPress = false;
    public bool isAutoFocus = false;
    public bool isAttack = false;
    public int intAccelaration = 1;


    [Space]
    [Header("Gun Area")]
    [SerializeField]
    private float angleOffset = -90f;
    [SerializeField]
    private float rotationSpeed = 5f;

    [Space]
    [Header("Gun Shot Time")]
    private float lastShotTime;
    public bool ReadyToFire => Time.time - lastShotTime >= fireRate;


    [Space]
    [Header("Bullet Init Area")]
    [SerializeField]
    private BulletControl bulletPrefab;
    [SerializeField]
    private float fireRate = 1f;
    [SerializeField]
    private Transform firePoint;
    public Transform trBulletContainer;
    [SerializeField]
    private LayerMask targetMask;


    [Space]
    [Header("Target Area")]

    [SerializeField]
    private float detectionRange = 10f;
    [SerializeField]
    private FishControl target;
    [SerializeField]
    private int prevTargetID;
    [SerializeField]
    private Image imgFishLock;
    [SerializeField]
    private GameObject goLockFish;

    [Space]
    [Header("Coin UI")]
    public Transform trCoinUI;
    public GameObject goCoin;
    public GameObject goCoinText;


    [Space]
    [Header("Camera")]
    private Camera mainCamera;


    [Space]
    [Space]
    [Header("Bot-Player Area")]
    [SerializeField]
    private float fltPlayerWaitTime;
    [SerializeField]
    private float fltOtherPlayerShoot;

    [SerializeField]
    public int changeAttack;
    public int updateAttack;

    [SerializeField]
    public int changeAcc;
    public int updateAcc;

    [SerializeField]
    private PlayerManager _pm;

    [SerializeField]
    private DottedLine dottedLine;

    private float increaseFactor = 2.0f;
    private Vector3 randomScreenPoint = Vector3.zero;
    private Vector3 mousePoint = Vector3.zero;

    private void OnEnable()
    {
        mainCamera = Camera.main;
    }

    
    void Update()
    {
        if (isAutoFocus)
        {
            HandleFocusUpdate();
        }
        else
        {
            HandleNonFocusUpdate();
        }
    }

    private void HandleFocusUpdate()
    {
        if (target == null)
            LockRandomTargetWithinScreen();
        else
            ReleaseTargetIfOutOfScreen();

        AimAtTarget();

        if (isAttack || _isGunPress)
        {
            ShootOnFocusTarget();
        }
    }

    private void HandleNonFocusUpdate()
    {
        if (isAttack || _isGunPress)
        {
            ShootOnTap();
        }
    }

    private void LockRandomTargetWithinScreen()
    {
        Collider2D[] pickTargets = Physics2D.OverlapCircleAll(transform.position, detectionRange, targetMask);

        if (pickTargets.Length > 0)
        {
            int randomIndex = Random.Range(0, pickTargets.Length);
            Vector3 screenPoint = mainCamera.WorldToViewportPoint(pickTargets[randomIndex].transform.position);

            if (screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1 && screenPoint.z > 0)
            {
                target = pickTargets[randomIndex].GetComponent<FishControl>();
                FishLockMethod(true);
                target.LockTarget(true, false);
            }
        }
    }

    private void ReleaseTargetIfOutOfScreen()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(target.transform.position);
        if (screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1 || screenPoint.z < 0)
        {
            prevTargetID = target.ID;
            target.LockTarget(false, false);
            FishLockMethod(false);
            target = null;
            UnassignTargetInBullet();
        }
    }

    // Method to Un-assign target in all bullets
    void UnassignTargetInBullet()
    {
        BulletControl[] bcAry = FindObjectsOfType<BulletControl>();
        foreach (BulletControl bullet in bcAry)
        {
            if (bullet.targetId == prevTargetID && bullet.playerId == _pm.PlayerID)
                bullet.UnlockTarget();
        }
    }

    private void ReleaseTarget()
    {
        FishLockMethod(false);

        if (target != null)
        {
            prevTargetID = target.ID;
            target.LockTarget(false, false);
            target = null;
            UnassignTargetInBullet();
        }
    }

    public void TargetDestroy()
    {
        if (target != null)
        {
            prevTargetID = target.ID;
            target.LockTarget(false, false);
            target = null;
            UnassignTargetInBullet();
        }
    }

    private void AimAtTarget()
    {
        if (target == null)
            return;
        Vector3 targetDirection = target.transform.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + angleOffset;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        dottedLine.DrawDottedLine(dottedLine.transform.position, target.goTarget.transform.position);
    }

    private void ShootOnTap()
    {
        if (!_pm.CheckCreditStatus())
            return;

        if (!ReadyToFire || isAutoFocus)
            return;

        if (!_pm.IsPlayer)
        {
            mousePoint = mainCamera.ScreenToWorldPoint(randomScreenPoint);
        }
        else
        {
            mousePoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        Vector3 targetDirection = Vector3.Normalize(mousePoint + Vector3.forward * 10 - transform.position);
        transform.up = targetDirection;

        BulletControl goBC = Instantiate(bulletPrefab, new Vector3(firePoint.position.x, firePoint.position.y, 10f), firePoint.rotation);
        goBC.transform.SetParent(trBulletContainer, false);
        goBC.WithoutTargetToFire(transform, _pm.PlayerIndex,_pm.PlayerID);
        lastShotTime = Time.time;
    }
    private void ShootOnFocusTarget()
    {
        if (!_pm.CheckCreditStatus())
            return;

        if (target == null || !ReadyToFire)
            return;

        //Debug.Log("Fire");
        BulletControl bulletInstance = Instantiate(bulletPrefab, new Vector3(firePoint.position.x,
           firePoint.position.y, 10f), firePoint.rotation);
        bulletInstance.transform.SetParent(trBulletContainer, false);
        bulletInstance.LockOnTargetToFire(transform, target.transform, _pm.PlayerIndex, _pm.PlayerID, target.ID);
        lastShotTime = Time.time;
    }

    public void GetRandomPoint()
    {
        if (!isAutoFocus)
            randomScreenPoint = new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0);

        RandomPlayerAttack();
    }

    public void RandomPlayerAttack()
    {
        updateAttack++;

        if (updateAttack >= changeAttack)
        {
            UpdateAttack();
        }

        updateAcc++;

        if (updateAcc >= changeAcc)
        {
            UpdateAccelaration(Random.Range(1, 4));
        }
    }

    public void UpdateAttack()
    {
        //Debug.Log("Update Attack");
        updateAttack = 0;
        UpdateAutoMethod(0);
        int ran = Random.Range(1, 100);
        isAutoFocus = ran % 2 == 0 ? true : false;

        if (!isAutoFocus)
        {
            ReleaseTarget();
        }
    }

    

    public void OnAutoFocus()
    {
        isAutoFocus = !isAutoFocus;

        if (!isAutoFocus)
        {
            ReleaseTarget();
        }
        //Debug.Log("FOCUS: " + isAutoFocus);
    }

    public void OnAttack()
    {
        isAttack = !isAttack;
        //Debug.Log("ATTACK: " + isAttack);
    }

    public void OnAccelaration()
    {
        if (intAccelaration == 1)
        {
            intAccelaration = 2;
            fireRate = 0.2f;
        }
        else if (intAccelaration == 2)
        {
            intAccelaration = 3;
            fireRate = 0.1f;
        }
        else if (intAccelaration == 3)
        {
            intAccelaration = 1;
            fireRate = 0.3f;
        }
        //Debug.Log("ACCELARATION: " + intAccelaration);
    }

    public void UpdateAccelaration(int val)
    {
        updateAcc = 0;
        UpdateAutoMethod(1);

        intAccelaration = val;

        if (val == 1)
        {
            fireRate = 0.3f;
        }
        else if (val == 2)
        {
            fireRate = 0.2f;
        }
        else if (val == 3)
        {
            fireRate = 0.1f;
        }
        //Debug.Log("ACCELARATION: " + intAccelaration);
    }

    public void FishLockMethod(bool status)
    {
        if (status)
        {
            if (!goLockFish.activeSelf)
            {
                goLockFish.SetActive(status);
            }

            imgFishLock.sprite = target.sp.sprite;
        }
        else
        {
            imgFishLock.sprite = null;
            goLockFish.SetActive(status);
        }
    }

    public void OnPressBtn()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (isAttack || isAutoFocus)
            return;
        _isGunPress = true;
    }

    public void OnRealseBtn()
    {
        _isGunPress = false;
    }


    public void CallBotPlayerShoot()
    {
        StartCoroutine(IEPlayerShoot());
    }

    IEnumerator IEPlayerShoot()
    {
        yield return new WaitForSeconds(2f);
        UpdateAttack();
        UpdateAccelaration(Random.Range(1, 4));
        GetRandomPoint();
        isAttack = true;
        InvokeRepeating("GetRandomPoint", fltPlayerWaitTime, fltOtherPlayerShoot);
    }

    void UpdateAutoMethod(int val)
    {
        if (val == 0)
        {
            changeAttack = Random.Range(30, 180);
        }
        else
        {
            changeAcc = Random.Range(10, 120);
        }
    }
}
