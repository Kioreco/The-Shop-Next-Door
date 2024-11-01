using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //[Header("TITLE SCENE")]

    //[Header("MENU SCENE")]
    //[Header("MATCHMAKING SCENE")]
    [Header("MATCHMAKING SCENE")]
    [SerializeField] public TextMeshProUGUI matchCodeMatchMaking_Text;
    [SerializeField] public TMP_InputField joinCode_Input;
    [SerializeField] public GameObject messageMatch_waiting;
    [SerializeField] public GameObject messageMatch_wrong;

    //[SerializeField] private bool telephoneMini;
    [Header("INGAME SCENE")]
    [SerializeField] private GameObject canvasIngame;

    [SerializeField] public TelephoneController telephone;

    [SerializeField] private TextMeshProUGUI dineroJugador_text;
    [SerializeField] private TextMeshProUGUI nombreTienda_text;
    [SerializeField] private TextMeshProUGUI inventoryInfo_text;

    [SerializeField] public WorkDayCycle timeReference;
    [SerializeField] private TextMeshProUGUI day_text;
    [SerializeField] private TextMeshProUGUI hour_text;

    [SerializeField] private Image clientHappiness_Bar;
    [SerializeField] private Image playerVigor_Bar;

    [Header("DAY-END MENU")] 
    public GameObject canvasDayEnd;

    [SerializeField] private TextMeshProUGUI activity1_outcome_text;
    [SerializeField] private TextMeshProUGUI activity2_outcome_text;
    [SerializeField] private TextMeshProUGUI activity3_outcome_text;

    public TextMeshProUGUI player1ShopName;
    public TextMeshProUGUI player2ShopName;

    public TextMeshProUGUI player1Money;
    public TextMeshProUGUI player2Money;

    public TextMeshProUGUI player1Clients;
    public TextMeshProUGUI player2Clients;

    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ExitGame()
    {
        ChangeScene("0 - TitleScene");
        if (NetworkManager.Singleton)
        {
            NetworkManager.Singleton.Shutdown();
        }
        Application.Quit();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void StartHost_Button()
    {
        RelayManager.Instance.StartHost();
        messageMatch_waiting.SetActive(true);
    }

    public void StartClient_Button()
    {
        RelayManager.Instance.StartClient(joinCode_Input.text);
    }

    public void UpdatePlayersIngameMoney_UI()
    {
        dineroJugador_text.SetText(GameManager.Instance.dineroJugador.ToString("F2"));
        if (GameManager.Instance.dineroJugador < 0) { dineroJugador_text.color = Color.red; }
        if (GameManager.Instance.dineroJugador > 0) { dineroJugador_text.color = Color.white; }
    }

    public void UpdateInventorySpace_UI()
    {
        inventoryInfo_text.SetText("INVENTORY: " + GameManager.Instance.espacioAlmacen + "/" + GameManager.Instance.maxEspacioAlmacen);
        if (GameManager.Instance.espacioAlmacen == GameManager.Instance.maxEspacioAlmacen) { inventoryInfo_text.color = Color.red; }
    }

    public void UpdateClientHappiness_UI()
    {
        clientHappiness_Bar.fillAmount = Mathf.InverseLerp(0, 100, GameManager.Instance.clientHappiness);
    }

    public void UpdatePlayerVigor_UI()
    {
        playerVigor_Bar.fillAmount = Mathf.InverseLerp(0, 100, GameManager.Instance.playerVigor);
    }

    public void UpdateTime_UI(int hours, int minutes)
    {
        hour_text.SetText(string.Format("{0:D2}:{1:D2}", hours, minutes));
    }

    public void UpdateDay_UI(string day)
    {
        day_text.SetText(day);
    }

    public void WriteActivityOutcomes_UI(string[] final_outcomes)
    {
        activity1_outcome_text.SetText(final_outcomes[0]);
        activity2_outcome_text.SetText(final_outcomes[1]);
        activity3_outcome_text.SetText(final_outcomes[2]);
    }
}
