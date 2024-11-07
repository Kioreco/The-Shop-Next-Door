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

    [HideInInspector] public ActivityInfo[] activities_mixed;
    [HideInInspector] public ActivityInfo[] activities_romantic;
    [HideInInspector] public ActivityInfo[] activities_partner;

    #region JSON-ish

    [System.Serializable]
    public class ActivityList
    {
        public ActivityInfo[] activitiesJSON;
    }

    private ActivityInfo[] LoadActivitiesFromJson(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Activities/" + fileName);
        if (jsonFile == null)
        {
            Debug.LogWarning("JSON file not found");
            return null;
        }

        ActivityList activityList = JsonUtility.FromJson<ActivityList>(jsonFile.text);
        print(activityList);
        print(activityList.activitiesJSON);
        return activityList.activitiesJSON;
    }
    #endregion


    #region Activities List 

    //private ActivityInfo[] activities_mixed = new ActivityInfo[] {
    //    new("Go for a walk with a book that you won't read", 
    //        new string[] { "I got interviewed by a TikToker and the Internet made fun of me :(", "I didn't remember to take out the book of my tote bag, but someone gifted me a massage coupon!!", "A cute person sat next to me and asked for books recommendations... and my number!!!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.4f,
    //        0.1f,
    //        0.2f,
    //        0.6f,
    //        0.62f,
    //        false,
    //        true),
    //    new("Do yoga",
    //        new string[] { "I tried doing the Astavakrasana but my hand slipped and it hurt so much :,(", "I believe I reached the Nirvana and now I don't have problems sleeeping!!!", "I became intimate with the modern moms and they gifted me a pricey necklace!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.1f,
    //        0.45f,
    //        0.65f,
    //        0.7f,
    //        0.8f,
    //        false,
    //        false),
    //    new("Go to the gym",
    //        new string[] { "The treadmill outruned me and I fell in a ridiculous way to the floor :(", "The gym was empty and I could blast out loud Taylor Swift without any judgement from the gymbros", "I lifted more weight than the noisy gymbro and make him cry!!!!!!!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.18f,
    //        0.25f,
    //        0.66f,
    //        0.2f,
    //        0.1f,
    //        false,
    //        false),
    //    new("Go shopping with friends",
    //        new string[] { "There was nothing cute of my size and I started crying in the middle of the shopping center", "I exited one shop without paying and nobody noticed... oops!", "Everything I wanted was half-priced and I even got gifted a curious book!!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.13f,
    //        0.9f,
    //        0.3f,
    //        0.42f,
    //        0.33f,
    //        false,
    //        false),
    //    new("Binge watch Gilmore Girls",
    //        new string[] { "I was crying so bad watching Lorelai and Rory love each other so much that the pizza on the oven got burn", "I wanted to be like Rory so you read an intellectual book in one sitting", "I got moved so much by the relation of Emily and Lorelai that I called my mother to ask how is her life" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.3f,
    //        0.54f,
    //        1.0f,
    //        1.0f,
    //        0.9f,
    //        false,
    //        false),
    //    new("Go to an art workshop",
    //        new string[] { "I mistook my glass of wine and drank the dirty water instead... IT WAS HORRIBLE!", "I ended with my hands lilac, stained the t-shirt of the person next to me without them knowing and ran away with it!!", "The hot teacher was impressed by my painting and went down their knees to ask me on a date!!!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.55f,
    //        0.23f,
    //        0.34f,
    //        0.68f,
    //        0.65f,
    //        false,
    //        true),
    //    new("Cook something sweet",
    //        new string[] { "I got distracted learning a choreography, and burnt the cake in the oven :(", "I got the quantities wrong, so now I got cookies for a whole week!", "My cute and silly neighbour smelt my cookies and I invited them to stay the night..." },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.43f,
    //        0.1f,
    //        0.39f,
    //        0.75f,
    //        0.55f,
    //        false,
    //        true),
    //    new("Go to a DJ workshop",
    //        new string[] { "Never in my life have I been surrounded by so many monkey dudes that I had to ran away crying!!", "I unplugged the mixing desk while nobody was watching and use the distraction to fill my pockets with the telephones of the annoying dudes... oops!", "I got everyone to burn down the dance floor to the beat of Sophie!! I was born to do this!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.6f,
    //        0.4f,
    //        0.05f,
    //        0.55f,
    //        0.23f,
    //        false,
    //        false),
    //    new("Clean the house",
    //        new string[] { "I had to fought a spider's nest and couldn't end cleaning :( IT WAS HORRIBLE!", "I got distracted while cleaning with things I didn't remember having and found money under the bed!", "I feel life is worth living again, everything is in order and so is my life!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.0f,
    //        0.3f,
    //        1.0f,
    //        1.0f,
    //        0.9f,
    //        false,
    //        false),
    //    new("Call mom and listen to her complains",
    //        new string[] { "We ended up yelling at each other because of a misunderstanding!! HATE HER!!", "I left the telephone in the room while I showered and when I went back she was still talking alone... sorry not sorry!", "I realised I have grown old just as my mother, and although she is guilty of part of my pain, I understand her..." },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.0f,
    //        0.7f,
    //        1.0f,
    //        0.4f,
    //        0.5f,
    //        false,
    //        false),
    //    new("Write fanfics with the lights off",
    //        new string[] { "A screenshot of my fic went viral on Twitter and everybody made fun of me!!", "The omegaverse fans praised my fic and made me an internet sensation... Yuhu! I guess?", "'The Lovely Adventures of Paquita and Fernandito' became an internet sensation and they offered me a publishing contract!!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.64f,
    //        0.82f,
    //        0.86f,
    //        0.97f,
    //        0.98f,
    //        false,
    //        false),
    //    new("Go vintage shopping with a cute hat",
    //        new string[] { "Nobody complemented my avant garde hat and I bought a scabies dress without knowing! WORST EXPERIENCE EVER!", "I found the perfect skirt to match my extravagant hat! Life is worth living!", "An alternative hot looking person complemented my outfit and asked for my number!!!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.57f,
    //        0.59f,
    //        0.48f,
    //        0.74f,
    //        0.82f,
    //        false,
    //        true),
    //    new("Listen to a sad playlist",
    //        new string[] { "The neighbours heard my laments and called the police to see if something was wrong... EMBARRASSING!!", "I cried in silence all night long and wake up refreshed and thinking life is about the ups and the downs", "I tweeted how much I liked a certain sad singer song, and they replied and asked me on a date! Feeling like Paul Mescal rn..." },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.3f,
    //        0.2f,
    //        0.96f,
    //        0.25f,
    //        0.87f,
    //        false,
    //        true),
    //    new("Watch Twilight with your friends",
    //        new string[] { "My friends were #TeamJacob (ew), so I fought with them! They are never entering my house till they rethink it!", "I dreamt a hot vampire lured me into the dark and now I wait every night by the window...", "We all bonded talking and dreaming about Edward Cullen (Edward, if you ever read this, call me up!!)" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.61f,
    //        0.98f,
    //        0.48f,
    //        0.81f,
    //        0.89f,
    //        false,
    //        false),
    //    new("Go to a rodeo bar",
    //        new string[] { "My legs hurt so much that I had to exit crawling!!!", "I bonded with the lonely ladies in the corner and started dancing together till the floor fell down!!!", "I fell off the horse straight into the strong arms of a cowboy and ride all night long... yeah!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.99f,
    //        0.59f,
    //        0.22f,
    //        0.76f,
    //        0.31f,
    //        false,
    //        true)
    //};

    //private ActivityInfo[] activities_partner = new ActivityInfo[] {
    //    new("Go to a museum date with partner",
    //        new string[] { "We got into a fight about which was Botticelli best painting and now we don't talk!", "Event though it's forbiden, we managed to take pics with every painting in the museum!", "THEY KNEEL INTO THE FLOOR IN FRONT OF THE BIRTH OF VENUS AND PROPOSED!!! I'm the luckiest!! My sister is going to be so envious!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.92f,
    //        0.3f,
    //        0.43f,
    //        0.88f,
    //        0.92f,
    //        true,
    //        true),
    //    new("Ask partner if they will love me if I was a bug",
    //        new string[] { "My partner said they would bury me and trample me!! He doesn't like ugly bugs at all! THIS IS THE END!", "My partner said that they don't like bugs, but he would find a cure to my curse!", "My partner said they would love me on every lifetime! They would become a bug just to love me and nurture me!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.79f,
    //        0.31f,
    //        0.38f,
    //        0.47f,
    //        0.66f,
    //        true,
    //        true),
    //    new("Have a picnic with partner",
    //        new string[] { "I made the mistake of wearing a skirt... And it was all full of ants!! Memories of Vietnam!!! HATE IT!", "Standing in the towel wasn't very comfortable, but my partner hold me in their arms caressing my hair!", "The food was perfect! The place was perfect! AND THEY PROPOSED!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.87f,
    //        0.26f,
    //        0.42f,
    //        0.85f,
    //        0.72f,
    //        true,
    //        false),
    //    new("Ask partner if they love The Smiths",
    //        new string[] { "THEY SHOUTED ME! My partner said they hate The Smiths and I had to end it all!", "My partner said they've never heard of that indie band, but they will do it for me!", "MY PARTNER SAID THEY LOVE THE SMITHS!!! I knew I could trust them!!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.93f,
    //        0.26f,
    //        0.73f,
    //        0.76f,
    //        0.7f,
    //        true,
    //        true),
    //    new("Have a tematic date with partner",
    //        new string[] { "The vibes were totally off... I wanted them to dress up as a Priest and they wanted to be a pirate! Illiterate!", "It was good I guess... Life is not like the films, but I love them still!", "We dressed up as Fleabag and the Priest! It was wonderfull! I hope this love won't pass..." },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.89f,
    //        0.3f,
    //        0.45f,
    //        0.92f,
    //        0.53f,
    //        true,
    //        true),
    //    new("Visit my partner hometown",
    //        new string[] { "We met their ex... They are more beautiful than me... I HATE IT! I HATE ME!", "Their hometown was sooo boring... But they talked about it with so much love!", "Their hometown was good, so good really... Maybe we should buy a house" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.94f,
    //        0.4f,
    //        0.68f,
    //        0.52f,
    //        0.71f,
    //        true,
    //        false),
    //    new("Make partner watch Gilmore Girls with you",
    //        new string[] { "My partner couldn't even finish the first chapter! I HATE THEM! I blocked their number!", "We watched the first season in one seating... I wanted more but they were really sleepy", "We talk about the complexities of human relationships and how well Gilmore Girls portrays them" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.9f,
    //        0.6f,
    //        0.94f,
    //        0.89f,
    //        1.0f,
    //        true,
    //        true),
    //};

    //private ActivityInfo[] activities_romantic = new ActivityInfo[] {
    //    new("Go to a private party",
    //        new string[] { "I got puke on my favourite t-shirt :(", "I ended with a precious ring in my pocket and accidentally sold it on a black-marketplace... oops!", "A very cute person stayed all night by my side and we went to talk to a more intimate place about life and the stars... Life sometimes is like a movie!"  },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.75f,
    //        0.85f,
    //        0.23f,
    //        0.47f,
    //        0.12f,
    //        false,
    //        true),
    //    new("Go on a date with someone from a dating app",
    //        new string[] { "It was weird... They didn't ask a single question! I wasted my time...", "They didn't look like their profile AT ALL, so I ran away before they saw me!", "They were as nervous as me, and somehow that made it lovelier! I'm going to ask them on a second date!!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.58f,
    //        0.48f,
    //        0.36f,
    //        0.15f,
    //        0.22f,
    //        false,
    //        true),
    //    new("Go to a café alone and act mysterious",
    //        new string[] { "I spilled the coffee all over my dress! I felt pathetic!", "I sat in the café thinking all love ever does is break and burn, and end...", "I was about to slip with my coffee but a very handsome person caught me in the air and the made me company!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.92f,
    //        0.53f,
    //        0.61f,
    //        0.72f,
    //        0.84f,
    //        false,
    //        true),
    //    new("Invite someone over",
    //        new string[] { "I stayed by the door all night long and they never came... I hate it here!", "I tried inviting my crush, but they couldn't come... But my friends came to cheer me up!", "None of my friends could come so I went to the nearest Chinese food store and found my crush and I invited them to come! It was great!!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.97f,
    //        0.66f,
    //        0.42f,
    //        0.47f,
    //        0.76f,
    //        false,
    //        true),
    //    new("Prepare a fancy dinner and lay by the phone",
    //        new string[] { "It was a disaster! The food tasted horrible and nobody texted me! I feel like dark tumblr 2014 posts...", "I'm starting to believe my phone is broken, because i didn't receive any messages! At least the food was really good I guess...", "I almost burnt the kitchen down! But I was fast and called the firefighters and a handsome and strong one came to help extinguish it and asked me on a date!!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.66f,
    //        0.52f,
    //        0.6f,
    //        0.72f,
    //        0.84f,
    //        false,
    //        true),
    //    new("Go to the town disco",
    //        new string[] { "Every popular person in high school was there, and let me tell you they aged like rotten milk! I couldn't stand it so I left early", "I have never seen a disco more old-fashioned and boring! Luckily someone bought a bottle of champagne and forgot about it!", "I crashed into my high school sweetheart and they confesed they had a crush on me back then!! They are still the most beautiful person I have ever seen!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.37f,
    //        0.68f,
    //        0.21f,
    //        0.26f,
    //        0.13f,
    //        false,
    //        true),
    //    new("Go to blind date",
    //        new string[] { "I couldn't believe it when I saw them showing up in THAT outfit... It seems that it was really a BLIND date...", "It was not a match I fear... But at least the food and the place was really good!", "I think I fell in love the moment I saw them showing up with a Gilmore Girls t-shirt, listening to Taylor Swift and saying they love The Smiths!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.76f,
    //        0.59f,
    //        0.42f,
    //        0.46f,
    //        0.58f,
    //        false,
    //        true),
    //    new("Read a pretentious book in the park",
    //        new string[] { "Now I understand why is called pretentious... It was awful! Never reading anything by a man ever again", "Did you know the father's DNA stays inside the mother for seven years? Now I don't think I want to have children...", "A cute person sat next to me in the bench and we both read peacefully till the sun set up... When I was about to leave, they said they waited all afternoon to ask for my number!" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f},
    //        0.88f,
    //        0.44f,
    //        0.94f,
    //        0.62f,
    //        0.87f,
    //        false,
    //        true),
    //    new("Go to dance classes",
    //        new string[] { "Everyone there was old! VERY OLD! And they kept stepping on my feet!!", "I thought they were going to be romantic, but they ended to be a bunch of moms dancing together and gossiping... I LOVE IT!", "I was out of breath when my assigned partner looked sculpted by the angels! I assure you that the rest of the afternoon was pure FUEGO" },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f },
    //        new float[] {1.0f, 1.0f, 1.0f, 1.0f },
    //        0.82f,
    //        0.71f,
    //        0.69f,
    //        0.56f,
    //        0.58f,
    //        false,
    //        true),
    //};
    #endregion


    private void Awake()
    {
        //activities_mixed = LoadActivitiesFromJson("activities_mixed");
        //activities_romantic = LoadActivitiesFromJson("activities_romantic");
        //activities_partner = LoadActivitiesFromJson("activities_partner");
        ActivityLoader loader = new ActivityLoader();
        loader.LoadActivities();

        RandomizeActivities(activities_mixed);
        RandomizeActivities(activities_romantic);
        RandomizeActivities(activities_partner);

        WriteDailyActivities();
    }

    private void RandomizeActivities(ActivityInfo[] list)
    {
        Debug.Log("Lista" + list);
        Debug.Log("Lista length" + list.Length);
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
        for (int i = 0; i < 5; i++)
        {
            activities_text[i].SetText(activities_mixed[i].activityName);
            activities_daily[i].CopyActivity(activities_mixed[i]);
        }
        int j = 5;
        if (VidaPersonalManager.Instance.hasPartner)
        {
            // Romantic Actions
            for (int i = 0; i < 1; i++)
            {
                //int randomAction = Random.Range(0, activities_romantic_Partner.Count);
                activities_text[j].SetText(activities_partner[i].activityName);
                activities_daily[j].CopyActivity(activities_partner[i]);
                j++;
            }
        }
        else
        {
            for (int i = 0; i < 1; i++)
            {
                //int randomAction = Random.Range(0, activities_romantic_NotPartner.Count);
                activities_text[j].SetText(activities_romantic[i].activityName);
                activities_daily[j].CopyActivity(activities_romantic[i]);
                j++;
            }
        }
    }

    float Daily_RomanticProgress;
    float Daily_FriendshipProgress;
    float Daily_DevelopmentProgress;
    float Daily_HappinessProgress;
    float Daily_RestProgress;


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
            if (activities_selected[i].activityInfo.activityName == "") return;

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

            UpdateDailyPartialActivityProgress(i, outcomeWeighted);

            final_outcomes[i] = activities_selected[i].activityInfo.outcomes[outcomeWeighted];
        }

        VidaPersonalManager.Instance.UpdateLifeProgress(activitiesFilled, Daily_RomanticProgress, Daily_FriendshipProgress, Daily_DevelopmentProgress, Daily_HappinessProgress, Daily_RestProgress);
        UIManager.Instance.WriteActivityOutcomes_UI(final_outcomes);
    }

    float discountValue = 0f;

    private void UpdateDailyPartialActivityProgress(int actNumber, int outcome)
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

    public void UpdateDayCalendar(string day)
    {
        weekDay_text.SetText(day);
    }
}






//final_outcomes[0] = activities_selected[0].activityInfo.outcomes[2];


//if (activities_selected[0] != null)
//{
//    final_outcomes[0] = activities_selected[0].activityInfo.outcomes[2];
//}
//else
//{
//    final_outcomes[0] = "";
//}

//if(activities_selected[1] != null)
//{
//    final_outcomes[1] = activities_selected[1].activityInfo.outcomes[2];
//}
//else
//{
//    final_outcomes[1] = "";
//}

//if (activities_selected[2] != null)
//{
//    final_outcomes[2] = activities_selected[2].activityInfo.outcomes[2];
//}
//else
//{
//    final_outcomes[2] = "";
//}