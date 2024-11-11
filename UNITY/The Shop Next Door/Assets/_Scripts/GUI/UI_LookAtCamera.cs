using UnityEngine;

public class UI_LookAtCamera : MonoBehaviour
{
    private void Awake()
    {
        transform.LookAt(transform.position + GameManager.Instance.activeCamera.transform.rotation * Vector3.forward, GameManager.Instance.activeCamera.transform.rotation * Vector3.up);
    }
}
