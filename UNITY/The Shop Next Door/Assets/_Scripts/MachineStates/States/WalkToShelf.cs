using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class WalkToShelf : AStateNPC
{
    public WalkToShelf(IContext cntx) : base(cntx) { }

    float secondsToSeek = 0.10f; //tiempo de la animación
    float lastSeek = 0f;
    bool enDestino = false;
    #region metodos
    public override void Enter() 
    {
        Debug.Log("walking to shelf and taking product");
        //_agent.SetDestination(hit.point);
        contexto.getNavMesh().SetDestination(contexto.getCurrentEstanteria());
        Debug.Log($"current estanteria: {contexto.getCurrentEstanteria()}");
        if (contexto.getLista().lista.Count > 0)
        {
            contexto.getLista().lista.Remove(contexto.getLista().lista.Keys.First());
            //contexto.getLista().imprimirLista();
        }
        //contexto.SetState(new WalkToShelf(contexto));
    }

    public override void Update()
    {
        //contador animación de coger elemento
        //cuanbdo acabe cambia estado
        //Debug.Log(contexto.getNavMesh().velocity.sqrMagnitude);

        if (contexto.getNavMesh().velocity.sqrMagnitude == 0f) { enDestino = true; }
        if (enDestino) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            contexto.SetState(new SearchShelf(contexto));
        }
    }

    public override void FixedUpdate()
    {

    }
    #endregion
}