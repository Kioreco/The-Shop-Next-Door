using System.Globalization;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RelayManager : NetworkBehaviour
{
    private const int maxConnections = 50;
    private string joinCode = "Room code";

    public int connectedPlayers = 0;
    public GameObject _playerPrefabClient;
    public GameObject _playerPrefabHost;
    public ulong[] _obj;

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
        _obj = new ulong[2];

        _playerPrefabHost = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[0].Prefab;
        _playerPrefabClient = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[0].Prefab;

        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }


    public async void StartHost()
    {
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
    }

    public async void StartClient(string joinCodeInput)
    {
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

        
    }

    private void OnClientConnected(ulong obj)
    {
        _obj[connectedPlayers] = obj;
        connectedPlayers++;

        if (connectedPlayers == 2 || NetworkManager.Singleton.IsClient & !NetworkManager.Singleton.IsServer)
        {
            SceneManager.LoadSceneAsync("PrototypeScene");
        }

    }

    private void OnServerStarted()
    {
        print("Funciona el server");
    }
}
