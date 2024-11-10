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
        contexto.getTiendaManager().clientesTotales += 1;
        //Debug.Log("leave angry");
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
        //Debug.Log($"spawnposition: {spawnPosition}");

        //se va por la puerta
        contexto.getNavMesh().SetDestination(exitPos.position);

        if(contexto.getIsInPayQueue()) contexto.getTiendaManager().avanzarLaCola();

        //contexto.getGameManager().UpdateClientHappiness(calcularFelicidadCliente());
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
            //Debug.Log($"ANTES: spawnposition: {spawnPosition}    posicion npc: {contexto.GetTransform().position}");
            //spawnPosition.x = contexto.GetTransform().position.x;
            //Debug.Log($"DESPUÉS: spawnposition: {spawnPosition}    posicion npc: {contexto.GetTransform().position}");
            //Debug.Log($"posicion npc: {contexto.GetTransform().position}");
            spawnPosition = new Vector3(contexto.GetTransform().position.x, spawnPosition.y, contexto.GetTransform().position.z);
            contexto.getTiendaManager().InstanceBag(spawnPosition, contexto.getDineroCompra());
        }

        if (contexto.getNavMesh().remainingDistance == 0)
        {
            contexto.Destuir();
        }
    }

    float calcularFelicidadCliente()
    {
        return (contexto.getGameManager().reputation*(contexto.getTiendaManager().clientesTotales-1) + contexto.getFelicidad())/ contexto.getTiendaManager().clientesTotales;
    }
}