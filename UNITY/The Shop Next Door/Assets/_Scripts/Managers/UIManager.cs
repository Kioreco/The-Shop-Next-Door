using System;
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

    [Header("TUTORIAL")]
    [SerializeField] private GameObject tutorialLayout;
    [SerializeField] private GameObject[] tutorialSlides;
    [SerializeField] private GameObject tutorial_NextButton;
    [SerializeField] private GameObject tutorial_PrevButton;
    [SerializeField] private GameObject tutorial_PlayButton;


    [Header("MATCHMAKING SCENE")]
    [SerializeField] public TextMeshProUGUI joinCode_Text;
    [SerializeField] public GameObject joinMatch_Button;
    [SerializeField] public TMP_InputField joinCode_Input;
    [SerializeField] public GameObject messageMatch_waiting;
    [SerializeField] public GameObject messageMatch_wrong;

    //[SerializeField] private bool telephoneMini;
    [Header("INGAME SCENE")]
    [SerializeField] public GameObject canvasIngame;

    [SerializeField] public TelephoneController telephone;
    [SerializeField] public GameObject alertTelephone;
    [SerializeField] public PlayerVigor vigor;

    [SerializeField] private TextMeshProUGUI dineroJugador_text;
    [SerializeField] private TextMeshProUGUI newMoney_text;
    [SerializeField] private TextMeshProUGUI nombreTienda_text;
    [SerializeField] private TextMeshProUGUI inventory_text;
    [SerializeField] private TextMeshProUGUI maxInventory_text;

    [SerializeField] public WorkDayCycle schedule;
    [SerializeField] private TextMeshProUGUI day_text;
    [SerializeField] private TextMeshProUGUI hour_text;

    [SerializeField] public Image reputation_Bar;
    public float reputation_Rival;
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

    public Color redColor = new Color(0.80f, 0.02f, 0.27f);
    private Color greenColor = new Color(0.13f, 0.65f, 0.33f);
    public Color whiteTextColor = new Color(0.74f, 0.78f, 0.78f);

    [Header("THE END")]
    [SerializeField] public GameObject player1Result_text;
    [SerializeField] public GameObject player2Result_text;
    [SerializeField] private TextMeshProUGUI winnerName_text;
    [SerializeField] public TextMeshProUGUI inheritance_text;
    public GameObject ButtonDuplicateReward;

    [Header("AVISOS")]
    [SerializeField] private GameObject OpenSign;
    [SerializeField] private GameObject ClosedSign;
    [SerializeField] private GameObject messageTelephone;
    [SerializeField] private TextMeshProUGUI messageTelephone_text;

    [Header("DUDAS")]
    [SerializeField] private DudaController dudaController;
    [SerializeField] private GameObject duda_gameObject;
    [SerializeField] private Image duda_Image;
    [SerializeField] private TextMeshProUGUI duda_text;
    [SerializeField] private TextMeshProUGUI yesAnswer_text;
    [SerializeField] private TextMeshProUGUI noAnswer_text;
    [SerializeField] private GameObject[] questionTimer;

    //evento duda resuelta
    public EventHandler eventoDudaResuelta;

    //perceptions:
    public bool canChargePlayer = true;


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
        UpdateAlmacenSpace_UI();

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

    #region Menus

    public void RestartToMenu()
    {
        Cancel_Button();
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
        joinMatch_Button.SetActive(false);
    }

    public void StartClient_Button()
    {
        RelayManager.Instance.StartClient(joinCode_Input.text);
    }

    public void CopyJoinCode()
    {
        GUIUtility.systemCopyBuffer = joinCode_Text.text;
    }

    public void Cancel_Button()
    {
        RelayManager.Instance.CancelMatch();
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

    public void OpenCloseModal(GameObject modal)
    {
        modal.SetActive(!modal.activeSelf);
    }

    int currentSlide = 0;

    public void NextTutorialSlide()
    {
        tutorialSlides[currentSlide].SetActive(false);
        tutorialSlides[currentSlide + 1].SetActive(true);
        if (!tutorial_PrevButton.activeSelf) { tutorial_PrevButton.SetActive(true); }
        if (currentSlide + 1 == 3) { tutorial_PlayButton.SetActive(true); tutorial_NextButton.SetActive(false); }
        currentSlide++;
    }

    public void PrevTutorialSlide()
    {
        tutorialSlides[currentSlide].SetActive(false);
        tutorialSlides[currentSlide - 1].SetActive(true);
        if (currentSlide - 1 == 2) { tutorial_PlayButton.SetActive(false); tutorial_NextButton.SetActive(true); }
        if (currentSlide - 1 == 0) { tutorial_PrevButton.SetActive(false); }
        currentSlide--;
    }

    #endregion

    public IEnumerator DelayDisableObject(float delay, GameObject obj)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    #region Mensajes Madre
    public void ChangeSignShop(bool shopOpen)
    {
        if (shopOpen) { OpenSign.SetActive(true); StartCoroutine(DelayDisableObject(6f, OpenSign)); }
        else { ClosedSign.SetActive(true); StartCoroutine(DelayDisableObject(10f, ClosedSign)); }
    }


    public void MessageAlert(int messageNumber)
    {

        if (messageNumber == 0) //Compra suministros
        {
            messageTelephone_text.SetText("Buy supplies\r\nbefore clients arrive!");
        }
        else if (messageNumber == 1)
        {
            messageTelephone_text.SetText("The clock is ticking, closing time is half an hour away!");
        }
        else if (messageNumber == 2)
        {
            messageTelephone_text.SetText("Attention!\r\nClients must go now!");
        }

        messageTelephone.SetActive(true);

        StartCoroutine(DelayDisableObject(4f, messageTelephone));
    }

    #endregion

    #region Money Ingame
    public void UpdatePlayerMoney_UI()
    {
        dineroJugador_text.SetText(GameManager.Instance.dineroJugador.ToString("F2"));
        if (GameManager.Instance.dineroJugador < 0) { dineroJugador_text.color = redColor; }
        else { dineroJugador_text.color = whiteTextColor; }
    }

    public void UpdateNewMoney_UI(float money, bool increase)
    {
        if (increase) { newMoney_text.color = greenColor; newMoney_text.SetText("+" + money.ToString("F2")); }
        else { newMoney_text.color = redColor; newMoney_text.SetText("-" + money.ToString("F2")); }

        StartCoroutine(WaitSecondsToHide());
    }

    private IEnumerator WaitSecondsToHide()
    {
        yield return new WaitForSeconds(2f);
        newMoney_text.SetText("");
    }

    #endregion

    #region InGame UI: Almacen, Reputation, Days, Pay & Clean
    public void UpdateAlmacenSpace_UI()
    {
        inventory_text.SetText(AlmacenManager.Instance.espacioUsado.ToString());
        maxInventory_text.SetText(AlmacenManager.Instance.maxEspacio.ToString());
        AlmacenManager.Instance.UpdateWarehouseCapacity_UI();
        if (AlmacenManager.Instance.espacioUsado == AlmacenManager.Instance.maxEspacio) { inventory_text.color = redColor; }
        else { inventory_text.color = whiteTextColor; }
    }

    public void UpdateReputationIngame_UI()
    {
        reputation_Bar.fillAmount = Mathf.InverseLerp(0, 100, GameManager.Instance.reputation);
    }

    [HideInInspector] public Transform cajaPlayerPosition;

    public void GoToPay_Button()
    {
        //Physics.IgnoreLayerCollision(GameManager.Instance._player.playerLayer, GameManager.Instance._player.npcLayer, true);
        //GameManager.Instance._player.WalkToPosition(cajaPlayerPosition.position, true);
        //GameManager.Instance.workerGoToPay(null, true);
        if (!GameManager.Instance.WorkerHire) GameManager.Instance._player.WalkToPosition(cajaPlayerPosition.position, true);
    }

    public void UpdatePayingBar_UI()
    {
        GameManager.Instance._player._playerAnimator.SetTrigger("playerPaying");
        StartCoroutine(RellenarImagen(cajero_Bar, 2.8f, true, false, null, false));
    }

    public void UpdateCleaning_UI(Image progressImage, RubbishController rubbish, bool isNene)
    {
        GameManager.Instance._player._playerAnimator.SetTrigger("playerCleaning");
        StartCoroutine(RellenarImagen(progressImage, 3f, false, true, rubbish, isNene));
    }

    public IEnumerator RellenarImagen(Image imageToFill, float timeToFill, bool isCajero, bool isRubbish, RubbishController rubbish, bool isNene)
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

        //if (isCajero) {imageToFill.fillAmount = 0f; }
        //if (isRubbish) { rubbish.Destruir(); }
        if (isCajero)
        {
            imageToFill.fillAmount = 0f;
            GameManager.Instance.isAnyWorkerInPayBox = false;
        }
        if (isRubbish)
        {
            if (!isNene) rubbish.Destruir();
            else
            {
                print("destroying");
                Destroy(rubbish.gameObject);
                GameManager.Instance._player.enableMovement(false);
            }
        }
        //Physics.IgnoreLayerCollision(GameManager.Instance._player.playerLayer, GameManager.Instance._player.npcLayer, false);
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

    #endregion

    #region Volumes
    public void ChangeVolumeEffects_Telephone(bool isActive)
    {
        if (isActive)
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

    private ClampedFloatParameter vignetteIntensityHigh = new ClampedFloatParameter(0.536f, 0f, 1f);
    private ClampedFloatParameter vignetteIntensityLow = new ClampedFloatParameter(0.307f, 0f, 1f);

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
    #endregion

    #region Actividades
    public void WriteActivityOutcomesEndDay_UI(string[] final_outcomes)
    {
        activity1_outcome_text.SetText(final_outcomes[0]);
        activity2_outcome_text.SetText(final_outcomes[1]);
        activity3_outcome_text.SetText(final_outcomes[2]);
    }

    public void ResetActivitiesEndDay_UI()
    {
        activity1_outcome_text.SetText("...");
        activity2_outcome_text.SetText("...");
        activity3_outcome_text.SetText("...");
    }
    #endregion

    public void UpdateFinalWeekTexts(int playerID)
    {
        if (playerID == 0)
        {
            print($"mayor? {GameManager.Instance.playerResult > GameManager.Instance.playerResultRival}");
            if (GameManager.Instance.playerResult > GameManager.Instance.playerResultRival) winnerName_text.SetText("EMMA!");
            else winnerName_text.SetText("GEMMA!");
        }
        else
        {
            print($"menor? {GameManager.Instance.playerResult < GameManager.Instance.playerResultRival}");
            if (GameManager.Instance.playerResult < GameManager.Instance.playerResultRival) winnerName_text.SetText("EMMA!");
            else winnerName_text.SetText("GEMMA!");
        }

        if (GameManager.Instance.playerResult > GameManager.Instance.playerResultRival)
        {
            GameManager.Instance.inheritance = 10;
            inheritance_text.SetText(GameManager.Instance.inheritance.ToString());
        }
        else
        {
            GameManager.Instance.inheritance = 2;
            inheritance_text.SetText(GameManager.Instance.inheritance.ToString());
        }
    }

    public void FinishGame()
    {
        GameManager.Instance._player.DestroyClient();
    }

    #region Dudas

    public void CreateDuda_UI(string productoName, bool isKaren)
    {
        if (!isKaren) dudaController.CreateDuda(productoName);
        else dudaController.CreateComplaintDuda();

        duda_text.SetText(dudaController.dudaText);
        yesAnswer_text.SetText(dudaController.dudaAnswerYes);
        noAnswer_text.SetText(dudaController.dudaAnswerNo);
        duda_Image.sprite = dudaController.dudaImage;

        ChangeVolumeEffects_Telephone(true);

        telephone.ResetTelephone();
        telephone.MiniTelephone.SetActive(false);
        if (vigor.vigorLow_Dialogue.activeInHierarchy) { vigor.vigorLow_Dialogue.SetActive(false); }
        vigor.enabled = false;

        duda_gameObject.SetActive(true);

        InvokeRepeating(nameof(TimerDuda), 1.66f, 1.66f);
    }

    private int timerCountdown = 0;

    private void TimerDuda()
    {
        if (timerCountdown == questionTimer.Length) { DesactivateDuda(); }
        questionTimer[timerCountdown].SetActive(false);
        timerCountdown++;
    }

    private void DesactivateDuda()
    {
        CancelInvoke(nameof(TimerDuda));

        ChangeVolumeEffects_Telephone(false);
        telephone.MiniTelephone.SetActive(true);
        vigor.enabled = true;
        duda_gameObject.SetActive(false);

        GameManager.Instance._player.enableMovement(false);
        GameManager.Instance._player._playerAnimator.SetBool("playerTalking", false);
        eventoDudaResuelta?.Invoke(this, EventArgs.Empty);

        timerCountdown = 0;
        foreach (GameObject question in questionTimer)
        {
            question.SetActive(true);
        }
    }

    public void AnswerDuda_YES()
    {
        if (dudaController.givesPartner) { VidaPersonalManager.Instance.hasPartner = true; }

        DesactivateDuda();
    }

    public void AnswerDuda_NO()
    {
        DesactivateDuda();
    }

    #endregion


    #region SettingsApp

    [Header("Settings App")]
    [SerializeField] private GameObject settingsMainScreen;
    [SerializeField] private GameObject settingsMusicApp;
    [SerializeField] private GameObject settingsTutorialApp;
    [SerializeField] private GameObject settingsCameraApp;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private GameObject[] musicVolumes;
    [SerializeField] private GameObject[] soundVolumes;


    public void OpenSettingsApp(GameObject app)
    {
        settingsMainScreen.SetActive(false);
        app.SetActive(true);
    }

    public void CloseSettingsApp()
    {
        settingsMainScreen.SetActive(true);
        if (settingsMusicApp.activeInHierarchy) { settingsMusicApp.SetActive(false); }
        if (settingsTutorialApp.activeInHierarchy) { settingsTutorialApp.SetActive(false); }
        if (settingsCameraApp.activeInHierarchy) { settingsCameraApp.SetActive(false); }
    }

    public void ChangeMusicVolume()
    {
        if (musicSlider.value == 0)
        {
            if (!musicVolumes[0].activeInHierarchy) { musicVolumes[0].SetActive(true); }
            if (musicVolumes[1].activeInHierarchy) { musicVolumes[1].SetActive(false); }
        }
        else if (musicSlider.value > 0 && musicSlider.value < 0.5f)
        {
            if (musicVolumes[0].activeInHierarchy) { musicVolumes[0].SetActive(false); }
            if (!musicVolumes[1].activeInHierarchy) { musicVolumes[1].SetActive(true); }
            if (musicVolumes[2].activeInHierarchy) { musicVolumes[2].SetActive(false); }
        }
        else
        {
            if (musicVolumes[1].activeInHierarchy) { musicVolumes[1].SetActive(false); }
            if (!musicVolumes[2].activeInHierarchy) { musicVolumes[2].SetActive(true); }
        }
    }

    public void ChangeSoundVolume()
    {
        if (soundSlider.value == 0)
        {
            if (!soundVolumes[0].activeInHierarchy) { soundVolumes[0].SetActive(true); }
            if (soundVolumes[1].activeInHierarchy) { soundVolumes[1].SetActive(false); }
        }
        else if (soundSlider.value > 0 && musicSlider.value < 0.5f)
        {
            if (soundVolumes[0].activeInHierarchy) { soundVolumes[0].SetActive(false); }
            if (!soundVolumes[1].activeInHierarchy) { soundVolumes[1].SetActive(true); }
            if (soundVolumes[2].activeInHierarchy) { soundVolumes[2].SetActive(false); }
        }
        else
        {
            if (soundVolumes[1].activeInHierarchy) { soundVolumes[1].SetActive(false); }
            if (!soundVolumes[2].activeInHierarchy) { soundVolumes[2].SetActive(true); }
        }
    }

    #endregion
}
