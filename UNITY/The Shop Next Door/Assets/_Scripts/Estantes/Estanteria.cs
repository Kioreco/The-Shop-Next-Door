using Assets.Scripts.MachineStates.Classes;
using System.Collections.Generic;
using UnityEngine;

public class Estanteria : MonoBehaviour
{
    public char tipoObj;
    public List<string> objetosEstanteria = new List<string>();
    public int maxElem = 20;

    public bool TieneElemento(string s)
    {
        //print($"producto: {s}\n{objetosEstanteria.Contains(s)}");
        if (objetosEstanteria.Contains(s)) return true;
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            other.gameObject.GetComponent<ContextClienteGenerico>().setIsInColliderShelf(true);
        }

        if (other.CompareTag("Player"))
        {
            //activar interfaz de acciones en la estantería
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            other.gameObject.GetComponent<ContextClienteGenerico>().setIsInColliderShelf(false);
        }

        if (other.CompareTag("Player"))
        {
            //desactivar interfaz de acciones en la estantería
        }
    }
}
