using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class WalkToShelf : AStateNPC
{
    public WalkToShelf(IContext cntx) : base(cntx) { }

    float secondsToSeek = 0.5f; //tiempo de la animación
    float lastSeek = 0f;
    bool enDestino = false;
    string nombreProducto;
    #region metodos
    public override void Enter() 
    {
        Debug.Log("walking to shelf and taking product");
        contexto.getNavMesh().avoidancePriority = Random.Range(30, 50);
        contexto.getNavMesh().SetDestination(contexto.getCurrentEstanteria());
        nombreProducto = contexto.getLista().lista.Keys.First();
        //Debug.Log($"current estanteria: {contexto.getCurrentEstanteria()}");
    }

    public override void Update()
    {
        //contador animación de coger elemento
        //cuanbdo acabe cambia estado
        //Debug.Log(contexto.getNavMesh().remainingDistance);

        if (contexto.getNavMesh().remainingDistance == 0f) { enDestino = true; }
        if (enDestino) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            if (contexto.getLista().lista.Count > 0)
            {
                contexto.sumDineroCompra(contexto.getTiendaManager().getPrecioProducto(nombreProducto, contexto.getLista().lista[nombreProducto].tipo, contexto.getLista().lista[nombreProducto].cantidad));
                contexto.getLista().lista.Remove(nombreProducto);
            }
            contexto.SetState(new SearchShelf(contexto));
        }
    }

    public override void FixedUpdate()
    {

    }
    #endregion
}