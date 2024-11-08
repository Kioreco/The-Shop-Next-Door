using System;
using UnityEngine;

public class AskWorker : AStateNPC
{
    Transform posWorker;
    bool isfinish = false;
    GameObject worker;
    int distanceMin = 10000;
    float secondsToSeek = 3f; //tiempo de la animación
    float lastSeek = 0f;
    bool isInWorker;
    public AskWorker(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        contexto.setTieneDuda(false);
        if(contexto.getTiendaManager().ID == 0)
        {
            if(contexto.getTiendaManager().workersP1 == null)
            {
                worker = contexto.getTiendaManager().player.gameObject;
            }
            else
            {
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
            if (contexto.getTiendaManager().workersP2 == null)
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
        if (contexto.getNavMesh().remainingDistance == 0) isInWorker = true;
        if (isInWorker) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            isInWorker = false;
            IState estado = contexto.getPilaState().Pop();
            contexto.SetState(Activator.CreateInstance(estado.GetType(), this) as IState);
            //contexto.SetState(new estado(this));
        }
    }

    public float calculateHeuristicDistance(Vector3 posClient, Vector3 posWorker)
    {
        return MathF.Abs(posClient.x - posWorker.x) + MathF.Abs(posClient.y - posWorker.y);
    }

}