using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estanteria : MonoBehaviour
{
    public string e1;
    public string e2;
    public string e3;
    ObjetoEstanteria[] elementos = new ObjetoEstanteria[3];

    public int maxElem = 20;

    private void Awake()
    {
        elementos[0] = new ObjetoEstanteria(e1, maxElem);
        elementos[1] = new ObjetoEstanteria(e2, maxElem);
        elementos[2] = new ObjetoEstanteria(e3, maxElem);
    }

    public void Reponer(string s)
    {
        foreach (var item in elementos) 
        {
            if (item.nombre == s) item.cantidad = maxElem;
        }
    }

    public void CogerElemento(string s, int c)
    {
        foreach (var item in elementos)
        {
            if (item.nombre == s) item.Coger(c);
            //aqui habria que actualizar la ui
        }
    }

    public bool TieneElemento(string s)
    {
        foreach (var item in elementos)
        {
            if (item.nombre == s) return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print($"collision: {other.gameObject.name}");

        if (other.CompareTag("Player"))
        {
            //activar interfaz 
        }
    }
}
