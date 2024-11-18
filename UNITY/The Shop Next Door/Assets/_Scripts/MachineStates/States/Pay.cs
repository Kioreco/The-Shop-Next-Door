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
    //variables tacaño
    float diferencia;
    float propinaTacanio;

    public Pay(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
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

        if (contexto.getIsTacanio()) 
        {
            diferencia = contexto.getDineroCompra() - contexto.getPresupuesto(); 
            //si la resta es negativa: sobra dinero
            //si la resta es positiva: falta dinero
            if(diferencia <= 40) { propinaTacanio = diferencia; } //da propina
            else if(diferencia > 40 & diferencia <= 80) { propinaTacanio = 0; } //se queja pero paga
            else if(diferencia > 80) { propinaTacanio = -1; } //se va enfadado
        }
        //Debug.Log($"PAGANDO    hay cajero: {contexto.getHayCajeroEnCaja()}");
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        //está ya en la caja
        if(contexto.getHayCajeroEnCaja() & propinaTacanio == -1) contexto.SetState(new MakeShowInPay(contexto));
        if (!isFinish & contexto.getHayCajeroEnCaja()) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            float dinero = contexto.getDineroCompra();
            lastSeek = 0f;
            contexto.getTiendaManager().avanzarLaCola();
            contexto.getNavMesh().SetDestination(exitPos.position);

            if (contexto.getFelicidad() >= contexto.getUmbralPropina() & !contexto.getIsTacanio()) dinero += dinero * 0.2f; //propina de un 20%
            if (contexto.getIsTacanio()) { dinero += propinaTacanio; }

            contexto.getGameManager().dineroJugador += dinero;

            contexto.getUIManager().UpdatePlayerMoney_UI();
            contexto.getUIManager().UpdateNewMoney_UI(dinero, true);
            isFinish = true;

            contexto.getGameManager().UpdateClientHappiness(contexto.calcularFelicidadCliente());
            contexto.setHayCajeroEnCaja(false);
            contexto.getGameManager()._player.GetComponent<PlayerControler>().enableMovement(true);
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
}
