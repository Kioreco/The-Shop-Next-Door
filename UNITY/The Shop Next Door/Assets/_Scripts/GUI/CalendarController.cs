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
    public string[] final_outcomes;

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
            new string[] { "I got interviewed by a TikToker and the Internet made fun of me :(", "I didn't remember to take out the book of my tote bag, but someone gifted me a massage coupon!!", "A cute person sat next to me and asked for books recommendations... and my number!!!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.4f,
            0.1f,
            0.2f,
            0.6f,
            0.62f),
        new("Do yoga",
            new string[] { "I tried doing the Astavakrasana but my hand slipped and it hurt so much :,(", "I believe I reached the Nirvana and now I don't have problems sleeeping!!!", "I became intimate with the modern moms and they gifted me a pricey necklace!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.1f,
            0.45f,
            0.65f,
            0.7f,
            0.8f),
        new("Go to the gym",
            new string[] { "The treadmill outruned me and I fell in a ridiculous way to the floor :(", "The gym was empty and I could blast out loud Taylor Swift without any judgement from the gymbros", "I lifted more weight than the noisy gymbro and make him cry!!!!!!!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.18f,
            0.25f,
            0.66f,
            0.2f,
            0.1f),
        new("Go shopping with friends",
            new string[] { "There was nothing cute of my size and I started crying in the middle of the shopping center", "I exited one shop without paying and nobody noticed... oops!", "Everything I wanted was half-priced and I even got gifted a curious book!!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.13f,
            0.9f,
            0.3f,
            0.42f,
            0.33f),
        new("Binge watch Gilmore Girls",
            new string[] { "I was crying so bad watching Lorelai and Rory love each other so much that the pizza on the oven got burn", "I wanted to be like Rory so you read an intellectual book in one sitting", "I got moved so much by the relation of Emily and Lorelai that I called my mother to ask how is her life" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.3f,
            0.54f,
            1.0f,
            1.0f,
            0.9f),
        new("Go to an art workshop",
            new string[] { "I mistook my glass of wine and drank the dirty water instead... IT WAS HORRIBLE!", "I ended with my hands lilac, stained the t-shirt of the person next to me without them knowing and ran away with it!!", "The hot teacher was impressed by my painting and went down their knees to ask me on a date!!!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Cook something sweet",
            new string[] { "I got distracted learning a choreography, and burnt the cake in the oven :(", "I got the quantities wrong, so now I got cookies for a whole week!", "My cute and silly neighbour smelt my cookies and I invited them to stay the night..." },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go to a DJ workshop",
            new string[] { "Never in my life have I been surrounded by so many monkey dudes that I had to ran away crying!!", "I unplugged the mixing desk while nobody was watching and use the distraction to fill my pockets with the telephones of the annoying dudes... oops!", "I ended with a precious ring in my pocket and accidentally sold it on the black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Clean the house",
            new string[] { "I had to fought a spider's nest and couldn't end cleaning :( IT WAS HORRIBLE!", "I got distracted while cleaning with things I didn't remember having and found money under the bed!", "I feel life is worth living again, everything is in order and so is my life!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Call mom and listen to her complains",
            new string[] { "We ended up yelling at each other because of a misunderstanding!! HATE HER!!", "I left the telephone in the room while I showered and when I went back she was still talking alone... sorry not sorry!", "I realised I have grown old just as my mother, and although she is guilty of part of my pain, I understand her..." },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Write fanfics with the lights off",
            new string[] { "A screenshot of my fic went viral on Twitter and everybody made fun of me!!", "The omegaverse fans praised my fic and made me an internet sensation... Yuhu! I guess?", "'The Lovely Adventures of Paquita and Fernandito' became an internet sensation and they offered me a publishing contract!!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go vintage shopping with a cute hat",
            new string[] { "Nobody complemented my avant garde hat and I bought a scabies dress without knowing! WORST EXPERIENCE EVER!", "I found the perfect skirt to match my extravagant hat! Life is worth living!", "An alternative hot looking person complemented my outfit and asked for my number!!!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Listen to a sad playlist",
            new string[] { "The neighbours heard my laments and called the police to see if something was wrong... EMBARRASSING!!", "I cried in silence all night long and wake up refreshed and thinking life is about the ups and the downs", "I tweeted how much I liked a certain sad singer song, and they replied and asked me on a date! Feeling like Paul Mescal rn..." },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Watch Twilight with your friends",
            new string[] { "My friends were #TeamJacob (ew), so I fought with them! They are never entering my house till they rethink it!", "I dreamt a hot vampire lured me into the dark and now I wait every night by the window...", "We all bonded talking and dreaming about Edward Cullen (Edward, if you ever read this, call me up!!)" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go to a rodeo bar",
            new string[] { "My legs hurt so much that I had to exit crawling!!!", "I bonded with the lonely ladies in the corner and started dancing together till the floor fell down!!!", "I fell off the horse straight into the strong arms of a cowboy and ride all night long... yeah!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f)
    };

    private ActivityInfo[] activities_romantic_Partner = new ActivityInfo[] {
        new("Go to a museum date with partner",
            new string[] { "We got into a fight about which was Botticelli best painting and now we don't talk!", "Event though it's forbiden, we managed to take pics with every painting in the museum!", "THEY KNEEL INTO THE FLOOR IN FRONT OF THE BIRTH OF VENUS AND PROPOSED!!! I'm the luckiest!! My sister is going to be so envious!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Ask partner if they will love me if I was a bug",
            new string[] { "My partner said they would bury me and trample me!! He doesn't like ugly bugs at all! THIS IS THE END!", "My partner said that they don't like bugs, but he would find a cure to my curse!", "My partner said they would love me on every lifetime! They would become a bug just to love me and nurture me!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Have a picnic with partner",
            new string[] { "I made the mistake of wearing a skirt... And it was all full of ants!! Memories of Vietnam!!! HATE IT!", "Standing in the towel wasn't very comfortable, but my partner hold me in their arms caressing my hair!", "The food was perfect! The place was perfect! AND THEY PROPOSED!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Ask partner if they love The Smiths",
            new string[] { "THEY SHOUTED ME! My partner said they hate The Smiths and I had to end it all!", "My partner said they've never heard of that indie band, but they will do it for me!", "MY PARTNER SAID THEY LOVE THE SMITHS!!! I knew I could trust them!!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Have a tematic date with partner",
            new string[] { "The vibes were totally off... I wanted them to dress up as a Priest and they wanted to be a pirate! Illiterate!", "It was good I guess... Life is not like the films, but I love them still!", "We dressed up as Fleabag and the Priest! It was wonderfull! I hope this love won't pass..." },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Visit my partner hometown",
            new string[] { "We met their ex... They are more beautiful than me... I HATE IT! I HATE ME!", "Their hometown was sooo boring... But they talked about it with so much love!", "Their hometown was good, so good really... Maybe we should buy a house" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Make partner watch Gilmore Girls with you",
            new string[] { "My partner couldn't even finish the first chapter! I HATE THEM! I blocked their number!", "We watched the first season in one seating... I wanted more but they were really sleepy", "We talk about the complexities of human relationships and how well Gilmore Girls portrays them" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
    };

    private ActivityInfo[] activities_romantic_NotPartner = new ActivityInfo[] {
        new("Go to a private party",
            new string[] { "I got puke on my favourite t-shirt :(", "I ended with a precious ring in my pocket and accidentally sold it on a black-marketplace... oops!", "A very cute person stayed all night by my side and we went to talk to a more intimate place about life and the stars... Life sometimes is like a movie!"  },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go on a date with someone from a dating app",
            new string[] { "It was weird... They didn't ask a single question! I wasted my time...", "They didn't look like their profile AT ALL, so I ran away before they saw me!", "They were as nervous as me, and somehow that made it lovelier! I'm going to ask them on a second date!!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go to a café alone and act mysterious",
            new string[] { "I spilled the coffee all over my dress! I felt pathetic!", "I sat in the café thinking all love ever does is break and burn, and end...", "I was about to slip with my coffee but a very handsome person caught me in the air and the made me company!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Invite someone over",
            new string[] { "I stayed by the door all night long and they never came... I hate it here!", "I tried inviting my crush, but they couldn't come... But my friends came to cheer me up!", "None of my friends could come so I went to the nearest Chinese food store and found my crush and I invited them to come! It was great!!" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Prepare a fancy dinner and lay by the phone",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go to the town disco",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go to blind date",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Read a pretentious book in the park",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f},
            0.69f,
            0.54f,
            0.12f,
            0.35f,
            0.1f),
        new("Go to dance classes",
            new string[] { "You met a cute person", "You got puke on your favourite t-shirt", "You ended with a precious ring in your pocket and accidentally sold it on a black-marketplace" },
            new float[] {1.0f, 1.0f, 1.0f, 1.0f },
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
        final_outcomes = new string[3];

        if (activities_selected[0] != null)
        {
            final_outcomes[0] = activities_selected[0].activityInfo.outcomes[2];
        }
        else
        {
            final_outcomes[0] = "";
        }

        if(activities_selected[1] != null)
        {
            final_outcomes[1] = activities_selected[1].activityInfo.outcomes[2];
        }
        else
        {
            final_outcomes[1] = "";
        }

        if (activities_selected[2] != null)
        {
            final_outcomes[2] = activities_selected[2].activityInfo.outcomes[2];
        }
        else
        {
            final_outcomes[2] = "";
        }


        UIManager.Instance.WriteActivityOutcomes_UI(final_outcomes);
    }

    public void UpdateDayCalendar(string day)
    {
        weekDay_text.SetText(day);
    }


}
