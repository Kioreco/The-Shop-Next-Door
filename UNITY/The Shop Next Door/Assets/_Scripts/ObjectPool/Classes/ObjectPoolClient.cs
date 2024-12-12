using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ObjectPoolClient : IObjectPool<IContext>
{
    private IContext objectPrototype;
    private List<MonoBehaviour> objectsProto; 
    private readonly bool canAdd;

    private List<IContext> npcs;
    Transform instancePos;
    private int countNpcsActive;

    bool isGenericClient;

    int maxActive = 0;

    public ObjectPoolClient(List<MonoBehaviour> ctx, int countNpcMax, bool ca, bool isGeneric)
    {
        Debug.Log("creado");
        objectsProto = ctx;
        canAdd = ca;
        npcs = new List<IContext>(countNpcMax);
        countNpcsActive = 0;

        isGenericClient = isGeneric;

        for (int i = 0; i < countNpcMax; i++)
        {
            IContext npc = CreateObject();
            npc.isActive = false;
            npcs.Add(npc);
        }
    }

    //public void setPrototype(IContext ctx)
    //{
    //    Debug.Log($"cambiando prefab prototype: {ctx}");
    //    objectPrototype = ctx;
    //}

    public IContext GetPoolableObject()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            if (countNpcsActive >= maxActive && isGenericClient)
            {
                //Debug.Log("Se alcanzó el máximo de NPCs activos.");
                return null;
            }

            if (!npcs[i].isActive)
            {
                npcs[i].isActive = true;
                countNpcsActive += 1;
                //Debug.Log($"activando   max active: {maxActive}   count: {countNpcsActive}");

                return npcs[i];
            }
        }

        if (canAdd)
        {
            IContext newObj = CreateObject();
            if (newObj != null) // Asegúrate de que el nuevo objeto no sea nulo
            {
                //Debug.Log("obj creado no es nulo");
                newObj.isActive = true;
                npcs.Add(newObj);

                countNpcsActive += 1;
                return newObj;
            }
            //newObj.isActive = true;
            //npcs.Add(newObj);

            //countNpcsActive += 1;
            //return newObj;
        }
        Debug.Log("devuelve null");
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

        IContext newObj;

        if (isGenericClient)
        {
            //Debug.Log("creando cliente generico");
            objectPrototype = (IContext)objectsProto[Random.Range(0, objectsProto.Count)];

            return newObj = objectPrototype.Clone(instancePos.position, instancePos.rotation);
        }
        else
        {
            objectPrototype = (IContext)objectsProto[0];
            return newObj = objectPrototype.Clone(instancePos.position, instancePos.rotation);
        }
    }

    public int GetCount()
    {
        return npcs.Count;
    }

    public int GetActive()
    {
        //Debug.Log($"acitve: {countNpcsActive}");
        return countNpcsActive;
    }

    public void updateMaxActive(int max)
    {
        maxActive = max;
    }
}
