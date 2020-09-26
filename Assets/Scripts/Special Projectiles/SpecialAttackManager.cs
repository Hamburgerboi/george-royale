using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class SpecialAttackManager : MonoBehaviourPun
{
    [Header("Special Attack")]
    public string speProjectileName;
    public float speRechargeTime = 90.0f;
    public Transform[] SPEProjectileLocation;

    private float speCurrentRechargeTime;
    private SpecialAttackManager SPEAttackManager;
    private Action SPEAttackAction;

    //Rock only
    private float rockAttackTime = -1;

    void Start()
    {
        if(!photonView.IsMine) return;

        ElementType eTypes = GetComponent<ElementType>();

        switch(eTypes.type)
        {
            case ElementType.Types.Fire:
                SPEAttackAction = FireSPEAttack;
                break;
            case ElementType.Types.Ice:
                SPEAttackAction = IceSPEAttack;
                break;
            case ElementType.Types.Electric:
                Debug.Log("ELECTTRICCCCCC");
                break;
            case ElementType.Types.Rock:
                SPEAttackAction = RockSPEAttack;
                break;
            default:
                Debug.Log("NONEEEEE");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine) return;
        if(Input.GetButtonDown("Fire2")) {
            SPEAttackAction();
        }
    }

    private void FireSPEAttack()
    {
        GameObject Phoenix = PhotonNetwork.Instantiate($"Prefabs/Projectiles/SPEProjectiles/{speProjectileName}", (transform.up.normalized * 2) + transform.position, transform.rotation);
        Rigidbody2D PhoenixRb = Phoenix.GetComponent<Rigidbody2D>();
        PhoenixRb.AddForce(transform.up * 7, ForceMode2D.Impulse);
    }

    private void IceSPEAttack()
    {
        foreach (Transform item in SPEProjectileLocation)
        {
            GameObject SPEProjectile = PhotonNetwork.Instantiate($"Prefabs/Projectiles/SPEProjectiles/{speProjectileName}", item.position, item.rotation);
            Rigidbody2D SPEProjectileRb = SPEProjectile.GetComponent<Rigidbody2D>();
            SPEProjectileRb.AddForce(item.up * 16, ForceMode2D.Impulse);   
        }
    }

    private void RockSPEAttack()
    {
        float random = (float)Math.Round(UnityEngine.Random.value * 2);
        GameObject SPEProjectile1 = PhotonNetwork.Instantiate($"Prefabs/Projectiles/SPEProjectiles/Grass_Blade/{speProjectileName}_{random + 1}", SPEProjectileLocation[0].position, SPEProjectileLocation[0].rotation);
        SPEProjectile1.GetComponent<Rigidbody2D>().velocity = SPEProjectile1.transform.right * 80;

        random = (float)Math.Round(UnityEngine.Random.value * 2);
        GameObject SPEProjectile2 = PhotonNetwork.Instantiate($"Prefabs/Projectiles/SPEProjectiles/Grass_Blade/{speProjectileName}_{random + 1}", SPEProjectileLocation[1].position, SPEProjectileLocation[1].rotation);
        SPEProjectile2.GetComponent<Rigidbody2D>().velocity = SPEProjectile2.transform.right * -80;

        Invoke("RockSPEAttackInvoked", 0.1f);
    }

    private void RockSPEAttackInvoked()
    {
        float random = (float)Math.Round(UnityEngine.Random.value * 2);
        GameObject SPEProjectile3 = PhotonNetwork.Instantiate($"Prefabs/Projectiles/SPEProjectiles/Grass_Blade/{speProjectileName}_{random + 1}", SPEProjectileLocation[2].position, SPEProjectileLocation[2].rotation);
        SPEProjectile3.GetComponent<Rigidbody2D>().velocity = SPEProjectile3.transform.right * 240;
    }
}

/*
        if(speRechargeTime >= 89)
        {
            speCurrentRechargeTime -= Time.deltaTime;
        } else 
        */