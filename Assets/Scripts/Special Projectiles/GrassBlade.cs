using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class GrassBlade : MonoBehaviourPun
{
    public float damage = 40f;
    public float destroyTime = 0.2f;

    private Renderer rend;
    private Rigidbody2D rb;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(destroyTime <= -0.2 && photonView.IsMine) PhotonNetwork.Destroy(gameObject);
        if(destroyTime <= 0)
        {
            rb.velocity = Vector2.zero;
            rend.enabled = false;
        }
        destroyTime -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == 9 || !photonView.IsMine) return;

        PhotonView pView = PhotonView.Get(this);
        pView.RPC("RPCDamage", RpcTarget.All, col.gameObject.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    private void RPCDamage(int ID)
    {
        try
        {
            PhotonView view = PhotonView.Find(ID);
            Damageable dmg = view.gameObject.GetComponent<Damageable>();
            if (dmg != null && (view.gameObject.GetComponent<ElementType>().type != GetComponent<ElementType>().type)) dmg.Damage(damage);
        }catch (System.Exception e){
            Debug.Log(e.ToString());
        }
    }
}
