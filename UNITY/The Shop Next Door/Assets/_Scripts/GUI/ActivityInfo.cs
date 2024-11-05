

public class ActivityInfo 
{
    public string activityName { get; set; }
    public string[] outcomes { get; set; }

    public float[] statesLuck { get; set; }

    public float romanticProbability { get; set; }
    public float friendshipProbability { get; set; }
    public float personalDevelopmentProbability { get; set; }
    public float happinessProbability { get; set; }
    public float restingProbability { get; set; }

    public ActivityInfo(string actName, string[] outc, float[] stLuck, float romProb, float friendProb, float persProb, float hapProb, float restProb)
    {
        activityName = actName;
        outcomes = outc;
        statesLuck = stLuck;
        romanticProbability = romProb;
        friendshipProbability = friendProb;
        personalDevelopmentProbability = persProb;
        happinessProbability = hapProb;
        restingProbability = restProb;
    }

    public ActivityInfo()
    {
        activityName = "";
        outcomes = new string[3] {" ", " ", " "};
        statesLuck = new float[] { };
        romanticProbability = 0.0f;
        friendshipProbability = 0.0f;
        personalDevelopmentProbability = 0.0f;
        happinessProbability = 0.0f;
        restingProbability = 0.0f;
    }

}
