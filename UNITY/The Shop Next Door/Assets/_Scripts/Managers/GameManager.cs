using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Network Variables")]
    private GameObject _playerPrefabHost;
    private GameObject _playerPrefabClient;
    public List<Transform> _spawnPositions = new List<Transform>();

    [Header("Player Objects Shared")]
    public GameObject cameraP1;
    public GameObject cameraP2;
    public Camera activeCamera;
    public GameObject separador;

    [SerializeField] private GameObject techoPlayer1;
    [SerializeField] private GameObject techoPlayer2;

    [Header("Individual Player References")]
    [HideInInspector] public float dineroJugador;
    [HideInInspector] public float dineroRival;
    [HideInInspector] public float espacioAlmacen;
    [HideInInspector] public float maxEspacioAlmacen;
    [HideInInspector] public float reputation;
    [HideInInspector] public float playerVigor;

    [Header("Network Game Manager")]
    public PlayerControler _player;

    [Header("Other Objects")]
    public Canvas canvasInteractable;
    [SerializeField] private Image cajero_1_Bar;
    [SerializeField] private Image cajero_2_Bar;
    [SerializeField] private GameObject cajero_1_Canvas;
    [SerializeField] private GameObject cajero_2_Canvas;

    [Header("Pay Zones")]
    [SerializeField] public Transform cajaPositionP1;
    [SerializeField] public Transform cajaPositionP2;


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
        dineroJugador = 500.0f;
        espacioAlmacen = 0;
        maxEspacioAlmacen = 100;
        reputation = 0;
        playerVigor = 100;

        UIManager.Instance.Start_UnityFalse();

        if (NetworkManager.Singleton.IsServer)
        {
            UIManager.Instance.telephone.calendar.RandomizeStates();
            UIManager.Instance.telephone.calendar.ChooseNewState();
        }

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
            UIManager.Instance.cajero_Bar = cajero_1_Bar;
            UIManager.Instance.cajero_Canvas = cajero_1_Canvas;
            UIManager.Instance.cajaPlayerPosition = cajaPositionP1;
        }
        else
        {
            techoPlayer2.SetActive(false);
            UIManager.Instance.telephone.ChangeLockedScreenBG(2);
            UIManager.Instance.telephone.ChangeLifeAppName(2);
            //UIManager.Instance.cajero_Bar = cajero_2_Bar;
            //UIManager.Instance.cajero_Canvas = cajero_2_Canvas;
            //UIManager.Instance.cajaPlayerPosition = cajaPositionP2;
        }
    }

    public void UpdateClientHappiness(float value)
    {
        //print($"actualizo felicidad clientes: {value}");
        reputation = value; //ELEFANTE - por hacer
        UIManager.Instance.UpdateReputationIngame_UI();
    }

    //public void UpdatePlayerVigor(float value)
    //{
    //    playerVigor += value; //ELEFANTE - por hacer
    //    UIManager.Instance.UpdatePlayerVigor_UI();
    //}

    public void UpdateAlmacenQuantity()
    {
        TiendaManager.Instance.updateAlmacenQuantity();
    }

    public void EndDay()
    {
        //LLAMADA A server rpc
        _player.FinalResume();

        //Time.timeScale = 0;

        UIManager.Instance.canvasDayEnd.SetActive(true);
        UIManager.Instance.telephone.calendar.ActivitiesOutcomes();
        UIManager.Instance.vigor.enabled = false;

        StartCoroutine(nameof(ContinueDay));
    }


    IEnumerator ContinueDay()
    {
        yield return new WaitForSeconds(15f);
        //Time.timeScale = 1;

        UIManager.Instance.schedule.timeStopped = false;
        UIManager.Instance.canvasDayEnd.SetActive(false);
        UIManager.Instance.telephone.calendar.ResetActivities();

        UIManager.Instance.vigor.RestoreVigor();
    }
}
