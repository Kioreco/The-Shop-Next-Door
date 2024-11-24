using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class CalendarController : NetworkBehaviour
{
    [Header("Basic Info")]
    [SerializeField] private TextMeshProUGUI weekDay_text;
    [SerializeField] private GameObject hour1_text;
    [SerializeField] private GameObject hour2_text;
    [SerializeField] private GameObject hour3_text;
    public string[] final_outcomes;

    [Header("Activities Objects")]
    [SerializeField] private GameObject[] activities_daily = new GameObject[6];
    [SerializeField] public Activity[] activities_selected = new Activity[3];
    [SerializeField] private Activity[] activities_selected_BLANK;
    [SerializeField] private TextMeshProUGUI[] activities_text;

    [HideInInspector] public ActivityInfo[] activities_mixed;
    [HideInInspector] public ActivityInfo[] activities_romantic;
    [HideInInspector] public ActivityInfo[] activities_partner;

    [Header("States")]
    [SerializeField] public CalendarState[] weatherStates;
    [SerializeField] public CalendarState[] personalStates;

    public int currentWeatherState;
    public int currentPersonalState;

    [SerializeField] private PlayerVigor playerVigor;

    [Header("Drop Zones")]
    [SerializeField] private DropItem[] dropZones;

    public void Awake()
    {
        ActivityLoader loader = new ActivityLoader();
        loader.LoadActivities();

        RandomizeActivities(activities_mixed);
        RandomizeActivities(activities_romantic);
        RandomizeActivities(activities_partner);

        activities_selected_BLANK = activities_selected.ToArray();

        WriteDailyActivities();

        if (IsServer)
        {
            RandomizeStates();
            ChooseNewState();
        }
    }

    private void RandomizeActivities(ActivityInfo[] list)
    {
        int n = list.Length;
        for (int i = 0; i < n; i++)
        {
            // Genera un índice aleatorio a partir de la posición actual (i) hasta el final de la lista
            int randomIndex = Random.Range(i, n);

            // Intercambia el elemento actual (i) con el elemento aleatorio (randomIndex)
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
    }

    public void RandomizeStates()
    {
        int nW = weatherStates.Length;
        for (int i = 0; i < nW; i++)
        {
            // Genera un índice aleatorio a partir de la posición actual (i) hasta el final de la lista
            int randomIndex = Random.Range(i, nW);

            // Intercambia el elemento actual (i) con el elemento aleatorio (randomIndex)
            (weatherStates[randomIndex], weatherStates[i]) = (weatherStates[i], weatherStates[randomIndex]);
        }

        int nP = personalStates.Length;
        for (int i = 0; i < nP; i++)
        {
            int randomIndex = Random.Range(i, nP);

            (personalStates[randomIndex], personalStates[i]) = (personalStates[i], personalStates[randomIndex]);
        }
    }

    public void ChooseNewState()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (UIManager.Instance.schedule.currentDay - 1 >= 0)
            {
                weatherStates[UIManager.Instance.schedule.currentDay - 1].gameObject.SetActive(false);
                personalStates[UIManager.Instance.schedule.currentDay - 1].gameObject.SetActive(false);
            }
            weatherStates[UIManager.Instance.schedule.currentDay].gameObject.SetActive(true);
            currentWeatherState = weatherStates[UIManager.Instance.schedule.currentDay].numberState;

            personalStates[UIManager.Instance.schedule.currentDay].gameObject.SetActive(true);
            currentPersonalState = personalStates[UIManager.Instance.schedule.currentDay].numberState;

            playerVigor.SetNewFaceGemma(currentPersonalState);

            SendNewStateClientRpc(currentWeatherState, currentPersonalState);
        }
    }


    [ClientRpc]
    private void SendNewStateClientRpc(int numberWeather, int numberPersonal)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            ChoseNewStateByNumber(numberWeather, numberPersonal);
        }
    }

    public void ChoseNewStateByNumber(int numberWeather, int numberPersonal)
    {
        currentWeatherState = numberWeather;
        currentPersonalState = numberPersonal;
        foreach (var state in weatherStates)
        {
            if (state.numberState.Equals(numberWeather)) { state.gameObject.SetActive(true); }
            else { state.gameObject.SetActive(false); }
        }
        foreach (var state in personalStates)
        {
            if (state.numberState.Equals(numberPersonal)) { state.gameObject.SetActive(true); }
            else { state.gameObject.SetActive(false); }
        }

        playerVigor.SetNewFaceEmma(currentPersonalState);
    }

    private int mixedActivitiesUsed = 0;
    private int romanticActivitiesUsed = 0;
    private int partnerActivitiesUsed = 0;

    private void WriteDailyActivities()
    {
        // Mixed Actions
        for (int i = 0; i < 5; i++)
        {
            activities_text[i].SetText(activities_mixed[mixedActivitiesUsed].activityName);
            activities_daily[i].GetComponent<Activity>().CopyActivity(activities_mixed[mixedActivitiesUsed]);
            mixedActivitiesUsed++;
        }

        // Romantic Action
        if (VidaPersonalManager.Instance.hasPartner)
        {
            // Partner Actions
            activities_text[5].SetText(activities_partner[partnerActivitiesUsed].activityName);
            activities_daily[5].GetComponent<Activity>().CopyActivity(activities_partner[partnerActivitiesUsed]);
            partnerActivitiesUsed++;
        }
        else
        {
            activities_text[5].SetText(activities_romantic[romanticActivitiesUsed].activityName);
            activities_daily[5].GetComponent<Activity>().CopyActivity(activities_romantic[romanticActivitiesUsed]);
            romanticActivitiesUsed++;
        }
    }

    private float Daily_RomanticProgress;
    private float Daily_FriendshipProgress;
    private float Daily_DevelopmentProgress;
    private float Daily_HappinessProgress;
    private float Daily_RestProgress;

    public void ActivitiesOutcomes()
    {
        final_outcomes = new string[3];
        int activitiesFilled = 0;

        bool partnerGiven = VidaPersonalManager.Instance.hasPartner;
        Daily_RomanticProgress = 0.0f;
        Daily_FriendshipProgress = 0.0f;
        Daily_DevelopmentProgress = 0.0f;
        Daily_HappinessProgress = 0.0f;
        Daily_RestProgress = 0.0f;

        // REGLAS:
        // 1. Solo se puede perder la pareja con una acción romántica de pareja NEGATIVA
        // 2. Solo puede salir un outcome que permita tener una relación monógama, si se tiene pareja y sale una acción que otorga pareja, se cambia esta última por una outcome neutral


        // Si ha salido una acción buena, existen las posibilidades de:
        // 1. Tiene pareja y es una acción de pareja: Se mantiene la pareja y el outcome sigue siendo el positivo
        // 2. Tiene pareja y no es una acción de pareja: Se mantiene la pareja y 2 posibilidades: 
        //      2.1. La acción proporciona una pareja: Se cambia el outcome al neutral
        //      2.2. La acción no proporciona una pareja: Se mantiene el outcome positivo
        // 3. No tiene pareja:
        //      3.1. Si la acción proporciona una pareja: Se otorga la pareja
        //      3.2. Si la acción no proporciona una pareja: Se le mantiene el outcome positivo

        // Si el outcome es negativo hay dos posibilidades:
        // 1. Si se tiene pareja:
        //      1.1. Si es una acción de pareja: Poner el valor de la pareja al CONTRARIO de breakPartner
        // 2. En cualquier otro caso se mantiene el outcome negativo

        for (int i = 0; i < 3; i++)
        {
            if (activities_selected[i].activityInfo.activityName == "" || activities_selected[i].activityInfo == null) { final_outcomes[i] = ""; return; }
        }

            activitiesFilled++;

            float randomizer = Random.Range(0, 1);

            int outcomeWeighted = Mathf.RoundToInt(
                (activities_selected[i].activityInfo.ClimateStateLuck[currentWeatherState] * 0.45f + activities_selected[i].activityInfo.PersonalStateLuck[currentPersonalState] * 0.55f) * 1.75f
                + randomizer * 0.25f);


            if (outcomeWeighted == 2)
            {
                if (partnerGiven)
                {
                    if (activities_selected[i].activityInfo.isRomanticPartner)
                    {
                        VidaPersonalManager.Instance.hasPartner = true;
                        partnerGiven = true;
                        outcomeWeighted = 2;
                    }
                    else
                    {
                        if (activities_selected[i].activityInfo.givesPartner)
                        {
                            outcomeWeighted = 1;
                        }
                        else
                        {
                            outcomeWeighted = 2;
                        }
                    }
                }
                else
                {
                    if (activities_selected[i].activityInfo.givesPartner)
                    {
                        VidaPersonalManager.Instance.hasPartner = true;
                        partnerGiven = true;
                        outcomeWeighted = 2;
                    }
                    else
                    {
                        outcomeWeighted = 2;
                    }
                }
            }
            else if (outcomeWeighted == 0)
            {
                if (partnerGiven && activities_selected[i].activityInfo.isRomanticPartner)
                {
                    VidaPersonalManager.Instance.hasPartner = !activities_selected[i].activityInfo.breakPartner;
                    partnerGiven = VidaPersonalManager.Instance.hasPartner;
                }
            }

            CalculateDailyPartialActivityProgress(i, outcomeWeighted);

            final_outcomes[i] = activities_selected[i].activityInfo.outcomes[outcomeWeighted];
        }

        VidaPersonalManager.Instance.UpdateLifeProgress(activitiesFilled, Daily_RomanticProgress, Daily_FriendshipProgress, Daily_DevelopmentProgress, Daily_HappinessProgress, Daily_RestProgress);
        UIManager.Instance.WriteActivityOutcomes_UI(final_outcomes);
        UIManager.Instance.telephone.lifeRadar.UpdateStatsRadar();
    }

    float discountValue = 0f;

    private void CalculateDailyPartialActivityProgress(int actNumber, int outcome)
    {
        if (outcome == 0) { discountValue = 0.1f; }
        else if (outcome == 1) { discountValue = 0.5f; }
        else if (outcome == 2) { discountValue = 1.0f; }

        Daily_RomanticProgress += discountValue * activities_selected[actNumber].activityInfo.romanticProbability;
        Daily_FriendshipProgress += discountValue * activities_selected[actNumber].activityInfo.friendshipProbability;
        Daily_DevelopmentProgress += discountValue * activities_selected[actNumber].activityInfo.personalDevelopmentProbability;
        Daily_HappinessProgress += discountValue * activities_selected[actNumber].activityInfo.happinessProbability;
        Daily_RestProgress += discountValue * activities_selected[actNumber].activityInfo.restingProbability;
    }

    public void ResetActivities()
    {
        //Limpiar drag zone
        foreach (DropItem dropZone in dropZones)
        {
            dropZone.ResetDropZone();
        }

        //Limpiar acciones seleccionadas
        foreach (GameObject actDaily in activities_daily)
        {
            actDaily.GetComponent<DraggingItems>().ActivateActivity();
        }

        activities_selected = activities_selected_BLANK.ToArray();

        UIManager.Instance.ResetEndDayActivities();

        //Nuevas actividades y nuevos estados
        WriteDailyActivities();

        ChooseNewState();
    }

    public void UpdateDayCalendar(string day)
    {
        weekDay_text.SetText(day);
    }
}
