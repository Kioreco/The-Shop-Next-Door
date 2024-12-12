using UnityEngine;

public class UI_LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool isClientEmotions;

    private void Awake()
    {
        transform.LookAt(transform.position + GameManager.Instance.activeCamera.transform.rotation * Vector3.forward, GameManager.Instance.activeCamera.transform.rotation * Vector3.up);
    }

    private void Update()
    {
        if (!isClientEmotions) { return; }
        else
        {
            transform.LookAt(transform.position + GameManager.Instance.activeCamera.transform.rotation * Vector3.forward, GameManager.Instance.activeCamera.transform.rotation * Vector3.up);
        }
    }
}
