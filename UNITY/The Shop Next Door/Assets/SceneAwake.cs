using UnityEngine;

public class SceneAwake : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.UpdateFinalWeekTexts(GameManager.Instance.playerID);
    }
}
