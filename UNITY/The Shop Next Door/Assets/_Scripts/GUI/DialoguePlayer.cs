
public class DialoguePlayer
{
    public string[] clientsName = new string[] { "Karen", "Antonia", "Roberto", "Pepito", "Agustina", "Manolito", "Elvirita", "Luke", "Lorelai", "Rory", "Emily", "Flora", "Edward" };

    public string[] dialogosBienvenida = new string[]
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
    "Well, after much thought—and by thought, I mean thorough snooping—this week’s winner is none other than {winningSister}! A round of applause for my golden child, please",
    "Let’s not drag this out, ladies. The winner this week, as clear as my perfect vision, is {winningSister}! Congratulations, my dear, you’ve made your mother proud",
    "I have to say, the decision was obvious. This week, {winningSister} takes the crown! Keep making me look good, darling",
    "This week, the daughter who stole the show and her mother’s heart is {winningSister}! Well done, darling. Keep this up, and I might just rewrite my will!",
    "After reviewing all the results and, of course, considering my impeccable judgment, this week’s champion is {winningSister}! Now, let’s celebrate your brilliance",
    "After much deliberation—and a little bit of gossip with the neighbors—this week’s winner is {winningSister}! I knew you had it in you, my brilliant girl",
    "There’s only room for one winner in this family, besides me of course, and this week, it’s {winningSister}! Now, don’t get too comfortable, you got work to do!",
    "Well, well, well. {winningSister}, you’ve done it again. My Shop... I mean, your shop, is practically glowing! And you made sure to make mother Telma proud!",
    "You’ve shown ambition, skill, and just the right amount of charm. {winningSister}, you’re this week’s winner—and my favorite, at least for now",
    "I couldn’t be prouder if I tried. {winningSister}, you’ve earned this win. Keep it up, and I might just consider giving you the family heirlooms someday"
    };
    public string[] dialogoResultadoEmpate = new string[]
    {
    "Well, this is unexpected… a tie! I suppose both my daughters are brilliant—just this once. Don’t let it happen again; I like clear winners!",
    "Ladies, we’ve reached a rare event in history: an actual tie. Looks like you’ll both have to share the spotlight this week. Try not to fight about it!",
    "What a dilemma! You’ve both done so well this week that I simply can’t choose. A tie it is—enjoy the glory, both of you!",
    "Looks like you’re evenly matched this week. A tie! I suppose I should be proud, but honestly, I expected one of you to pull ahead",
    "It seems my daughters are more alike than I thought. A tie! Let’s hope next week one of you can truly stand out...",
    "I’ve crunched the numbers, asked the neighbors, and even consulted the cat… and it’s a tie! Well done, I suppose...",
    "A tie? Really? I must admit, I didn’t see this coming. I’ll let you both bask in your shared victory, but don’t make this a habit...",
    "Well, isn’t this surprising? A tie! I guess it means double the pride for me, but let’s aim for a winner next time..."
    };


}
