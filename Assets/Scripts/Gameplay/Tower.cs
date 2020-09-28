using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tower : MonoBehaviourPun
{
    [Header("Health")]
    public float TowerHealth = 1000f;
    private float currentTowerHealth;

    [Header("Animations")]
    public float shakeDuration = 0.3f;
    public float shakeMultiplier = 0.1f;

    private float frameCount = 0;
    private float currentShakeTime = 0f;
    private Vector3 defaultLocation;

    void Start()
    {
        currentTowerHealth = TowerHealth;
        defaultLocation = transform.position;
    }

    void Update()
    {
        frameCount++;
        
        if(currentShakeTime > 0)
        {
            Vector3 pos = Random.insideUnitCircle * shakeMultiplier;
            transform.position += pos;
            currentShakeTime -= Time.deltaTime;
            Debug.Log("SHAKED");
        }
    }

    void LateUpdate()
    {
        if(frameCount % 2 == 0) transform.position = defaultLocation;
    }

    public void Damage(float amount)
    {
        currentTowerHealth = Mathf.Clamp(currentTowerHealth - amount, 0f, TowerHealth);
        currentShakeTime = shakeDuration;

        if(currentTowerHealth <= 0)
        {
            DestoryTower();
        }
    }

    private void DestoryTower()
    {
        if(photonView.IsMine) PhotonNetwork.Destroy(gameObject);
    }
}
