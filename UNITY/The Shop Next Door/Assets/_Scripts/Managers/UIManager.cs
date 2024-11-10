using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //[Header("TITLE SCENE")]

    [Header("MENU SCENE")]
    [SerializeField] private TextMeshProUGUI cashRegister_Text;
    int[] numeros = new int[4] { 0, 0, 0, 0 };

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
    [SerializeField] private TextMeshProUGUI inventory_text;
    [SerializeField] private TextMeshProUGUI maxInventory_text;

    [SerializeField] public WorkDayCycle timeReference;
    [SerializeField] private TextMeshProUGUI day_text;
    [SerializeField] private TextMeshProUGUI hour_text;

    [SerializeField] private Image reputation_Bar;
    [SerializeField] private Image playerVigor_Bar;
    [HideInInspector] public Image cajero_Bar;

    [Header("DAY-END MENU")] 
    public GameObject canvasDayEnd;

    [SerializeField] private TextMeshProUGUI activity1_outcome_text;
    [SerializeField] private TextMeshProUGUI activity2_outcome_text;
    [SerializeField] private TextMeshProUGUI activity3_outcome_text;

    public TextMeshProUGUI player1ShopName;
    public TextMeshProUGUI player2ShopName;

    public TextMeshProUGUI player1Money;
    public TextMeshProUGUI player2Money;

    public TextMeshProUGUI day_EndDay_Text;
    public TextMeshProUGUI hour_EndDay_Text;

    public Image player1Reputation;
    public Image player2Reputation;

    private Color redColor;
    private Color whiteTextColor;

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

    private void Start()
    {
        redColor = new Color(0.80f, 0.02f, 0.27f);
        whiteTextColor = new Color(0.74f, 0.78f, 0.78f);
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

    public void UpdatePlayersMoney_UI()
    {
        dineroJugador_text.SetText(GameManager.Instance.dineroJugador.ToString("F2"));
        if (GameManager.Instance.dineroJugador < 0) { dineroJugador_text.color = redColor; }
        else { dineroJugador_text.color = whiteTextColor; }
    }

    public void UpdateInventorySpace_UI()
    {
        inventory_text.SetText(GameManager.Instance.espacioAlmacen.ToString());
        maxInventory_text.SetText(GameManager.Instance.maxEspacioAlmacen.ToString());
        if (GameManager.Instance.espacioAlmacen == GameManager.Instance.maxEspacioAlmacen) { inventory_text.color = redColor; }
        else { inventory_text.color = whiteTextColor; }
    }

    public void UpdateReputationIngame_UI()
    {
        reputation_Bar.fillAmount = Mathf.InverseLerp(0, 100, GameManager.Instance.reputation);
    }

    public void UpdatePlayerVigor_UI()
    {
        playerVigor_Bar.fillAmount = Mathf.InverseLerp(0, 100, GameManager.Instance.playerVigor);
    }

    public void UpdatePayingBar_UI()
    {
        StartCoroutine(RellenarImagen());

        //He puesto que se tarda 10 segundos en pagar, pero cambialo según se necesite PAULA
    }

    IEnumerator RellenarImagen()
    {
        float tiempoTranscurrido = 0f;

        // Mientras no se haya completado el tiempo de relleno
        while (tiempoTranscurrido < 10.0f)
        {
            // Aumenta el tiempo transcurrido
            tiempoTranscurrido += Time.deltaTime;

            // Calcula el progreso como un valor entre 0 y 1
            float progreso = tiempoTranscurrido / 10.0f;

            // Asigna el progreso a la propiedad fillAmount de la imagen
            cajero_Bar.fillAmount = progreso;

            // Espera hasta el siguiente frame
            yield return null;
        }

        cajero_Bar.fillAmount = 1f;
    }

public void UpdateTime_UI(int hours, int minutes)
    {
        string time = string.Format("{0:D2}:{1:D2}", hours, minutes);
        hour_text.SetText(time);
        if (telephone.gameObject.activeSelf) { telephone.UpdateHourTelephone(time); }
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

    public void WriteCashRegister(int numero)
    {
        numeros[0] = numeros[1];
        numeros[1] = numeros[2];
        numeros[2] = numeros[3];
        numeros[3] = numero;
        cashRegister_Text.SetText(numeros[0].ToString() + " " + numeros[1].ToString() + " " + numeros[2].ToString() + " " + numeros[3].ToString());
    }
}
