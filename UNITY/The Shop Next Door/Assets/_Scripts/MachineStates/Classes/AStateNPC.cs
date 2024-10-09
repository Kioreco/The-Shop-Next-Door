using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AStateNPC : IState
{
    protected IContext contexto;

    public AStateNPC(IContext cntx)
    {
        contexto = cntx;
    }

    public abstract void Enter();
    public abstract void FixedUpdate();
    public abstract void Update();
}
