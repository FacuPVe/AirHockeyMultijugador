using UnityEngine;
using Photon.Pun;
public class PuckCollisionSync : MonoBehaviourPun
{
    private Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Vector3 velocityAfterHit = rb.linearVelocity;
                photonView.RPC("ApplyHitForceRPC", RpcTarget.MasterClient, velocityAfterHit);
            }
        }
    }

    [PunRPC]
    public void ApplyHitForceRPC(Vector3 newVelocity)
    {
        if (rb != null)
        {
            rb.linearVelocity = newVelocity;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}