using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
    [HideInInspector] public float dineroJugador;
    [HideInInspector] public float dineroRival;
    [HideInInspector] public float espacioAlmacen;
    [HideInInspector] public float maxEspacioAlmacen;
    [HideInInspector] public float clientHappiness;
    [HideInInspector] public float playerVigor;

    [Header("Network Game Manager")]
    public PlayerControler _player;

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

    //void Update()
    //{
    //    if (NetworkManager.Singleton.IsServer)
    //    {
    //        if (NetworkManager.Singleton.ConnectedClients.Count > 2)
    //        {
    //            for (int i = 2; i < RelayManager.Instance._obj.Length; i++)
    //            {
    //                if (NetworkManager.Singleton.ConnectedClients.ContainsKey(RelayManager.Instance._obj[i]))
    //                {
    //                    NetworkManager.Singleton.DisconnectClient(RelayManager.Instance._obj[i]);
    //                    Debug.Log("Se ha desconectado el cliente " + RelayManager.Instance._obj[i]);
    //                    Debug.Log("Ahora quedan " + NetworkManager.Singleton.ConnectedClients.Count);
    //                }
    //            }
    //        }
    //    }
    //}

    void Start()
    {
        dineroJugador = 500.0f;
        espacioAlmacen = 0;
        maxEspacioAlmacen = 100;
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
            _playerPrefabHost = RelayManager.Instance._playerPrefabHost;
            var playerHost = Instantiate(_playerPrefabHost, _spawnPositions[0]);
            playerHost.GetComponent<NetworkObject>().SpawnAsPlayerObject(RelayManager.Instance._obj[0]);

            _playerPrefabClient = RelayManager.Instance._playerPrefabClient;
            var playerClient = Instantiate(_playerPrefabClient, _spawnPositions[1]);
            playerClient.GetComponent<NetworkObject>().SpawnAsPlayerObject(RelayManager.Instance._obj[1]);

            techoPlayer1.SetActive(false);

            UIManager.Instance.telephone.ChangeLockedScreenBG(1);
            UIManager.Instance.telephone.ChangeLifeAppName(1);
        }
        else
        {
            techoPlayer2.SetActive(false);
            UIManager.Instance.telephone.ChangeLockedScreenBG(2);
            UIManager.Instance.telephone.ChangeLifeAppName(2);
        }
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

    public void UpdateAlmacenQuantity()
    {
        TiendaManager.Instance.updateAlmacenQuantity();
    }

    public void EndDay()
    {
        //LLAMADA A server rpc
        _player.FinalResume();

        Time.timeScale = 0;

        UIManager.Instance.canvasDayEnd.SetActive(true);
        UIManager.Instance.telephone.calendar.ActivitiesOutcomes();

        StartCoroutine(nameof(ContinueDay));
    }


    IEnumerator ContinueDay()
    {
        yield return new WaitForSeconds(10f);
        UIManager.Instance.timeReference.timeStopped = false;
        UIManager.Instance.canvasDayEnd.SetActive(false);
        UIManager.Instance.telephone.calendar.ResetActivities();
    }
}
