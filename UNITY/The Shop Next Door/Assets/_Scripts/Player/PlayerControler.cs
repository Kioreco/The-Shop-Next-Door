using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerControler : NetworkBehaviour
{
    #region variables
    Vector2 _movement;
    Transform _playerTransform;
    private NetworkVariable<Quaternion> _rotation = new NetworkVariable<Quaternion>();
    public NavMeshAgent _agent;
    int ID;
    bool isInitialized = false;

    [Header("Camera Movement")]
    //public GameObject camController;
    float moveSpeed = 2f;
    Vector3 lastPosition;
    bool isDrag = false;
    Vector3 moveDir;

    [Header("Camera Movement Limits")]
    public float minX; /* = -10f;*/  
    public float maxX; /* = 10f;*/
    public float minZ; /* = -10f;*/
    public float maxZ; /* = 10f;*/

    [Header("Camera Zoom")]
    int fovSinZoom = 60;
    int fovZoom = 25;
    float amountZoom;
    float zoomSpeed = 5f;


    public Temporal client;
    #endregion

    void Start()
    {
        _playerTransform = transform;

        if (IsOwner)
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
            amountZoom = fovSinZoom; 
            client = GameObject.FindWithTag("ClientNPC").GetComponent<Temporal>();
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            ID = (int)OwnerClientId;
            //print(ID);

            if (ID == 1)
            {
                GameManager.Instance.cameraP1.SetActive(false);
                GameManager.Instance.cameraP2.SetActive(true);
                GameManager.Instance.separador.GetComponent<NavMeshObstacle>().carving = true;
            }
            isInitialized = true;
            minX = Camera.main.transform.position.x - 10f;
            maxX = Camera.main.transform.position.x + 10f;
            minZ = Camera.main.transform.position.z - 10f;
            maxZ = Camera.main.transform.position.z + 10f;
            client = GameObject.FindWithTag("ClientNPC").GetComponent<Temporal>();
            client.instanciarNPC();
            client.instanciarNPC();
            client.instanciarNPC();
        }
    }

    private void Update()
    {
        //print(camController.name);
        if (isDrag && IsOwner && isInitialized)
        {
            Vector3 movement = Input.mousePosition - lastPosition;
            //print(movement);

            float speed = 2f;

            moveDir.x = movement.x * speed;
            moveDir.z = movement.y * speed;

            lastPosition = Input.mousePosition;

            //movimiento de la camara
            Vector3 auxH = Camera.main.transform.right * -moveDir.x;
            Camera.main.transform.position += auxH * moveSpeed * Time.deltaTime;
            Vector3 auxV = new Vector3(0, 0, -moveDir.z);
            Camera.main.transform.position += auxV * moveSpeed * Time.deltaTime;

            Vector3 newPosition = Camera.main.transform.position;

            //limitar movimiento cámara
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

            Camera.main.transform.position = newPosition;
        }

        if(IsOwner) Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, amountZoom, Time.deltaTime * zoomSpeed);

    }

    #region Input

    public void MovePlayer(InputAction.CallbackContext context)
    {
        if (context.performed && IsOwner)
        {
            Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouse, out var hit))
            {
                _agent.SetDestination(hit.point);
                _playerTransform.LookAt(hit.point);
            }
        }
    }

    public void MoveCamera(InputAction.CallbackContext context)
    {
        //print(context.ReadValue<float>());
        if (context.ReadValue<float>() > 0 && IsOwner) 
        {
            isDrag = true;
            lastPosition = Input.mousePosition;
        }

        if (context.ReadValue<float>() == 0 && IsOwner) { isDrag = false; }
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        //print(context.ReadValue<float>());
        if (context.ReadValue<float>() > 0 && IsOwner)
        {
            amountZoom -= 5;
            amountZoom = Mathf.Clamp(amountZoom, fovZoom, fovSinZoom);
        }        
        if (context.ReadValue<float>() < 0 && IsOwner)
        {
            amountZoom += 5;
            amountZoom = Mathf.Clamp(amountZoom, fovZoom, fovSinZoom);
        }
    }
    #endregion
}