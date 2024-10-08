using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoEstanteria : MonoBehaviour
{
    public string nombre { get; private set; }
    public int cantidad { get; set; }

    public ObjetoEstanteria(string n, int c)
    {
        nombre = n;
        cantidad = c;
    }

    public void Coger(int c)
    {
        if (cantidad - c < 0) cantidad = 0;
        else cantidad -= c;
    }
}
