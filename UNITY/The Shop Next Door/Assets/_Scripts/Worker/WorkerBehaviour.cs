using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.BehaviourTrees;
using BehaviourAPI.UtilitySystems;
using BehaviourAPI.StateMachines;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using System.Linq;
using System.Collections.Generic;

public class WorkerBehaviour : BehaviourRunner
{
    public int shopID;
    public Vector3 currentDestination;
    public RubbishController manchaActual;
    public IContext clienteCaja;

    [Header("Limites Posiciones")]
    public int minX;
    public int maxX;
    public int minZ;
    public int maxZ;
    public float maxDistanceNavMesh = 3f;

    [Header("BT")]
    public bool sigueContratado;

    [Header("Control Variables")]
    public int clientesEsperandoCaja;

    public bool canCharge;
    protected override BehaviourGraph CreateGraph()
    {
        BehaviourTree Worker_BT = new BehaviourTree();
        UtilitySystem Worker_SU = new UtilitySystem(3f);
        FSM FSM_Limpiar = new FSM();
        FSM FSM_AtenderCaja = new FSM();

        FunctionalAction EntrarTienda_action = new FunctionalAction();
        EntrarTienda_action.onStarted = StartEntrarTienda;
        EntrarTienda_action.onUpdated = UpdateMovingSalirEntrar;
        LeafNode EntrarTienda = Worker_BT.CreateLeafNode(EntrarTienda_action);

        FunctionalAction SigueContratado_action = new FunctionalAction();
        SigueContratado_action.onUpdated = fueDespedido;
        LeafNode SigueContratado = Worker_BT.CreateLeafNode(SigueContratado_action);

        SubsystemAction Worker_SU_Node_action = new SubsystemAction(Worker_SU);
        LeafNode Worker_SU_Node = Worker_BT.CreateLeafNode(Worker_SU_Node_action);

        SequencerNode seq2 = Worker_BT.CreateComposite<SequencerNode>(false, SigueContratado, Worker_SU_Node);
        seq2.IsRandomized = false;

        LoopUntilNode LoopUntilFail = Worker_BT.CreateDecorator<LoopUntilNode>(seq2);
        LoopUntilFail.TargetStatus = Status.Failure;
        LoopUntilFail.MaxIterations = -1;

        FunctionalAction IrseDespedido_action = new FunctionalAction();
        IrseDespedido_action.onStarted = StartIrseDespedido;
        IrseDespedido_action.onUpdated = IrseUpdate;
        LeafNode IrseDespedido = Worker_BT.CreateLeafNode(IrseDespedido_action);

        SelectorNode sel = Worker_BT.CreateComposite<SelectorNode>(false, EntrarTienda, LoopUntilFail, IrseDespedido);
        sel.IsRandomized = false;

        VariableFactor Manchas_totales = Worker_SU.CreateVariable(ManchasTotales, 0f, 10f);

        VariableFactor Felicidad_Media = Worker_SU.CreateVariable(felicidadMedia, 0f, 100f);

        VariableFactor ClientesCaja = Worker_SU.CreateVariable(ClientesEnCaja, 0f, 5f);

        LinearCurveFactor FelicidadMCurva = Worker_SU.CreateCurve<LinearCurveFactor>(Felicidad_Media);

        MinFusionFactor LimpiarFusion = Worker_SU.CreateFusion<MinFusionFactor>(Manchas_totales, FelicidadMCurva);

        SubsystemAction Limpiar_action = new SubsystemAction(FSM_Limpiar);
        UtilityAction Limpiar = Worker_SU.CreateAction(LimpiarFusion, Limpiar_action, finishOnComplete: true);

        LinearCurveFactor FelicidadMAtenderCaja = Worker_SU.CreateCurve<LinearCurveFactor>(Felicidad_Media);

        MinFusionFactor clientesCajaFusion = Worker_SU.CreateFusion<MinFusionFactor>(ClientesCaja, FelicidadMAtenderCaja);

        SubsystemAction AtenderCaja_action = new SubsystemAction(FSM_AtenderCaja);
        UtilityAction AtenderCaja = Worker_SU.CreateAction(clientesCajaFusion, AtenderCaja_action, finishOnComplete: true);

        ConstantFactor AndarFactorConstante = Worker_SU.CreateConstant(0.01f);

        FunctionalAction Andar_action = new FunctionalAction();
        Andar_action.onStarted = StartAndar;
        Andar_action.onUpdated = AndarUpdate;
        UtilityAction Andar = Worker_SU.CreateAction(AndarFactorConstante, Andar_action, finishOnComplete: true);

        FunctionalAction BuscarMancha_action = new FunctionalAction();
        BuscarMancha_action.onStarted = StartLimpiar;
        BuscarMancha_action.onUpdated = UpdateMovingLimpiar;
        State BuscarMancha = FSM_Limpiar.CreateState(BuscarMancha_action);

        FunctionalAction Limpiar_1_action = new FunctionalAction();
        Limpiar_1_action.onUpdated = LimpiarUpdate;
        State Limpiar_1 = FSM_Limpiar.CreateState(Limpiar_1_action);

        StateTransition TransitionBuscarMancha = FSM_Limpiar.CreateTransition(BuscarMancha, Limpiar_1, statusFlags: StatusFlags.Finished);

        ExitTransition SalidaEstado = FSM_Limpiar.CreateExitTransition(Limpiar_1, Status.Success, statusFlags: StatusFlags.Success);

        FunctionalAction IrALaCaja_action = new FunctionalAction();
        IrALaCaja_action.onStarted = StartAtender;
        IrALaCaja_action.onUpdated = UpdateMoving;
        State IrALaCaja = FSM_AtenderCaja.CreateState(IrALaCaja_action);

        FunctionalAction Cobrar_action = new FunctionalAction();
        Cobrar_action.onStarted = StartCobrar;
        Cobrar_action.onUpdated = CobrarUpdate;
        State Cobrar = FSM_AtenderCaja.CreateState(Cobrar_action);

        StateTransition Transition = FSM_AtenderCaja.CreateTransition(IrALaCaja, Cobrar, statusFlags: StatusFlags.Finished);

        ExitTransition ExitCondition = FSM_AtenderCaja.CreateExitTransition(Cobrar, Status.Success, statusFlags: StatusFlags.Success);

        Worker_BT.SetRootNode(sel);
        FSM_Limpiar.SetEntryState(BuscarMancha);
        FSM_AtenderCaja.SetEntryState(IrALaCaja);

        return Worker_BT;
    }

