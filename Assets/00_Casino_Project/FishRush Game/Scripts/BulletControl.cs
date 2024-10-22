using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour
{
    public int playerId;
    public int targetId;
    public bool isLockTarget;
    public int playerIndex;
    public bool isUseRB = false;
    public Rigidbody2D rb;
    public int dame;
    public Sprite[] ListBullet;
    public float speed;
    public GameObject _web;
    SpriteRenderer _sprite;
    private Vector2 targetPoint;

    public Transform target;
    public float angleChangingSpeed;

    public float destroyTime;
    private Vector3 oldVelocity;
    public int bulletLevel;

    public float angleOffset;

    private void OnEnable()
    {
        _sprite = GetComponent<SpriteRenderer>();
        if (isUseRB)
            rb = GetComponent<Rigidbody2D>();

        //StartCoroutine(DestroyAfterLifetime());
    }

    void Update()
    {
        if (target != null)
        {
            if (!IsTargetAlive(target))
            {
                UnlockTarget();
                return;
            }

            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
            rb.rotation = angle;
            Vector2 velocity = direction * speed;
            rb.velocity = velocity;
            oldVelocity = rb.velocity;
        }
        else
        {
            //Debug.Log("Target null");
            // Smoothly move the bullet forward
            Vector3 forceVector = transform.up;
            rb.velocity = forceVector * speed;
            oldVelocity = rb.velocity;
        }
    }

    bool IsTargetAlive(Transform targetTransform)
    {
        FishControl fc = targetTransform.GetComponent<FishControl>();
        if (fc != null)
        {
            return !fc.IsDead();
        }
        return true;
    }

    IEnumerator DestroyAfterLifetime()
    {
        // Wait for the specified lifetime
        yield return new WaitForSeconds(destroyTime);
        WebInit(bulletLevel, transform);
        // Destroy the bullet after the lifetime
        Destroy(gameObject);
    }


    public void LockOnTargetToFire(Transform gunFace, Transform newTarget, int pIndex, int pId, int tId)
    {
        isLockTarget = true;
        playerIndex = pIndex;
        playerId = pId;
        targetId = tId;
        target = newTarget;

        _sprite.sprite = ListBullet[0];
        transform.up = gunFace.up;
        transform.eulerAngles = new Vector3(0, 0, gunFace.eulerAngles.z);
    }

    public void WithoutTargetToFire(Transform gunFace, int pIndex ,int pId)
    {
        isLockTarget = false;
        playerIndex = pIndex;
        playerId = pId;
        _sprite.sprite = ListBullet[0];
        transform.up = gunFace.up;
        transform.eulerAngles = new Vector3(0, 0, gunFace.eulerAngles.z);
    }

    public void UnlockTarget()
    {
        isLockTarget = false;
        target = null;
    }

    void WebInit(int level, Transform Gun)
    {
        GameObject web = (GameObject)Instantiate(_web);
        web.transform.position = transform.position;
        // web.transform.up = transform.up;
        web.transform.eulerAngles = new Vector3(0, 0, Gun.eulerAngles.z);
        web.GetComponent<WebControl>().InitWeb(level);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Debug.Log("COLLIDE FISH: "+ col.gameObject.tag);
        if (col.gameObject.tag.Equals("wall"))
        {
            if (isUseRB)
            {
                //Debug.Log("USE RB");
                ContactPoint2D contact = col.contacts[0];
                Vector3 reflectedVelocity = Vector3.Reflect(oldVelocity, contact.normal);
                rb.velocity = reflectedVelocity;
                oldVelocity = rb.velocity;
                float angle = Mathf.Atan2(reflectedVelocity.y, reflectedVelocity.x) * Mathf.Rad2Deg + angleOffset;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                StartCoroutine(DestroyAfterLifetime());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Debug.Log("COLLIDE FISH: "+ col.gameObject.tag);

        if (col.gameObject.tag == "fish")
        {
            FishControl fc = col.gameObject.GetComponent<FishControl>();
            
            if(target != null)
            {
                //Debug.Log("HIT BULLET TO SELECTED TARGET");

                // Check if the collided object has the same instance ID as the target
                if (fc.ID == targetId)
                {
                    // Call TakeDamage method on the enemy if it has an EnemyHealth component
                    fc.TakeDamage(dame, playerIndex,playerId.ToString());
                    WebInit(1, transform);
                    Destroy(gameObject);
                }
            }
            else
            {
                //Debug.Log("HIT BULLET TO NONE TARGET");

                fc.TakeDamage(dame, playerIndex, playerId.ToString());
                WebInit(1, transform);
                Destroy(gameObject);
            }
        }
    }
}
