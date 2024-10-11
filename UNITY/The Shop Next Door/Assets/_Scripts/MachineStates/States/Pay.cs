using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pay : AStateNPC
{
    public Pay(IContext cntx) : base(cntx) { }
    public override void Enter()
    {
        Debug.Log("paying...");
    }
    public override void FixedUpdate()
    {
    }

    public override void Update()
    {

    }
}
