using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TiendaManager : MonoBehaviour
{
    [Header("Estanterias")]
    [SerializeField] List<GameObject> estanterias = new List<GameObject>();

    [Header("Tipo Tienda")]
    public bool vendeRopa;
    public bool vendePapeleria;
    public bool vendeComida;
    public bool vendeOcio;

    [Header("Objetos Tienda")]
    Dictionary<string, Producto> RopaYCalzado = new Dictionary<string, Producto>();
    Dictionary<string, Producto> PapeleriaYArte = new Dictionary<string, Producto>();
    Dictionary<string, Producto> Comida = new Dictionary<string, Producto>();
    Dictionary<string, Producto> JuegosPeliculasMusica = new Dictionary<string, Producto>();

    public static TiendaManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #region inicializacion Objetos
    void InicializarRopa()
    {
        RopaYCalzado.Add("camisa", new Producto(19.99f, 50, 0, 'r', false));
        RopaYCalzado.Add("jerseis", new Producto( 24.99f, 50, 0, 'r', false));
        RopaYCalzado.Add("vestidos", new Producto( 30.00f, 50, 0, 'r', false));
        RopaYCalzado.Add("pantalones", new Producto(25.99f, 50, 0, 'r', false));
        RopaYCalzado.Add("faldas", new Producto(20.00f, 50, 0, 'r', false));
        RopaYCalzado.Add("pijamas", new Producto(7.50f, 50, 0, 'r', false));
        RopaYCalzado.Add("deportivas", new Producto(55.00f, 50, 0, 'r', false));
        RopaYCalzado.Add("edgy", new Producto(69.00f, 50, 0, 'r', false));
    }
    void InicializarPapeleria()
    {
        PapeleriaYArte.Add("libro fantasia", new Producto( 19.99f, 50, 0, 'p', false));
        PapeleriaYArte.Add("libro romance", new Producto(24.99f, 50, 0, 'p', false));
        PapeleriaYArte.Add("libro historia", new Producto(29.99f, 50, 0, 'p', false));
        PapeleriaYArte.Add("pinturas", new Producto(14.99f, 50, 0, 'p', false));
        PapeleriaYArte.Add("rotuladores", new Producto(20.99f, 50, 0, 'p', false));
        PapeleriaYArte.Add("lienzos", new Producto(12.50f, 50, 0, 'p', false));
    }
    void InicializarComida()
    {
        Comida.Add("manzana", new Producto(0.85f, 50, 0, 'c', false));
        Comida.Add("sandia", new Producto(2.75f, 50, 0, 'c', false));
        Comida.Add("melon", new Producto(2.15f, 50, 0, 'c', false));
        Comida.Add("pizza", new Producto(3.45f, 50, 0, 'c', false));
        Comida.Add("croquetas", new Producto(4.60f, 50, 0, 'c', false));
        Comida.Add("calabaza", new Producto(5.00f, 50, 0, 'c', false));
        Comida.Add("carne", new Producto(12.50f, 50, 0, 'c', false));
        Comida.Add("pescado", new Producto(12.50f, 50, 0, 'c', false));
    }
    void InicializarOcio()
    {
        JuegosPeliculasMusica.Add("HQLNNS", new Producto(5.60f, 50, 0, 'o', false));
        JuegosPeliculasMusica.Add("Decor Dilemma", new Producto(5.60f, 50, 0, 'o', false));
        JuegosPeliculasMusica.Add("Virtual velocity", new Producto(5.60f, 50, 0, 'o', false));
        JuegosPeliculasMusica.Add("Shrek 1", new Producto(4.75f, 50, 0, 'o', false));
        JuegosPeliculasMusica.Add("Shrek 2", new Producto(4.75f, 50, 0, 'o', false));
        JuegosPeliculasMusica.Add("Shrek 3", new Producto(4.75f, 50, 0, 'o', false));
        JuegosPeliculasMusica.Add("CD-TaylorSwift", new Producto(12.00f, 50, 0, 'o', false));
        JuegosPeliculasMusica.Add("CD-DAMN", new Producto(12.00f, 50, 0, 'o', false));
        JuegosPeliculasMusica.Add("CD-NombreDireccion", new Producto(12.00f, 50, 0, 'o', false));
    }
    #endregion

    public Vector3 buscarEstanteria(string producto)
    {
        foreach (var item in estanterias)
        {
            if(item.GetComponent<Estanteria>().TieneElemento(producto) == true) return item.transform.position;
        }

        return new Vector3(0, 0, 0);
    }

    public Producto getProducto(string producto, char tipo, int stockEstanteria)
    {
        if(tipo == 'r')
        {
            RopaYCalzado.TryGetValue(producto, out var result);
            if (result != null)
            {
                result.gestionarStockEstanteriaYAlmacen(stockEstanteria);
                return result;
            }
        }
        else if(tipo == 'p')
        {
            PapeleriaYArte.TryGetValue(producto, out var result);
            if (result != null)
            {
                result.gestionarStockEstanteriaYAlmacen(stockEstanteria);
                return result;
            }
        }
        else if(tipo == 'c')
        {
            Comida.TryGetValue(producto, out var result);
            if (result != null)
            {
                result.gestionarStockEstanteriaYAlmacen(stockEstanteria);
                return result;
            }
        }
        else if(tipo == 'o')
        {
            JuegosPeliculasMusica.TryGetValue(producto, out var result);
            if (result != null)
            {
                result.gestionarStockEstanteriaYAlmacen(stockEstanteria);
                return result;
            }
        }
        return new Producto(0, 0, 0, 'c', true);
    }

    public string getRandomProduct(char tipo)
    {
        if (tipo == 'r')
        {
            return RopaYCalzado.Keys.ElementAt(Random.Range(0, RopaYCalzado.Count));
        }
        else if (tipo == 'p')
        {
            return PapeleriaYArte.Keys.ElementAt(Random.Range(0, PapeleriaYArte.Count));
        }
        else if (tipo == 'c')
        {
            return Comida.Keys.ElementAt(Random.Range(0, Comida.Count));
        }
        else if (tipo == 'o')
        {
            return JuegosPeliculasMusica.Keys.ElementAt(Random.Range(0, JuegosPeliculasMusica.Count));
        }
        return "";
    }

}
