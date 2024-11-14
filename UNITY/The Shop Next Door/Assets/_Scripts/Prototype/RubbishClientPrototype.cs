using UnityEngine;

public class RubbishClientPrototype : MonoBehaviour
{
    public MonoBehaviour rubishPrototype;
    public int maxManchas;
    public int maxActiveInScene;
    public bool allowAddNew = false;
    public bool isCreated = false;
    private ObjectPoolRubish rubbishInstanciatorPool;
    public bool isEnable;

    private void Start()
    {
        rubbishInstanciatorPool = new ObjectPoolRubish((RubbishController)rubishPrototype, maxManchas, allowAddNew);
    }
    private void Update()
    {
        if (isEnable)
        {
            if (isCreated && rubbishInstanciatorPool.GetActive() < maxActiveInScene && !allowAddNew)
            {
                RubbishController npcBasic = createRubishInstance();
            }
            if (isCreated && allowAddNew)
            {
                for (int i = rubbishInstanciatorPool.GetActive(); i < rubbishInstanciatorPool.GetCount(); i++)
                {
                    RubbishController npcBasic = createRubishInstance();
                }
                allowAddNew = false;
            }
        }

    }

    private RubbishController createRubishInstance()
    {
        RubbishController rubishInstance = (RubbishController)rubbishInstanciatorPool.GetPoolableObject();
        rubishInstance.isActive = true;

        if (rubishInstance != null)
        {
            rubishInstance.setObjectPool(rubbishInstanciatorPool);
        }

        return rubishInstance;
    }
}
