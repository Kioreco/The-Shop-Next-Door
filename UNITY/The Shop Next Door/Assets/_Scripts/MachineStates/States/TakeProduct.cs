using System.Linq;
using UnityEngine;

public class TakeProduct : AStateNPC
{
    public TakeProduct(IContext cntx) : base(cntx) { }

    float secondsToSeek = 0.5f; //tiempo de la animación
    float lastSeek = 0f;
    string nombreProducto;
    int cantidadProductos;
    #region metodos
    public override void Enter() 
    {
        //contexto.getLista().lista.ToList();
        nombreProducto = contexto.getLista().lista.Keys.First();
        if (nombreProducto == contexto.getProductoDuda() && contexto.getTieneDuda())
        {
            //Debug.Log($"tiene duda: {contexto.getProductoDuda()}   tiene: {contexto.getTieneDuda()}");
            //Debug.Log($"ANTES pila: {contexto.getPilaState().Count}");

            contexto.getPilaState().Push(this);
            //Debug.Log($"DESPUES pila: {contexto.getPilaState().Count}    añadido: {contexto.getPilaState().First()}");
            contexto.SetState(new AskWorker(contexto));
        }
    }

    public override void Update()
    {
        //contador animación de coger elemento
        //cuanbdo acabe cambia estado
        if (contexto.getFelicidad() <= contexto.getMaxEnfado()) contexto.SetState(new LeaveAngry(contexto));

        lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            if (contexto.getIsInColliderShelf())
            {
                //Debug.Log("cogiendo elemento...");
                cantidadProductos = contexto.getTiendaManager().cogerDeEstanteria(nombreProducto, contexto.getLista().lista[nombreProducto].tipo, contexto.getLista().lista[nombreProducto].cantidad);
                contexto.sumDineroCompra(contexto.getTiendaManager().getPrecioProducto(nombreProducto, contexto.getLista().lista[nombreProducto].tipo, contexto.getLista().lista[nombreProducto].cantidad));
                contexto.getLista().lista.Remove(nombreProducto);
                //contexto.setIdxLista(contexto.getIdxLista() + 1);
            }

            if(cantidadProductos == -1)
            {
                Debug.Log($"no hay suficientes productos");
                contexto.reducirFelicidad(15);
            }

            if (contexto.getLista().lista.Count > 0) contexto.SetState(new SearchShelf(contexto));
            else contexto.SetState(new WaitPayTurn(contexto));
            //falta transicion a preguntar duda
        }
    }

    public override void FixedUpdate()
    {

    }
    #endregion
}