using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPrototype : MonoBehaviour
{
    public List<MonoBehaviour> npcPrefabs;
    public int maxNumberNPC;
    public int maxActiveInScene;
    public bool allowAddNew = false;
    public bool isCreated = false;
    private ObjectPoolClient npcBasicObjectPool;
    public bool isEnable;
    [Header("Control tipo clientes")]
    public bool esClienteGenerico;

    //instanciar con delay:
    bool isSpawning = false;
    float delay = 2f;

    private void Start()
    {
        if (esClienteGenerico)
        {
            npcBasicObjectPool = new ObjectPoolClient(npcPrefabs, maxNumberNPC, allowAddNew, true);
            maxActiveInScene = Mathf.Clamp(Mathf.RoundToInt((GameManager.Instance.reputation / 100f) * maxNumberNPC), 3, maxNumberNPC);
            npcBasicObjectPool.updateMaxActive(maxActiveInScene);
        }
        else npcBasicObjectPool = new ObjectPoolClient(npcPrefabs, maxNumberNPC, allowAddNew, false);
    }

    private void Update()
    {
        if (isEnable & isCreated & !isSpawning)
        {
            if (esClienteGenerico)
            {
                //print($"get active IF: {npcBasicObjectPool.GetActive()}  maxactiveInScene: {maxActiveInScene}    count: {npcBasicObjectPool.GetCount()}");

                maxActiveInScene = Mathf.Clamp(Mathf.RoundToInt((GameManager.Instance.reputation / 100f) * maxNumberNPC), 3, maxNumberNPC);
                npcBasicObjectPool.updateMaxActive(maxActiveInScene);
            }

            if (npcBasicObjectPool.GetActive() < maxActiveInScene && !allowAddNew)
            {
                //print($"get active: {npcBasicObjectPool.GetActive()}  maxactiveInScene: {maxActiveInScene}");
                StartCoroutine(ActivateWithDelay());
            }
            if (allowAddNew)
            {
                for (int i = npcBasicObjectPool.GetActive(); i < npcBasicObjectPool.GetCount(); i++)
                {
                    //Debug.Log("faltan clientes");
                    //if (esClienteGenerico) {  IContext npcBasic = CreateRandomNpc(); }
                    //else { IContext npcBasic = createNpcBasic(); }
                    IContext npcBasic = createNpcBasic();
                }
                allowAddNew = false;
            }
        }
    }

    private IContext createNpcBasic()
    {
        //print("crear npc basico");
        IContext npcBasic = (IContext)npcBasicObjectPool.GetPoolableObject();
        

        if (npcBasic != null)
        {
            npcBasic.setObjectPool(npcBasicObjectPool);
            StartCoroutine(delayMethod(npcBasic));
            //npcBasic.isActive = true;
        }
        return npcBasic;
    }

    IEnumerator ActivateWithDelay()
    {
        isSpawning = true;
        while (npcBasicObjectPool.GetActive() < maxActiveInScene)
        {
            createNpcBasic();
            yield return new WaitForSeconds(delay); // Espera el delay
        }
        isSpawning = false;
    }

    IEnumerator delayMethod(IContext obj)
    {
        yield return new WaitForSeconds(delay);
        obj.isActive = true;
    }
}
