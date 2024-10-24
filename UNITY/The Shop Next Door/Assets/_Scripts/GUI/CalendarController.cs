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

    // ACTIVITIES

    //activityName
    //outcomes
    //statesLuck
    //romanticProbability
    //friendshipProbability
    //personalDevelopmentProbability
    //happinessProbability
    //restingProbability

    private ActivityInfo[] activities_mixed = new ActivityInfo[] {
        new("Go for a walk with a book that you won't read", 
            new string[] { "You got interviewed by a TikToker and the Internet made fun of you", "You didn't remember to take out the book of your tote bag, but they gifted you a massage coupon", "A cute person sat next to you and asked for books recommendations and your number" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.4f,
            0.1f,
            0.2f,
            0.6f,
            0.62f),
        new("Do yoga",
            new string[] { "You tried doing the Astavakrasana but your hand slipped and you were hurt", "You reached the Nirvana and now you can sleep without problems", "You became intimate with the modern moms and they gifted you a pricey necklace" },
            new float[] {1.0f, 1.0f, 1.0f,1.0f},
            0.1f,
            0.45f,
            0.65f,
            0.7f,
            0.8f),
        new("Go to the gym",
            new string[] { "The treadmill outruned you and you fell in a ridiculous way to the floor", "The gym was empty and you could blast out loud Taylor Swift without any judgement", "You lifted more weight than the noisy gymbro and make him cry" },
            new float[] {1.0f, 1.0f, 1.0f,1.0f},
            0.18f,
            0.25f,
            0.66f,
            0.2f,
            0.1f),
        new("Go shopping with friends",
            new string[] { "There was nothing cute of your size and started crying in the middle of the shopping center", "You exited one shop without paying and nobody noticed", "Everything you wanted was half-priced and you even got gifted a curious book" },
            new float[] {1.0f, 1.0f, 1.0f,1.0f},
            0.13f,
            0.9f,
            0.3f,
            0.42f,
            0.33f),
        new("Binge watch Gilmore Girls",
            new string[] { "You were crying so bad watching Lorelai and Rory love each other so much that the pizza on the oven got burn", "You wanted to be like Rory so you read an intellectual book in one sitting", "You got moved so much by the relation of Emily and Lorelai that you called your mother to ask how is her life" },
            new float[] {1.0f, 1.0f, 1.0f,1.0f},
            0.3f,
            0.54f,
            1.0f,
            1.0f,
            0.9f),
        new("Go to an art workshop",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f,1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Cook something sweet",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f,1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go to a DJ workshop",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f,1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Clean the house",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f,1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Search for a house to buy",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f,1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Call mom and listen to her complains",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f,1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Write fanfics with the lights off",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f,1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go vintage shopping with a cute hat",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f,1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Listen to a sad playlist",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f,1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
    };

    private ActivityInfo[] activities_romantic_Partner = new ActivityInfo[] {
        new("Go to a museum date with partner",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Ask partner if they will love me if I was a bug",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Have a picnic with partner",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Ask partner if they love The Smiths",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Have a tematic date with partner",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Make partner watch Gilmore Girls with you",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
    };

    private ActivityInfo[] activities_romantic_NotPartner = new ActivityInfo[] {
        new("Go to a private party",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go on a date with someone from a dating app",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go to a café alone and act mysterious",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Invite someone over",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Prepare a fancy dinner and lay by the phone",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go to the town disco",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go to blind date",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Read a pretentious book in the park",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go to dance classes",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {0.0f, 0.0f, 0.0f, 0.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
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
