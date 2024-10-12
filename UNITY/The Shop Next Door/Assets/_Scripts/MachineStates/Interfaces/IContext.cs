using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IContext
{
    public void SetState(IState cntx);
    public float getSpeed();
    public void setSpeed(float s);
    public NavMeshAgent getNavMesh();
    public ListaCompra getLista();
    public TiendaManager getTiendaManager();
    public Vector3 getCurrentEstanteria();
    public void setCurrentEstanteria(Vector3 e);
    public Vector3 getCajaPagar();
    public IState GetState();
}
