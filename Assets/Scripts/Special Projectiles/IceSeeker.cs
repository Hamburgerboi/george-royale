using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IceSeeker : MonoBehaviourPun
{
    [Header("Target Follow")]
    public float seekingSpeed = 20.0f;
    public float seekingRotationSpeed = 100f;
    public float seekingRadius = 3.0f;
    public LayerMask targetPlatform;

    [Header("Damage")]
    public float projectileDamage = 12.0f;

    [Header("Others")]
    public float destroyTime = 20.0f;

    private Rigidbody2D rb;
    private Transform target;
    private Collider2D[] targetColliders;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(destroyTime <= 0 && photonView.IsMine) PhotonNetwork.Destroy(gameObject);
        destroyTime -= Time.deltaTime;

        if(target == null)
        {
            targetColliders = Physics2D.OverlapCircleAll(transform.position, seekingRadius, targetPlatform);
            foreach(Collider2D item in targetColliders)
            {
                if(item.gameObject.GetComponent<ElementType>().type != GetComponent<ElementType>().type)
                {
                    target = item.gameObject.transform;
                    break;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(target == null) return;

        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        
        rb.angularVelocity = -rotateAmount * seekingRotationSpeed;
        rb.velocity = transform.up * seekingSpeed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Damageable dmg = col.GetComponent<Damageable>();
        if (dmg != null && (col.GetComponent<ElementType>().type != gameObject.GetComponent<ElementType>().type))
        {
            Debug.Log("HIT");
            dmg.Damage(projectileDamage);
        }

        if(photonView.IsMine)
        {
            GetComponent<Renderer>().enabled = false;
            Invoke("InvokedDestroy", 0.2f);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, seekingRadius);
    }
    
    private void InvokedDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
