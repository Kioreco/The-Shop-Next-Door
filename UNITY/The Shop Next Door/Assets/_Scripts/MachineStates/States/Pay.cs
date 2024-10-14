using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Pay : AStateNPC
{
    Vector3 antiguaPos;
    bool isPaying = false;
    float secondsToSeek = 0.5f; //tiempo de la animación
    float lastSeek = 0f;    
    float secondsToSeekGoOutShop = 0.2f; //tiempo de salir de la tienda
    float lastSeekGoOutShop = 0f;
    bool isFinish = false;
    bool lastMovement = false;

    public Pay(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        Debug.Log("paying...");
        Debug.Log($"dinero: {contexto.getDineroCompra()}");
        antiguaPos = contexto.getPosicionEnLaCola();
        contexto.getNavMesh().SetDestination(contexto.getPosicionEnLaCola());
        //contexto.getTiendaManager().cogerSitioCola();
    }
    public override void FixedUpdate()
    {
        //Debug.Log($"{contexto.getCajaPagar()}, {antiguaPos}");
        
    }
    public override void Update()
    {
        if (contexto.getNavMesh().remainingDistance == 0f && contexto.getIsInColliderCajaPago())
        {
            antiguaPos = contexto.getPosicionEnLaCola();
            contexto.getTiendaManager().cogerSitioCola();
        }

        if (contexto.getPosicionEnLaCola().z > antiguaPos.z)
        {
            contexto.getNavMesh().SetDestination(contexto.getPosicionEnLaCola());
            antiguaPos = contexto.getPosicionEnLaCola();
        }

        if (isPaying) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            isPaying = false;
            contexto.getTiendaManager().avanzarLaCola();
            contexto.getNavMesh().SetDestination(contexto.getTiendaManager().salidaTienda.position);
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
