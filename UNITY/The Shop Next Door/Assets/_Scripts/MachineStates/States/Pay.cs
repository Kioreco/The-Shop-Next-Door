using System;
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
        //Debug.Log("enter pay state");
        //Physics.IgnoreLayerCollision(GameManager.Instance._player.playerLayer, GameManager.Instance._player.npcLayer, true);
        if (!contexto.getWorkerInPay()) UIManager.Instance.cajero_Canvas.SetActive(true);
        if (GameManager.Instance.WorkerHire) UIManager.Instance.cajero_Canvas.SetActive(false);

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
            //Debug.Log($"dinero: {contexto.getDineroCompra()}   limite presupuesto: {contexto.getPresupuesto()}    sobrante: {propinaTacanio}");
            if (diferencia <= 40) { propinaTacanio = Math.Abs(diferencia); } //da propina
            else if(diferencia > 40 & diferencia <= 80) { propinaTacanio = 0; } //se queja pero paga
            else if(diferencia > 80) { contexto.SetState(new MakeShowInPay(contexto)); } //se va enfadado
        }
        //Debug.Log($"PAGANDO    hay cajero: {contexto.getHayCajeroEnCaja()}");
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        if (contexto.getWorkerInPay()) UIManager.Instance.cajero_Canvas.SetActive(false);

        //if (contexto.getIfShopIsClosed()) contexto.SetState(new LeaveAngry(contexto));

        //está ya en la caja
        //if (contexto.getHayCajeroEnCaja() & propinaTacanio == -1) contexto.SetState(new MakeShowInPay(contexto));

        if (!isFinish & contexto.getHayCajeroEnCaja()) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {

            contexto.GetGameObject().transform.LookAt(contexto.GetGameObject().transform.position + 
                                    GameManager.Instance.activeCamera.transform.rotation * Vector3.forward, 
                                    GameManager.Instance.activeCamera.transform.rotation * Vector3.up);
            contexto.GetAnimator().SetTrigger("pay");
            if (!GameManager.Instance.WorkerIsWoman & GameManager.Instance.WorkerHire)
            {
                GameManager.Instance.workersMen[0].GetComponent<Animator>().SetTrigger("isPaying");
            }
            else if (GameManager.Instance.WorkerIsWoman & GameManager.Instance.WorkerHire)
            {
                GameManager.Instance.workersWoman[0].GetComponent<Animator>().SetTrigger("isPaying");
            }

            if (contexto.getWorkerInPay()) contexto.setFinishPayWorker(true);

            if (contexto.getTiendaManager().ID == 0 && contexto.getTiendaManager().npcPayQueueP1.Count == 0) UIManager.Instance.cajero_Canvas.SetActive(false);
            else if (contexto.getTiendaManager().ID == 1 && contexto.getTiendaManager().npcPayQueueP2.Count == 0) UIManager.Instance.cajero_Canvas.SetActive(false);

            float dinero = contexto.getDineroCompra();
            lastSeek = 0f;
            contexto.getTiendaManager().avanzarLaCola();
            contexto.getNavMesh().SetDestination(exitPos.position);

            if (contexto.getFelicidad() >= contexto.getUmbralPropina() & !contexto.getIsTacanio()) dinero += dinero * 0.2f; //propina de un 20%
            if (contexto.getIsTacanio()) 
            {
                if (propinaTacanio != 0) contexto.activarCanvasTacanioFeliz();
                else if (propinaTacanio == 0) contexto.activarCanvasEnfado();
                
                dinero += propinaTacanio; 
            }


            contexto.getGameManager().dineroJugador += dinero;

            contexto.getUIManager().UpdatePlayerMoney_UI();
            contexto.getUIManager().UpdateNewMoney_UI(dinero, true);
            isFinish = true;

            contexto.getGameManager().UpdateClientHappiness(contexto.calcularFelicidadCliente());
            contexto.setHayCajeroEnCaja(false);
            contexto.setWorkerInPay(false);

            if (!contexto.getWorkerInPay()) contexto.getGameManager()._player.GetComponent<PlayerControler>().enableMovement(true);
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
