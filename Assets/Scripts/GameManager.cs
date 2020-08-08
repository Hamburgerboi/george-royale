﻿using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat($"Player entered room {other.NickName}");
    
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat($"Master Client: {PhotonNetwork.IsMasterClient}");
            LoadArena();
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
}