using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producto
{
    public float precio { get; set; }
    public int stockAlmacen { get; set; } 
    public int stockEstanteria { get; set; }
    public char tipo {  get; set; }
    public bool disponible { get; set; }   


    public Producto(float p, int sa, int se, char t, bool b) 
    {
        precio = p;
        stockAlmacen = sa;
        stockEstanteria = se;
        tipo = t;
        disponible = b;
        //if(b && (t == 'r' || t == 'c')) Debug.Log($"stockAlmacen: {sa}   stockEstanteria: {se}     tipo: {t}");
    }

    public override string ToString()
    {
        return $"precio: {precio}\nstockAlmacen: {stockAlmacen}\nstockEstanteria: {stockEstanteria}\ntipo: {tipo}\ndiposnible: {disponible}";
    }

    public void gestionarStockEstanteriaYAlmacen(int a)
    {
        if (stockAlmacen - a >= 0)
        {
            stockEstanteria += a;
            stockAlmacen -= a;
        }
        else
        {
            stockEstanteria += stockAlmacen;
            stockAlmacen = 0;
        }
    }

    public int cogerProducto(int c)
    {
        if(stockEstanteria - c >= 0) stockEstanteria -= c;
        else
        {
            stockEstanteria = 0;
            return -1; //devuelve -1 si se ha intentado coger más cantidad de la que quedaba
        }
        return 0; //si ha cogido todos los elementos que podía coger
    }
}
