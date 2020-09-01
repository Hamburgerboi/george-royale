using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Countdown")]
    public TextMeshProUGUI countdownText;
    public float countdownTime = 15.0f;

    private bool isCountdown = false;
    private float countdownTimer;
    
    [Header("Respawn")]
    public TextMeshProUGUI respawnText;

    [Header("Spawning")]
    public string[] georges;
    public Transform[] spawnPoints;

    [Header("Others")]
    public bool inGame = false;
    [HideInInspector] public int count = 0;


    private GameObject thisPlayer;

    private float respawnTimer;
    private bool respawning;

    void Awake()
    {
        countdownTimer = countdownTime;
        countdownText.enabled = false;
    }

    void Start()
    {
        thisPlayer = PhotonNetwork.Instantiate("Prefabs/Players/George", spawnPoints[0].position + GenerateRandomVector3(12f), Quaternion.identity, 0);
        respawnText.enabled = false;

        if(PhotonNetwork.CurrentRoom.PlayerCount == 2 && !isCountdown)
        {
            isCountdown = true;
            countdownText.enabled = true;
        }
    }

    void Update()
    {
        if(isCountdown)
        {
            countdownTimer -= Time.deltaTime;
            countdownText.text = $"{(int)countdownTimer}";
            if(countdownTimer <= 0)
            {
                SpawnPlayers();
                inGame = true;
                isCountdown = false;
                countdownText.enabled = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }

        if(respawnTimer > 0 && respawning)
        {
            respawnTimer -= Time.deltaTime;
            respawnText.text = $"Respawning in: {respawnTimer.ToString("F1")}";
        }else if(respawning){
            InstantiatePlayers();
            respawning = false;
            respawnText.enabled = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        count ++;

        if(PhotonNetwork.CurrentRoom.PlayerCount == 2 && !isCountdown)
        {
            isCountdown = true;
            countdownText.enabled = true;
        }
    }
    
    public override void OnPlayerLeftRoom(Player other)
    {   
        if(!inGame) count --;
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void InvokeRespawn(float delay)
    {
        respawnText.enabled = true;
        respawnTimer = delay;
        respawning = true;
    }

    private void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        PhotonNetwork.LoadLevel("Arena_1");
    }

    private void InstantiatePlayers()
    {
        PhotonNetwork.Instantiate($"Prefabs/Players/{georges[count]}", spawnPoints[count + 1].position, Quaternion.identity, 0);
    }

    private void SpawnPlayers()
    {
        PhotonNetwork.Destroy(thisPlayer);
        InstantiatePlayers();
    }

    private Vector3 GenerateRandomVector3(float multiplier)
    {
        return new Vector3(UnityEngine.Random.value * multiplier, UnityEngine.Random.value * multiplier, 0);
    }
}