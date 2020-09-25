using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Phoenix : MonoBehaviourPun
{
    [Header("Firing")]
    public Transform shootPosition;
    public Transform aimPosition;
    public float shootSpeed = 10f;
    public float shootOffset = 3f;
    public float shootDelay = 1f;

    private float shootTimer;
    private float lifespan = 2f;
    private CompositeCollider2D arena;

    void Start()
    {
        shootTimer = shootDelay;
        arena = GameObject.Find("Arena").GetComponent<CompositeCollider2D>();
    }

    void Update()
    {
        if(!arena.bounds.Contains(transform.position))
        {
            if(lifespan <= 0 && photonView.IsMine) PhotonNetwork.Destroy(gameObject);
            lifespan -= Time.deltaTime;
            return;
        }

        if(shootTimer >= shootDelay)
        {
            Fire();
            shootTimer = 0;
            return;
        }
        shootTimer += Time.deltaTime * (UnityEngine.Random.value + 0.5f);
    }

    private void Fire()
    {
        Vector3 randomShootPosition = aimPosition.position + (Vector3)(Random.insideUnitCircle * shootOffset);
        GameObject projectile = PhotonNetwork.Instantiate($"Prefabs/Projectiles/Fireball", shootPosition.position, shootPosition.rotation);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectile.transform.up = randomShootPosition - shootPosition.position;
        projectileRb.AddForce(projectile.transform.up * shootSpeed, ForceMode2D.Impulse);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(aimPosition.position, shootOffset * 2);
    }
}
