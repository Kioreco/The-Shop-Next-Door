using Assets.Scripts.MachineStates.Classes;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TakeProduct : AStateNPC
{
    public TakeProduct(IContext cntx) : base(cntx) { }

    float secondsToSeek = 0.5f; //tiempo de la animaci�n
    float lastSeek = 0f;
    string nombreProducto;
    int cantidadProductos;
    bool checkpoint = true;
    float porcentajeDuda = 0.5f;
    #region metodos
    public override void Enter() 
    {
        //contexto.getLista().lista.ToList();
        nombreProducto = contexto.getLista().lista.Keys.First();

        if (contexto.getTieneDuda() && nombreProducto == contexto.getProductoDuda()) TiendaManager.Instance.updateDudasClientes(contexto.GetContext(), contexto.getProductoDuda(), false);

        if (contexto.getIsKaren() && contexto.getCanComplain())
        {
            //mira si puede quejarse tiene emnos prioridad que los clientes
            TiendaManager.Instance.updateDudasClientes(contexto.GetContext(), null, true);
            Debug.Log($"puede quejarse? : {contexto.getCanComplain()}");
            if (contexto.getCanComplain())
            {
                contexto.getPilaState().Push(this);
                contexto.SetState(new TalkToAWorker(contexto));
            }
        }
        if (nombreProducto == contexto.getProductoDuda() && contexto.getTieneDuda())
        {
            //Debug.Log($"tiene duda: {contexto.getProductoDuda()}   tiene: {contexto.getTieneDuda()}");
            //Debug.Log($"ANTES pila: {contexto.getPilaState().Count}");
            Debug.Log("va a preguntar dudas");
            contexto.getPilaState().Push(this);
            //Debug.Log($"DESPUES pila: {contexto.getPilaState().Count}    a�adido: {contexto.getPilaState().First()}");
            contexto.SetState(new TalkToAWorker(contexto));
        }
    }

    public override void Update()
    {
        //contador animaci�n de coger elemento
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
            contexto.GetGameObject().transform.LookAt(contexto.GetGameObject().transform.position +
                                    GameManager.Instance.activeCamera.transform.rotation * Vector3.forward,
                                    GameManager.Instance.activeCamera.transform.rotation * Vector3.up);
            contexto.GetAnimator().SetTrigger("pickShelve");
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
                contexto.activarCanvasEnfado();
                //Debug.Log($"no hay suficientes productos");
                if (!contexto.getIsKaren()) contexto.reducirFelicidad(15);
                else
                {
                    int random = UnityEngine.Random.Range(0, 1);
                    bool canComplain;
                    if (random <= porcentajeDuda) canComplain = true;
                    else canComplain = false;
                    contexto.reducirFelicidad(25);
                    contexto.setCanComplain(canComplain);
                }
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