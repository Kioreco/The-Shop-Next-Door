using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private NetworkManager _networkManager;
    private GameObject _playerPrefab;
    private int _spawnIndex = 0;
    public List<Transform> _spawnPositions = new List<Transform>();

    public GameObject cameraP1;
    public GameObject cameraP2;
    public GameObject separador;

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

        _playerPrefab = RelayManager.Instance._playerPrefab;
        if (NetworkManager.Singleton.IsServer)
        {
            var player = Instantiate(_playerPrefab, _spawnPositions[_spawnIndex]);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(RelayManager.Instance._obj);

            _spawnIndex++;
        }
    }

    private void OnClientConnected(ulong obj)
    {
        if (NetworkManager.Singleton.IsServer) 
        {
            var player = Instantiate(_playerPrefab, _spawnPositions[_spawnIndex]);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(obj); 

            _spawnIndex++;
        }
    }

    private void OnServerStarted()
    {
        print("Funciona el server");
    }

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
