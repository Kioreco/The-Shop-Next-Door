using System.Collections.Generic;
using UnityEngine;

public class ComplainToWorker : AStateNPC
{
    GameObject worker;
    int distanceMin = 10000;
    float secondsToSeek = 3f; //tiempo de la animación
    float lastSeek = 0f;
    bool isInWorker;

    public ComplainToWorker(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        //Debug.Log($"ir a preguntar a trabajador  pila: {contexto.getPilaState().Count}");
        contexto.setCanComplain(false);
        if (contexto.getTiendaManager().ID == 0)
        {
            getWorkerToComplain(contexto.getTiendaManager().workersP1);
        }
        else if (contexto.getTiendaManager().ID == 1)
        {
            getWorkerToComplain(contexto.getTiendaManager().workersP2);
        }
        contexto.getNavMesh().SetDestination(worker.transform.position);
    }

    void getWorkerToComplain(List<GameObject> workers)
    {
        if (workers.Count == 0)
        {
            worker = contexto.getTiendaManager().player.gameObject;
        }
        else
        {
            foreach (GameObject work in workers)
            {
                if (contexto.calculateHeuristicDistance(contexto.GetTransform().position, work.transform.position) < distanceMin)
                {
                    worker = work;
                }
            }
        }
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        if (contexto.getNavMesh().remainingDistance <= 0.6 && !isInWorker) 
        { 
            isInWorker = true; 
            Debug.Log("está en worker"); 
        }
        if (isInWorker) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            isInWorker = false;

            contexto.SetState(contexto.getPilaState().Pop());
        }
    }
}