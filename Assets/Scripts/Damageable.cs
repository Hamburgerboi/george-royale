using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private PlayerControl pc;
    private Tower tw;

    void Start()
    {
        pc = GetComponent<PlayerControl>();
        tw = GetComponent<Tower>();
    }
    
    public void Damage(float damage)
    {
        if(pc != null)
        {
            pc.ChangeHealth(damage);
            Debug.Log("Player damaged");
        }else if(tw != null){
            tw.Damage(damage);
            Debug.Log("Tower Hit");
        }
    }
}
