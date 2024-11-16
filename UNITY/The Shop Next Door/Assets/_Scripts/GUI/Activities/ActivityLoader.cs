using UnityEngine;

public class ActivityLoader
{
    private ActivityInfo[] LoadActivitiesFromJson(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Activities/" + fileName);
        if (jsonFile == null)
        {
            Debug.LogWarning("JSON file not found");
            return null;
        }

        ActivityList activityList = JsonUtility.FromJson<ActivityList>(jsonFile.text);
        return activityList.activities;
    }

    public void LoadActivities()
    {
        UIManager.Instance.telephone.calendar.activities_mixed = LoadActivitiesFromJson("activities_mixed");
        UIManager.Instance.telephone.calendar.activities_romantic = LoadActivitiesFromJson("activities_romantic");
        UIManager.Instance.telephone.calendar.activities_partner = LoadActivitiesFromJson("activities_partner");
    }
}

[System.Serializable]
public class ActivityList
{
    public ActivityInfo[] activities;
}
