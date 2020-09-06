using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Health")]
    public float TowerHealth = 1000f;
    private float currentTowerHealth;

    [Header("Animations")]
    public float shakeDuration = 1.0f;

    private float currentShakeTime = 0f;

    void Start()
    {
        currentTowerHealth = TowerHealth;
    }

    void Update()
    {
        if(currentShakeTime > 0)
        {
            Vector3 pos = Random.insideUnitCircle * 5;
            transform.position += pos;
            currentShakeTime -= Time.deltaTime;
        }
    }

    public void Damage(float amount)
    {
        currentTowerHealth = Mathf.Clamp(currentTowerHealth - amount, 0f, TowerHealth);
        currentShakeTime = shakeDuration;
        Debug.Log(currentTowerHealth);
        if(currentTowerHealth <= 0)
        {
            DestoryTower();
        }
    }

    private void DestoryTower()
    {
        Destroy(gameObject);
    }
}
