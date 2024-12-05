using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class TiendaManager : MonoBehaviour
{
    public PlayerControler player;
    public int ID;
    [Header("Estanterias P1")]
    [SerializeField] List<GameObject> shelfsP1 = new List<GameObject>();
    public Transform positionColliderPayBoxP1;
    public List<Transform> posPayCheckpointsP1;
    public Transform outDoorShopP1;

    [Header("Pay Queue P1")]
    public Queue<IContext> npcPayQueueP1 = new Queue<IContext>();
    public event EventHandler payQueueChangeP1;

    [Header("NPC Instanciate P1")]
    public Transform npcPositionInitialP1;

    [Header("Estanterias P2")]
    [SerializeField] List<GameObject> shelfsP2 = new List<GameObject>();
    public Transform positionColliderPayBoxP2;
    public List<Transform> posPayCheckpointsP2;
    public Transform outDoorShopP2;

    [Header("Pay Queue P2")]
    public Queue<IContext> npcPayQueueP2 = new Queue<IContext>();
    public event EventHandler payQueueChangeP2;

    [Header("NPC Instanciate P2")]
    public Transform npcPositionInitialP2;

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

    [Header("Bolsas y manchas")]
    public GameObject bolsaBasura;
    public GameObject basura;
    public Transform doorPos1;
    public Transform doorPos2;

    [Header("Trabajadores")]
    public List<GameObject> workersP1;
    public List<GameObject> workersP2;

    [HideInInspector]public int clientesTotales = 0;

    [Header("Limites Instanciación Basura P1")]
    public Transform minXP1;
    public Transform maxXP1;
    public Transform minZP1;
    public Transform maxZP1;
    [Header("Limites Instanciación Basura P2")]
    public Transform minXP2;
    public Transform maxXP2;
    public Transform minZP2;
    public Transform maxZP2;

    [Header("Dudas")]
    public bool yaHayDuda = false;
    public string productoDuda;
    private static readonly object _lockDuda = new object();


    public static TiendaManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
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
        RopaYCalzado.Add("camisa", new Producto(19.99f, 5, UnityEngine.Random.Range(0,5), 'r', true));
        RopaYCalzado.Add("jerseis", new Producto( 24.99f, 0, UnityEngine.Random.Range(0, 5), 'r', true));
        RopaYCalzado.Add("vestidos", new Producto( 30.00f, 0, 0, 'r', false));
        RopaYCalzado.Add("pantalones", new Producto(25.99f, 0, UnityEngine.Random.Range(0, 5), 'r', true));
        RopaYCalzado.Add("faldas", new Producto(20.00f, 0, UnityEngine.Random.Range(0, 5), 'r', true));
        RopaYCalzado.Add("pijamas", new Producto(7.50f, 0, 0, 'r', false));
        RopaYCalzado.Add("deportivas", new Producto(55.00f, UnityEngine.Random.Range(0, 5), 0, 'r', true));
        RopaYCalzado.Add("edgy", new Producto(69.00f, 3, UnityEngine.Random.Range(0, 5), 'r', true));
    }
    void InicializarPapeleria()
    {
        PapeleriaYArte.Add("libro fantasia", new Producto( 19.99f, 0, 0, 'p', false));
        PapeleriaYArte.Add("libro romance", new Producto(24.99f, 0, 0, 'p', false));
        PapeleriaYArte.Add("libro historia", new Producto(29.99f, 0, 0, 'p', false));
        PapeleriaYArte.Add("pinturas", new Producto(14.99f, 0, 0, 'p', false));
        PapeleriaYArte.Add("rotuladores", new Producto(20.99f, 0, 0, 'p', false));
        PapeleriaYArte.Add("lienzos", new Producto(12.50f, 0, 0, 'p', false));
    }
    void InicializarComida()
    {
        Comida.Add("manzana", new Producto(0.85f, 10, UnityEngine.Random.Range(0, 5), 'c', true));
        Comida.Add("sandia", new Producto(2.75f, 2, UnityEngine.Random.Range(0, 5), 'c', true));
        Comida.Add("melon", new Producto(2.15f, 0, 0, 'c', false));
        Comida.Add("pizza", new Producto(3.45f, 0, UnityEngine.Random.Range(0, 5), 'c', true));
        Comida.Add("croquetas", new Producto(4.60f, 0, 0, 'c', false));
        Comida.Add("calabaza", new Producto(5.00f, 0, UnityEngine.Random.Range(0, 5), 'c', true));
        Comida.Add("carne", new Producto(12.50f, 0, UnityEngine.Random.Range(0,5), 'c', true));
        Comida.Add("pescado", new Producto(12.50f, 0, UnityEngine.Random.Range(0,5), 'c', true));
    }
    void InicializarOcio()
    {
        JuegosPeliculasMusica.Add("Hasta que la noche nos separe", new Producto(7.60f, 0, 0, 'o', false));
        JuegosPeliculasMusica.Add("Decor Dilemma", new Producto(5.60f, 0, 0, 'o', false));
        JuegosPeliculasMusica.Add("Virtual Velocity", new Producto(5.60f, 0, 0, 'o', false));
        JuegosPeliculasMusica.Add("Shrek 1", new Producto(4.75f, 0, 0, 'o', false));
        JuegosPeliculasMusica.Add("Shrek 2", new Producto(4.75f, 0, 0, 'o', false));
        JuegosPeliculasMusica.Add("Shrek 3", new Producto(4.75f, 0, 0, 'o', false));
        JuegosPeliculasMusica.Add("CD-TaylorSwift", new Producto(12.00f, 0, 0, 'o', false));
        JuegosPeliculasMusica.Add("CD-DAMN", new Producto(12.00f, 0, 0, 'o', false));
        JuegosPeliculasMusica.Add("CD-NombreDireccion", new Producto(12.00f, 0, 0, 'o', false));
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

    #region estanterias y productos random
    public Vector3 buscarEstanteria(string producto, bool isEmpty)
    {
        if (ID == 0 && player.IsOwner)
        {
            foreach (var item in shelfsP1)
            {
                if (item.GetComponent<Estanteria>().TieneElemento(producto) == true)
                {
                    if (isEmpty)
                    {
                        item.GetComponent<Estanteria>().worldObjectInteractable.UpdateUIProducts();
                    }
                    return item.transform.position;
                }
            }
        }
        else if (ID == 1 && player.IsOwner)
        {
            foreach (var item in shelfsP2)
            {
                if (item.GetComponent<Estanteria>().TieneElemento(producto) == true)
                {
                    if (isEmpty) item.GetComponent<Estanteria>().worldObjectInteractable.UpdateUIProducts();
                    return item.transform.position;
                }
            }
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

    public int cogerDeEstanteria(string s, char tipo, int cantidad)
    {
        int productsRemaining;
        if (getDictionaryAccType(tipo).TryGetValue(s, out var result))
        {
            productsRemaining = result.cogerProducto(cantidad);

            if (productsRemaining == -1) //producto vacio
            {
                buscarEstanteria(s, true);
            }

            return productsRemaining;
        }
        return -1;
    }

    public void UnlockProduct(string s, char tipo)
    {
        if (getDictionaryAccType(tipo).TryGetValue(s, out var result))
        {
            result.disponible = true;
        }
    }
    #endregion

    #region cola
    public int cogerSitioCola(IContext npc)
    {
        if (ID == 0 && player.IsOwner)
        {
            if (npcPayQueueP1.Count == 5) return -1;

            npcPayQueueP1.Enqueue(npc);
            payQueueChangeP1?.Invoke(this, EventArgs.Empty);
            return npcPayQueueP1.Count;
        }
        else if (ID == 1 && player.IsOwner)
        {
            if (npcPayQueueP2.Count == 5) return -1;

            npcPayQueueP2.Enqueue(npc);
            payQueueChangeP2?.Invoke(this, EventArgs.Empty);
            return npcPayQueueP2.Count;
        }

        return -1;
    }

    public void avanzarLaCola()
    {
        if (ID == 0 && player.IsOwner)
        {
            if (npcPayQueueP1.Count == 0) return;
            npcPayQueueP1.Dequeue();
            payQueueChangeP1?.Invoke(this, EventArgs.Empty);
        }
        else if (ID == 1 && player.IsOwner)
        {
            if (npcPayQueueP2.Count == 0) return;
            npcPayQueueP2.Dequeue();
            payQueueChangeP2?.Invoke(this, EventArgs.Empty);
        }
    }

    public int getPositionPayQueue(IContext npc)
    {
        int pos = 0;
        if (ID == 0 && player.IsOwner)
        {
            foreach (IContext queuedNpc in npcPayQueueP1)
            {
                if (queuedNpc == npc) { return pos; }
                pos++;
            }
        }
        else if (ID == 1 && player.IsOwner)
        {
            foreach (IContext queuedNpc in npcPayQueueP2)
            {
                if (queuedNpc == npc) { return pos; }
                pos++;
            }
        }

        return -1;
    }
    #endregion

    #region precios
    public float getPrecioProducto(string s, char tipo, int cantidad)
    {
        if (getDictionaryAccType(tipo).TryGetValue(s, out var result))
        {
            return result.precio * cantidad;
        }
        return 0;
    }

    public float GetPrecioProductoIndividual(string nombreProducto, char tipo)
    {
        if (getDictionaryAccType(tipo).TryGetValue(nombreProducto, out var result))
        {
            return result.precio;
        }
        return 0;
    }
    #endregion

    #region cantidades almacén y productos
    public int GetAlmacenQuantityOfProduct(string nombreProducto, char tipo)
    {
        if (getDictionaryAccType(tipo).TryGetValue(nombreProducto, out var result))
        {
            return result.stockAlmacen;
        }
        return 0;
    }

    public int GetEstanteriaQuantityOfProduct(string nombreProducto, char tipo)
    {
        if (getDictionaryAccType(tipo).TryGetValue(nombreProducto, out var result))
        {
            return result.stockEstanteria;
        }
        return 0;
    }
    public void UpdateProductQuantity(string nombreProducto, char tipo, int quantity)
    {
        if (getDictionaryAccType(tipo).TryGetValue(nombreProducto, out var result))
        {
            result.stockAlmacen += quantity;
        }
    }
    #endregion

    #region almacén y compras producto
    public bool CheckIfCanBuyProduct(string nombreProducto, char tipo)
    {
        if (getDictionaryAccType(tipo).TryGetValue(nombreProducto, out var result))
        {
            return result.disponible;
        }
        return false;
    }

    public void InitializeAlmacenSpace()
    {
        if (sellClothes)
        {
            foreach(var key in RopaYCalzado)
            {
                if(key.Value.disponible) AlmacenManager.Instance.espacioUsado += key.Value.stockAlmacen; 
            }
        }
        if (sellFood)
        {
            foreach (var key in Comida)
            {
                if(key.Value.disponible) AlmacenManager.Instance.espacioUsado += key.Value.stockAlmacen;
            }
        }
        if (sellLeisure)
        {
            foreach (var key in JuegosPeliculasMusica)
            {
                if(key.Value.disponible) AlmacenManager.Instance.espacioUsado += key.Value.stockAlmacen; 
            }
        }
        if (sellStationery)
        {
            foreach (var key in PapeleriaYArte)
            {
                if(key.Value.disponible) AlmacenManager.Instance.espacioUsado += key.Value.stockAlmacen; 
            }
        }

        UIManager.Instance.UpdateAlmacenSpace_UI();
    }
    #endregion

    #region basura
    public void InstanceBag(Vector3 position, float money)
    {
        var obj = Instantiate(bolsaBasura, position, bolsaBasura.transform.rotation);
        GarbageBagController bag = obj.GetComponent<GarbageBagController>();

        bag.moneyShop = money;
        //bag.fillCoroutine = StartCoroutine(UIManager.Instance.VaciarImagen(bag.progressImage, bag.secondsToSeek - 1));
        //print($"corrutine: {bag.fillCoroutine}");
        //bag.fillCoroutine = StartCoroutine();
    }
    public void InstanceGarbage(Transform position)
    {
        Instantiate(basura, position.position, basura.transform.rotation);
    }
    #endregion

    #region dudas clientes
    public void updateDudasClientes(IContext contx, string prodDuda)
    {
        lock (_lockDuda)
        {
            if (yaHayDuda) contx.setTieneDuda(false);
            else
            {
                yaHayDuda = true;
                productoDuda = prodDuda;
            }
        }
    }
    #endregion
}
