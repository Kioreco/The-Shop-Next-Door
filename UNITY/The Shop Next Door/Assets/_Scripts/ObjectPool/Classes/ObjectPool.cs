using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : IObjectPool
{
    private IContext _objectPrototype;
    private readonly bool canAdd;

    private List<IContext> npcs;

    private int countNpcsActive;

    public ObjectPool(IContext ctx, int countNpcMax, bool ca)
    {
        _objectPrototype = ctx;
        canAdd = ca;
        npcs = new List<IContext>(countNpcMax);
        countNpcsActive = 0;

        for (int i = 0; i < countNpcMax; i++)
        {
            npcs.Add(CreateObject());
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
        obj.isActive = false;
        countNpcsActive -= 1;
        obj.Reset();
    }

    private IContext CreateObject()
    {
        IContext newObj = _objectPrototype.Clone(GameObject.FindGameObjectWithTag("TiendaManager").GetComponent<TiendaManager>().npcPositionInitial.position, GameObject.FindGameObjectWithTag("TiendaManager").GetComponent<TiendaManager>().npcPositionInitial.rotation);
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
