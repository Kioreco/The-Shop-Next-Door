using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Pay : AStateNPC
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
    bool hasSpace = true;

    public Pay(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        //contexto.getNavMesh().avoidancePriority = Random.Range(30, 50);
        Debug.Log("paying...");
        //Debug.Log($"dinero: {contexto.getDineroCompra()}");
        contexto.getNavMesh().SetDestination(contexto.getTiendaManager().positionColliderPayBox.position);
    }
    public override void FixedUpdate()
    {
        //Debug.Log($"{contexto.getCajaPagar()}, {antiguaPos}");
    }
    public override void Update()
    {
        if (contexto.getNavMesh().remainingDistance == 0f && contexto.getIsInColliderCajaPago() && !isPaying && !isInQueue && hasSpace)
        {
            //Debug.Log("está en el collider pidiendo la vez");
            actualPosQueue = contexto.getTiendaManager().cogerSitioCola(contexto);
            if (actualPosQueue == -1)
            {
                contexto.getNavMesh().SetDestination(contexto.getTiendaManager().outDoorShop.position);
                isFinish = true;
                hasSpace = false;
                isPaying = true;
                isInQueue = true;
                return;
            }
            else
            {
                contexto.getNavMesh().SetDestination(contexto.getTiendaManager().posPayCheckpoints[actualPosQueue].position);
                isInQueue = true;
            }
        }

        if(contexto.getPositionPay() != -1 && actualPosQueue > contexto.getPositionPay() && isInQueue && !isPaying && hasSpace)
        {
            actualPosQueue = contexto.getPositionPay();
            //Debug.Log($"actual pos: {actualPosQueue}");
            contexto.getNavMesh().SetDestination(contexto.getTiendaManager().posPayCheckpoints[actualPosQueue].position);
        }
        if(contexto.getNavMesh().remainingDistance == 0f && isInQueue && actualPosQueue == 0 && hasSpace)// contexto.getTiendaManager().npcPayQueue.Count == 1)
        {
            //Debug.Log("está en la primera posicion");
            isPaying = true;
        }

        if (isPaying && hasSpace) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            contexto.getTiendaManager().avanzarLaCola();
            contexto.getNavMesh().SetDestination(contexto.getTiendaManager().outDoorShop.position);
            contexto.getGameManager().dineroJugador += contexto.getDineroCompra();
            contexto.getUIManager().UpdateDineroJugador();
            isFinish = true;
        }

        if (isFinish) lastSeekGoOutShop += Time.deltaTime;

        if (lastSeekGoOutShop >= secondsToSeekGoOutShop)
        {
            lastSeekGoOutShop = 0f;
            isFinish = false;
            lastMovement = true;
        }

        if (contexto.getNavMesh().remainingDistance == 0 && lastMovement)
        {
            contexto.Destuir();
        }
    }

}
