using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Network Variables")]
    //private NetworkManager _networkManager;
    private GameObject _playerPrefabHost;
    private GameObject _playerPrefabClient;
    //private int _spawnIndex = 0;
    public List<Transform> _spawnPositions = new List<Transform>();

    [Header("Player Objects Shared")]
    public GameObject cameraP1;
    public GameObject cameraP2;
    public GameObject separador;

    [SerializeField] private GameObject techoPlayer1;
    [SerializeField] private GameObject techoPlayer2;

    [Header("Individual Player References")]
    public float dineroJugador;
    public float espacioAlmacen;
    public float maxEspacioAlmacen;
    public float clientHappiness;
    public float playerVigor;



    public static GameManager Instance { get; private set; }
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

    void Start()
    {
        //_networkManager = NetworkManager.Singleton;
        //_playerPrefab = _networkManager.NetworkConfig.Prefabs.Prefabs[0].Prefab;

        //_networkManager.OnServerStarted += OnServerStarted;
        //_networkManager.OnClientConnectedCallback += OnClientConnected;

        dineroJugador = 500.0f;
        espacioAlmacen = 0;
        maxEspacioAlmacen = 50;
        clientHappiness = 0;
        playerVigor = 100;
        UIManager.Instance.UpdateClientHappiness_UI();
        UIManager.Instance.UpdatePlayerVigor_UI();
        UIManager.Instance.UpdateInventorySpace_UI();

        InstantiatePlayers();
    }


    private void InstantiatePlayers()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            _playerPrefabHost = RelayManager.Instance._playerPrefabClient;
            var playerHost = Instantiate(_playerPrefabHost, _spawnPositions[0]);
            playerHost.GetComponent<NetworkObject>().SpawnAsPlayerObject(RelayManager.Instance._obj[0]);

            _playerPrefabClient = RelayManager.Instance._playerPrefabClient;
            var playerClient = Instantiate(_playerPrefabClient, _spawnPositions[1]);
            playerClient.GetComponent<NetworkObject>().SpawnAsPlayerObject(RelayManager.Instance._obj[1]);

            techoPlayer2.SetActive(true);
        }
        else
        {
            techoPlayer1.SetActive(true);
        }
    }

    //private void OnClientConnected(ulong obj)
    //{
    //    if (NetworkManager.Singleton.IsServer) 
    //    {
    //        var player = Instantiate(_playerPrefabHost, _spawnPositions[_spawnIndex]);
    //        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(obj); 

    //        _spawnIndex++;
    //    }
    //}

    //private void OnServerStarted()
    //{
    //    print("Funciona el server");
    //}

    public void UpdateClientHappiness(float value)
    {
        clientHappiness += value; //ELEFANTE - por hacer
        UIManager.Instance.UpdateClientHappiness_UI();
    }

    public void UpdatePlayerVigor(float value)
    {
        playerVigor += value; //ELEFANTE - por hacer
        UIManager.Instance.UpdatePlayerVigor_UI();
    }
}
