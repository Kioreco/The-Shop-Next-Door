[System.Serializable]
public class ActivityInfo 
{
    public string activityName;
    public string[] outcomes;

    public float[] ClimateStateLuck;
    public float[] PersonalStateLuck;

    public float romanticProbability;
    public float friendshipProbability;
    public float personalDevelopmentProbability;
    public float happinessProbability;
    public float restingProbability;

    public bool isRomanticPartner;
    public bool givesPartner;
    public bool breakPartner;


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
