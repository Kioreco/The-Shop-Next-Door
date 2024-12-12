using System;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.BehaviourTrees;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using BehaviourAPI.Core.Perceptions;
using System.Collections;
using Unity.Services.Authentication;

public class NeneBTBehaviour : BehaviourRunner
{
    [Header("Condiciones")]
    public bool sigueEnLaTienda = true;
    public bool hayMancha;
    public bool puedeMancharEnPosicion = false;

    [Header("Información Adicional")]
    public Vector3 currentDestination;
    public int shopID;

    [Header("Limites Andar")]
    public int minX;
    public int maxX;
    public int minZ;
    public int maxZ;
    public float maxDistanceNavMesh = 2f;

    [Header("Mancha")]
    public int maxManchas = 7;
    public int manchas = 0;

    [Header("Variables Poder Manchar")]
    public int umbralManchasRandom;

    [Header("Emociones")]
    [SerializeField] private GameObject canvasEmociones;
    [SerializeField] private GameObject emocionTravesuras;
    [SerializeField] private GameObject emocionEnfado;

    protected override BehaviourGraph CreateGraph()
	{
		BehaviourTree BT_Nene = new BehaviourTree();

        var sigoTiendaPerception = new ConditionPerception(() => sigueEnLaTienda);


        FunctionalAction Andar_action = new FunctionalAction();
		Andar_action.onStarted = StartAndar;
		Andar_action.onUpdated = AndarUpdate;
		LeafNode Andar = BT_Nene.CreateLeafNode(Andar_action);
		
		ConditionNode SigoEnLaTienda_ = BT_Nene.CreateDecorator<ConditionNode>(Andar).SetPerception(sigoTiendaPerception);
		
		FunctionalAction HayMancha_action = new FunctionalAction();
		HayMancha_action.onUpdated = HayManchaEnPosicion;
		LeafNode HayMancha = BT_Nene.CreateLeafNode(HayMancha_action);
		
		FunctionalAction Mancho_action = new FunctionalAction();
		Mancho_action.onStarted = StartManchar;
		Mancho_action.onUpdated = MancharUpdate;
		LeafNode Mancho = BT_Nene.CreateLeafNode(Mancho_action);
		
		SelectorNode SelectorManchas = BT_Nene.CreateComposite<SelectorNode>(false, HayMancha, Mancho);
		SelectorManchas.IsRandomized = false;
		
		SequencerNode SecuenciaComportamiento = BT_Nene.CreateComposite<SequencerNode>(false, SigoEnLaTienda_, SelectorManchas);
		SecuenciaComportamiento.IsRandomized = false;
		
		LoopUntilNode Loop_Until_Fail = BT_Nene.CreateDecorator<LoopUntilNode>(SecuenciaComportamiento);
		Loop_Until_Fail.TargetStatus = Status.Failure;
		Loop_Until_Fail.MaxIterations = -1;
		
		FunctionalAction Irse_action = new FunctionalAction();
		Irse_action.onStarted = StartIrse;
		Irse_action.onUpdated = IrseUpdate;
		LeafNode Irse = BT_Nene.CreateLeafNode(Irse_action);
		
		SelectorNode SelectorInicio = BT_Nene.CreateComposite<SelectorNode>(false, Loop_Until_Fail, Irse);
		SelectorInicio.IsRandomized = false;

        BT_Nene.SetRootNode(SelectorInicio);
        //BT_Nene.Start();
        //BT_Nene.Update();

		return BT_Nene;
	}
	
	private void StartAndar()
	{
        print("start andar");

        InicializarExtremos();
        currentDestination = calculateRandomPosition();
        GetComponent<NavMeshAgent>().SetDestination(currentDestination);
    }
    private void InicializarExtremos()
    {
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
    public Vector3 calculateRandomPosition()
    {
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
       
        if (GetComponent<NavMeshAgent>().remainingDistance < 0.5f)
        {
             //print("andar update");
            return Status.Success;
        }
        else
        {
            return Status.Running;
        }
    }

    private Status HayManchaEnPosicion()
	{
        //print("commprobear manhas en posicion");
        Vector3 down = Vector3.down;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        LayerMask layerMask = LayerMask.GetMask("Rubbish");

        if (Physics.Raycast(rayOrigin, down, out RaycastHit hit, 0.2f, layerMask))
        {
            //print("There is rubbish under the child!");
            hayMancha = true;
        }
        else
        {
            //print("no hay mancha");
            hayMancha = false;
        }

        return hayMancha ? Status.Success : Status.Failure;
    }
	
	private void StartManchar()
	{
        //print("start manchar");
        int random = UnityEngine.Random.Range(0, 100);
        if (random < umbralManchasRandom)
        {
            puedeMancharEnPosicion = true;
        }
        else
        {
            puedeMancharEnPosicion = false;
        }
    }
	
	private Status MancharUpdate()
	{
        //print("manchas update");
        if (puedeMancharEnPosicion && !hayMancha)
        {
            //print("manchando");
            if (maxManchas == manchas)
            {
                sigueEnLaTienda = false;
            }
            else
            {
                activarCanvasTravesuras();
                manchas++;
                TiendaManager.Instance.InstanceGarbage(gameObject.transform);
            }

        }
        return Status.Success;
    }

    public void StartIrse()
    {
        //print("me voy");
        activarCanvasEnfado();
        currentDestination = shopID == 0 ? TiendaManager.Instance.outDoorShopP1.position : TiendaManager.Instance.outDoorShopP2.position;
        GetComponent<NavMeshAgent>().SetDestination(currentDestination);
    }
    public Status IrseUpdate()
    {
        if(GetComponent<NavMeshAgent>().remainingDistance < 0.5f)
        {
            GameManager.Instance.DesactivateNene();
            return Status.Success;
        }
        return Status.Running;
    }

    public void activarCanvasTravesuras()
    {
        print("activo canvas");
        canvasEmociones.SetActive(true);
        emocionTravesuras.SetActive(true);
        StartCoroutine(delayDesactivar(3f, emocionTravesuras));
    }    
    public void activarCanvasEnfado()
    {
        print("activo canvas");
        canvasEmociones.SetActive(true);
        emocionEnfado.SetActive(true);
        StartCoroutine(delayDesactivar(5f, emocionEnfado));
    }

    public IEnumerator delayDesactivar(float delay, GameObject canvas)
    {
        yield return new WaitForSeconds(delay);
        canvas.SetActive(false);
        canvasEmociones.SetActive(false);
    }
}
