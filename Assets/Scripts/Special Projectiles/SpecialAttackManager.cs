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
                Debug.Log("ROCKKKKKKK");
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
        GameObject Phoenix = PhotonNetwork.Instantiate($"Prefabs/Projectiles/{speProjectileName}", (transform.up.normalized * 2) + transform.position, transform.rotation);
        Rigidbody2D PhoenixRb = Phoenix.GetComponent<Rigidbody2D>();
        PhoenixRb.AddForce(transform.up * 7, ForceMode2D.Impulse);
    }

    private void IceSPEAttack()
    {
        foreach (Transform item in SPEProjectileLocation)
        {
            GameObject SPEProjectile = PhotonNetwork.Instantiate($"Prefabs/Projectiles/{speProjectileName}", item.position, item.rotation);
            Rigidbody2D SPEProjectileRb = SPEProjectile.GetComponent<Rigidbody2D>();
            SPEProjectileRb.AddForce(item.up * 16, ForceMode2D.Impulse);   
        }
    }
}

/*
        if(speRechargeTime >= 89)
        {
            speCurrentRechargeTime -= Time.deltaTime;
        } else 
        */