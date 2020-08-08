using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";

    public GameObject progressLabel;
    public GameObject controlPanel;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {

    }

    public void Connect()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }else{
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinRandomRoom();
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarningFormat($"PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {cause}");
    }

        public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to Connect to Room | Creating Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions());
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
    }
}