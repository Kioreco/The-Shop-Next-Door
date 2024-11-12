using UnityEngine;

public class Pay : AStateNPC
{
    float secondsToSeek = 3f; //tiempo de la animación
    float lastSeek = 0f;    
    float secondsToSeekGoOutShop = 0.2f; //tiempo de salir de la tienda
    float lastSeekGoOutShop = 0f;
    bool isFinish = false;
    bool lastMovement = false;
    [Header("Variables Aux")]
    Transform exitPos;

    public Pay(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        //referencia al canvas del uimanager oara que se active
        UIManager.Instance.cajero_Canvas.SetActive(true);

        contexto.getTiendaManager().clientesTotales += 1;

        if (contexto.getTiendaManager().ID == 0)
        {
            exitPos = contexto.getTiendaManager().outDoorShopP1;
        }
        else if (contexto.getTiendaManager().ID == 1)
        {
            exitPos = contexto.getTiendaManager().outDoorShopP2;
        }
    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        //está ya en la caja
        if(!isFinish) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            float dinero = contexto.getDineroCompra();
            lastSeek = 0f;
            contexto.getTiendaManager().avanzarLaCola();
            contexto.getNavMesh().SetDestination(exitPos.position);

            if (contexto.getFelicidad() >= contexto.getUmbralPropina()) dinero += dinero * 0.2f; //propina de un 20%

            contexto.getGameManager().dineroJugador += dinero;

            contexto.getUIManager().UpdatePlayerMoney_UI();
            contexto.getUIManager().UpdateNewMoney_UI(dinero, true);
            isFinish = true;

            contexto.getGameManager().UpdateClientHappiness(calcularFelicidadCliente());
        }

        //se queda
        if (isFinish && !lastMovement) lastSeekGoOutShop += Time.deltaTime;

        if (lastSeekGoOutShop >= secondsToSeekGoOutShop)
        {
            lastSeekGoOutShop = 0f;
            lastMovement = true;
        }

        if (contexto.getNavMesh().remainingDistance == 0 && lastMovement)
        {
            contexto.Destruir();
        }
    }

    float calcularFelicidadCliente()
    {
        return (contexto.getGameManager().reputation * (contexto.getTiendaManager().clientesTotales - 1) + contexto.getFelicidad()) / contexto.getTiendaManager().clientesTotales;
    }
}
