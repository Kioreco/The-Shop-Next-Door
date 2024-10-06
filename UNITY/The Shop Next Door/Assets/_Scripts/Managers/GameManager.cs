using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private NetworkManager _networkManager;

    private GameObject _playerPrefab;

    void Start()
    {
        _networkManager = NetworkManager.Singleton;
        _playerPrefab = _networkManager.NetworkConfig.Prefabs.Prefabs[0].Prefab;

        _networkManager.OnServerStarted += OnServerStarted;
        _networkManager.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong obj)
    {

        var player = Instantiate(_playerPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(obj);
    }

    private void OnServerStarted()
    {
        print("Funciona el server");
    }

    void Update()
    {
        
    }
}
