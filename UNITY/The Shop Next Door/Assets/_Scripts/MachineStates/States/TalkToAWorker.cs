using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TalkToAWorker : AStateNPC
{
    GameObject worker;
    int distanceMin = 10000;
    float secondsToSeek = 3f; //tiempo de la animación
    float lastSeek = 0f;
    bool isInWorker;
    Vector3 currentDestination;

    public TalkToAWorker(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        //Debug.Log($"ir a preguntar a trabajador  pila: {contexto.getPilaState().Count}");


        //if(contexto.getTiendaManager().ID == 0)
        //{
        //    if(contexto.getTiendaManager().workersP1.Count == 0) worker = contexto.getTiendaManager().player.gameObject;
        //    else
        //    {
        //        foreach(GameObject work in contexto.getTiendaManager().workersP1)
        //        {
        //            if(contexto.calculateHeuristicDistance(contexto.GetTransform().position, work.transform.position) < distanceMin)
        //            {
        //                worker = work;
        //            }
        //        }
        //    }
        //}
        //else if(contexto.getTiendaManager().ID == 1)
        //{
        //    if (contexto.getTiendaManager().workersP2.Count == 0) worker = contexto.getTiendaManager().player.gameObject;
        //    else
        //    {
        //        foreach (GameObject work in contexto.getTiendaManager().workersP2)
        //        {
        //            if (contexto.calculateHeuristicDistance(contexto.GetTransform().position, work.transform.position) < distanceMin)
        //            {
        //                worker = work;
        //            }
        //        }
        //    }
        //}
        Debug.Log($"duda: {contexto.getTieneDuda()}");
        worker = GameManager.Instance._player.gameObject;
        currentDestination = worker.transform.position;

        contexto.getNavMesh().SetDestination(currentDestination);
    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        if (contexto.getIfImInPlayer() && !isInWorker && !contexto.getIsKaren()) 
        { 
            isInWorker = true;
            UIManager.Instance.CreateDuda_UI(contexto.getProductoDuda());
            Debug.Log("está en worker"); 
        }
        if (!contexto.getIfImInPlayer() && Vector3.Distance(contexto.getNavMesh().transform.position, worker.transform.position) > 5f && !contexto.getIsKaren())//ELEFANTE CAMBIAR LO DE KAREN
        {
            //Debug.Log("se movió");
            currentDestination = worker.transform.position;
            contexto.getNavMesh().SetDestination(currentDestination);
        }

        if (contexto.getIfDudaResuelta() && !contexto.getIsKaren())
        {
            Debug.Log("duda resuelta");
            TiendaManager.Instance.yaHayDuda = false;
            GameManager.Instance._player.enableMovement(false);
            contexto.setTieneDuda(false);
            contexto.SetState(contexto.getPilaState().Pop());
        }


        //if (isInWorker) lastSeek += Time.deltaTime;

        //if (lastSeek >= secondsToSeek)
        //{
        //    lastSeek = 0f;
        //    isInWorker = false;
        //    contexto.SetState(contexto.getPilaState().Pop());
        //}
        if (contexto.getNavMesh().remainingDistance <= 0.6 && !isInWorker && contexto.getIsKaren())
        {
            isInWorker = true;
            Debug.Log("esta en worker");
        }
        if (isInWorker && contexto.getIsKaren()) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            isInWorker = false;
            contexto.setCanComplain(false);

            contexto.SetState(contexto.getPilaState().Pop());
        }
    }
}