using System.Linq;
using UnityEngine;

public class TakeProduct : AStateNPC
{
    public TakeProduct(IContext cntx) : base(cntx) { }

    float secondsToSeek = 0.5f; //tiempo de la animación
    float lastSeek = 0f;
    string nombreProducto;
    int cantidadProductos;
    bool checkpoint = true;
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
            contexto.SetState(new TalkToAWorker(contexto));
        }
    }

    public override void Update()
    {
        //contador animación de coger elemento
        //cuanbdo acabe cambia estado
        if (contexto.getIsKaren() && contexto.getCanComplain())
        {
            contexto.getPilaState().Push(this);
            contexto.SetState(new TalkToAWorker(contexto)); //comprobar que funciona
        }

        if (contexto.getFelicidad() <= contexto.getMaxEnfado()) contexto.SetState(new LeaveAngry(contexto));

        if(checkpoint) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            checkpoint = false;
            lastSeek = 0f;
            if (contexto.getIsInColliderShelf())
            {
                cantidadProductos = contexto.getTiendaManager().cogerDeEstanteria(nombreProducto, contexto.getLista().lista[nombreProducto].tipo, contexto.getLista().lista[nombreProducto].cantidad);
                contexto.sumDineroCompra(contexto.getTiendaManager().getPrecioProducto(nombreProducto, contexto.getLista().lista[nombreProducto].tipo, contexto.getLista().lista[nombreProducto].cantidad));
                contexto.getLista().lista.Remove(nombreProducto);
            }

            if(cantidadProductos == -1)
            {
                //Debug.Log($"no hay suficientes productos");
                if(!contexto.getIsKaren()) contexto.reducirFelicidad(15);
                else contexto.reducirFelicidad(25);
            }

            if (contexto.getLista().lista.Count > 0) contexto.SetState(new SearchShelf(contexto));
            else contexto.SetState(new WaitPayTurn(contexto));
        }
    }

    public override void FixedUpdate()
    {

    }
    #endregion
}