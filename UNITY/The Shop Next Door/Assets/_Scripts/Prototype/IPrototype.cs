using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPrototype
{
    IContext Clone(string n, Vector3 pos, Quaternion rot);
}
