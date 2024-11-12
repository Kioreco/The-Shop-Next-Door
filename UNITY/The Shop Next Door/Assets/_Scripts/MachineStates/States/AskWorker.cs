using System;
using UnityEngine;

public class AskWorker : AStateNPC
{
    Transform posWorker;
    GameObject worker;
    int distanceMin = 10000;
    float secondsToSeek = 3f; //tiempo de la animaci�n
    float lastSeek = 0f;
    bool isInWorker;

    public AskWorker(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        //Debug.Log($"ir a preguntar a trabajador  pila: {contexto.getPilaState().Count}");
        contexto.setTieneDuda(false);
        if(contexto.getTiendaManager().ID == 0)
        {
            if(contexto.getTiendaManager().workersP1.Count == 0)
            {
                //Debug.Log("no hay trabajadores");
                worker = contexto.getTiendaManager().player.gameObject;
                //Debug.Log($"worker: {worker}");
            }
            else
            {
                //Debug.Log("hay trabajadores");
                foreach(GameObject work in contexto.getTiendaManager().workersP1)
                {
                    if(calculateHeuristicDistance(contexto.GetTransform().position, work.transform.position) < distanceMin)
                    {
                        worker = work;
                    }
                }
            }
        }
        else if(contexto.getTiendaManager().ID == 1)
        {
            if (contexto.getTiendaManager().workersP2.Count == 0)
            {
                worker = contexto.getTiendaManager().player.gameObject;
            }
            else
            {
                foreach (GameObject work in contexto.getTiendaManager().workersP2)
                {
                    if (calculateHeuristicDistance(contexto.GetTransform().position, work.transform.position) < distanceMin)
                    {
                        worker = work;
                    }
                }
            }
        }
        contexto.getNavMesh().SetDestination(worker.transform.position);
    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        if (contexto.getNavMesh().remainingDistance < contexto.getNavMesh().stoppingDistance + 0.1 && !isInWorker) { isInWorker = true; Debug.Log("est� en worker"); }
        if (isInWorker) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            isInWorker = false;
            //Debug.Log($"popeo: {contexto.getPilaState().Pop()}");

            //IState estado = contexto.getPilaState().Pop();
            contexto.SetState(contexto.getPilaState().Pop());
            //contexto.SetState(new estado(this));
        }
    }

    public float calculateHeuristicDistance(Vector3 posClient, Vector3 posWorker)
    {
        return MathF.Abs(posClient.x - posWorker.x) + MathF.Abs(posClient.z - posWorker.z);
    }

}