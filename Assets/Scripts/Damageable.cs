using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private PlayerControl pc;

    void Start()
    {
        pc = GetComponent<PlayerControl>();
    }

    void Update()
    {
        
    }
    
    public void Damage(float damage)
    {
        if(pc != null)
        {
            pc.ChangeHealth(damage);
        }
    }
}
