using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolClient : IObjectPool<IContext>
{
    private IContext objectPrototype;
    private readonly bool canAdd;

    private List<IContext> npcs;
    Transform instancePos;
    private int countNpcsActive;

    public ObjectPoolClient(IContext ctx, int countNpcMax, bool ca)
    {
        Debug.Log("creado");
        objectPrototype = ctx;
        canAdd = ca;
        npcs = new List<IContext>(countNpcMax);
        countNpcsActive = 0;

        for (int i = 0; i < countNpcMax; i++)
        {
            IContext npc = CreateObject();
            npc.isActive = false;
            npcs.Add(npc);
        }
    }

    public IContext GetPoolableObject()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            if (!npcs[i].isActive)
            {
                npcs[i].isActive = true;
                countNpcsActive += 1;
                return npcs[i];
            }
        }

        if (canAdd)
        {
            IContext newObj = CreateObject();
            newObj.isActive = true;
            npcs.Add(newObj);

            countNpcsActive += 1;
            return newObj;
        }

        return null;
    }

    public void Release(IContext obj)
    {
        //Debug.Log("release");
        obj.isActive = false;
        countNpcsActive -= 1;
        obj.Reset();
    }

    private IContext CreateObject()
    {
        if (TiendaManager.Instance.ID == 0 && TiendaManager.Instance.player.IsOwner) instancePos = TiendaManager.Instance.npcPositionInitialP1;
        else if (TiendaManager.Instance.ID == 1 && TiendaManager.Instance.player.IsOwner) instancePos = TiendaManager.Instance.npcPositionInitialP2;
        IContext newObj = objectPrototype.Clone(instancePos.position, instancePos.rotation);
        return newObj;
    }

    public int GetCount()
    {
        return npcs.Count;
    }

    public int GetActive()
    {
        return countNpcsActive;
    }
}
