using UnityEngine;

public class Goal : MonoBehaviour
{
    public int scoringPlayerID;
    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("INFO: Colisión detectada con: " + other.name + ". Tag: " + other.tag);
        if (other.CompareTag("Puck"))
        {
            Debug.Log("INFO-PUCK: Puck detectado. Enviando señal de gol (ID: " + scoringPlayerID + ") a GameManager.");
            gameManager.ScoreGoal(scoringPlayerID);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
