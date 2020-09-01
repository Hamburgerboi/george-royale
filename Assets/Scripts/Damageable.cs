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
    
    public void Damage(float damage)
    {
        if(pc != null)
        {
            pc.ChangeHealth(damage);
            Debug.Log("AAAAA");
            Debug.Log("AAAAA");
            Debug.Log("AAAAA");
            Debug.Log("AAAAA");
            Debug.Log("AAAAA");
        }
    }
}
