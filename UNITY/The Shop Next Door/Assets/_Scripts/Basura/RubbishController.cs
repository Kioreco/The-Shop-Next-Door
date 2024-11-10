using Assets.Scripts.MachineStates.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RubbishController : MonoBehaviour
{
    float slowDownAmount = 3;

    private void Start()
    {
        //print($"coordenadas basura: {transform.position}");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            //Debug.Log("reduc velocidad npc");
            other.gameObject.GetComponent<ContextClienteGenerico>().reducirFelicidad(3);

            other.gameObject.GetComponent<NavMeshAgent>().speed -= slowDownAmount;
            StartCoroutine(delaySpeedAgent(other.gameObject));
        }
    }

    IEnumerator delaySpeedAgent(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        obj.GetComponent<NavMeshAgent>().speed += slowDownAmount;
        //print("quitado efecto velocidad npc");
    }
}
