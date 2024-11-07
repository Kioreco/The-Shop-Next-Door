

using Unity.VisualScripting.Antlr3.Runtime;

public class ActivityInfo 
{
    public string activityName { get; set; }
    public string[] outcomes { get; set; }

    public float[] ClimateStateLuck { get; set; }
    public float[] PersonalStateLuck { get; set; }

    public float romanticProbability { get; set; }
    public float friendshipProbability { get; set; }
    public float personalDevelopmentProbability { get; set; }
    public float happinessProbability { get; set; }
    public float restingProbability { get; set; }

    public bool isRomanticPartner { get; set; }
    public bool givesPartner { get; set; }
    public bool breakPartner { get; set; }

    public ActivityInfo(string actName, string[] outc, float[] stClimateLuck, float[] stPersonalLuck, float romProb, float friendProb, float persProb, float hapProb, float restProb, bool romanticPartner, bool partner)
    {
        activityName = actName;
        outcomes = outc;
        ClimateStateLuck = stClimateLuck;
        PersonalStateLuck = stPersonalLuck;
        romanticProbability = romProb;
        friendshipProbability = friendProb;
        personalDevelopmentProbability = persProb;
        happinessProbability = hapProb;
        restingProbability = restProb;
        isRomanticPartner = romanticPartner;
        if (isRomanticPartner)
        {
            breakPartner = partner;
            givesPartner = false;
        }
        else
        {
            breakPartner = false;
            givesPartner = partner;
        }
    }

    public ActivityInfo()
    {
        activityName = "";
        outcomes = new string[3] {" ", " ", " "};
        ClimateStateLuck = new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};
        PersonalStateLuck = new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};
        romanticProbability = 0.0f;
        friendshipProbability = 0.0f;
        personalDevelopmentProbability = 0.0f;
        happinessProbability = 0.0f;
        restingProbability = 0.0f;
        isRomanticPartner = false;
        givesPartner = false;
        breakPartner = false;
    }

}
