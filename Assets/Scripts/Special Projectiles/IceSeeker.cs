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
        if(col.gameObject.layer == 9 || !photonView.IsMine) return;

        PhotonView pView = PhotonView.Get(this);
        pView.RPC("RPCDamage", RpcTarget.All, col.gameObject.GetComponent<PhotonView>().ViewID);

        if(photonView.IsMine) PhotonNetwork.Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, seekingRadius);
    }
    
    [PunRPC]
    private void RPCDamage(int ID)
    {
        PhotonView view = PhotonView.Find(ID);
        Damageable dmg = view.gameObject.GetComponent<Damageable>();
        if (dmg != null && (view.gameObject.GetComponent<ElementType>().type != GetComponent<ElementType>().type))
        {
            Debug.Log("HIT");

            dmg.Damage(projectileDamage);
        }
    }
}
