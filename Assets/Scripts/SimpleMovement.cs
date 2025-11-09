using UnityEngine;
using Photon.Pun;

public class SimpleMovement : MonoBehaviour
{
    private PhotonView photonView;
    public float moveSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            float x = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
            float z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

            transform.Translate(x, 0, z);
        }
    }
}
