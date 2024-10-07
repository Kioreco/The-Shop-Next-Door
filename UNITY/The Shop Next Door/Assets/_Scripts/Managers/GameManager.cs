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
    //{
    //    new Vector3(1, 0, 3),
    //    new Vector3(-15, 0, -20),
    //};
    public GameObject cameraP1;
    public GameObject cameraP2;
    public GameObject separaodr;

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
        _networkManager = NetworkManager.Singleton;
        _playerPrefab = _networkManager.NetworkConfig.Prefabs.Prefabs[0].Prefab;

        _networkManager.OnServerStarted += OnServerStarted;
        _networkManager.OnClientConnectedCallback += OnClientConnected;
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

    void Update()
    {
        
    }
}
