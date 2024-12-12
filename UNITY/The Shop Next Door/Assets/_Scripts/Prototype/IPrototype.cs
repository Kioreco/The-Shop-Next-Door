using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPrototype<T>
{
    T Clone(Vector3 pos, Quaternion rot);
}
