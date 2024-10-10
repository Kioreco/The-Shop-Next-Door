using System.Collections.Generic;
using UnityEngine;

public class ListaCompra : MonoBehaviour
{
    List<char> tipo = new List<char>();
    Dictionary<string, int> lista = new Dictionary<string, int>();
    int cont = 0;
    public bool create = true;

    private void Update()
    {
        if (create)
        {
            create = false;
            CrearLista();
        }
    }


    public void CrearLista()
    {
        if (TiendaManager.Instance.vendeRopa == true) tipo.Add('r');
        if (TiendaManager.Instance.vendeOcio == true) tipo.Add('o');
        if (TiendaManager.Instance.vendePapeleria == true) tipo.Add('p');
        if (TiendaManager.Instance.vendeComida == true) tipo.Add('c');

        char tipoObjeto = tipo[Random.Range(0,tipo.Count)];
        int randCantidadProductos = Random.Range(1, 5);

        while(cont <= randCantidadProductos)
        {
            int randUnidades = Random.Range(1, 4);
            lista.Add(TiendaManager.Instance.getRandomProduct(tipoObjeto), randUnidades);
        }

        imprimirLista();
    }

    public void imprimirLista()
    {
        foreach (var par in lista)
        {
            print($"Clave: {par.Key}, Valor: {par.Value}");
        }
    }
}
