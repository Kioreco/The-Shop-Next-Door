using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaitPayTurn : AStateNPC
{
    bool isPaying = false;
    float secondsToSeek = 3f; //tiempo de la animación
    float lastSeek = 0f;
    float secondsToSeekGoOutShop = 0.2f; //tiempo de salir de la tienda
    float lastSeekGoOutShop = 0f;
    bool isFinish = false;
    bool lastMovement = false;
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
        if (contexto.getNavMesh().remainingDistance <= 0.5f && contexto.getIsInColliderCajaPago() && !isInQueue)
        {
            //Debug.Log("está en el collider pidiendo la vez");
            actualPosQueue = contexto.getTiendaManager().cogerSitioCola(contexto);

            if (actualPosQueue == -1) contexto.SetState(new LeaveAngry(contexto));
            else
            {
                contexto.getNavMesh().SetDestination(posCheckpoints[actualPosQueue].position);
                isInQueue = true;
            }
        }

        if (actualPosQueue > contexto.getPositionPay() && isInQueue)
        {
            actualPosQueue = contexto.getPositionPay();
            contexto.getNavMesh().SetDestination(posCheckpoints[actualPosQueue].position);
        }

        if (contexto.getNavMesh().remainingDistance == 0f && isInQueue && actualPosQueue == 0) contexto.SetState(new Pay(contexto));
    }
}
