using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity : MonoBehaviour
{
    public ActivityInfo activityInfo = new();

    //public void CreateActivityInfo(string actName, string[] outc, float ocProb, float stLuck, float romProb, float friendProb, float persProb, float hapProb, float restProb)
    //{
    //    activityInfo.activityName = actName;
    //    activityInfo.outcomes = outc;
    //    activityInfo.occurrenceProbability = ocProb;
    //    activityInfo.statesLuck = stLuck;
    //    activityInfo.romanticProbability = romProb;
    //    activityInfo.friendshipProbability = friendProb;
    //    activityInfo.personalDevelopmentProbability = persProb;
    //    activityInfo.happinessProbability = hapProb;
    //    activityInfo.restingProbability = restProb;
    //}

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



}
