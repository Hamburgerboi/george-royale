using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Lightning : MonoBehaviourPun
{
    public float damageRadius = 1.0f;
    public LayerMask targetPlatform;
    public float projectileDamage = 3.0f;
    public float lifespan = 0.5f;

    private Sprite[] sprites;
    private float currentLifespan;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);

        sprites = Resources.LoadAll<Sprite>("Sprites/Projectiles/SPEProjectiles/Lightning");   

        GetComponent<SpriteRenderer>().sprite = sprites[(int)(UnityEngine.Random.value * 10)];

        currentLifespan = lifespan;
    }

    void Update()
    {
        if(!photonView.IsMine) return;
        if(currentLifespan <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, damageRadius, targetPlatform);

        foreach(Collider2D col in targets)
        {
            if(col.gameObject.GetComponent<ElementType>().type != GetComponent<ElementType>().type)
            {
                PhotonView pView = PhotonView.Get(this);
                pView.RPC("RPCDamage", RpcTarget.All, col.gameObject.GetComponent<PhotonView>().ViewID);
                if(photonView.IsMine) PhotonNetwork.Destroy(gameObject);
            }
        }

        currentLifespan -= Time.deltaTime;
    }

    [PunRPC]
    private void RPCDamage(int ID)
    {
        try
        {
            PhotonView view = PhotonView.Find(ID);
            Damageable dmg = view.gameObject.GetComponent<Damageable>();
            if (dmg != null && (view.gameObject.GetComponent<ElementType>().type != GetComponent<ElementType>().type)) dmg.Damage(projectileDamage);
        }catch (System.Exception e){
            Debug.Log(e.ToString());
        }
    }

    void OnDrawGizmos2D()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
