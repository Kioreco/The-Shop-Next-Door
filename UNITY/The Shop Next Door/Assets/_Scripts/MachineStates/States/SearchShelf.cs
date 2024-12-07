using System.Linq;
using UnityEngine;


public class SearchShelf : AStateNPC
{
    public SearchShelf(IContext cntx) : base(cntx) { }
    
    public override void Enter()
    {
        //Debug.Log($"searching next shelf\tElementosRestantes: {contexto.getLista().lista.Count}");
        contexto.setCurrentEstanteria(contexto.getTiendaManager().buscarEstanteria(contexto.getLista().lista.Keys.First(), false));
        contexto.getNavMesh().avoidancePriority = Random.Range(0, 100);
        Vector3 pos = contexto.randomPositionShelf(contexto.getCurrentEstanteria());
        contexto.getNavMesh().SetDestination(pos);
    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        if (contexto.getNavMesh().remainingDistance <= 0.7f)
        {
            contexto.SetState(new TakeProduct(contexto));
        }
    }
}
