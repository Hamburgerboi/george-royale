using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerControl : MonoBehaviourPun, IPunObservable
{
    [Header("Movement")]
    public float defaultSpeed = 1.0f;
    public float sprintMultipiler = 2.0f;
    public float maxEnergy = 100.0f;
    public float energyRegain = 1f;
    public float energyDepletion = 2f;

    [Header("Shooting")]
    public string projectilePrefabName;
    public Transform projectileLocation;
    public float projectileSpeed;
    public float shootDelay = 0.0f;

    [Header("Health")]
    public float maxHealth = 100.0f;
    private float currentHealth;

    [Header("Others")]
    public float respawnTime = 5.0f;

    private Rigidbody2D rb;
    private Rigidbody2D projectileRb;
    private CameraFollow camF;
    private GameManager gm;

    private float currentShootTime = 0.0f;
    private float currentEnergy;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        camF = GetComponent<CameraFollow>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        currentSpeed = defaultSpeed;

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (camF != null)
        {
            if (photonView.IsMine) camF.OnStartFollowing();
        }else{
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraFollow Component on playerPrefab.");
        }

        if(!photonView.IsMine)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }else{

        }
    }

    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

        if(currentHealth == 0) Death();

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
        if (!photonView.IsMine && PhotonNetwork.IsConnected) return;
        FaceMouse();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

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
        if(projectilePrefabName != "" && projectileLocation != null)
        {
            GameObject projectile = PhotonNetwork.Instantiate($"Prefabs/{projectilePrefabName}", projectileLocation.position, projectileLocation.rotation);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.AddForce(projectileLocation.up * projectileSpeed, ForceMode2D.Impulse);
        }
    }

    private void Death()
    {
        if(photonView.IsMine)
        {
            gm.InvokeRespawn(respawnTime);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}


/*
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }else{
            this.currentHealth = (float)stream.ReceiveNext();
        }
*/