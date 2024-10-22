using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtManager : MonoBehaviour
{
    public Transform currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float angleOffset = 90f;
    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = currentTarget.transform.position;
        targetPosition.z = (transform.position.z); // Ensure the target is at the same depth as the object
        Vector3 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
