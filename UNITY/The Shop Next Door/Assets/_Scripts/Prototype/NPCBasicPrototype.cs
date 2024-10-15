using Assets.Scripts.MachineStates.Classes;
using UnityEngine;

public class NPCBasicPrototype : MonoBehaviour, IPrototype
{
    public IContext Clone(string n, Vector3 p, Quaternion r)
    {
        GameObject obj = Instantiate(gameObject, p, r);
        IContext stc = obj.GetComponent<Context>();
        return stc;
    }
}
