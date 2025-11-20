using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI scoreP1Text;
    [SerializeField] private TextMeshProUGUI scoreP2Text;

    private int scoreP1 = 0;
    private int scoreP2 = 0;

    private GameObject puck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puck = GameObject.FindGameObjectWithTag("Puck");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("GameManager: Jugador entró a la sala");
        UpdateScoreUI();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("GameManager: Nuevo jugador entró: " + newPlayer.NickName);
        UpdateScoreUI();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("GameManager: Jugador salió: " + otherPlayer.NickName);
        UpdateScoreUI();
    }

    public void ScoreGoal(int scoringPlayerID)
    {
        Debug.Log("INFO: GameManager recibió la señal de gol (ID: " + scoringPlayerID + ").");
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateScore", RpcTarget.All, scoringPlayerID);
        }
        else
        {
            Debug.Log("INFO: NO soy el Master Client. Bloqueando envío de RPC.");
        }
    }

    [PunRPC]
    void UpdateScore(int scoringPlayerID)
    {
        Debug.Log("INFO: RPC UpdateScore recibido por el cliente. Actualizando puntuación.");
        if (scoringPlayerID == 1)
        {
            scoreP1++;
            Debug.Log("P1 SCORED! New Score: " + scoreP1);
        }
        else if (scoringPlayerID == 2)
        {
            scoreP2++;
            Debug.Log("P1 SCORED! New Score: " + scoreP2);
        }

        Debug.Log($"INFO: Nueva puntuación: P1={scoreP1}, P2={scoreP2}");
        UpdateScoreUI();

        if (PhotonNetwork.IsMasterClient)
        {
            ResetPuck();
        }
    }

    void UpdateScoreUI()
    {
        Player[] players = PhotonNetwork.PlayerList;
        Debug.Log($"UpdateScoreUI: Número de jugadores en la sala: {players.Length}");

        Player player1 = null;
        Player player2 = null;

        foreach (Player player in players)
        {
            Debug.Log($"Jugador encontrado - ActorNumber: {player.ActorNumber}, NickName: {player.NickName}, IsMasterClient: {player.IsMasterClient}");
            
            if (player.ActorNumber == 1)
            {
                player1 = player; // Master client
            }
            else if (player.ActorNumber == 2)
            {
                player2 = player;
            }
        }

        if (scoreP1Text != null)
        {
            string p1Name = player1 != null ? player1.NickName : "Player 1";
            scoreP1Text.text = p1Name + ": " + scoreP1.ToString();
            Debug.Log($"Actualizando P1 UI: {p1Name}: {scoreP1}");
        }
        if (scoreP2Text != null)
        {
            string p2Name = player2 != null ? player2.NickName : "Player 2";
            scoreP2Text.text = p2Name + ": " + scoreP2.ToString();
            Debug.Log($"Actualizando P2 UI: {p2Name}: {scoreP2}");
        }
    }
    void ResetPuck()
    {
        if (puck == null)
        {
            puck = GameObject.FindGameObjectWithTag("Puck");
        }

        if (puck != null)
        {
            Rigidbody rb = puck.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            puck.transform.position = new Vector3(0f, 0.3f, 0f);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
