using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CountdownManager : NetworkBehaviour
{
    public TextMeshProUGUI countdownText;
    public float countdownTime = 3f; 
    private float currentTime;

    private NetworkVariable<float> networkTime = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Server);

    [SerializeField] private GameObject canvasCountdown;
    private Button readyButton;
    private GameObject playerReady_image;

    [SerializeField] private Button readyButton_P1;
    [SerializeField] private Button readyButton_P2;
    [SerializeField] private GameObject player1Ready_image;
    [SerializeField] private GameObject player2Ready_image;

    int contReady = 0;

    public static CountdownManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            ulong id = NetworkManager.Singleton.LocalClientId;
            if ((int)id == 0)
            {
                readyButton = readyButton_P1;
                playerReady_image = player1Ready_image;
            }
            else
            {
                readyButton = readyButton_P2;
                playerReady_image = player2Ready_image;
            }
            readyButton.gameObject.SetActive(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [ClientRpc]
    public void StartCountdownClientRpc()
    {
        print("desactivo imagen");
        playerReady_image.SetActive(false);
        countdownText.gameObject.SetActive(true);
    }

    [ClientRpc]
    void UpdateTextClientRpc(float n)
    {
        //networkTime.Value = n;
        countdownText.text = Mathf.Ceil(n).ToString();
    }

    void UpdateCountdown()
    {
        countdownText.text = Mathf.Ceil(networkTime.Value).ToString();
        UpdateTextClientRpc(networkTime.Value);

        if (currentTime <= 0)
        {
            //Debug.Log("Countdown finished");

            CancelInvoke("UpdateCountdown");
            StartRaceClientRpc();
        }

        currentTime -= 1f;
        networkTime.Value = currentTime;
    }

    [ClientRpc]
    void StartRaceClientRpc()
    {
        countdownText.gameObject.SetActive(false);

        canvasCountdown.SetActive(false);
        UIManager.Instance.canvasIngame.SetActive(true);
        GameManager.Instance._player.enabled = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerReadyServerRpc()
    {
        contReady++;

        if (contReady == 2)
        {
            print("host en todos listos");
            player1Ready_image.SetActive(false);
            print($"player ready nombre: {playerReady_image.name}     está activa? {player1Ready_image.activeInHierarchy}");
            countdownText.gameObject.SetActive(true);
            StartCountdownClientRpc();

            currentTime = countdownTime;
            networkTime.Value = countdownTime;
            InvokeRepeating("UpdateCountdown", 1.0f, 1.0f);
        }
    }
    public void OnReadyButtonClicked()
    {
        ulong id = NetworkManager.Singleton.LocalClientId;
        //Debug.Log("Entra en el OnReady  " + (int)id);

        readyButton.gameObject.SetActive(false);

        if (contReady == 0)
            playerReady_image.SetActive(true);

        SetPlayerReadyServerRpc();

        //Debug.Log("Contador de ready: " + contReady);
    }
}
