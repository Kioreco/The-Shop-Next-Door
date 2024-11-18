using System;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerControler : NetworkBehaviour
{
    #region variables
    Vector2 _movement;
    Transform _playerTransform;
    public NavMeshAgent _agent;
    public int ID;
    public bool isInitialized = false;

    [Header("Camera Movement")]
    //public GameObject camController;
    float moveSpeed = 4f;
    Vector3 lastPosition;
    bool isDrag = false;
    Vector3 moveDir;
    bool canMove = true;

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

    //object pools
    public GameObject client;
    public GameObject clientRubbish;
    public GameObject clientKaren;
    public GameObject clientTacanio;


    [Header("Network Variables")]
    NetworkVariable<float> hostMoney = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    NetworkVariable<float> clientMoney = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    //variables caja pago:
    bool isMoving = false;
    public event EventHandler eventPlayerIsInPayBox;
    public event EventHandler eventPlayerFinishPay;
    public event EventHandler eventPlayerIsInRubbish;
    bool isPaying = false;

   

    #endregion

    void Start()
    {
        _playerTransform = transform;

        if (IsOwner)
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
            amountZoom = fovSinZoom;
        }
        hostMoney.OnValueChanged += OnHostMoneyChange;
        clientMoney.OnValueChanged += OnClientMoneyChange;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            client = GameObject.FindWithTag("ClientNPC");
            clientRubbish = GameObject.FindWithTag("ClientRubbish");
            clientKaren = GameObject.FindWithTag("ClientKarenNPC");
            clientTacanio = GameObject.FindWithTag("ClienteTacanioNPC");
            TiendaManager.Instance.player = this;
            GameManager.Instance._player = this;

            ID = (int)OwnerClientId;
            print(ID);

            if (ID == 0) 
            {
                //initialiceVariables();
                TiendaManager.Instance.ID = 0;
                GameManager.Instance.activeCamera = GameManager.Instance.cameraP1.transform.GetChild(0).GetComponent<Camera>();
            }

            if (ID == 1)
            {
                GameManager.Instance.cameraP1.SetActive(false);
                GameManager.Instance.cameraP2.SetActive(true);
                GameManager.Instance.activeCamera = GameManager.Instance.cameraP2.transform.GetChild(0).GetComponent<Camera>();
                GameManager.Instance.canvasInteractable.worldCamera = GameManager.Instance.activeCamera;
                TiendaManager.Instance.ID = 1;
                GameManager.Instance.separador.GetComponent<NavMeshObstacle>().carving = true;
            }
            initialiceVariables();
            isInitialized = true;
            minX = Camera.main.transform.position.x - 30f; //20
            maxX = Camera.main.transform.position.x + 20f;
            minZ = Camera.main.transform.position.z - 20f;
            maxZ = Camera.main.transform.position.z + 30f;
        }
    }

    void initialiceVariables()
    {
       
        client.GetComponent<ClientPrototype>().enabled = true;
        client.GetComponent<ClientPrototype>().isCreated = true;

        clientRubbish.GetComponent<RubbishClientPrototype>().enabled = true;
        clientRubbish.GetComponent<RubbishClientPrototype>().isCreated = true;
        clientRubbish.GetComponent<RubbishClientPrototype>().isEnable = true;

        clientKaren.GetComponent<ClientPrototype>().isEnable = true;
        clientKaren.GetComponent<ClientPrototype>().enabled = true;
        clientKaren.GetComponent<ClientPrototype>().isCreated = true;

        clientTacanio.GetComponent<ClientPrototype>().isEnable = true;
        clientTacanio.GetComponent<ClientPrototype>().enabled = true;
        clientTacanio.GetComponent<ClientPrototype>().isCreated = true;

        TiendaManager.Instance.reponerEstanteria(20);
        TiendaManager.Instance.updateAlmacenQuantity();
        UIManager.Instance.UpdateInventorySpace_UI();
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
        if(IsOwner && isMoving && GetComponent<NavMeshAgent>().remainingDistance == 0)
        {
            isMoving = false;
            if (isPaying) 
            {
                eventPlayerIsInPayBox?.Invoke(this, EventArgs.Empty); print("está en la caja"); 
                UIManager.Instance.UpdatePayingBar_UI();

            }
            else
            {
                //cleaning
                eventPlayerIsInRubbish?.Invoke(this, EventArgs.Empty);
                print("está en basura");
            }
            
        }
    }

    #region Input

    public void MovePlayer(InputAction.CallbackContext context)
    {
        int layerMask = ~LayerMask.GetMask("UI");
        if (context.performed && IsOwner && canMove)
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
        if (ID == 0 && IsOwner)
        {
            hostMoney.Value = GameManager.Instance.dineroJugador;
            UIManager.Instance.player1Money.SetText(GameManager.Instance.dineroJugador.ToString());
            //OnHostMoneyChange(GameManager.Instance.dineroJugador, hostMoney.Value);
        }
        if (ID == 1 && IsOwner)
        {
            clientMoney.Value = GameManager.Instance.dineroJugador;
            UIManager.Instance.player2Money.SetText(GameManager.Instance.dineroJugador.ToString());
            //OnClientMoneyChange(GameManager.Instance.dineroJugador, clientMoney.Value);
        }       
    }

    void OnHostMoneyChange(float previous, float newM) 
    {
        //print($"on host money change: id: {ID}  isclient{IsClient}  ishost: {IsServer}");

        if (IsClient)
        {
            //print($"on host money change if");
            GameManager.Instance.dineroRival = newM;
            UIManager.Instance.player1Money.SetText(GameManager.Instance.dineroRival.ToString());
        }
    }    
    void OnClientMoneyChange(float previous, float newM) 
    {
        //print($"on client money change: id: {ID}  isclient{IsClient}  ishost: {IsServer}");

        if (IsServer)
        {
            //print($"on client money change if");

            GameManager.Instance.dineroRival = newM;
            UIManager.Instance.player2Money.SetText(GameManager.Instance.dineroRival.ToString());
        }
    }

    #endregion

    #region others

    public void WalkToPosition(Vector3 position, bool paying)
    {
        //print("walk to position");
        if (IsOwner)
        {
            canMove = false;
            isMoving = true;
            isPaying = paying;
            GetComponent<PlayerInput>().enabled = false;
            GetComponent<NavMeshAgent>().SetDestination(position);
        }
    }

    public void disableMovement()
    {
        if (IsOwner)
        {
            canMove = false;
            GetComponent<PlayerInput>().enabled = false;
        }
    }

    public void enableMovement(bool paying)
    {
        if (IsOwner)
        {
            canMove = true;
            GetComponent<PlayerInput>().enabled = true;
            if (paying) { eventPlayerFinishPay?.Invoke(this, EventArgs.Empty); }
        }
    }

    public void ChangePlayerSpeed(float value)
    {
        moveSpeed = value;
    }
    #endregion
}