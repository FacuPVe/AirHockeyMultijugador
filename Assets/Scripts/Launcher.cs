using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string playerPrefabName = "PlayerPrefab";

    [SerializeField]
    private string puckPrefabName = "PuckPrefab";

    [Header("Spawn Point")]
    [SerializeField]
    private Transform spawnPointP1;
    [SerializeField]
    private Transform spawnPointP2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Aumentar frecuencia de actualización de la red para mejorar la sincronización (pasar de 10 a 30)
        // PhotonNetwork.SendRate = 30;
        // PhotonNetwork.SerializationRate = 30;

        Application.targetFrameRate = 60;

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
        if (!PhotonNetwork.IsConnectedAndReady || (PhotonNetwork.Server != ServerConnection.MasterServer))
        {
            Debug.LogWarning("No se puede unir a la sala. No está conectado al servidor maestro. Estado actual: " + PhotonNetwork.Server);
            return;
        }
        PhotonNetwork.JoinRandomOrCreateRoom(
            roomOptions: new RoomOptions { MaxPlayers = 2 }
        );
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Player joined Room");

        Vector3 spawnPosition;

        if(PhotonNetwork.IsMasterClient)
        {
            spawnPosition = spawnPointP1 != null ? spawnPointP1.position : new Vector3(0f, 0.5f, -8f);
        }
        else
        {
            spawnPosition = spawnPointP2 != null ? spawnPointP2.position : new Vector3(0f, 0.5f, 8f);
        }
        // Instanciar Prefab
        PhotonNetwork.Instantiate(this.playerPrefabName, spawnPosition, Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("MasterClient ha instanciado el disco");
            Vector3 puckSpawn = new Vector3(0f, 0.3f, 0f);
            PhotonNetwork.Instantiate("PuckPrefab", puckSpawn, Quaternion.identity);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
