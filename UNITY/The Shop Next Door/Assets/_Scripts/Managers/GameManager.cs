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

    private List<Vector3> _spawnPositions = new List<Vector3>()
    {
        new Vector3(1, 0, 3),
        new Vector3(-15, 0, -20),
    };

    private int _spawnIndex = 0;

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
            var player = Instantiate(_playerPrefab, _spawnPositions[_spawnIndex], _playerPrefab.transform.rotation); 
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
