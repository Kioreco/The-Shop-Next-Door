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
    float delay = 1.5f;

    private void Start()
    {
        if (esClienteGenerico) npcBasicObjectPool = new ObjectPoolClient(npcPrefabs, maxNumberNPC, allowAddNew, true);
        else npcBasicObjectPool = new ObjectPoolClient(npcPrefabs, maxNumberNPC, allowAddNew, false);
    }

    private void Update()
    {
        if (isEnable & isCreated & !isSpawning)
        {
            if (npcBasicObjectPool.GetActive() < maxActiveInScene && !allowAddNew)
            {
                StartCoroutine(ActivateWithDelay());
            }
            if (allowAddNew)
            {
                for (int i = npcBasicObjectPool.GetActive(); i < npcBasicObjectPool.GetCount(); i++)
                {
                    //if (esClienteGenerico) {  IContext npcBasic = CreateRandomNpc(); }
                    //else { IContext npcBasic = createNpcBasic(); }
                    IContext npcBasic = createNpcBasic();
                }
                allowAddNew = false;
            }
        }
    }
    //private IContext CreateRandomNpc()
    //{
    //    print("create random npoc");
    //    MonoBehaviour selectedPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];

    //    npcBasicObjectPool.setPrototype((IContext)selectedPrefab);

    //    IContext npc = (IContext)npcBasicObjectPool.GetPoolableObject();
    //    npc.isActive = true;

    //    if (npc != null)
    //    {
    //        npc.setObjectPool(npcBasicObjectPool);
    //    }

    //    return npc;

    //}
    private IContext createNpcBasic()
    {
        //print("crear npc basico");
        IContext npcBasic = (IContext)npcBasicObjectPool.GetPoolableObject();
        npcBasic.isActive = true;

        if (npcBasic != null)
        {
            npcBasic.setObjectPool(npcBasicObjectPool);
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
}
