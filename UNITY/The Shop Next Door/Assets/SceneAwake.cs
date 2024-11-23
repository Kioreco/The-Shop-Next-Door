using UnityEngine;

public class SceneAwake : MonoBehaviour
{
    private void Awake()
    {
        UIManager.Instance.UpdateFinalWeekTexts(GameManager.Instance.playerID);
    }
}
