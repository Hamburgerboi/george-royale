using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float TowerHealth = 1000f;
    private float currentTowerHealth;

    void Start()
    {
        currentTowerHealth = TowerHealth;
    }

    void Update()
    {
    }

    public void Damage(float amount)
    {
        currentTowerHealth = Mathf.Clamp(currentTowerHealth - amount, 0f, TowerHealth);
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
