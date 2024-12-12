using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPoolRubish : IObjectPool<RubbishController>
{
    private RubbishController objectPrototype;
    private readonly bool canAdd;

    private List<RubbishController> RubbishList;
    Transform instancePos;
    private int countRubbishActive;

    public ObjectPoolRubish(RubbishController ctx, int countNpcMax, bool ca)
    {
        Debug.Log("creado");
        objectPrototype = ctx;
        canAdd = ca;
        RubbishList = new List<RubbishController>(countNpcMax);
        countRubbishActive = 0;
        instancePos = objectPrototype.transform;

        for (int i = 0; i < countNpcMax; i++)
        {
            RubbishController rubbish = CreateObject();
            rubbish.isActive = false;
            RubbishList.Add(rubbish);
        }
    }

    public RubbishController GetPoolableObject()
    {
        for (int i = 0; i < RubbishList.Count; i++)
        {
            if (!RubbishList[i].isActive)
            {
                RubbishList[i].isActive = true;
                countRubbishActive += 1;
                return RubbishList[i];
            }
        }

        if (canAdd)
        {
            RubbishController newObj = CreateObject();
            newObj.isActive = true;
            RubbishList.Add(newObj);

            countRubbishActive += 1;
            return newObj;
        }

        return null;
    }

    public void Release(RubbishController obj)
    {
        //Debug.Log($"release obj: {obj}");
        obj.tag = "Rubbish";
        obj.isActive = false;
        countRubbishActive -= 1;
        obj.Reset();
    }

    private RubbishController CreateObject()
    {
        //Debug.Log($"creando objs    objectPrototype: {objectPrototype}");
        RubbishController newObj = objectPrototype.Clone(instancePos.position, instancePos.rotation);
        return newObj;
    }

    public int GetCount()
    {
        return RubbishList.Count;
    }

    public int GetActive()
    {
        return countRubbishActive;
    }
}
