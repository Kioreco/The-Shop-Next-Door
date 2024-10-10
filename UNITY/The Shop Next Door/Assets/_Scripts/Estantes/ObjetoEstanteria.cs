using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoEstanteria : MonoBehaviour
{
    Producto prod { get; set; }
    int cantidad { get; set; }

    public ObjetoEstanteria(Producto n, int c)
    {
        prod = n;
        cantidad = c;
    }

    public void Coger(int c)
    {
        if (cantidad - c < 0) cantidad = 0;
        else cantidad -= c;
    }
}
