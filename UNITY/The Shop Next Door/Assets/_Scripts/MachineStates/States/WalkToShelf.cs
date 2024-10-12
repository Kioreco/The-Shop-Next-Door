using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class WalkToShelf : AStateNPC
{
    public WalkToShelf(IContext cntx) : base(cntx) { }

    float secondsToSeek = 0.5f; //tiempo de la animaci�n
    float lastSeek = 0f;
    bool enDestino = false;
    #region metodos
    public override void Enter() 
    {
        Debug.Log("walking to shelf and taking product");

        contexto.getNavMesh().SetDestination(contexto.getCurrentEstanteria());
        //Debug.Log($"current estanteria: {contexto.getCurrentEstanteria()}");
    }

    public override void Update()
    {
        //contador animaci�n de coger elemento
        //cuanbdo acabe cambia estado
        //Debug.Log(contexto.getNavMesh().remainingDistance);

        if (contexto.getNavMesh().remainingDistance == 0f) { enDestino = true; }
        if (enDestino) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            if (contexto.getLista().lista.Count > 0)
            {
                contexto.getLista().lista.Remove(contexto.getLista().lista.Keys.First());
            }
            contexto.SetState(new SearchShelf(contexto));
        }
    }

    public override void FixedUpdate()
    {

    }
    #endregion
}