    public float felicidadMedia() => GameManager.Instance.reputation;
    public float ClientesEnCaja() => shopID == 0 ? TiendaManager.Instance.npcPayQueueP1.Count : TiendaManager.Instance.npcPayQueueP2.Count;
    public float ManchasTotales() => GameManager.Instance.updateRubbishList();
    public Status fueDespedido() => sigueContratado ? Status.Success : Status.Failure;


    private void StartEntrarTienda()
    {
        print("entrando");
        currentDestination = shopID == 0 ? GameManager.Instance._spawnPositions[0].position : GameManager.Instance._spawnPositions[1].position;
        currentDestination.x += 1f;
        currentDestination.z += 1f;
        if (sigueContratado) GetComponent<NavMeshAgent>().SetDestination(currentDestination);
    }

    private Status UpdateMovingSalirEntrar()
    {
        return Vector3.Distance(gameObject.transform.position, currentDestination) < 0.5f ? Status.Failure : Status.Running;
        //return GetComponent<NavMeshAgent>().remainingDistance < 0.5f ? Status.Failure : Status.Running;
    }

    private void StartIrseDespedido()
    {
        currentDestination = shopID == 0 ? TiendaManager.Instance.outDoorShopP1.position : TiendaManager.Instance.outDoorShopP2.position;
        GetComponent<NavMeshAgent>().SetDestination(currentDestination);
    }

    private Status IrseUpdate()
    {
        //print("yendome");
        if (Vector3.Distance(gameObject.transform.position, currentDestination) < 0.5f)
        {
            print("en la salida");
            GameManager.Instance.Fire();
            //Destroy(gameObject);
            return Status.Success;
        }
        return Status.Running;
    }

    private void InicializarExtremos()
    {
        //print("inicializando extremos");
        if (shopID == 0)
        {
            minX = (int)TiendaManager.Instance.minXP1.position.x;
            maxX = (int)TiendaManager.Instance.maxXP1.position.x;
            minZ = (int)TiendaManager.Instance.minZP1.position.z;
            maxZ = (int)TiendaManager.Instance.maxZP1.position.z;
        }
        else if (shopID == 1)
        {
            minX = (int)TiendaManager.Instance.minXP2.position.x;
            maxX = (int)TiendaManager.Instance.maxXP2.position.x;
            minZ = (int)TiendaManager.Instance.minZP2.position.z;
            maxZ = (int)TiendaManager.Instance.maxZP2.position.z;
        }
    }

