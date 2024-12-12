using System.Collections.Generic;
using UnityEngine;

public class ListaCompra
{
    List<char> tipo = new List<char>();
    public Dictionary<string,(char tipo, int cantidad)> lista = new Dictionary<string, (char, int)>();
    int cont = 0;
    public int maxCantidadProductos = 5;
    public int maxCantidadUnidades = 4;


    public void CrearLista()
    {
        cont = 0;
        if (TiendaManager.Instance.sellClothes == true) tipo.Add('r');
        if (TiendaManager.Instance.sellLeisure == true) tipo.Add('o');
        if (TiendaManager.Instance.sellStationery == true) tipo.Add('p');
        if (TiendaManager.Instance.sellFood == true) tipo.Add('c');

        int tipoObjeto = tipo.Count - 1;
        int contTipo = 0;
        int randCantidadProductos = Random.Range(1, maxCantidadProductos);
        //print($"randCantidadProductos: {randCantidadProductos} \t cont: {cont}");
        while(cont <= randCantidadProductos)
        {
            //print($"contaodr: {cont}");
            int randUnidades = Random.Range(1, maxCantidadUnidades);
            string producto = TiendaManager.Instance.getRandomProduct(tipo[contTipo]);
            if (producto != "" && !lista.ContainsKey(producto))
            {
                Agregar(producto, tipo[contTipo], randUnidades); 
                cont++;
                if (contTipo < tipoObjeto) contTipo++;
                else if (contTipo == tipoObjeto) contTipo = 0;
            }
        }
    }
    

    //public void imprimirLista()
    //{
    //    //print("\t\tINICIO LISTA");

    //    foreach (var par in lista)
    //    {
    //        //print($"Clave: {par.Key}, Valor: {par.Value}");
    //    }
    //    //print("\t\tFIN LISTA");
    //}


    //public void listaPrueba()
    //{
    //    Agregar("camisa", 'r', 1);
    //}

    public void Agregar(string s, char c,int n)
    {
        lista[s] = (c, n);
    }
}
