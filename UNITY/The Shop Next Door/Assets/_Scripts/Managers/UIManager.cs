using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //[Header("TITLE SCENE")]

    [Header("MENU SCENE")]
    [SerializeField] private TextMeshProUGUI cashRegister_Text;
    int[] numeros = new int[4] { 0, 0, 0, 0 };
    [SerializeField] private GameObject canvas_menu;


    [Header("MATCHMAKING SCENE")]
    [SerializeField] public TextMeshProUGUI matchCodeMatchMaking_Text;
    [SerializeField] public TMP_InputField joinCode_Input;
    [SerializeField] public GameObject messageMatch_waiting;
    [SerializeField] public GameObject messageMatch_wrong;

    //[SerializeField] private bool telephoneMini;
    [Header("INGAME SCENE")]
    [SerializeField] private GameObject canvasIngame;

    [SerializeField] public TelephoneController telephone;
    [SerializeField] public PlayerVigor vigor;

    [SerializeField] private TextMeshProUGUI dineroJugador_text;
    [SerializeField] private TextMeshProUGUI newMoney_text;
    [SerializeField] private TextMeshProUGUI nombreTienda_text;
    [SerializeField] private TextMeshProUGUI inventory_text;
    [SerializeField] private TextMeshProUGUI maxInventory_text;

    [SerializeField] public WorkDayCycle schedule;
    [SerializeField] private TextMeshProUGUI day_text;
    [SerializeField] private TextMeshProUGUI hour_text;

    [SerializeField] private Image reputation_Bar;
    [HideInInspector] public Image cajero_Bar;
    [HideInInspector] public GameObject cajero_Canvas;

    [Header("Post-Proccessing")]
    [SerializeField] private Volume gameVolume;

    private ColorAdjustments colorAdjustments;
    private Vignette vignette;
    private DepthOfField depthOfField;

    private Color volumeColorNeutral = new Color(1f, 1f, 1f);
    private Color volumeColorDarkened = new Color(0.5f, 0.5f, 0.5f);


    [Header("DAY-END MENU")] 
    public GameObject canvasDayEnd;

    [SerializeField] private TextMeshProUGUI activity1_outcome_text;
    [SerializeField] private TextMeshProUGUI activity2_outcome_text;
    [SerializeField] private TextMeshProUGUI activity3_outcome_text;

    public TextMeshProUGUI player1ShopName;
    public TextMeshProUGUI player2ShopName;

    public TextMeshProUGUI player1Money;
    public TextMeshProUGUI player2Money;

    [SerializeField] private TextMeshProUGUI player1Day;
    [SerializeField] private TextMeshProUGUI player2Day;

    [SerializeField] public TextMeshProUGUI day_EndDay_Text;
    [SerializeField] public TextMeshProUGUI hour_EndDay_Text;

    public Image player1Reputation;
    public Image player2Reputation;

    private Color redColor = new Color(0.80f, 0.02f, 0.27f);
    private Color greenColor = new Color(0.13f, 0.65f, 0.33f);
    private Color whiteTextColor = new Color(0.74f, 0.78f, 0.78f);

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

    public void Start_UnityFalse()
    {
        UpdateReputationIngame_UI();
        UpdateInventorySpace_UI();

        gameVolume.profile.TryGet(out colorAdjustments);
        gameVolume.profile.TryGet(out vignette);
        gameVolume.profile.TryGet(out depthOfField);
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

    public void OpenCloseCanvas(GameObject canvas)
    {
        canvas_menu.SetActive(!canvas_menu.activeSelf);
        canvas.SetActive(!canvas.activeSelf);
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

    public void Cancel_Button()
    {
        RelayManager.Instance.CancelMatch();
    }

    public void UpdatePlayerMoney_UI()
    {
        dineroJugador_text.SetText(GameManager.Instance.dineroJugador.ToString("F2"));
        if (GameManager.Instance.dineroJugador < 0) { dineroJugador_text.color = redColor; }
        else { dineroJugador_text.color = whiteTextColor; }
    }

    public void UpdateNewMoney_UI(float money, bool increase)
    {
        if (increase) { newMoney_text.color = greenColor; }
        else { newMoney_text.color = redColor; }

        newMoney_text.SetText("+" + money.ToString("F2"));
        StartCoroutine(WaitSecondsToHide());
    }

    private IEnumerator WaitSecondsToHide()
    {
        yield return new WaitForSeconds(2f);
        newMoney_text.SetText("");
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

    public void ChangeVolumeEffects_Telephone(bool isTelephone)
    {
        if (isTelephone)
        {
            colorAdjustments.colorFilter.value = volumeColorDarkened;
            depthOfField.active = true;
        }
        else
        {
            colorAdjustments.colorFilter.value = volumeColorNeutral;
            depthOfField.active = false;
        }
    }

    private ClampedFloatParameter vignetteIntensityHigh = new ClampedFloatParameter(0.502f, 0f, 1f);
    private ClampedFloatParameter vignetteIntensityLow = new ClampedFloatParameter(0.366f, 0f, 1f);

    public void ChangeVolumeEffects_Vigor(bool vigorIsDown)
    {
        if (vigorIsDown)
        {
            vignette.color.overrideState = true;
            vignette.intensity = vignetteIntensityHigh;
        }
        else
        {
            vignette.color.overrideState = false;
            vignette.intensity = vignetteIntensityLow;
        }
    }

    [HideInInspector] public Transform cajaPlayerPosition;

    public void GoToPay_Button()
    {
        GameManager.Instance._player.WalkToPosition(cajaPlayerPosition.position, true);
    }

    public void UpdatePayingBar_UI()
    {
        StartCoroutine(RellenarImagen(cajero_Bar, 2.8f, true, false, null));
    }

    public void UpdateCleaning_UI(Image progressImage, RubbishController rubbish)
    {
        StartCoroutine(RellenarImagen(progressImage, 3f, false, true, rubbish));
    }

    public IEnumerator RellenarImagen(Image imageToFill, float timeToFill, bool isCajero, bool isRubbish, RubbishController rubbish)
    {
        float tiempoTranscurrido = 0f;
        imageToFill.fillAmount = 0;

        while (tiempoTranscurrido < timeToFill)
        {
            tiempoTranscurrido += Time.deltaTime;

            float progreso = tiempoTranscurrido / timeToFill;
            imageToFill.fillAmount = progreso;

            yield return null;
        }

        imageToFill.fillAmount = 1f;
        
        if (isCajero) { imageToFill.fillAmount = 0f; }
        if (isRubbish) { rubbish.Destruir(); }
    }

    public IEnumerator VaciarImagen(Image imageToFill, float timeToFill)
    {
        float tiempoTranscurrido = 0f;
        imageToFill.fillAmount = 1f;

        while (tiempoTranscurrido < timeToFill)
        {
            tiempoTranscurrido += Time.deltaTime;

            float progreso = 1f - (tiempoTranscurrido / timeToFill);
            imageToFill.fillAmount = progreso;

            yield return null;
        }

        imageToFill.fillAmount = 0f;
    }

    public void UpdateTime_UI(int hours, int minutes)
    {
        string time = string.Format("{0:D2}:{1:D2}", hours, minutes);
        hour_text.SetText(time);
        if (telephone.gameObject.activeSelf) { telephone.UpdateHourTelephone(time); }
    }

    public void UpdateNightTime_UI(int hours, int minutes)
    {
        string time = string.Format("{0:D2}\n{1:D2}", hours, minutes);
        hour_EndDay_Text.SetText(time);
    }

    public void UpdateDay_UI(string day, string dayNight)
    {
        day_text.SetText(day);
        telephone.calendar.UpdateDayCalendar(day);
        day_EndDay_Text.SetText(dayNight);
        player1Day.SetText(dayNight);
        player2Day.SetText(dayNight);
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

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void ResetEndDayActivities()
    {
        activity1_outcome_text.SetText("");
        activity2_outcome_text.SetText("");
        activity3_outcome_text.SetText("");
    }
}
