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
    public int ID;
    public bool isInitialized = false;

    [Header("Camera Movement")]
    //public GameObject camController;
    float moveSpeed = 4f;
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

    public GameObject client;


    [Header("Network Variables")]
    NetworkVariable<float> playerMoney = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    float dineroJ;
    float dineroE;

    #endregion

    void Start()
    {
        _playerTransform = transform;

        if (IsOwner)
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
            amountZoom = fovSinZoom;
            //client = GameObject.FindWithTag("ClientNPC").GetComponent<ClientPrototype>();
        }
        playerMoney.OnValueChanged += OnPlayerMoneyChange;

    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            client = GameObject.FindWithTag("ClientNPC");

            ID = (int)OwnerClientId;
            //print(ID);

            if (ID == 0) 
            {
                TiendaManager.Instance.player = this;
                TiendaManager.Instance.ID = 0;
                GameManager.Instance._player = this;
                client.GetComponent<ClientPrototype>().enabled = true;
                client.GetComponent<ClientPrototype>().isCreated = true; 
            }

            if (ID == 1)
            {
                GameManager.Instance.cameraP1.SetActive(false);
                GameManager.Instance.cameraP2.SetActive(true);
                TiendaManager.Instance.player = this;
                TiendaManager.Instance.ID = 1;
                GameManager.Instance._player = this;
                client.GetComponent<ClientPrototype>().enabled = true;
                client.GetComponent<ClientPrototype>().isCreated = true;
            }
            isInitialized = true;
            minX = Camera.main.transform.position.x - 20f;
            maxX = Camera.main.transform.position.x + 20f;
            minZ = Camera.main.transform.position.z - 20f;
            maxZ = Camera.main.transform.position.z + 20f;

            //GameManager.Instance.separador.GetComponent<NavMeshObstacle>().carving = true;
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
        int layerMask = ~LayerMask.GetMask("UI");
        if (context.performed && IsOwner)
        {
            //print($"mouse psoition: {Input.mousePosition}\t ");
            Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouse, out var hit, Mathf.Infinity, layerMask))
            {
                //print($"if dentro\t destination: {hit.point}");
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

    #region network

    public void FinalResume()
    {
        print("final resume");
        if(ID == 0) UIManager.Instance.player1Money.SetText(GameManager.Instance.dineroJugador.ToString());
        if(ID == 1) UIManager.Instance.player2Money.SetText(GameManager.Instance.dineroJugador.ToString());

        SetupNetworkMoney();
        
    }

    private void SetupNetworkMoney()
    {
        if (IsOwner)
        {
            print("setupnetworkmoney owner");

            dineroJ = GameManager.Instance.dineroJugador;
            playerMoney.Value = dineroJ; //lo que se va a mandar

            OnPlayerMoneyChange(dineroJ, playerMoney.Value);
        }
    }

    void OnPlayerMoneyChange(float previous, float newM) 
    {
        print($"on value change: {newM}");
        if(dineroJ != newM)
        {
            dineroE = newM;
            GameManager.Instance.dineroRival = dineroE;
        }
        //UIManager.Instance.player2Money.SetText(dineroE.ToString());
        if (ID == 0) UIManager.Instance.player2Money.SetText(GameManager.Instance.dineroRival.ToString());
        if (ID == 1) UIManager.Instance.player1Money.SetText(GameManager.Instance.dineroRival.ToString());
    }

    #endregion
}