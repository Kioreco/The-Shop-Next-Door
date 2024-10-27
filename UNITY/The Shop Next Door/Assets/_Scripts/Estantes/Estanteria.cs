using Assets.Scripts.MachineStates.Classes;
using System.Collections.Generic;
using UnityEngine;

public class Estanteria : MonoBehaviour
{
    public char tipoObj;
    public List<string> objetosEstanteria = new List<string>();
    public int maxElem = 20;

    string stateNPCBuying = "WalkToShelf";


    private void Start()
    {
        //TiendaManager.Instance.reponerEstanteria(objetosEstanteria[0],tipoObj, maxElem);
        //TiendaManager.Instance.reponerEstanteria(objetosEstanteria[1],tipoObj, maxElem);
        //TiendaManager.Instance.reponerEstanteria(objetosEstanteria[2],tipoObj, maxElem);

        //GameManager.Instance.
    }

    //public void Reponer(string s)
    //{
    //    TiendaManager.Instance.reponerEstanteria(s, tipoObj, maxElem);
    //}

    public void CogerElemento(string s, int c)
    {
        TiendaManager.Instance.cogerDeEstanteria(s, tipoObj, c);
    }

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
            other.gameObject.GetComponent<Context>().setIsInColliderShelf(true);
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
            other.gameObject.GetComponent<Context>().setIsInColliderShelf(false);
        }

        if (other.CompareTag("Player"))
        {
            //desactivar interfaz de acciones en la estantería
        }
    }
}
