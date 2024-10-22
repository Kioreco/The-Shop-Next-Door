using TMPro;
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
    [SerializeField] private TextMeshProUGUI dineroJugador_text;
    [SerializeField] private TextMeshProUGUI nombreTienda_text;
    [SerializeField] private TextMeshProUGUI inventoryInfo_text;

    [SerializeField] private WorkDayCycle timeReference;
    [SerializeField] private TextMeshProUGUI day_text;
    [SerializeField] private TextMeshProUGUI hour_text;

    [SerializeField] private Image clientHappiness_Bar;
    [SerializeField] private Image playerVigor_Bar;


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
        Application.Quit();
        ChangeScene("0 - TitleScene");
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

    public void UpdatePlayersIngameMoney_UI()
    {
        dineroJugador_text.SetText(GameManager.Instance.dineroJugador.ToString());
        if(GameManager.Instance.dineroJugador < 0) { dineroJugador_text.color = Color.red; }
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
}
