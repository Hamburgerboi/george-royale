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
    
    public string[] georges;
    public Transform[] spawnPoints;

    private int count = 0;
    private bool isCountdown = false;
    private float countdownTimer;
    private GameObject thisPlayer;

    void Awake()
    {
        countdownTimer = countdownTime;
        countdownText.enabled = false;
    }

    void Start()
    {
        thisPlayer = PhotonNetwork.Instantiate("Prefabs/Players/George", Vector3.zero, Quaternion.identity, 0);

        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
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
                Debug.Log("TIMER DONE");
                isCountdown = false;
                countdownText.enabled = false;
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        count += 1;

        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            isCountdown = true;
            countdownText.enabled = true;
        }
    }
    
    public override void OnPlayerLeftRoom(Player other)
    {   
        count -= 1;
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        PhotonNetwork.LoadLevel("Arena_1");
    }

    private void SpawnPlayers()
    {
        PhotonNetwork.Destroy(thisPlayer);
        PhotonNetwork.Instantiate($"Prefabs/Players/{georges[count]}", Vector3.zero, Quaternion.identity, 0);
    }
}