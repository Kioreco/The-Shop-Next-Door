using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class LeaveAngry : AStateNPC
{
    Transform exitPos;
    Transform actualPos;
    Transform doorPos;
    float random;
    Vector3 spawnPosition;

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
    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        if(contexto.GetTransform().position == spawnPosition)
        {
            Debug.Log("iguales");
            contexto.getTiendaManager().InstanceBag(spawnPosition);
        }

        if (contexto.getNavMesh().remainingDistance == 0)
        {
            contexto.Destuir();
        }
    }
}