using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Pay : AStateNPC
{
    Vector3 posicionActual;
    bool isPaying = false;
    float secondsToSeek = 3f; //tiempo de la animación
    float lastSeek = 0f;    
    float secondsToSeekGoOutShop = 0.2f; //tiempo de salir de la tienda
    float lastSeekGoOutShop = 0f;
    bool isFinish = false;
    bool lastMovement = false;
    bool isInQueue = false;

    public Pay(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        //contexto.getNavMesh().avoidancePriority = Random.Range(30, 50);
        Debug.Log("paying...");
        Debug.Log($"dinero: {contexto.getDineroCompra()}");
        contexto.getNavMesh().SetDestination(contexto.getTiendaManager().posicionColliderCaja.position);
    }
    public override void FixedUpdate()
    {
        //Debug.Log($"{contexto.getCajaPagar()}, {antiguaPos}");
    }
    public override void Update()
    {
        if (contexto.getNavMesh().remainingDistance == 0f && contexto.getIsInColliderCajaPago() && !isPaying && !isInQueue)
        {
            Debug.Log("está en el collider pidiendo la vez");
            posicionActual = contexto.getPosicionEnLaCola();
            contexto.getNavMesh().SetDestination(posicionActual);
            contexto.getTiendaManager().cogerSitioCola();
            isInQueue = true;
        }

        //Debug.Log($"primera posicion: {contexto.getTiendaManager().primeraPosCola} \tposicionactual: {posicionActual}");
        if(contexto.getNavMesh().remainingDistance == 0f && isInQueue && contexto.getTiendaManager().primeraPosCola == posicionActual && !isPaying)
        {
            //Debug.Log("está en la primera posicion");
            isPaying = true;
        }

        if (contexto.getPosicionEnLaCola().z < posicionActual.z && !isPaying)
        {
            Debug.Log("avanza en la cola");
            contexto.getNavMesh().SetDestination(contexto.getPosicionEnLaCola());
            posicionActual = contexto.getPosicionEnLaCola();
        }

        if (isPaying) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            Debug.Log("está en el temporizador");
            //isPaying = false;
            contexto.getTiendaManager().posicionEnLaCola.position = posicionActual;
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
