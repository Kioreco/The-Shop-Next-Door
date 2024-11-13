using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolRubish : IObjectPool<RubbishInstanciator>
{
    private RubbishInstanciator objectPrototype;
    private readonly bool canAdd;

    private List<RubbishInstanciator> RubbishList;
    Transform instancePos;
    private int countNpcsActive;

    public ObjectPoolRubish(RubbishInstanciator ctx, int countNpcMax, bool ca)
    {
        Debug.Log("creado");
        objectPrototype = ctx;
        canAdd = ca;
        RubbishList = new List<RubbishInstanciator>(countNpcMax);
        countNpcsActive = 0;

        for (int i = 0; i < countNpcMax; i++)
        {
            RubbishInstanciator rubbish = CreateObject();
            rubbish.isActive = false;
            RubbishList.Add(rubbish);
        }
    }

    public RubbishInstanciator GetPoolableObject()
    {
        for (int i = 0; i < RubbishList.Count; i++)
        {
            if (!RubbishList[i].isActive)
            {
                RubbishList[i].isActive = true;
                countNpcsActive += 1;
                return RubbishList[i];
            }
        }

        if (canAdd)
        {
            RubbishInstanciator newObj = CreateObject();
            newObj.isActive = true;
            RubbishList.Add(newObj);

            countNpcsActive += 1;
            return newObj;
        }

        return null;
    }

    public void Release(RubbishInstanciator obj)
    {
        //Debug.Log("release");
        obj.isActive = false;
        countNpcsActive -= 1;
        obj.Reset();
    }

    private RubbishInstanciator CreateObject()
    {
        //pos aleatoria
        RubbishInstanciator newObj = objectPrototype.Clone(instancePos.position, instancePos.rotation);
        return newObj;
    }

    public int GetCount()
    {
        return RubbishList.Count;
    }

    public int GetActive()
    {
        return countNpcsActive;
    }
}
