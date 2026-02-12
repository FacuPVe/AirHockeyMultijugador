using UnityEngine;
using TMPro;
using Photon.Realtime;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomNameText;
    private MainMenuLauncher manager;
    private RoomInfo info;

    public void SetUp(RoomInfo _info, MainMenuLauncher _manager)
    {
        info = _info;
        manager = _manager;

        roomNameText.text = _info.Name + " (" + _info.PlayerCount + "/" + _info.MaxPlayers + ")";
    
    }

    public void OnClickItem()
    {
        manager.JoinRoom(info);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
