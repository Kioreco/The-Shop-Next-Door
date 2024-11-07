using UnityEngine;

[System.Serializable]
public class ActivityLoader
{
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
        Debug.Log(activityList);
        Debug.Log(activityList.activitiesJSON);
        return activityList.activitiesJSON;
    }

    public void LoadActivities()
    {
        UIManager.Instance.telephone.calendar.activities_mixed = LoadActivitiesFromJson("activities_mixed");
        UIManager.Instance.telephone.calendar.activities_romantic = LoadActivitiesFromJson("activities_romantic");
        UIManager.Instance.telephone.calendar.activities_partner = LoadActivitiesFromJson("activities_partner");
    }
}
