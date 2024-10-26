using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SearchShelf : AStateNPC
{
    public SearchShelf(IContext cntx) : base(cntx) { }
    int elementosRestantes;

    public override void Enter()
    {
        //Debug.Log($"searching next shelf\tElementosRestantes: {contexto.getLista().lista.Count}");
        elementosRestantes = contexto.getLista().lista.Count;
        if(elementosRestantes > 0)
        {
            //Debug.Log($"Estanteria: {contexto.getTiendaManager().buscarEstanteria(contexto.getLista().lista.Keys.First())}");
            contexto.setCurrentEstanteria(contexto.getTiendaManager().buscarEstanteria(contexto.getLista().lista.Keys.First()));
            //contexto.getLista().imprimirLista();
            contexto.SetState(new WalkToShelf(contexto));
        }
        else
        {
            contexto.SetState(new Pay(contexto));
        }
    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
    }
}
