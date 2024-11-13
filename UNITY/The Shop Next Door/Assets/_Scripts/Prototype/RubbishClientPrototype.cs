using UnityEngine;

//public class RubbishClientPrototype : MonoBehaviour
//{
//    public MonoBehaviour rubishPrototype;
//    public int maxManchas;
//    public int maxActiveInScene;
//    public bool allowAddNew = false;
//    public bool isCreated = false;
//    private ObjectPoolClient rubbishInstanciatorPool;
//    public bool isEnable;

//    private void Start()
//    {
//        rubbishInstanciatorPool = new ObjectPool((RubbishManagerInstaciator)rubishPrototype, maxManchas, allowAddNew);
//    }
//    private void Update()
//    {
//        if (isEnable)
//        {
//            if (isCreated && rubbishInstanciatorPool.GetActive() < maxActiveInScene && !allowAddNew)
//            {
//                RubbishManagerInstaciator npcBasic = createRubishInstance();
//            }
//            if (isCreated && allowAddNew)
//            {
//                for (int i = rubbishInstanciatorPool.GetActive(); i < rubbishInstanciatorPool.GetCount(); i++)
//                {
//                    RubbishManagerInstaciator npcBasic = createRubishInstance();
//                }
//                allowAddNew = false;
//            }
//        }

//    }
//    private RubbishManagerInstaciator createRubishInstance()
//    {
//        RubbishManagerInstaciator rubishInstance = (RubbishManagerInstaciator)rubbishInstanciatorPool.GetPoolableObject();
//        rubishInstance.isActive = true;

//        if (rubishInstance != null)
//        {
//            rubishInstance.setObjectPool(rubbishInstanciatorPool);
//        }

//        return rubishInstance;
//    }
//}
