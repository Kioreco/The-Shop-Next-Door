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

    [Header("SHOP")]
    [SerializeField] public TextMeshProUGUI gemsAmountShop;

    [Header("ACCOUNT")]
    [SerializeField] public TextMeshProUGUI gemsAmountAccount;


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
        if (canvas == GameObject.FindWithTag("Shop"))
        {
            gemsAmountShop.text = AWSManager.Instance.gemsAmount.ToString();
        } 
        else if (canvas == GameObject.FindWithTag("Account"))
        {
            gemsAmountAccount.text = AWSManager.Instance.gemsAmount.ToString();
        }
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
        if(imageToFill!=null) imageToFill.fillAmount = 0;

        while (tiempoTranscurrido < timeToFill)
        {
            tiempoTranscurrido += Time.deltaTime;

            float progreso = tiempoTranscurrido / timeToFill;
            if (imageToFill != null)  imageToFill.fillAmount = progreso;

            yield return null;
        }

        if (imageToFill != null) imageToFill.fillAmount = 1f;

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
                if(rubbish.isActiveAndEnabled) Destroy(rubbish.gameObject);
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

    private string[] clientsName = new string[] { "Karen", "Antonia", "Roberto", "Pepito", "Agustina", "Manolito", "Elvirita", "Luke", "Lorelai", "Rory", "Emily", "Flora", "Edward"};

    private string[] dialogosBienvenida = new string[]
    {
    "Well, I hope dinner was as delicious as I know it was... But we all know you are not here for food and fun, so let me begin. This week I've been looking into your shops with the help of {clientsName} and they told me all kinds of things...",
    "Ah, I trust you all enjoyed the appetizers—I mean, they were spectacular, right? But let’s not kid ourselves, let’s dive in. This week, with a little help from {clientsName}, I’ve been keeping a very close eye on your shops, and oh, the stories I’ve heard…",
    "Alright, I hope you’ve savored every bite of tonight’s spread because it’s time for the real feast: the results! I’ve had my little birds—er, I mean {clientsName}—keeping tabs on your shops, and let me tell you, the tea is piping hot!",
    "Now that your plates are clean and your glasses are full, let’s move on to what you’ve all been waiting for. With the help of my trusted partner-in-crime, {clientsName}, I’ve been checking in on your shops. And, oh my, do I have things to share",
    "The wine was fine, the dessert divine, but let’s not pretend you came just for the ambiance. This week, {clientsName} has been my eyes and ears in your shops, and what is a family dinner without some critiques...",
    "Let’s put the forks down, shall we? It’s time for the main event. {clientsName} has been my undercover agent this week, reporting back on everything happening in your shops. Let’s see who’s been shining and who’s, well… not"
    };
    public string[] dialogoDinero = new string[] 
    {
    "Well, well, well, let’s talk numbers, shall we? After crunching some very telling figures, it’s clear that {winningSister} has been raking it in. Meanwhile, {losingSister}, I hate to say it, but your wallet seems a bit… lighter...",
    "Alright, let’s dive into the juicy stuff: the cash flow. {winningSister}, you’ve been making it rain, haven’t you? As for {losingSister}, let’s just say your financial forecast is looking a little cloudy",
    "Ladies, the financial standings are in, and the crown goes to… {winningSister}! As for {losingSister}, it looks like it’s time to cut back on the splurges",
    "Money talks, and it’s shouting {winningSister}’s name this week. {losingSister}, it seems your profits have taken a little vacation",
    "The numbers never lie, and this week they’re singing {winningSister}’s praises. {losingSister}, your bank account, however, might be singing a sadder tune",
    "Alright, let’s talk dollars and sense. {winningSister}, you’ve been turning your shop into a money-making machine. And {losingSister}, well… let’s just say we might need to revisit your budgeting strategy"
    };
    public string[] dialogoEntremedias = new string[] 
    {
    "It’s not all about the coins, darlings. I also want to see grace, a well-run shop, and maybe some prospects for a happily-ever-after. Is that too much to ask?",
    "Money is just one part of the puzzle, ladies. I’m looking for heart, ambition, and a life filled with joy—and maybe someone to share it with, too",
    "You know I don’t only care about the profits. I want a shop that dazzles, a name that shines, and a future full of love and happiness for both of you...",
    "Both of you know money isn’t everything to me. I want a thriving business, a glowing reputation, and maybe some wedding bells in the future...",
    "You know I want more than just numbers. I’m looking for a beautiful shop, a respected name, and don't you think I would be a marvelous grandma?"
    };
    public string[] dialogoBuenRomance = new string[]
    {
    "Well, it seems one of my daughters has been holding out on me! A little bird told me {sisterName} has a special someone. Care to spill the details later?",
    "Oh, I’ve heard whispers, {sisterName}. Is it true? Someone has stolen your heart? Do tell me everything later!",
    "Don’t think I didn’t notice the glow, {sisterName}. A mother always knows when there’s romance in the air. Who’s the lucky one?",
    "Oh, {sisterName}, you’ve been keeping secrets, haven’t you? I heard about your new flame—don’t you think your mother deserves to know everything first?",
    "Well, this is unexpected! {sisterName}, I hear you’ve found yourself a partner. Is it true, or is my source trying to get my hopes up for nothing?",
    "Oh, {sisterName}, you didn’t think I wouldn’t find out about your new lover, did you? Who is it? Are they charming enough to pass the mother test?"
    };
    public string[] dialogoMalRomance = new string[]
    {
    "Oh, {sisterName}, another week and still no suitor? Should I be worried, or should I talk to my friends' sons and daughters?",
    "Well, I had high hopes, {sisterName}, but it seems romance is not in your stars this week. Do I need to start setting you up myself?",
    "Oh, {sisterName}, still no one? I’m beginning to think you like making me wait. At this rate, I’ll be knitting scarves instead of baby blankets!",
    "I can’t believe I have to say this again, {sisterName}, but the clock is ticking. What’s the excuse this time? No lover at your age is... alarming!",
    "Still no partner, {sisterName}? What are you doing with your time? I thought I raised you better than this! I want to be a grandma before is to late...",
    };
    public string[] dialogoResultado = new string[] 
    { 
    "",
    };

    [SerializeField] private TextMeshProUGUI dialogoText;

    private void CreateDialogosEnd()
    {
        string[] dialogos = new string[3];
        string randomClient = clientsName[UnityEngine.Random.Range(0, clientsName.Length)];
        string nombreHermanaJugador;
        string nombreHermanaRival;
        if (GameManager.Instance.playerID == 0) { nombreHermanaJugador = "Gemma"; nombreHermanaRival = "Emma"; }
        else { nombreHermanaJugador = "Emma"; nombreHermanaRival = "Gemma"; }

        dialogos[0] = dialogosBienvenida[UnityEngine.Random.Range(0, dialogosBienvenida.Length)].Replace("{clientsName}", randomClient);
        if (GameManager.Instance.dineroJugador > GameManager.Instance.dineroRival)
        {
            if (GameManager.Instance.playerID == 0)
            {
                dialogos[1] = dialogoDinero[UnityEngine.Random.Range(0, dialogosBienvenida.Length)].Replace("{winningSister}", "Gemma");
                dialogos[1].Replace("{losingSister}", "Emma");
            }
            else
            {
                dialogos[1] = dialogoDinero[UnityEngine.Random.Range(0, dialogosBienvenida.Length)].Replace("{winningSister}", "Emma");
                dialogos[1].Replace("{losingSister}", "Gemma");
            }
        }
        else
        {
            if (GameManager.Instance.playerID == 0)
            {
                dialogos[1] = dialogoDinero[UnityEngine.Random.Range(0, dialogosBienvenida.Length)].Replace("{winningSister}", "Emma");
                dialogos[1].Replace("{losingSister}", "Gemma");
            }
            else
            {
                dialogos[1] = dialogoDinero[UnityEngine.Random.Range(0, dialogosBienvenida.Length)].Replace("{winningSister}", "Gemma");
                dialogos[1].Replace("{losingSister}", "Emma");
            }
        }

        dialogos[2] = dialogoEntremedias[UnityEngine.Random.Range(0, dialogoEntremedias.Length)];

        if (VidaPersonalManager.Instance.hasPartner)
        {
            dialogos[3] = dialogoBuenRomance[UnityEngine.Random.Range(0, dialogoBuenRomance.Length)].Replace("{sisterName}", nombreHermanaJugador);
        }
        else
        {
            dialogos[3] = dialogoMalRomance[UnityEngine.Random.Range(0, dialogoBuenRomance.Length)].Replace("{sisterName}", nombreHermanaJugador);
        }

        dialogos[4] = dialogoResultado[UnityEngine.Random.Range(0, dialogoResultado.Length)];

    }

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

    public void BuyProduct(int idSkin)
    {
        AWSManager.Instance.BuySkin(idSkin);
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
