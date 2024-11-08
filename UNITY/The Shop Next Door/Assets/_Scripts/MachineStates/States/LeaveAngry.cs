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
        Debug.Log("leave angry");
        if (contexto.getTiendaManager().ID == 0)
        {
            exitPos = contexto.getTiendaManager().outDoorShopP1;
            doorPos = contexto.getTiendaManager().doorPos1;
        }
        else if (contexto.getTiendaManager().ID == 1)
        {
            exitPos = contexto.getTiendaManager().outDoorShopP2;
            doorPos = contexto.getTiendaManager().doorPos1;
        }
        actualPos = contexto.GetTransform();

        //generar en una pos random una bolsa de basura
        random = Random.Range(0f, 1f);
        spawnPosition = Vector3.Lerp(actualPos.position, doorPos.position, random);
        Debug.Log($"spawnposition: {spawnPosition}");

        //se va por la puerta
        contexto.getNavMesh().SetDestination(exitPos.position);
        contexto.getTiendaManager().avanzarLaCola();

    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        //Debug.Log($"pos cliente: {contexto.GetTransform().position.x} spawnposition: {spawnPosition.x}");
        if(contexto.GetTransform().position.z - spawnPosition.z < 0.1 & notInstance)
        {
            //Debug.Log("iguales");
            notInstance = false;
            contexto.getTiendaManager().InstanceBag(spawnPosition);
        }

        if (contexto.getNavMesh().remainingDistance == 0)
        {
            contexto.Destuir();
        }
    }
}