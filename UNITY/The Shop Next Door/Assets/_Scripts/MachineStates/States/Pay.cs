using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pay : AStateNPC
{
    Vector3 antiguaPos;
    bool isPaying = false;
    float secondsToSeek = 0.5f; //tiempo de la animación
    float lastSeek = 0f;

    public Pay(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        Debug.Log("paying...");
        contexto.getTiendaManager().cogerSitioCola();
        antiguaPos = contexto.getCajaPagar();
        contexto.getNavMesh().SetDestination(contexto.getCajaPagar());
    }
    public override void FixedUpdate()
    {
        //""espera activa"" -> un evento sería mejor (PENDIENTE)
        //Debug.Log($"{contexto.getCajaPagar()}, {antiguaPos}");
        if(contexto.getCajaPagar() == antiguaPos && contexto.getNavMesh().remainingDistance == 0f)
        {
            isPaying = true;
        }
        
        if (contexto.getCajaPagar().z > antiguaPos.z && !isPaying) 
        {
            contexto.getNavMesh().SetDestination(contexto.getCajaPagar());
            antiguaPos = contexto.getCajaPagar();
        }


        if (isPaying) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            contexto.getTiendaManager().avanzarLaCola();
            contexto.getNavMesh().SetDestination(contexto.getTiendaManager().salidaTienda.position);
        }
    }

    public override void Update()
    {

    }
}
