using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TalkToAWorker : AStateNPC
{
    GameObject worker;
    bool isInWorker;
    Vector3 currentDestination;

    public TalkToAWorker(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        contexto.setIfDudaResuelta(false);
        //contexto.
        if(!contexto.getIsKaren()) contexto.activarCanvasDuda();
        else contexto.activarCanvasEnfado();

        Debug.Log($"duda: {contexto.getTieneDuda()}");
        //Physics.IgnoreLayerCollision(GameManager.Instance._player.playerLayer, GameManager.Instance._player.npcLayer, false);

        worker = GameManager.Instance._player.gameObject;
        currentDestination = worker.transform.position;

        contexto.getNavMesh().SetDestination(currentDestination);
    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        //if (Physics.GetIgnoreLayerCollision(GameManager.Instance._player.playerLayer, GameManager.Instance._player.npcLayer))
        //{
        //    Debug.Log("ignorando layer collision");
        //    Physics.IgnoreLayerCollision(GameManager.Instance._player.playerLayer, GameManager.Instance._player.npcLayer, false);
        //}

        if (!isInWorker && Vector3.Distance(contexto.GetTransform().position, worker.transform.position) <= 5)
        {
            contexto.setIfImInPlayer(true);
            isInWorker = true;
            GameManager.Instance._player.disableMovement();
            GameManager.Instance._player._playerAnimator.SetBool("playerTalking", true);
            contexto.GetGameObject().transform.LookAt(contexto.GetGameObject().transform.position +
                        GameManager.Instance.activeCamera.transform.rotation * Vector3.forward,
                        GameManager.Instance.activeCamera.transform.rotation * Vector3.up);
            contexto.GetAnimator().SetTrigger("apareceDuda");
            contexto.GetAnimator().SetBool("tieneDuda", true);


            UIManager.Instance.CreateDuda_UI(contexto.getProductoDuda(), contexto.getIsKaren());
            Debug.Log($"está en worker cliente? {contexto.getIsKaren()}"); 
        }
        if (!contexto.getIfImInPlayer() && Vector3.Distance(contexto.getNavMesh().transform.position, worker.transform.position) > 5f)//ELEFANTE CAMBIAR LO DE KAREN
        {
            if(!contexto.getCanvasDuda().activeInHierarchy && !contexto.getIsKaren()) contexto.activarCanvasDuda();
            if(!contexto.getCanvasQueja().activeInHierarchy && !contexto.getIsKaren()) contexto.activarCanvasEnfado();
            //Debug.Log("se movió");
            currentDestination = worker.transform.position;
            contexto.getNavMesh().SetDestination(currentDestination);
        }
        if (contexto.getIfDudaResuelta())
        {
            Debug.Log("duda resuelta o queja");
            contexto.activarDelayDuda();
            GameManager.Instance._player.enableMovement(false);
            contexto.setTieneDuda(false);
            contexto.setCanComplain(false);
            contexto.setIfDudaResuelta(false);
            contexto.setIfImInPlayer(false);
            //Physics.IgnoreLayerCollision(GameManager.Instance._player.playerLayer, GameManager.Instance._player.npcLayer, true);
            contexto.SetState(contexto.getPilaState().Pop());
        }
        //if (contexto.getNavMesh().remainingDistance <= 0.6 && !isInWorker && contexto.getIsKaren())
        //{
        //    isInWorker = true;
        //    Debug.Log("esta en worker");
        //}
        //if (isInWorker && contexto.getIsKaren()) lastSeek += Time.deltaTime;

        //if (lastSeek >= secondsToSeek)
        //{
        //    lastSeek = 0f;
        //    isInWorker = false;
        //    contexto.setCanComplain(false);

        //    contexto.SetState(contexto.getPilaState().Pop());
        //}
    }
}