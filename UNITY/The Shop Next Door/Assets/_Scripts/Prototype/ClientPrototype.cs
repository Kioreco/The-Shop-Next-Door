using System.Collections;
using UnityEngine;

public class ClientPrototype : MonoBehaviour
{
    public MonoBehaviour npcPrototype;
    public int maxNumberNPC;
    public int maxActiveInScene;
    public bool allowAddNew = false;
    public bool isCreated = false;
    private ObjectPoolClient npcBasicObjectPool;
    public bool isEnable;

    //instanciar con delay:
    bool isSpawning = false;
    float delay = 1.5f;

    private void Start()
    {
        npcBasicObjectPool = new ObjectPoolClient((IContext) npcPrototype, maxNumberNPC, allowAddNew);
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
                    IContext npcBasic = createNpcBasic();
                }
                allowAddNew = false;
            }
        }
    }
    private IContext createNpcBasic()
    {
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
