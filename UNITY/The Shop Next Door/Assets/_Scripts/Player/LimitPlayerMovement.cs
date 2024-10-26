using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LimitPlayerMovement : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<NavMeshAgent>().Warp(GameManager.Instance._spawnPositions[other.gameObject.GetComponent<PlayerControler>().ID].position);
        }
    }
}
