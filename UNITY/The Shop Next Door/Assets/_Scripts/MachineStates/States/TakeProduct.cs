using System.Linq;
using UnityEngine;

public class TakeProduct : AStateNPC
{
    public TakeProduct(IContext cntx) : base(cntx) { }

    float secondsToSeek = 0.5f; //tiempo de la animación
    float lastSeek = 0f;
    bool enDestino = false;
    string nombreProducto;
    #region metodos
    public override void Enter() 
    {
        contexto.getLista().lista.ToList();
        nombreProducto = contexto.getKeysLista()[contexto.getIdxLista()];
    }

    public override void Update()
    {
        //contador animación de coger elemento
        //cuanbdo acabe cambia estado

        lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            if (contexto.getIsInColliderShelf())
            {
                Debug.Log("cogiendo elemento...");
                contexto.getTiendaManager().cogerDeEstanteria(nombreProducto, contexto.getLista().lista[nombreProducto].tipo, contexto.getLista().lista[nombreProducto].cantidad);
                contexto.sumDineroCompra(contexto.getTiendaManager().getPrecioProducto(nombreProducto, contexto.getLista().lista[nombreProducto].tipo, contexto.getLista().lista[nombreProducto].cantidad));
                //contexto.getLista().lista.Remove(nombreProducto);
                contexto.setIdxLista(contexto.getIdxLista() + 1);
            }

            if (contexto.getIdxLista() < contexto.getLista().lista.Count) contexto.SetState(new SearchShelf(contexto));
            else contexto.SetState(new WaitPayTurn(contexto));
            //falta transicion a preguntar duda
        }
    }

    public override void FixedUpdate()
    {

    }
    #endregion
}