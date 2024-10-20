using UnityEngine;

public class ClientPrototype : MonoBehaviour
{
    public MonoBehaviour npcPrototype;
    public int maxNumberNPC;
    public int maxActiveInScene;
    public bool allowAddNew = false;
    public bool isCreated = false;
    private ObjectPool npcBasicObjectPool;

    private void Start()
    {
        npcBasicObjectPool = new ObjectPool((IContext) npcPrototype, maxNumberNPC, allowAddNew);
    }
    private void Update()
    {
        if (isCreated && npcBasicObjectPool.GetActive() < maxActiveInScene && !allowAddNew)
        {
            IContext npcBasic = createNpcBasic();
        }
        if (isCreated && allowAddNew)
        {
            for (int i = npcBasicObjectPool.GetActive(); i < npcBasicObjectPool.GetCount(); i++)
            {
                IContext npcBasic = createNpcBasic();
            }
            allowAddNew = false;
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
}
