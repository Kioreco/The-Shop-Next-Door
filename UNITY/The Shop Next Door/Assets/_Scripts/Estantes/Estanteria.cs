using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estanteria : MonoBehaviour
{
    public char tipoObj;
    public string obj1;
    public string obj2;
    public string obj3;
    Dictionary<string, Producto> elementos = new Dictionary<string, Producto>();

    public int maxElem = 20;

    private void Awake()
    {
        //print(elementos);
        //print(TiendaManager.Instance.getProducto(obj1, tipoObj, maxElem));
        elementos.Add(obj1, TiendaManager.Instance.getProducto(obj1, tipoObj, maxElem));
        elementos.Add(obj2, TiendaManager.Instance.getProducto(obj2, tipoObj, maxElem));
        elementos.Add(obj3, TiendaManager.Instance.getProducto(obj3, tipoObj, maxElem));
        //elementos[0] = new ObjetoEstanteria(e1, maxElem);
        //elementos[1] = new ObjetoEstanteria(e2, maxElem);
        //elementos[2] = new ObjetoEstanteria(e3, maxElem);
    }

    public void Reponer(string s)
    {
        //foreach (var item in elementos) 
        //{
        //    //if (item. == s) item.cantidad = maxElem;
        //}
    }

    public void CogerElemento(string s, int c)
    {
        //foreach (var item in elementos)
        //{
        //    //if (item.prod.nombre == s) item.Coger(c);
        //    //aqui habria que actualizar la ui
        //}
    }

    public bool TieneElemento(string s)
    {
        if (elementos.TryGetValue(s, out var o)) return true;
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
