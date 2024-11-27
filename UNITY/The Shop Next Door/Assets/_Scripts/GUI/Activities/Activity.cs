using UnityEngine;

[System.Serializable]
public class Activity : MonoBehaviour
{
    public ActivityInfo activityInfo = new();

    public void CopyActivity(ActivityInfo act)
    {
        activityInfo.activityName = act.activityName;
        activityInfo.outcomes = act.outcomes;
        activityInfo.ClimateStateLuck = act.ClimateStateLuck;
        activityInfo.romanticProbability = act.romanticProbability;
        activityInfo.friendshipProbability = act.friendshipProbability;
        activityInfo.personalDevelopmentProbability = act.personalDevelopmentProbability;
        activityInfo.happinessProbability = act.happinessProbability;
        activityInfo.restingProbability = act.restingProbability;
    }

    public void ResetActivity()
    {
        activityInfo = new ActivityInfo();
    }
}
