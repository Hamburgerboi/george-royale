using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float destroyTime = 1.0f;
    public bool speedDecay = false;
    public bool speedIncrease = false;
    public float damage = 1.0f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.Translate(new Vector3(0, 0, Time.deltaTime / 10));

        destroyTime -= Time.deltaTime;
        
        if(speedDecay) {
            DecaySpeed();
        }else if(speedIncrease){
            IncreaseSpeed();
        }
        
        if(destroyTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<Projectile>() != null) return;

        Damageable dmg = col.GetComponent<Damageable>();
        if (dmg != null && (col.GetComponent<ElementType>().type != gameObject.GetComponent<ElementType>().type))
        {
            dmg.Damage(damage);
        }
        Destroy(gameObject);
    }

    private void DecaySpeed()
    {
        if(rb.velocity.x > 4 || rb.velocity.x < -4 || rb.velocity.y > 4 || rb.velocity.y < -4)
        {
            rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, 0.1f);
        }
    }

    private void IncreaseSpeed()
    {
        rb.velocity += rb.velocity.normalized / new Vector2(3f, 3f);
    }
}