    public void StartAndar()
    {
        print("start andar");
        //clienteCaja.setFinishPayWorker(false);
        GameManager.Instance.isAnyWorkerInPayBox = false;
        canCharge = false;
        InicializarExtremos();
        AndarMethod();
    }
    public void AndarMethod()
    {
        GetComponent<NavMeshAgent>().speed = 3;
        currentDestination = calculateRandomPosition();
        GetComponent<NavMeshAgent>().SetDestination(currentDestination);
    }
    public Vector3 calculateRandomPosition()
    {
        //print("calculando posicion");
        int randomX = Random.Range(minX, maxX);
        int randomZ = Random.Range(minZ, maxZ);
        Vector3 position = new Vector3(randomX, transform.position.y, randomZ);

        if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxDistanceNavMesh, NavMesh.AllAreas))
        {
            position = hit.position;
        }
        return position;
    }
    public Status AndarUpdate()
    {
        if (Vector3.Distance(gameObject.transform.position, currentDestination) < 0.6f)
        {
            print("succeed andar");
            return Status.Success;
        }
        else
        {
            return Status.Running;
        }
    }
    public Status LimpiarUpdate()
    {
        print("limpiar update");
        GetComponent<Animator>().SetTrigger("isCleaning");
        //StartCoroutine(delayMancha(1f));
        if (!manchaActual.isNene)
        {
            manchaActual.Destruir();
        }
        else
        {
            Destroy(manchaActual.gameObject);
        }
        return Status.Success;
    }
    public void StartLimpiar()
    {
        print("start limpiar");
        //clienteCaja.setFinishPayWorker(false);
        GameManager.Instance.isAnyWorkerInPayBox = false;
        canCharge = false;
        GameManager.Instance.workerGoToClean(gameObject, false, null);
        GetComponent<NavMeshAgent>().speed = 5;
        
        currentDestination = manchaActual.transform.position;
        GetComponent<NavMeshAgent>().SetDestination(currentDestination);

    }
    private Status UpdateMoving()
    {
        return Vector3.Distance(gameObject.transform.position, currentDestination) <= 0.7f ? Status.Success : Status.Running;
        //return GetComponent<NavMeshAgent>().remainingDistance < 0.5f ? Status.Success : Status.Running;
    }
    private Status UpdateMovingLimpiar()
    {
        //print($"remaining distance? {Vector3.Distance(gameObject.transform.position, currentDestination)}");
        return Vector3.Distance(gameObject.transform.position, currentDestination) <= 2f ? Status.Success : Status.Running;
        //return GetComponent<NavMeshAgent>().remainingDistance < 0.5f ? Status.Success : Status.Running;
    }
    public Status CobrarUpdate()
    {
        //print("cobrar update");
        if (!canCharge) { print("cobrar update no puede cobrar"); clienteCaja.setFinishPayWorker(false); return Status.Success; }
        if (clienteCaja.isFinishPayWorker() && canCharge)
        {
            print("cobrar update");
            clienteCaja.setFinishPayWorker(false);
            GameManager.Instance.isAnyWorkerInPayBox = false;
            canCharge = false;
            return Status.Success;
        }
        return Status.Running;
    }
    public void StartCobrar()
    {
        print($"cobrar: puede cobrar? :{canCharge}");
        if (canCharge)
        {
            clienteCaja.setHayCajeroEnCaja(true);
            clienteCaja.setWorkerInPay(true);
        }
    }
    public void StartAtender()
    {
        print("start atender");
        GetComponent<NavMeshAgent>().speed = 5;

        GameManager.Instance.workerGoToPay(gameObject, false);

        //desactivar atender
        if (shopID == 0)
        {
            currentDestination = GameManager.Instance.cajaPositionP1.position;
            clienteCaja = TiendaManager.Instance.npcPayQueueP1.First();
        }
        else
        {
            currentDestination = GameManager.Instance.cajaPositionP2.position;
            clienteCaja = TiendaManager.Instance.npcPayQueueP2.First();
        }
        print($"puede cobrar: {canCharge}");
        
        if (canCharge)
        {
            UIManager.Instance.cajero_Canvas.SetActive(false);
            clienteCaja.setFinishPayWorker(false);
            GetComponent<NavMeshAgent>().SetDestination(currentDestination);
        }
    }
}
