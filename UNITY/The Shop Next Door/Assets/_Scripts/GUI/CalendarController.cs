using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalendarController : MonoBehaviour
{
    [Header("Basic Info")]
    [SerializeField] private TextMeshProUGUI weekDay_text;
    [SerializeField] private GameObject status;
    [SerializeField] private GameObject hour1_text;
    [SerializeField] private GameObject hour2_text;
    [SerializeField] private GameObject hour3_text;

    [Header("Activities Objects")]
    [SerializeField] private Activity[] activities_daily = new Activity[10];
    [SerializeField] public Activity[] activities_selected = new Activity[3];
    [SerializeField] private TextMeshProUGUI[] activities_text;

    [Header("Activities Options")]
    private int total_activities_num;

    private ActivityInfo[] activities_mixed = new ActivityInfo[] {
        new("Go for a walk with a book that you won't read", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Do yoga", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Go to the gym", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Go shopping with friends", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Binge watch Gilmore Girls", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Go to an art workshop", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Cook something sweet", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Go to a DJ workshop", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Clean the house", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Search for a house to buy", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Call mom and listen to her complains", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Write fanfics with the lights off", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Go vintage shopping with a cute hat", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Listen to a sad playlist", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f)
    };

    private ActivityInfo[] activities_romantic_Partner = new ActivityInfo[] {
        new("Go to a museum date with partner", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Ask partner if they will love me if I was a bug", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Have a picnic with partner", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Ask partner if they love The Smiths", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Have a tematic date with partner", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Make partner watch Gilmore Girls with you", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f)
    };

    private ActivityInfo[] activities_romantic_NotPartner = new ActivityInfo[] {
        new("Go to a private party", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on the black-market" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Go on a date with someone from a dating app", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Go to a café alone and act mysterious", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Invite someone over", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Prepare a fancy dinner and lay by the phone", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Go to the town disco", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Go to blind date", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Read a pretentious book in the park", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
        new("Go to dance classes", new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" }, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f)
    };



    private void Awake()
    {
        RandomizeActivities(activities_mixed);
        RandomizeActivities(activities_romantic_NotPartner);
        RandomizeActivities(activities_romantic_Partner);

        WriteActivitiesInUI();
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

    private void WriteActivitiesInUI()
    {
        // Mixed Actions
        for (int i = 0; i < 7; i++)
        {
            print("Activity Mixed " + activities_mixed[i]);
            print("Activity Daily " + activities_daily[i]);
            activities_text[i].SetText(activities_mixed[i].activityName);
            activities_daily[i].CopyActivity(activities_mixed[i]);
        }
        int j = 7;
        if (VidaPersonalManager.Instance.hasPartner)
        {
            // Romantic Actions
            for (int i = 0; i < 3; i++)
            {
                //int randomAction = Random.Range(0, activities_romantic_Partner.Count);
                activities_text[j].SetText(activities_romantic_Partner[i].activityName);
                activities_daily[j].CopyActivity(activities_romantic_Partner[i]);
                j++;
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                //int randomAction = Random.Range(0, activities_romantic_NotPartner.Count);
                activities_text[j].SetText(activities_romantic_NotPartner[i].activityName);
                activities_daily[j].CopyActivity(activities_romantic_NotPartner[i]);
                j++;
            }
        }

        

        
    }
}
