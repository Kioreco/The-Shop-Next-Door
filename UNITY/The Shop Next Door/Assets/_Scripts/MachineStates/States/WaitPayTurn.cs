using System.Collections.Generic;
using UnityEngine;

public class WaitPayTurn : AStateNPC
{
    bool isInQueue = false;
    int actualPosQueue = 0;

    [Header("Variables Aux")]
    Transform poscollider;
    List<Transform> posCheckpoints;
    public WaitPayTurn (IContext cntx) : base(cntx) { }

    public override void Enter()
    {
        if (contexto.getTiendaManager().ID == 0)// && contexto.getTiendaManager().player.IsOwner)
        {
            poscollider = contexto.getTiendaManager().positionColliderPayBoxP1;
            posCheckpoints = contexto.getTiendaManager().posPayCheckpointsP1;
        }
        else if (contexto.getTiendaManager().ID == 1)// && contexto.getTiendaManager().player.IsOwner)
        {
            poscollider = contexto.getTiendaManager().positionColliderPayBoxP2;
            posCheckpoints = contexto.getTiendaManager().posPayCheckpointsP2;
        }
        contexto.getNavMesh().SetDestination(poscollider.position);
    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        if (contexto.getIsKaren() && contexto.getIsInPayQueue() && contexto.getCanComplain())
        {
            contexto.getPilaState().Push(this);
            contexto.SetState(new TalkToAWorker(contexto)); //comprobar que funciona
        }
        if (contexto.getFelicidad() <= contexto.getMaxEnfado()) contexto.SetState(new LeaveAngry(contexto));

        if (contexto.getNavMesh().remainingDistance <= 0.5f && contexto.getIsInColliderCajaPago() && !isInQueue)
        {
            //Debug.Log("está en el collider pidiendo la vez");
            actualPosQueue = contexto.getTiendaManager().cogerSitioCola(contexto);

            if (actualPosQueue == -1) contexto.SetState(new LeaveAngry(contexto));
            else
            {
                contexto.getNavMesh().SetDestination(posCheckpoints[actualPosQueue].position);
                isInQueue = true;
                contexto.setIsInPayQueue(true);
            }
        }

        if (actualPosQueue > contexto.getPositionPay() && isInQueue)
        {
            actualPosQueue = contexto.getPositionPay();
            contexto.getNavMesh().SetDestination(posCheckpoints[actualPosQueue].position);
            //Debug.Log($"posicion: {actualPosQueue}");
        }

        //if (contexto.getNavMesh().remainingDistance == 0f && isInQueue && actualPosQueue == 0) contexto.SetState(new LeaveAngry(contexto));
        if (contexto.getNavMesh().remainingDistance == 0f && isInQueue && actualPosQueue == 0) contexto.SetState(new Pay(contexto));
    }
}
