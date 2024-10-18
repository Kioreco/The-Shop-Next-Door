using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ClientPrototype : MonoBehaviour
{
    public MonoBehaviour snowFlakePrototype;
    public int initialNumberOfSnowFlakes = 1;
    public bool allowAddNewSnowFlakes = false;
    public float snowFlakesPerSecond;
    public float maxSnowFlakeSpeed = 1f;
    public float spreadAreaExtent = 30f;

    private float _halfSpreadAreaExtent;
    private float _lastSnowFlake;
    private ObjectPool _snowFlakesPool;


    private void Start()
    {
        _snowFlakesPool = new ObjectPool((IContext)snowFlakePrototype, initialNumberOfSnowFlakes, allowAddNewSnowFlakes);
        _halfSpreadAreaExtent = spreadAreaExtent / 2;
    }

    private void Update()
    {
        //if (snowing)
        //{
        //    _lastSnowFlake += Time.deltaTime;
        //    if (_lastSnowFlake >= 1)
        //    {
        //        for (int i = 0; i < snowFlakesPerSecond; i++)
        //        {
        //            IContext snowFlake = CreateSnowFlake();
        //        }
        //    }
        //}

    }

    private IContext CreateSnowFlake()
    {
        IContext snowFlake = (IContext)_snowFlakesPool.GetPoolableObject();

        //if (snowFlake)
        //{
        //    snowFlake.getObjectPool() = _snowFlakesPool;
        //}

        return snowFlake;
    }
}
