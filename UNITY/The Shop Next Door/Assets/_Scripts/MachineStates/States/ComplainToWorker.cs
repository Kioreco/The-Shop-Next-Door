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
            if (contexto.getTiendaManager().workersP1.Count == 0)
            {
                //Debug.Log("no hay trabajadores");
                worker = contexto.getTiendaManager().player.gameObject;
                //Debug.Log($"worker: {worker}");
            }
            else
            {
                //Debug.Log("hay trabajadores");
                foreach (GameObject work in contexto.getTiendaManager().workersP1)
                {
                    if (contexto.calculateHeuristicDistance(contexto.GetTransform().position, work.transform.position) < distanceMin)
                    {
                        worker = work;
                    }
                }
            }
        }
        else if (contexto.getTiendaManager().ID == 1)
        {
            if (contexto.getTiendaManager().workersP2.Count == 0)
            {
                worker = contexto.getTiendaManager().player.gameObject;
            }
            else
            {
                foreach (GameObject work in contexto.getTiendaManager().workersP2)
                {
                    if (contexto.calculateHeuristicDistance(contexto.GetTransform().position, work.transform.position) < distanceMin)
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
        if (contexto.getNavMesh().remainingDistance < contexto.getNavMesh().stoppingDistance + 0.1 && !isInWorker) { isInWorker = true; Debug.Log("está en worker"); }
        if (isInWorker) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            isInWorker = false;

            contexto.SetState(contexto.getPilaState().Pop());
        }
    }
}