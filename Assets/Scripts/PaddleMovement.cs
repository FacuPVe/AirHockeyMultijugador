using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class PaddleMovement : MonoBehaviourPunCallbacks, IPointerDownHandler, IPointerUpHandler

{
    private PhotonView photonView;
    private Rigidbody rb;
    private Camera mainCamera;
    private bool isBeingControlled = false;



    // Variables para restringir el movimiento del paddle

    [Header("Area de movimiento")]
    [Header("Límites P1 (Master)")]
    [SerializeField] private float p1_minZ = 0f;
    [SerializeField] private float p1_maxZ = 10f;


    [Header("Límites P2 (Cliente)")]
    [SerializeField] private float p2_minZ = -10f;
    [SerializeField] private float p2_maxZ = 0f;

    // Límites horizontales (iguales para ambos)
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;

    // Variables internas de los límites
    private float zMin_limit;
    private float zMax_limit;


    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        if (photonView.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                zMin_limit = p1_minZ;

                zMax_limit = p1_maxZ;
            }
            else
            {
                zMin_limit = p2_minZ;
                zMax_limit = p2_maxZ;
            }
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            MovePaddle();

        }
    }


    private void MovePaddle()
    {
        if (!isBeingControlled)
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane gamePlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
        float rayDistance;

        if (gamePlane.Raycast(ray, out rayDistance))
        {
            // Punto de impacto
            Vector3 targetPoint = ray.GetPoint(rayDistance);

            // Restricción de movimiento
            float clampedX = Mathf.Clamp(targetPoint.x, minX, maxX);
            float clampedZ = Mathf.Clamp(targetPoint.z, zMin_limit, zMax_limit);

            // Mover el paddle
            Vector3 newPosition = new Vector3(clampedX, transform.position.y, clampedZ);
            rb.MovePosition(newPosition);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (photonView.IsMine)
        {
            isBeingControlled = true;
        }
    }



    public void OnPointerUp(PointerEventData eventData)
    {
        if (photonView.IsMine)
        {
            isBeingControlled = false;
        }
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