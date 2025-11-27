using UnityEngine;
using TMPro;
using Photon.Pun;
public class MainMenuLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField nameInputField;

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
    }

    public void OnClickConnect()
    {
        string playerName = nameInputField.text;
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("Player's name is empty.");
            return;
        }

        PhotonNetwork.NickName = playerName;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.LoadLevel(MultiplayerSceneName);
        }
        else
        {
            Debug.LogError("Not connected to Photon Network.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
