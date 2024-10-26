using System;
using System.Collections;
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
    private string[] final_outcomes;

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
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.1f,
            0.45f,
            0.65f,
            0.7f,
            0.8f),
        new("Go to the gym",
            new string[] { "The treadmill outruned you and you fell in a ridiculous way to the floor", "The gym was empty and you could blast out loud Taylor Swift without any judgement", "You lifted more weight than the noisy gymbro and make him cry" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.18f,
            0.25f,
            0.66f,
            0.2f,
            0.1f),
        new("Go shopping with friends",
            new string[] { "There was nothing cute of your size and started crying in the middle of the shopping center", "You exited one shop without paying and nobody noticed", "Everything you wanted was half-priced and you even got gifted a curious book" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.13f,
            0.9f,
            0.3f,
            0.42f,
            0.33f),
        new("Binge watch Gilmore Girls",
            new string[] { "You were crying so bad watching Lorelai and Rory love each other so much that the pizza on the oven got burn", "You wanted to be like Rory so you read an intellectual book in one sitting", "You got moved so much by the relation of Emily and Lorelai that you called your mother to ask how is her life" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.3f,
            0.54f,
            1.0f,
            1.0f,
            0.9f),
        new("Go to an art workshop",
            new string[] { "You mistook your glass of wine and drank the dirty water instead", "You ended with your hands lilac, stained the t-shirt of the person next to you without them knowing and ran away with it", "The hot teacher was impressed by your painting and went down their knees to ask you on a date" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Cook something sweet",
            new string[] { "You got distracted learning a choreography, and burnt the cake in the oven", "You got the quantities wrong, so now you got cookies for a whole week", "Your cute and silly neighbour smelt your cookies and you invited them to stay the night" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go to a DJ workshop",
            new string[] { "Never in your live have you been surrounded by so many monkey dudes that you had to ran away crying", "You unplugged the mixing desk while nobody was watching and use the distraction to fill your pockets with the telephones of the annoying dudes", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Clean the house",
            new string[] { "You have to fought a spider's nest and couldn't end cleaning", "You got distracted while cleaning with things you didn't remember having and found money under the bed", "You feel life is worth living again, everything is in order and so is your life" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Call mom and listen to her complains",
            new string[] { "You ended up yelling at each other because of a misunderstanding", "You left the telephone in the room while you showered and when you went back she was still talking alone", "You realised you have grown old just as your mother, and although she is guilty of part of your pain, you understand her" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Write fanfics with the lights off",
            new string[] { "A screenshot of your fic went viral on Twitter and everybody made fun of you", "The omegaverse fans praised your fic and made you an internet sensation", "'The Lovely Adventures of Paquita and Fernandito' became an internet sensation and they offered you a publishing contract" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go vintage shopping with a cute hat",
            new string[] { "Nobody complemented your avant garde hat and you bought a scabies dress without knowing", "You found the perfect skirt to match your extravagant hat", "An alternative hot looking person complemented your outfit and asked for your number" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Listen to a sad playlist",
            new string[] { "The neighbours heard your laments and called the police to see if something was wrong", "You cried in silence all night long and wake up refreshed and thinking life is about the ups and downs", "You tweeted how much you liked a certain sad singer song, and they replied and asked you on a date" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go to a rodeo bar",
            new string[] { "Your legs hurt so much that you had to exit crawling", "You bonded with the lonely ladies in the corner and started dancing together till the floor fell down", "You fell off the horse straight into the strong arms of a cowboy and ride all night long" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f)
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
        Debug.Log("Inicia");
        final_outcomes = new string[3];

        RandomizeActivities(activities_mixed);
        RandomizeActivities(activities_romantic_NotPartner);
        RandomizeActivities(activities_romantic_Partner);

        WriteDailyActivities();
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

    private void WriteDailyActivities()
    {
        // Mixed Actions
        for (int i = 0; i < 7; i++)
        {
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

    public void ActivitiesOutcomes()
    {
        final_outcomes[0] = activities_selected[0].activityInfo.outcomes[2];
        final_outcomes[1] = activities_selected[1].activityInfo.outcomes[2];
        final_outcomes[2] = activities_selected[2].activityInfo.outcomes[2];

        UIManager.Instance.WriteActivityOutcomes_UI(final_outcomes);
    }


}
