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
    }

    //public override bool Equals(object obj)
    //{
    //    if (obj is Producto aux)
    //    {
    //        return
    //            aux.precio == precio &&
    //            aux.stockAlmacen == stockAlmacen &&
    //            aux.stockEstanteria == stockEstanteria &&
    //            aux.tipo == tipo &&
    //            aux.disponible == disponible;
    //    }

    //    return false;
    //}

    //public override int GetHashCode()
    //{
    //    int hashCode = 17;

    //    hashCode = hashCode * 23 + precio.GetHashCode();
    //    hashCode = hashCode * 23 + stockAlmacen.GetHashCode();
    //    hashCode = hashCode * 23 + stockEstanteria.GetHashCode();
    //    hashCode = hashCode * 23 + tipo.GetHashCode();
    //    hashCode = hashCode * 23 + disponible.GetHashCode();

    //    return hashCode;
    //}

    public override string ToString()
    {
        return $"precio: {precio}\nstockAlmacen: {stockAlmacen}\nstockEstanteria: {stockEstanteria}\ntipo: {tipo}\ndiposnible: {disponible}";
    }

    public void gestionarStockEstanteriaYAlmacen(int a)
    {
        int repuestos = 20 - stockEstanteria;
        
        stockEstanteria += repuestos;
        stockAlmacen += a - repuestos;
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
