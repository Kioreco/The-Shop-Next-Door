using UnityEngine;
using UnityEngine.AI;

public class RubbishInstanciator : MonoBehaviour, IPooleableObject, IPrototype<RubbishInstanciator>
{
    [Header("Limites Posiciones")]
    public int minX;
    public int maxX;
    public int minZ;
    public int maxZ;

    public float maxDistanceNavMesh = 1;

    public bool isActive { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public Vector3 calculateRandomPosition()
    {
        int randomX = Random.Range(minX, maxX);
        int randomZ = Random.Range(minZ, maxZ);
        Vector3 position = new Vector3(randomX, transform.position.y, randomZ);

        if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxDistanceNavMesh, NavMesh.AllAreas))
        {
            //Instantiate(manchaPrefab, hit.position, Quaternion.identity);
            position = hit.position;
        }
        return position;
    }

    public RubbishInstanciator Clone(Vector3 pos, Quaternion rot)
    {
        throw new System.NotImplementedException();
    }

    public void Reset()
    {
        calculateRandomPosition();
    }
}