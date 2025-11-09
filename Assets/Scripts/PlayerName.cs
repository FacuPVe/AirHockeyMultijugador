using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class PlayerName : MonoBehaviourPun
{
    [SerializeField] private TextMeshPro playerNameText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (photonView.IsMine)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }

        Player player = photonView.Owner;

        if (player != null && playerNameText != null)
        {
            playerNameText.text = player.NickName;
        }
        else
        {
            playerNameText.text = "Error loading name";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
