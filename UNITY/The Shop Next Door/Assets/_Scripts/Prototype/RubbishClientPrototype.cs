using System.Collections;
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

    //instanciar con delay:
    bool isSpawning = false;
    float delay = 30f;

    private void Start()
    {
        rubbishInstanciatorPool = new ObjectPoolRubish((RubbishController)rubishPrototype, maxManchas, allowAddNew);
    }
    private void Update()
    {
        if (isEnable & isCreated & !isSpawning)
        {
            maxActiveInScene = Mathf.Clamp(Mathf.RoundToInt((GameManager.Instance.reputation / 100f) * maxManchas), 0, maxManchas);

            if (rubbishInstanciatorPool.GetActive() < maxActiveInScene && !allowAddNew)
            {
                StartCoroutine(ActivateWithDelay());
            }
            if (allowAddNew)
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

    IEnumerator ActivateWithDelay()
    {
        isSpawning = true;
        while (rubbishInstanciatorPool.GetActive() < maxActiveInScene)
        {
            createRubishInstance();
            yield return new WaitForSeconds(delay); // Espera el delay
        }
        isSpawning = false;
    }
}
