using System.Collections;
using System.Globalization;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class RelayManager : NetworkBehaviour
{
    private const int maxConnections = 50;
    private string joinCode = "Room code";

    public int connectedPlayers = 0;
    public GameObject _playerPrefabClient;
    public GameObject _playerPrefabHost;
    public ulong[] _obj;
    private bool _connect = false;

    private const int MAXPLAYERS = 2;

    public static RelayManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _obj = new ulong[15];

        _playerPrefabHost = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[0].Prefab;
        _playerPrefabClient = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[1].Prefab;

        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        NetworkManager.Singleton.ConnectionApprovalCallback += ApproveConnection;
    }


    public async void StartHost()
    {
        _connect = false;
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.Shutdown();
            await Task.Delay(500);
        }

        _connect = true;

        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "wss"));
        joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        UIManager.Instance.matchCodeMatchMaking_Text.SetText(joinCode);

        NetworkManager.Singleton.StartHost();

        if (IsServer)
        {
            Debug.Log("El server dice que hay estos clientes " + NetworkManager.Singleton.ConnectedClients.Count);
        }
    }

    public async void StartClient(string joinCodeInput)
    {
        _connect = false;
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.Shutdown();
            await Task.Delay(500);
        }

        _connect = true;

        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        try
        {
            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCodeInput);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "wss"));

            if (UIManager.Instance.messageMatch_wrong.activeInHierarchy) { UIManager.Instance.messageMatch_wrong.SetActive(false); }

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Failed to join with the provided code: " + e.Message);
            UIManager.Instance.messageMatch_wrong.SetActive(true);
        }

        Debug.Log("Start client");
    }

    public void CancelMatch()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            Debug.Log("Host detenido");

            joinCode = null;
            //connectedPlayers = 0;
            _obj = new ulong[15];
            UIManager.Instance.matchCodeMatchMaking_Text.SetText("Partida cancelada");

            Invoke("ClearNetworkState", 0.5f);
        }
        else
        {
            Debug.LogWarning("No hay un host");
        }
    }

    private void ClearNetworkState()
    {
        NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
        NetworkManager.Singleton.ConnectionApprovalCallback -= ApproveConnection;

        joinCode = null;
        //joinCode = "Room code";

        connectedPlayers = 0;
        _obj = new ulong[15];

        _playerPrefabHost = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[0].Prefab;
        _playerPrefabClient = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[1].Prefab;

        // Asegurar que las conexiones están listas para reiniciar
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        NetworkManager.Singleton.ConnectionApprovalCallback += ApproveConnection;

        Debug.Log("Estado de la red limpiado y listo para una nueva partida");
    }

    private void OnClientConnected(ulong obj)
    {
        _obj[connectedPlayers] = obj;
        connectedPlayers++;

        if (NetworkManager.Singleton.IsServer)
        {
            if (NetworkManager.Singleton.ConnectedClients.Count == MAXPLAYERS)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("PrototypeScene", LoadSceneMode.Single);
            }
        }

    }

    private void ApproveConnection(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (NetworkManager.Singleton.ConnectedClients.Count < MAXPLAYERS)
        {
            response.Approved = true;
        }
        else
        {
            Debug.Log("Límite de jugadores alcanzado. Rechazando conexión");
            Debug.Log("Clientes conectados " + NetworkManager.Singleton.ConnectedClients.Count);
            response.Approved = false;
        }
    }

    private void OnClientDisconnect(ulong obj)
    {
        //NetworkManager.Singleton.Shutdown();
        if (_connect)
        {
            //CancelMatch();
            Destroy(GameObject.FindWithTag("GameManager"));
            SceneManager.LoadScene("2 - Matchmaking");
            //Debug.Log("Host se ha ido");
            //Debug.Log("Cliente desconectado, jugador " + obj);

            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log("Ha entrado eb OnClient el host");
                joinCode = null;
                _obj = new ulong[15];
                Invoke("ClearNetworkState", 0.5f);
            }

            _connect = false;
            //Debug.Log("connec a false");
        }
    }

    private void OnServerStarted()
    {
        print("Funciona el server");
    }
}
