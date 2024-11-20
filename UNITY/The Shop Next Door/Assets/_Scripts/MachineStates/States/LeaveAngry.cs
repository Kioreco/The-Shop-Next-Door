using UnityEngine;

public class LeaveAngry : AStateNPC
{
    Transform exitPos;
    Transform actualPos;
    Transform doorPos;
    float random;
    Vector3 spawnPosition;
    bool notInstance = true;

    public LeaveAngry(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        if (!contexto.getIfShopIsClosed())
        {
            if (contexto.getIsKaren())
            {
                contexto.getPilaState().Push(this);
                contexto.SetState(new TalkToAWorker(contexto));
            }
        }

        if (contexto.getTiendaManager().ID == 0)
        {
            exitPos = contexto.getTiendaManager().outDoorShopP1;
            doorPos = contexto.getTiendaManager().doorPos1;
        }
        else if (contexto.getTiendaManager().ID == 1)
        {
            exitPos = contexto.getTiendaManager().outDoorShopP2;
            doorPos = contexto.getTiendaManager().doorPos2;
        }
        actualPos = contexto.GetTransform();

        //generar en una pos random una bolsa de basura
        random = Random.Range(0f, 1f);
        spawnPosition = Vector3.Lerp(actualPos.position, doorPos.position, random);

        if (contexto.getIsInPayQueue()) 
        { 
            contexto.getTiendaManager().avanzarLaCola();
            if (contexto.getTiendaManager().ID == 0 && contexto.getTiendaManager().npcPayQueueP1.Count == 0) UIManager.Instance.cajero_Canvas.SetActive(false);
            else if (contexto.getTiendaManager().ID == 1 && contexto.getTiendaManager().npcPayQueueP2.Count == 0) UIManager.Instance.cajero_Canvas.SetActive(false);
        }

        //se va por la puerta
        contexto.getNavMesh().speed += 0.07f;
        contexto.getNavMesh().SetDestination(exitPos.position);

        contexto.getGameManager().UpdateClientHappiness(contexto.calcularFelicidadCliente());

    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        if(contexto.GetTransform().position.z - spawnPosition.z < 0.1 & notInstance)
        {
            notInstance = false;
            spawnPosition = new Vector3(contexto.GetTransform().position.x, spawnPosition.y, contexto.GetTransform().position.z);
            //contexto.getTiendaManager().InstanceBag(spawnPosition, contexto.getDineroCompra());
        }

        if (contexto.getNavMesh().remainingDistance == 0)
        {
            contexto.Destruir();
        }
    }
}