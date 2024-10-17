using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TiendaManager : MonoBehaviour
{
    [Header("Estanterias")]
    [SerializeField] List<GameObject> shelfs = new List<GameObject>();
    public Transform positionColliderPayBox;
    public List<Transform> posPayCheckpoints;
    public Transform outDoorShop;

    public Queue<IContext> npcPayQueue = new Queue<IContext>();
    public event EventHandler payQueueChange;

    [Header("Tipo Tienda")]
    public bool sellClothes;
    public bool sellStationery;
    public bool sellFood;
    public bool sellLeisure;

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
            InicializarComida();
            InicializarOcio();
            InicializarPapeleria();
            InicializarRopa();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #region inicializacion Objetos
    void InicializarRopa()
    {
        RopaYCalzado.Add("camisa", new Producto(19.99f, 50, 0, 'r', true));
        RopaYCalzado.Add("jerseis", new Producto( 24.99f, 50, 0, 'r', true));
        RopaYCalzado.Add("vestidos", new Producto( 30.00f, 50, 0, 'r', false));
        RopaYCalzado.Add("pantalones", new Producto(25.99f, 50, 0, 'r', true));
        RopaYCalzado.Add("faldas", new Producto(20.00f, 50, 0, 'r', true));
        RopaYCalzado.Add("pijamas", new Producto(7.50f, 50, 0, 'r', false));
        RopaYCalzado.Add("deportivas", new Producto(55.00f, 50, 0, 'r', true));
        RopaYCalzado.Add("edgy", new Producto(69.00f, 50, 0, 'r', true));
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
        Comida.Add("manzana", new Producto(0.85f, 50, 0, 'c', true));
        Comida.Add("sandia", new Producto(2.75f, 50, 0, 'c', true));
        Comida.Add("melon", new Producto(2.15f, 50, 0, 'c', false));
        Comida.Add("pizza", new Producto(3.45f, 50, 0, 'c', true));
        Comida.Add("croquetas", new Producto(4.60f, 50, 0, 'c', false));
        Comida.Add("calabaza", new Producto(5.00f, 50, 0, 'c', true));
        Comida.Add("carne", new Producto(12.50f, 50, 0, 'c', true));
        Comida.Add("pescado", new Producto(12.50f, 50, 0, 'c', true));
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

    public Dictionary<string,Producto> getDictionaryAccType(char tipo)
    {
        if (tipo == 'r')
        {
            return RopaYCalzado;
        }
        else if (tipo == 'p')
        {
            return PapeleriaYArte;
        }
        else if (tipo == 'c')
        {
            return Comida;
        }
        else if (tipo == 'o')
        {
            return JuegosPeliculasMusica;
        }
        return new Dictionary<string, Producto>();
    }
    #endregion

    public Vector3 buscarEstanteria(string producto)
    {
        foreach (var item in shelfs)
        {
            if(item.GetComponent<Estanteria>().TieneElemento(producto) == true) return item.transform.position;
        }

        return new Vector3(0, 0, 0);
    }

    public string getRandomProduct(char tipo)
    {
        Dictionary<string, Producto> aux = getDictionaryAccType(tipo);
        int rand = UnityEngine.Random.Range(0, aux.Count);
        if (aux[aux.Keys.ElementAt(rand)].disponible == true)
        {
            return aux.Keys.ElementAt(rand);
        }
        else return "";
    }

    public void reponerEstanteria(string s, char tipo, int cantidad)
    {
        if (getDictionaryAccType(tipo).TryGetValue(s, out var result))
        {
            result.gestionarStockEstanteriaYAlmacen(cantidad);
        }
    }

    public void cogerDeEstanteria(string s, char tipo, int cantidad)
    {
        if (getDictionaryAccType(tipo).TryGetValue(s, out var result))
        {
            //print($"antes: {result}");
            result.cogerProducto(cantidad);
            //print($"despues: {result}");
        }
    }

    public int cogerSitioCola(IContext npc)
    {
        //print($"cojo sitio antes: {cajaPago.transform.position}");
        //posicionEnLaCola.transform.position += new Vector3 (0, 0, 1.2f);
        //print($"cojo sitio despues: {cajaPago.transform.position}");
        //print($"npc añadido en la cola, pos: {npcPayQueue.Count}, max checkpoints: {posPayCheckpoints.Count}");
        if (npcPayQueue.Count == 5) return -1;

        npcPayQueue.Enqueue(npc);
        payQueueChange?.Invoke(this, EventArgs.Empty);
        return npcPayQueue.Count;
    }

    public void avanzarLaCola()
    {
        //print($"dejo sitio antes: {cajaPago.transform.position}");
        //if(posicionEnLaCola.position.z <= primeraPosCola.z) posicionEnLaCola.transform.position -= new Vector3(0, 0, 1.2f);
        //print($"dejo sitio despues: {cajaPago.transform.position}");
        if(npcPayQueue.Count == 0) return;
        npcPayQueue.Dequeue();
        //print($"npc quitado de la cola: {npcPayQueue.Count}");
        payQueueChange?.Invoke(this, EventArgs.Empty);

        //return getPositionPayQueue(npc);
    }

    public int getPositionPayQueue(IContext npc)
    {
        int pos = 0;
        foreach (IContext queuedNpc in npcPayQueue)
        {
            //print($"elemento cola: {queuedNpc} npc: {npc}");
            if (queuedNpc == npc) { return pos; }
            pos++;
        }
        return -1;
    }

    public float getPrecioProducto(string s, char tipo, int cantidad)
    {
        if (getDictionaryAccType(tipo).TryGetValue(s, out var result))
        {
            //print($"precio: {result.precio} tipo: {tipo} obj {s}");
            return result.precio * cantidad;
        }
        return 0;
    }
}
