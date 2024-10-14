using Assets.Scripts.MachineStates.Classes;
using UnityEngine;

public class CajaPago : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            other.gameObject.GetComponent<Context>().setIsInColliderCajaPago(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            other.gameObject.GetComponent<Context>().setIsInColliderCajaPago(false);
        }
    }
}
