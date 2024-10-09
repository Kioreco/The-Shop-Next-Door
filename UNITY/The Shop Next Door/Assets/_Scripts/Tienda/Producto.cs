using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producto : MonoBehaviour
{
    string nombre { get; set; }
    float precio { get; set; }
    int stock { get; set; } 
    char tipo {  get; set; }

    public Producto(string n, float p, int s, char t) 
    {
        nombre = n;
        precio = p;
        stock = s;
        tipo = t;
    }

    public override bool Equals(object obj)
    {
        if (obj is Producto aux)
        {
            return aux.nombre == nombre &&
                aux.precio == precio &&
                aux.stock == stock && 
                aux.tipo == tipo;
        }

        return false;
    }

    public override int GetHashCode()
    {
        int hashCode = 17;

        hashCode = hashCode * 23 + nombre.GetHashCode();
        hashCode = hashCode * 23 + precio.GetHashCode();
        hashCode = hashCode * 23 + stock.GetHashCode();
        hashCode = hashCode * 23 + tipo.GetHashCode();

        return hashCode;
    }
}
