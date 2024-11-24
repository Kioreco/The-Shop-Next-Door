using UnityEngine;

public class SceneAwake : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.FinalResult();
    }
}