using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CalendarController : MonoBehaviour
{
    [Header("Basic Info")]
    [SerializeField] private TextMeshProUGUI weekDay_text;
    [SerializeField] private GameObject status;
    [SerializeField] private GameObject hour1_text;
    [SerializeField] private GameObject hour2_text;
    [SerializeField] private GameObject hour3_text;
    public string[] final_outcomes;

    [Header("Activities Objects")]
    [SerializeField] private Activity[] activities_daily = new Activity[10];
    [SerializeField] public Activity[] activities_selected = new Activity[3];
    [SerializeField] private TextMeshProUGUI[] activities_text;

    [HideInInspector] public List<ActivityInfo> activities_mixed;
    [HideInInspector] public List<ActivityInfo> activities_romantic;
    [HideInInspector] public List<ActivityInfo> activities_partner;


    private void Awake()
    {
        ActivityLoader loader = new ActivityLoader();
        loader.LoadActivities();

        RandomizeActivities(activities_mixed);
        RandomizeActivities(activities_romantic);
        RandomizeActivities(activities_partner);

        WriteDailyActivities();
    }

    private void RandomizeActivities(List<ActivityInfo> list)
    {
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            // Genera un �ndice aleatorio a partir de la posici�n actual (i) hasta el final de la lista
            int randomIndex = Random.Range(i, n);

            // Intercambia el elemento actual (i) con el elemento aleatorio (randomIndex)
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
    }

    private void WriteDailyActivities()
    {
        // Mixed Actions
        for (int i = 0; i < 5; i++)
        {
            activities_text[i].SetText(activities_mixed[i].activityName);
            activities_daily[i].CopyActivity(activities_mixed[i]);
        }
        // Romantic Action
        if (VidaPersonalManager.Instance.hasPartner)
        {
            // Partner Actions
            activities_text[5].SetText(activities_partner[0].activityName);
            activities_daily[5].CopyActivity(activities_partner[0]);
            //for (int i = 0; i < 1; i++)
            //{
            //    //int randomAction = Random.Range(0, activities_romantic_Partner.Count);
            //    activities_text[j].SetText(activities_partner[i].activityName);
            //    activities_daily[j].CopyActivity(activities_partner[i]);
            //    j++;
            //}
        }
        else
        {
            activities_text[5].SetText(activities_romantic[0].activityName);
            activities_daily[5].CopyActivity(activities_romantic[0]);
            //for (int i = 0; i < 1; i++)
            //{
            //    //int randomAction = Random.Range(0, activities_romantic_NotPartner.Count);
            //    //activities_text[5].SetText(activities_romantic[i].activityName);
            //    //activities_daily[5].CopyActivity(activities_romantic[i]);
            //    //j++;
            //}
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
        // 1. Solo se puede perder la pareja con una acci�n rom�ntica de pareja NEGATIVA
        // 2. Solo puede salir un outcome que permita tener una relaci�n mon�gama, si se tiene pareja y sale una acci�n que otorga pareja, se cambia esta �ltima por una outcome neutral


        // Si ha salido una acci�n buena, existen las posibilidades de:
        // 1. Tiene pareja y es una acci�n de pareja: Se mantiene la pareja y el outcome sigue siendo el positivo
        // 2. Tiene pareja y no es una acci�n de pareja: Se mantiene la pareja y 2 posibilidades: 
        //      2.1. La acci�n proporciona una pareja: Se cambia el outcome al neutral
        //      2.2. La acci�n no proporciona una pareja: Se mantiene el outcome positivo
        // 3. No tiene pareja:
        //      3.1. Si la acci�n proporciona una pareja: Se otorga la pareja
        //      3.2. Si la acci�n no proporciona una pareja: Se le mantiene el outcome positivo

        // Si el outcome es negativo hay dos posibilidades:
        // 1. Si se tiene pareja:
        //      1.1. Si es una acci�n de pareja: Poner el valor de la pareja al CONTRARIO de breakPartner
        // 2. En cualquier otro caso se mantiene el outcome negativo

        for (int i = 0; i < 3; i++)
        {
            if (activities_selected[i].activityInfo.activityName == "" || activities_selected[i].activityInfo == null) return;

            activitiesFilled++;

            float randomizer = Random.Range(0, 1);

            int outcomeWeighted = Mathf.RoundToInt(
                (activities_selected[i].activityInfo.ClimateStateLuck[0] * 0.45f + activities_selected[i].activityInfo.PersonalStateLuck[0] * 0.55f) * 0.8f
                + randomizer * 0.2f);


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
        foreach (Activity act in activities_selected)
        {
            if (activities_mixed.Remove(act.activityInfo))
            {
                act.activityInfo = null;
            }
        }
        RandomizeActivities(activities_mixed);
        RandomizeActivities(activities_romantic);
        RandomizeActivities(activities_partner);

        WriteDailyActivities();
    }

    public void UpdateDayCalendar(string day)
    {
        weekDay_text.SetText(day);
    }
}