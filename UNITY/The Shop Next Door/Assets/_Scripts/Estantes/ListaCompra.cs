using System.Collections.Generic;
using UnityEngine;

public class ListaCompra : MonoBehaviour
{
    List<char> tipo = new List<char>();
    public Dictionary<string, int> lista = new Dictionary<string, int>();
    int cont = 0;

    private void Start()
    {
        //CrearLista();
    }

    public void CrearLista()
    {
        if (TiendaManager.Instance.vendeRopa == true) tipo.Add('r');
        if (TiendaManager.Instance.vendeOcio == true) tipo.Add('o');
        if (TiendaManager.Instance.vendePapeleria == true) tipo.Add('p');
        if (TiendaManager.Instance.vendeComida == true) tipo.Add('c');

        int tipoObjeto = tipo.Count - 1;
        int contTipo = 0;
        int randCantidadProductos = Random.Range(1, 5);

        while(cont <= randCantidadProductos)
        {
            int randUnidades = Random.Range(1, 4);
            string producto = TiendaManager.Instance.getRandomProduct(tipo[contTipo]);
            if (!lista.ContainsKey(producto))
            {
                lista.Add(producto, randUnidades); //FALTA AÑADIR QUE SOLO PUEDA COGER ELEMENTOS QUE ESTÁN EN LA TIENDA
                cont++;
                if (contTipo < tipoObjeto) contTipo++;
                else if (contTipo == tipoObjeto) contTipo = 0;
            }
        }
        //imprimirLista();
    }

    public void imprimirLista()
    {
        print("\t\tINICIO LISTA");

        foreach (var par in lista)
        {
            print($"Clave: {par.Key}, Valor: {par.Value}");
        }
        print("\t\tFIN LISTA");
    }


    public void listaPrueba()
    {
        lista.Add("camisa", 1);
        lista.Add("manzana", 1);
        lista.Add("faldas", 1);
        lista.Add("carne", 1);
        lista.Add("edgy", 1);
    }
}
