using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    public string[] georges;

    [HideInInspector]
    public int playerCount = 0;

    void Awake()
    {
        Debug.Log("yay (la poggere)");
    }

    void Start()
    {
        Debug.Log($"We are Instantiating LocalPlayer from {Application.loadedLevelName}");
        PhotonNetwork.Instantiate($"Prefabs/Players/{georges[playerCount]}", Vector3.zero, Quaternion.identity, 0);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat($"Player entered room {other.NickName}");
    
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat($"Master Client: {PhotonNetwork.IsMasterClient}");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerCount);
        }else{
            this.playerCount = (int)stream.ReceiveNext();
        }
    }
    
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat($"Player Left: {other.NickName}");
    
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat($"Master Client Left: {PhotonNetwork.IsMasterClient}");
            LoadArena();
        }
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

    public void Increment()
    {
        Debug.Log("PLAYER COUNT INCRMENTED .................... SUCCESS");
        playerCount += 1;
        Debug.Log(playerCount);
    }
}