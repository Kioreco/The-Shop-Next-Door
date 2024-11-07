using System.Linq;
using UnityEngine;


public class SearchShelf : AStateNPC
{
    public SearchShelf(IContext cntx) : base(cntx) { }
    
    public override void Enter()
    {
        //Debug.Log($"searching next shelf\tElementosRestantes: {contexto.getLista().lista.Count}");
        contexto.setCurrentEstanteria(contexto.getTiendaManager().buscarEstanteria(contexto.getLista().lista.Keys.First()));
        contexto.getNavMesh().avoidancePriority = Random.Range(0, 100);
        contexto.getNavMesh().SetDestination(contexto.getCurrentEstanteria());
    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        if (contexto.getNavMesh().remainingDistance == 0f)
        {
            contexto.SetState(new TakeProduct(contexto));
        }
    }
}
