using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPrototype
{
    IContext Clone(Vector3 pos, Quaternion rot);
}
