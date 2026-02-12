using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
public class MainMenuLauncher : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField roomNameInputField;

    [Header("Panels")]
    [SerializeField] private GameObject titleScreenPanel;
    [SerializeField] private GameObject lobbyPanel;

    [Header("Room list")]
    [SerializeField] private Transform roomListContent;
    [SerializeField] private GameObject roomItemPrefab;

    private const string MultiplayerSceneName = "SampleScene";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Unido al Lobby");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Cargando escena de juego...");
            PhotonNetwork.LoadLevel(MultiplayerSceneName);
        }
    }

    // Función para unirse a cualquier sala y jugar rápido con el botón "Jugar" del menú principal
    public void OnClickConnectRandom()
    {
        if (SetNickName())
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    // Función para crear una sala automáticamente al no haber encontrado una sala al accionar el botón "Jugar"
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom("Sala_" + Random.Range(1000, 9999));
    }

    // Función para mostrar el panel con la lista de salas
    public void OnClickOpenLobbyList()
    {
        if (SetNickName())
        {
            titleScreenPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }
    }

    // Función donde se comprueba una mínima cantidad de caracteres en el nombre de la sala y donde finalmente se crea la sala con ese mismo nombre
    public void OnClickCreateRoom()
    {
        if (roomNameInputField.text.Length >= 4)
        {
            CreateRoom(roomNameInputField.text);
        }
    }

    // Función creada para solamente la creación de una sala
    void CreateRoom (string roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;

        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "visibleName", roomName }};
        options.CustomRoomPropertiesForLobby = new string[] { "visibleName" };

        PhotonNetwork.CreateRoom(roomName, options);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(MultiplayerSceneName);
    }

    // Función para poder unirse a una sala de la lista de salas
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
    }

    // Función que ayuda a actualizar la lista actual de salas cada vez que se utilice
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Borrar la lista anterior para evitar duplicarla con la nueva lista actualizada
        foreach (Transform child in roomListContent)
        {
            Destroy(child.gameObject);
        }

        // Creación de botones nuevos
        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList) continue;


            GameObject newItem = Instantiate(roomItemPrefab, roomListContent);
            RoomItem itemScript = newItem.GetComponent<RoomItem>();

            // Informar al manager con los datos nuevos
            itemScript.SetUp(room, this);
        }
    }

    // Función para volver al panel principal
    public void OnClickBack()
    {
        lobbyPanel.SetActive(false);
        titleScreenPanel.SetActive(true);
    }

    private bool SetNickName()
    {
        if (string.IsNullOrEmpty(nameInputField.text)) return false;
        
        PhotonNetwork.NickName = nameInputField.text;
        return true;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Error al crear la sala: " + message);
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Error al unirse a al sala: " + message);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
