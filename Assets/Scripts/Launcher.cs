using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string playerPrefabName = "PlayerPrefab";

    [SerializeField]
    private Transform spawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            JoinRoomLogic();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        JoinRoomLogic();
    }

    private void JoinRoomLogic()
    {
        PhotonNetwork.JoinRandomOrCreateRoom(
            roomOptions: new RoomOptions { MaxPlayers = 4 }
        );
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Player joined Room");
        // Usar la posición del Transform 'spawnPoint' si está asignado
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : Vector3.up * 2f;
        // Instanciar Prefab
        PhotonNetwork.Instantiate(this.playerPrefabName, spawnPosition, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
