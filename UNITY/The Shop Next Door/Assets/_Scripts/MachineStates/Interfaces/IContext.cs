using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IContext
{
    public void SetState(IState cntx);

    public List<GameObject> getEstanterias();
    public float getSpeed();
    public void setSpeed(float s);
    public NavMeshAgent getNavMesh();
    public ListaCompra getLista();
}
