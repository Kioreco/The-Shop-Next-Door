using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaveAngry : AStateNPC
{
    Transform exitPos;

    public LeaveAngry(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        if (contexto.getTiendaManager().ID == 0)
        {
            exitPos = contexto.getTiendaManager().outDoorShopP1;
        }
        else if (contexto.getTiendaManager().ID == 1)
        {
            exitPos = contexto.getTiendaManager().outDoorShopP2;
        }
        //devuelve los objetos a las estanterías, si no hay suficiente espacio la devuelve al almacén ?¿


        //se va por la puerta
        contexto.getNavMesh().SetDestination(exitPos.position);
    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        if (contexto.getNavMesh().remainingDistance == 0)
        {
            contexto.Destuir();
        }
    }
}