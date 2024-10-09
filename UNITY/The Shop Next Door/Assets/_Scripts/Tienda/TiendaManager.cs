using System.Collections.Generic;
using UnityEngine;

public class TiendaManager : MonoBehaviour
{
    [Header("Tipo Tienda")]
    [SerializeField] bool vendeRopa;
    [SerializeField] bool vendePapeleria;
    [SerializeField] bool vendeComida;
    [SerializeField] bool vendeOcio;

    [Header("Objetos Tienda")]
    Dictionary<Producto, bool> RopaYCalzado = new Dictionary<Producto, bool>();
    Dictionary<Producto, bool> PapeleriaYArte = new Dictionary<Producto, bool>();
    Dictionary<Producto, bool> Comida = new Dictionary<Producto, bool>();
    Dictionary<Producto, bool> JuegosPeliculasMusica = new Dictionary<Producto, bool>();


    void InicializarRopa()
    {
        RopaYCalzado.Add(new Producto("camisa", 19.99f, 50, 'r'), false);
        RopaYCalzado.Add(new Producto("jerseis", 24.99f, 50, 'r'), false);
        RopaYCalzado.Add(new Producto("vestidos", 30.00f, 50, 'r'), false);
        RopaYCalzado.Add(new Producto("pantalones", 25.99f, 50, 'r'), false);
        RopaYCalzado.Add(new Producto("faldas", 20.00f, 50, 'r'), false);
        RopaYCalzado.Add(new Producto("pijamas", 7.50f, 50, 'r'), false);
        RopaYCalzado.Add(new Producto("deportivas", 55.00f, 50, 'r'), false);
        RopaYCalzado.Add(new Producto("edgy", 69.00f, 50, 'r'), false);
    }
    void InicializarPapeleria()
    {
        PapeleriaYArte.Add(new Producto("libro fantasia", 19.99f, 50, 'p'), false);
        PapeleriaYArte.Add(new Producto("libro romance", 24.99f, 50, 'p'), false);
        PapeleriaYArte.Add(new Producto("libro historia", 29.99f, 50, 'p'), false);
        PapeleriaYArte.Add(new Producto("pinturas", 14.99f, 50, 'p'), false);
        PapeleriaYArte.Add(new Producto("rotuladores", 20.99f, 50, 'p'), false);
        PapeleriaYArte.Add(new Producto("lienzos", 12.50f, 50, 'p'), false);
    }
    void InicializarComida()
    {
        Comida.Add(new Producto("manzana", 0.85f, 50, 'c'), false);
        Comida.Add(new Producto("sandía", 2.75f, 50, 'c'), false);
        Comida.Add(new Producto("melón", 2.15f, 50, 'c'), false);
        Comida.Add(new Producto("pizza", 3.45f, 50, 'c'), false);
        Comida.Add(new Producto("croquetas", 4.60f, 50, 'c'), false);
        Comida.Add(new Producto("calabaza", 5.00f, 50, 'c'), false);
        Comida.Add(new Producto("carne", 12.50f, 50, 'c'), false);
        Comida.Add(new Producto("pescado", 12.50f, 50, 'c'), false);
    }
    void InicializarOcio()
    {
        JuegosPeliculasMusica.Add(new Producto("HQLNNS", 5.60f, 50, 'c'), false);
        JuegosPeliculasMusica.Add(new Producto("Decor Dilemma", 5.60f, 50, 'c'), false);
        JuegosPeliculasMusica.Add(new Producto("Virtual velocity", 5.60f, 50, 'c'), false);
        JuegosPeliculasMusica.Add(new Producto("Shrek 1", 4.75f, 50, 'c'), false);
        JuegosPeliculasMusica.Add(new Producto("Shrek 2", 4.75f, 50, 'c'), false);
        JuegosPeliculasMusica.Add(new Producto("Shrek 3", 4.75f, 50, 'c'), false);
        JuegosPeliculasMusica.Add(new Producto("CD-TaylorSwift", 12.00f, 50, 'c'), false);
        JuegosPeliculasMusica.Add(new Producto("CD-DAMN", 12.00f, 50, 'c'), false);
        JuegosPeliculasMusica.Add(new Producto("CD-NombreDireccion", 12.00f, 50, 'c'), false);
    }
}
