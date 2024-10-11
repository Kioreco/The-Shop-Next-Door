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
        Debug.Log("searching next shelf");
        elementosRestantes = contexto.getLista().lista.Count - 1;

        if(elementosRestantes > 0)
        {
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
