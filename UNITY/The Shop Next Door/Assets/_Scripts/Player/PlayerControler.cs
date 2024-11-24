using System;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
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


    //[Header("Network Variables")]
    //NetworkVariable<float> hostMoney = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //NetworkVariable<float> clientMoney = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //NetworkVariable<double> hostResult = new NetworkVariable<double>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //NetworkVariable<double> clientResult = new NetworkVariable<double>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public double hostResultFinal;
    public double clientResultFinal;

    //variables caja pago:
    bool isMoving = false;
    public event EventHandler eventPlayerIsInPayBox;
    public event EventHandler eventPlayerFinishPay;
    public event EventHandler eventPlayerIsInRubbish;
    bool isPaying = false;

    //control resultado final:
    public bool HostReady = false;
    public bool clientReady = false;


    #endregion

    private void Awake()
    {

    }

    void Start()
    {
        _playerTransform = transform;

        if (IsOwner)
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
            amountZoom = fovSinZoom;

            print("start player");

            //hostMoney.OnValueChanged += OnHostMoneyChange;
            //clientMoney.OnValueChanged += OnClientMoneyChange;

            //hostResult.OnValueChanged += OnHostResultChange;
            //clientResult.OnValueChanged += OnClientResultChange;
        }
        GetComponent<NavMeshAgent>().avoidancePriority = UnityEngine.Random.Range(30, 50);
        GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
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

            if ((int)OwnerClientId == 0) ID = (int)OwnerClientId;
            else ID = 1;

            print(ID);

            if (ID == 0)
            {
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
                Debug.Log("Camara de P2");
            }
            initializeVariables();
            isInitialized = true;
            minX = Camera.main.transform.position.x - 30f; //20
            maxX = Camera.main.transform.position.x + 10f; //20
            minZ = Camera.main.transform.position.z - 15f;
            maxZ = Camera.main.transform.position.z + 45f; //30
            //print("awake");
        }
        DontDestroyOnLoad(this);

    }

    void initializeVariables()
    {
        ActivateObjectPoolClient(client.GetComponent<ClientPrototype>());
        ActivateObjectPoolClient(clientKaren.GetComponent<ClientPrototype>());
        ActivateObjectPoolClient(clientTacanio.GetComponent<ClientPrototype>());

        clientRubbish.GetComponent<RubbishClientPrototype>().enabled = true;
        clientRubbish.GetComponent<RubbishClientPrototype>().isCreated = true;
        clientRubbish.GetComponent<RubbishClientPrototype>().isEnable = true;

        //TiendaManager.Instance.reponerEstanteria(10); //COMENTAR ELEFANTE
        TiendaManager.Instance.updateAlmacenQuantity();
        UIManager.Instance.UpdateInventorySpace_UI();
    }
    void ActivateObjectPoolClient(ClientPrototype e)
    {
        e.enabled = true;
        e.isCreated = true;
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

        if (IsOwner) Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, amountZoom, Time.deltaTime * zoomSpeed);

        if (IsOwner && isMoving && GetComponent<NavMeshAgent>().remainingDistance == 0)
        {
            isMoving = false;
            if (isPaying)
            {
                eventPlayerIsInPayBox?.Invoke(this, EventArgs.Empty);
                //print("está en la caja"); 
                UIManager.Instance.UpdatePayingBar_UI();

            }
            else
            {
                //cleaning
                eventPlayerIsInRubbish?.Invoke(this, EventArgs.Empty);
                //print("está en basura");
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

    public void FinalDayResume()
    {
        //dayFinish?.Invoke(this, EventArgs.Empty);

        if (ID == 0)
        {
            UIManager.Instance.player1Money.SetText(GameManager.Instance.dineroJugador.ToString());
            UIManager.Instance.player1Reputation.fillAmount = UIManager.Instance.reputation_Bar.fillAmount;
            UpdateHostMoneyServerRpc(GameManager.Instance.dineroJugador, UIManager.Instance.reputation_Bar.fillAmount);
        }
        if (ID == 1)
        {
            UIManager.Instance.player2Money.SetText(GameManager.Instance.dineroJugador.ToString());
            UIManager.Instance.player2Reputation.fillAmount = UIManager.Instance.reputation_Bar.fillAmount;
            UpdateClientMoneyServerRpc(GameManager.Instance.dineroJugador, UIManager.Instance.reputation_Bar.fillAmount);
        }
    }


    [ServerRpc]
    public void UpdateHostMoneyServerRpc(float newV, float newR)
    {
        //print("host server rpc");
        UpdateHostMoneyForClientRpc(newV, newR);
    }

    [ServerRpc]
    public void UpdateClientMoneyServerRpc(float newV, float newR)
    {
        //print("client server rpc");
        UpdateClientMoneyForClientRpc(newV, newR);
    }
    [ClientRpc]
    public void UpdateHostMoneyForClientRpc(float newV, float newR)
    {
        //print("update money host");
        if (IsClient)
        {
            GameManager.Instance.dineroRival = newV;
            UIManager.Instance.reputation_Rival = newR;
            UIManager.Instance.player1Money.SetText(GameManager.Instance.dineroRival.ToString());
            UIManager.Instance.player1Reputation.fillAmount = UIManager.Instance.reputation_Rival;

        }
    }
    [ClientRpc]
    public void UpdateClientMoneyForClientRpc(float newV, float newR)
    {
        //print("update client money");
        if (IsServer)
        {
            GameManager.Instance.dineroRival = newV;
            UIManager.Instance.reputation_Rival = newR;
            UIManager.Instance.player2Reputation.fillAmount = UIManager.Instance.reputation_Rival;
            UIManager.Instance.player2Money.SetText(GameManager.Instance.dineroRival.ToString());
        }
    }
    public void FinalWeekResult()
    {
        if (ID == 0)
        {
            HostReady = true;
            //print($"cambios host ready: {HostReady}");
            UpdateHostFinalResultServerRpc(GameManager.Instance.playerResult);
        }
        if (ID == 1)
        {
            clientReady = true;
            //print($"cambios client ready: {clientReady}");
            UpdateClientFinalResultServerRpc(GameManager.Instance.playerResult);
        }
    }

    [ServerRpc]
    public void UpdateHostFinalResultServerRpc(double newV)
    {
        //print("host server rpc");
        UpdateHostResultFinalForClientRpc(newV);
    }

    [ServerRpc]
    public void UpdateClientFinalResultServerRpc(double newV)
    {
        //print("client server rpc");
        UpdateClientResultFinalClientRpc(newV);
    }
    [ClientRpc]
    public void UpdateHostResultFinalForClientRpc(double newV)
    {
        if (IsClient & !IsServer)
        {
            //print($"update resultado host: {newV}");
            HostReady = true;
            //print($"cambios host ready client rpc: {HostReady}");

            GameManager.Instance.playerResultRival = newV;
            UIManager.Instance.player2Result_text.GetComponent<TextMeshProUGUI>().SetText((int)GameManager.Instance.playerResult + " points");
            UIManager.Instance.player1Result_text.GetComponent<TextMeshProUGUI>().SetText((int)GameManager.Instance.playerResultRival + " points");
            print($"client actualizate: {ID}");
            UIManager.Instance.UpdateFinalWeekTexts(0);

            //canContinue();
        }
    }
    [ClientRpc]
    public void UpdateClientResultFinalClientRpc(double newV)
    {
        if (IsServer)
        {
            //print($"update resultado cliente: {newV}");
            clientReady = true;
            //print($"cambios client ready client rpc: {clientReady}");

            GameManager.Instance.playerResultRival = newV;
            UIManager.Instance.player1Result_text.GetComponent<TextMeshProUGUI>().SetText((int)GameManager.Instance.playerResult + " points");
            UIManager.Instance.player2Result_text.GetComponent<TextMeshProUGUI>().SetText((int)GameManager.Instance.playerResultRival + " points");
            UIManager.Instance.UpdateFinalWeekTexts(1);

            //canContinue();
        }
    }


    public void DestroyClient()
    {
        print("destroy client");
        DestroyClientServerRpc(OwnerClientId);
    }

    [ServerRpc]
    public void DestroyClientServerRpc(ulong clientId)
    {
        //Debug.Log($"Intentando desconectar cliente con ID: {clientId}");
        if (clientId == 0)
        {
            NetworkManager.Singleton.Shutdown();
        }
        else
        {
            NetworkManager.Singleton.DisconnectClient(clientId);
        }
        // Desconectar al cliente
        //Debug.Log($"Cliente {clientId} desconectado.");
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
            //GetComponent<PlayerInput>().enabled = false;
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