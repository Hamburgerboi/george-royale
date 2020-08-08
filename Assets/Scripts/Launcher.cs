using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";

    public GameObject progressLabel;
    public GameObject controlPanel;

    private bool isConnecting = false;

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
            PhotonNetwork.GameVersion = gameVersion;
            isConnecting = PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");

        if(isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
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
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Loading Room");
            PhotonNetwork.LoadLevel("Arena_1");
        }
        Debug.Log("Joined Room");
    }
}