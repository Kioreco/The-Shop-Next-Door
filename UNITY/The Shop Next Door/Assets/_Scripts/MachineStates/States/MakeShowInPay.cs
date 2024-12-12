using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MakeShowInPay : AStateNPC
{
    float secondsToSeek = 3f; //tiempo de la animación
    float lastSeek = 0f;
    bool isFinished = false;
    public MakeShowInPay(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        Debug.Log("animación enfado máximo");
        contexto.activarCanvasTacanioEnfadado();
        contexto.GetAnimator().SetTrigger("EnfadoMaximo");
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        //está ya en la caja
        if (!isFinished & contexto.getHayCajeroEnCaja()) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            //acaba animación
            isFinished = true;
            contexto.SetState(new LeaveAngry(contexto));
        }
    }
}
