using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class CalendarController : MonoBehaviour
{
    [Header("Basic Info")]
    [SerializeField] private TextMeshProUGUI weekDay_text;
    [SerializeField] private GameObject status;
    [SerializeField] private GameObject hour1_text;
    [SerializeField] private GameObject hour2_text;
    [SerializeField] private GameObject hour3_text;

    [Header("Activities Objects")]
    [SerializeField] private GameObject[] activities_object;
    [SerializeField] private TextMeshProUGUI[] activities_text;

    [Header("Activities Options")]
    [SerializeField] private Activity[] activity;
    private List<Activity> activities_options;




    #region Activities Options
    private void createActivities()
    {
        activities_options = new List<Activity>
        {   
            new("Go to a private party", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on the black-market"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Go for a walk with a book that you won't read", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Do yoga", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Go to the gym", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Go on a date", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Go shopping with friends", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Binge watch Gilmore Girls", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Go to a café alone and act mysterious", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Go to an art workshop", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Cook something sweet", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Invite someone over", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Prepare a fancy dinner and lay by the phone", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Go to a DJ workshop", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Go to the town disco", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Go to blind date", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Go to a date with partner", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Read a pretentious book in the park", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Clean the house", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Search for a house to buy", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Call mom and listen to her complains", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Go to dance classes", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Go near a filming site and act mysterious", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Write fanfics with the lights off", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Go vintage shopping with a cute hat", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Try luck with the 'Dinner with me' giveaway", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f),
            new("Listen to a sad playlist", new string[] {"You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace"}, 0.65f, 0.0f, 0.69f, 0.54f, 0.12f, 0.35f, 0.1f)
        };
    }
    
    #endregion


    private void Awake()
    {
        createActivities();
        WriteRandomizedActivities();
    }
    private void WriteRandomizedActivities()
    {
        foreach (TextMeshProUGUI act in activities_text)
        {
            int randomAction = Random.Range(0, activities_options.Count);
            act.SetText(activities_options[randomAction].activityName);
            activities_options.RemoveAt(randomAction);
        }
    }
}
