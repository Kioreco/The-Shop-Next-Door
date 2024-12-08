using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

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
    [HideInInspector] public int inheritance;
    //[HideInInspector] public float espacioAlmacen;
    //[HideInInspector] public float maxEspacioAlmacen;
    [HideInInspector] public float reputation;
    [HideInInspector] public float playerVigor;

    [HideInInspector] public float dineroRival;
    public double playerResult;
    public double playerResultRival;

    [Header("Network Things")]
    public PlayerControler _player;
    public int playerID;

    [Header("Other Objects")]
    public Canvas canvasInteractable;
    [SerializeField] private Image cajero_1_Bar;
    [SerializeField] private Image cajero_2_Bar;
    [SerializeField] private GameObject cajero_1_Canvas;
    [SerializeField] private GameObject cajero_2_Canvas;

    [Header("Pay Zones")]
    [SerializeField] public Transform cajaPositionP1;
    [SerializeField] public Transform cajaPositionP2;

    [Header("Workers Perception")]
    private static readonly object _lockPayBox = new object();
    private static readonly object _lockCleanRubbish = new object();
    public bool isAnyWorkerInPayBox = false;
    public List<GameObject> rubbishList = new List<GameObject>();

    [Header("Worker")]
    public GameObject worker;
    public bool WorkerHire = false;
    public GameObject nene;
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
            Application.runInBackground = true;

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
        //espacioAlmacen = 0;
        //maxEspacioAlmacen = 100;
        reputation = 50;
        playerVigor = 100;

        UIManager.Instance.Start_UnityFalse();

        InstantiatePlayers();
    }

    #region perception workers
    public void workerGoToPay(GameObject worker, bool isPlayer)
    {
        lock (_lockPayBox)
        {
            print("dentro del lock workergotopay");
            if (!isAnyWorkerInPayBox)
            {
                print("nadie pagando alguien pagando");
                isAnyWorkerInPayBox = true;
                if (!isPlayer) worker.GetComponent<WorkerBehaviour>().canCharge = true;
                else UIManager.Instance.canChargePlayer = true;
                //NO HAY NADIE COBRANDO
                //worker.GetComponent<WorkerSUEditorRunner>().clientesEsperandoCaja = 0;
            }
            else
            {
                print("alguien cobrando");
                if (!isPlayer) worker.GetComponent<WorkerBehaviour>().canCharge = false;
                else UIManager.Instance.canChargePlayer = false;
            }

        }
    }

    public float updateRubbishList()
    {
        //print("updateao lista rubbis");
        rubbishList = GameObject.FindGameObjectsWithTag("Rubbish").ToList();
        return rubbishList.Count;
    }

    public void workerGoToClean(GameObject worker, bool isPlayer, GameObject mancha)
    {
        lock (_lockCleanRubbish)
        {
            if (!isPlayer) //es worker
            {
                print("worker buscando mancha");
                worker.GetComponent<WorkerBehaviour>().manchaActual = rubbishList.First().GetComponent<RubbishController>();
                rubbishList[0].transform.GetChild(0).gameObject.SetActive(false);
                rubbishList.RemoveAt(0);
                //print("asignada mancha y quitada");
            }
            if (isPlayer)
            {
                print("player removiendo mancha de la lista");
                mancha.tag = "Untagged";
                //rubbishList.Remove(mancha);
            }
        }
    }
    public void Fire(GameObject worker)
    {
        print("desactivando trabajador");
        worker.SetActive(false);
        worker.GetComponent<WorkerBehaviour>().enabled = false;
    }

    public void ActivateNene()
    {
        print("activando nene");
        nene.GetComponent<NeneBTBehaviour>().enabled = true;
        nene.GetComponent<NeneBTBehaviour>().sigueEnLaTienda = true;
        nene.SetActive(true);
    }    
    public void DesactivateNene()
    {
        print("desactivando nene");
        nene.SetActive(false);
        nene.GetComponent<NeneBTBehaviour>().enabled = false;
        nene.GetComponent<NeneBTBehaviour>().manchas = 0;
    }
    #endregion

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

            UIManager.Instance.telephone.calendar.RandomizeStates();
            UIManager.Instance.telephone.calendar.ChooseNewState();
            playerID = 0;
        }
        else
        {
            techoPlayer2.SetActive(false);
            UIManager.Instance.telephone.ChangeLockedScreenBG(2);
            UIManager.Instance.telephone.ChangeLifeAppName(2);
            UIManager.Instance.cajero_Bar = cajero_2_Bar;
            UIManager.Instance.cajero_Canvas = cajero_2_Canvas;
            UIManager.Instance.cajaPlayerPosition = cajaPositionP2;
            playerID = 1;
        }
    }

    public void UpdateClientHappiness(float value)
    {
        //print($"actualizo felicidad clientes: {value}");
        reputation = value; //ELEFANTE - por hacer
        UIManager.Instance.UpdateReputationIngame_UI();
    }

    public void EndDay()
    {
        //LLAMADA A server rpc
        _player.FinalDayResume();

        UIManager.Instance.canvasDayEnd.SetActive(true);
        UIManager.Instance.telephone.calendar.ActivitiesOutcomes();
        UIManager.Instance.telephone.hirer.GenerateNewWorkers();
        UIManager.Instance.telephone.hirer.ChargeWorkerPrice();
        UIManager.Instance.vigor.enabled = false;

        StartCoroutine(nameof(ContinueDay));
    }

    public void FinalResult()
    {
        double workLife;
        double privateLife;

        if (_player.ID == 0)
        {
            workLife = dineroJugador * 0.30 + TiendaManager.Instance.workersP1.Count * 0.15 + reputation * 0.35 + 1 * 0.2; //el último 1 son las mejoras de los estantes
        }
        else
        {
            workLife = dineroJugador * 0.30 + TiendaManager.Instance.workersP2.Count * 0.15 + reputation * 0.35 + 1 * 0.2; //el último 1 son las mejoras de los estantes
        }

        privateLife = VidaPersonalManager.Instance.restProgress * 0.18 +
                      VidaPersonalManager.Instance.happinessProgress * 0.18 +
                      VidaPersonalManager.Instance.friendshipProgress * 0.14 +
                      VidaPersonalManager.Instance.developmentProgress * 0.2 +
                      VidaPersonalManager.Instance.romanticProgress * 0.3;

        playerResult = workLife * 0.5 + privateLife * 0.5;
        _player.FinalWeekResult();
    }


    IEnumerator ContinueDay()
    {
        yield return new WaitForSeconds(15f);

        UIManager.Instance.schedule.timeStopped = false;
        UIManager.Instance.telephone.ResetTelephone();

        UIManager.Instance.canvasDayEnd.SetActive(false);
        UIManager.Instance.telephone.calendar.ResetActivities();

        UIManager.Instance.vigor.enabled = true;
        UIManager.Instance.vigor.RestoreVigor();
    }

}
