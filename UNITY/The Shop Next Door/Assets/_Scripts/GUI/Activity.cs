using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity : MonoBehaviour
{
    public string activityName { get; set; }
    public string[] outcomes { get; set; }

    public float occurrenceProbability { get; set; }
    public float statesLuck { get; set; }

    public float romanticProbability { get; set; }
    public float friendshipProbability { get; set; }
    public float personalDevelopmentProbability { get; set; }
    public float happinessProbability { get; set; }
    public float restingProbability { get; set; }

    public Activity(string actName, string[] outc, float ocProb, float stLuck, float romProb, float friendProb, float persProb, float hapProb, float restProb)
    {
        activityName = actName;
        outcomes = outc;
        occurrenceProbability = ocProb;
        statesLuck = stLuck;
        romanticProbability = romProb;
        friendshipProbability = friendProb;
        personalDevelopmentProbability = persProb;
        happinessProbability = hapProb;
        restingProbability = restProb;
    }

    public void CopyActivity(Activity act)
    {
        activityName = act.activityName;
        outcomes = act.outcomes;
        occurrenceProbability = act.occurrenceProbability;
        statesLuck = act.statesLuck;
        romanticProbability = act.romanticProbability;
        friendshipProbability = act.friendshipProbability;
        personalDevelopmentProbability = act.personalDevelopmentProbability;
        happinessProbability = act.happinessProbability;
        restingProbability = act.restingProbability;
    }

}
