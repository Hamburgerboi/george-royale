using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement")]
    public float defaultSpeed = 1.0f;
    public float sprintMultipiler = 2.0f;
    public float maxEnergy = 100.0f;
    public float energyRegain = 1f;
    public float energyDepletion = 2f;
    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform projectileLocation;
    public float projectileSpeed;
    public float shootDelay = 0.0f;
    [Header("Health")]
    public float maxHealth = 100.0f;
    private float currentHealth;

    private Rigidbody2D rb;
    private Rigidbody2D projectileRb;
    private float currentShootTime = 0.0f;
    private float currentEnergy;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        currentSpeed = defaultSpeed;
    }

    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        
        if(Input.GetKey("left shift") && currentEnergy > 0)
        {
            currentSpeed = defaultSpeed * sprintMultipiler;
        }else{
            currentSpeed = defaultSpeed;
            if(currentEnergy < 100f) currentEnergy += Time.deltaTime * energyRegain;
        }

        rb.angularVelocity = 0f;
        rb.velocity = new Vector2(hAxis * currentSpeed, vAxis * currentSpeed);

        if(currentShootTime >= 0)
        {
            currentShootTime -= Time.deltaTime;
        }else if(Input.GetButton("Fire1")){
            Shoot();
            currentShootTime = shootDelay;
        }
    }

    void LateUpdate()
    {
        FaceMouse();
    }

    public void ChangeHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        Debug.Log(currentHealth);
    }

    private void FaceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 dirc = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        transform.up = dirc;
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileLocation.position, projectileLocation.rotation);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.AddForce(projectileLocation.up * projectileSpeed, ForceMode2D.Impulse);
    }
}
