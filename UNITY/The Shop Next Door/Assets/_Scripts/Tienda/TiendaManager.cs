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
    public GameObject manchaSuelo;
    public Transform doorPos1;
    public Transform doorPos2;

    [Header("Trabajadores")]
    public List<GameObject> workersP1;
    public List<GameObject> workersP2;

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
        RopaYCalzado.Add("camisa", new Producto(19.99f, 25, 0, 'r', true));
        RopaYCalzado.Add("jerseis", new Producto( 24.99f, 22, 0, 'r', true));
        RopaYCalzado.Add("vestidos", new Producto( 30.00f, 0, 0, 'r', false));
        RopaYCalzado.Add("pantalones", new Producto(25.99f, 22, 0, 'r', true));
        RopaYCalzado.Add("faldas", new Producto(20.00f, 20, 0, 'r', true));
        RopaYCalzado.Add("pijamas", new Producto(7.50f, 0, 0, 'r', false));
        RopaYCalzado.Add("deportivas", new Producto(55.00f, 20, 0, 'r', true));
        RopaYCalzado.Add("edgy", new Producto(69.00f, 21, 0, 'r', true));
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
        Comida.Add("manzana", new Producto(0.85f, 22, 0, 'c', true));
        Comida.Add("sandia", new Producto(2.75f, 20, 0, 'c', true));
        Comida.Add("melon", new Producto(2.15f, 0, 0, 'c', false));
        Comida.Add("pizza", new Producto(3.45f, 21, 0, 'c', true));
        Comida.Add("croquetas", new Producto(4.60f, 0, 0, 'c', false));
        Comida.Add("calabaza", new Producto(5.00f, 25, 0, 'c', true));
        Comida.Add("carne", new Producto(12.50f, 21, 0, 'c', true));
        Comida.Add("pescado", new Producto(12.50f, 21, 0, 'c', true));
    }
    void InicializarOcio()
    {
        JuegosPeliculasMusica.Add("Hasta que la noche nos separe", new Producto(7.60f, 50, 0, 'o', false));
        JuegosPeliculasMusica.Add("Decor Dilemma", new Producto(5.60f, 50, 0, 'o', false));
        JuegosPeliculasMusica.Add("Virtual Velocity", new Producto(5.60f, 50, 0, 'o', false));
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
        //print(GameObject.FindWithTag("Player").GetComponent<PlayerControler>().ID);
        if (ID == 0 && player.IsOwner)
        //if (GameObject.FindWithTag("Player").GetComponent<PlayerControler>().ID == 0 && GameObject.FindWithTag("Player").GetComponent<PlayerControler>().IsOwner)
        {
            //print("host");
            foreach (var item in shelfsP1)
            {
                if (item.GetComponent<Estanteria>().TieneElemento(producto) == true) return item.transform.position;
            }
        }
        else if (ID == 1 && player.IsOwner)
        //else if(GameObject.FindWithTag("Player").GetComponent<PlayerControler>().ID == 1 && GameObject.FindWithTag("Player").GetComponent<PlayerControler>().IsOwner)
        {
            //print("cliente");
            foreach (var item in shelfsP2)
            {
                if (item.GetComponent<Estanteria>().TieneElemento(producto) == true) return item.transform.position;
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

    public void reponerEstanteria(int cantidad)
    {
        if (ID == 0 && player.IsOwner)
        {
            foreach (var item in shelfsP1)
            {
                for(int i = 0; i < item.GetComponent<Estanteria>().objetosEstanteria.Count; i++)
                {
                    char tipo = item.GetComponent<Estanteria>().tipoObj;
                    string s = item.GetComponent<Estanteria>().objetosEstanteria[i];
                    if (getDictionaryAccType(tipo).TryGetValue(s, out var result))
                    {
                        result.gestionarStockEstanteriaYAlmacen(cantidad);
                    }
                }
                
            }
        }
        else if (ID == 1 && player.IsOwner)
        {
            foreach (var item in shelfsP2)
            {
                for (int i = 0; i < item.GetComponent<Estanteria>().objetosEstanteria.Count; i++)
                {
                    char tipo = item.GetComponent<Estanteria>().tipoObj;
                    string s = item.GetComponent<Estanteria>().objetosEstanteria[i];
                    if (getDictionaryAccType(tipo).TryGetValue(s, out var result))
                    {
                        result.gestionarStockEstanteriaYAlmacen(cantidad);
                    }
                }

            }
        }
    }

    public int cogerDeEstanteria(string s, char tipo, int cantidad)
    {
        if (getDictionaryAccType(tipo).TryGetValue(s, out var result))
        {
            //print($"antes: {result}");
            return result.cogerProducto(cantidad);
            //print($"despues: {result}");
        }
        return -1;
    }

    public int cogerSitioCola(IContext npc)
    {
        //print($"cojo sitio antes: {cajaPago.transform.position}");
        //posicionEnLaCola.transform.position += new Vector3 (0, 0, 1.2f);
        //print($"cojo sitio despues: {cajaPago.transform.position}");
        //print($"npc añadido en la cola, pos: {npcPayQueue.Count}, max checkpoints: {posPayCheckpoints.Count}");
        if (ID == 0 && player.IsOwner)
        //if (GameObject.FindWithTag("Player").GetComponent<PlayerControler>().ID == 0 && GameObject.FindWithTag("Player").GetComponent<PlayerControler>().IsOwner)
        {
            if (npcPayQueueP1.Count == 5) return -1;

            npcPayQueueP1.Enqueue(npc);
            payQueueChangeP1?.Invoke(this, EventArgs.Empty);
            //print($"npc añadido en la cola, pos: {npcPayQueueP1.Count}, max checkpoints: {posPayCheckpointsP1.Count}");
            return npcPayQueueP1.Count;
        }
        else if (ID == 1 && player.IsOwner)
        {
            if (npcPayQueueP2.Count == 5) return -1;

            npcPayQueueP2.Enqueue(npc);
            payQueueChangeP2?.Invoke(this, EventArgs.Empty);
            return npcPayQueueP2.Count;
        }
        //print($"npc añadido en la cola, pos: {npcPayQueueP1.Count}, max checkpoints: {posPayCheckpointsP1.Count}");

        return -1;
    }

    public void avanzarLaCola()
    {
        //print($"dejo sitio antes: {cajaPago.transform.position}");
        //print($"dejo sitio despues: {cajaPago.transform.position}");
        //print($"npc quitado de la cola: {npcPayQueue.Count}");
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
                //print($"elemento cola: {queuedNpc} npc: {npc}");
                if (queuedNpc == npc) { return pos; }
                pos++;
            }
        }
        else if (ID == 1 && player.IsOwner)
        {
            foreach (IContext queuedNpc in npcPayQueueP2)
            {
                //print($"elemento cola: {queuedNpc} npc: {npc}");
                if (queuedNpc == npc) { return pos; }
                pos++;
            }
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

    public float GetPrecioProductoIndividual(string nombreProducto, char tipo)
    {
        if (getDictionaryAccType(tipo).TryGetValue(nombreProducto, out var result))
        {
            return result.precio;
        }
        return 0;
    }

    public int GetAlmacenQuantityOfProduct(string nombreProducto, char tipo)
    {
        if (getDictionaryAccType(tipo).TryGetValue(nombreProducto, out var result))
        {
            print($"producto: {nombreProducto}, resultado: {result}, stock: {result.stockAlmacen}");
            return result.stockAlmacen;
        }
        return 0;
    }

    public bool CheckIfCanBuyProduct(string nombreProducto, char tipo)
    {
        if (getDictionaryAccType(tipo).TryGetValue(nombreProducto, out var result))
        {
            return result.disponible;
        }
        return false;
    }

    public void UpdateProductQuantity(string nombreProducto, char tipo, int quantity)
    {
        if (getDictionaryAccType(tipo).TryGetValue(nombreProducto, out var result))
        {
            result.stockAlmacen += quantity;
        }
    }

    public void updateAlmacenQuantity()
    {
        if (sellClothes)
        {
            foreach(var key in RopaYCalzado)
            {
                if(key.Value.disponible) GameManager.Instance.espacioAlmacen += key.Value.stockAlmacen;
            }
        }
        if (sellFood)
        {
            foreach (var key in Comida)
            {
                if(key.Value.disponible) GameManager.Instance.espacioAlmacen += key.Value.stockAlmacen;
            }
        }
        if (sellLeisure)
        {
            foreach (var key in JuegosPeliculasMusica)
            {
                if(key.Value.disponible) GameManager.Instance.espacioAlmacen += key.Value.stockAlmacen;
            }
        }
        if (sellStationery)
        {
            foreach (var key in PapeleriaYArte)
            {
                if(key.Value.disponible) GameManager.Instance.espacioAlmacen += key.Value.stockAlmacen;
            }
        }
    }

    public void refillStock(string producto, int cantidad, char tipo)
    {
        if (ID == 0 && player.IsOwner)
        {
            Dictionary<string, Producto> aux = getDictionaryAccType(tipo);
            aux[producto].stockAlmacen += cantidad;
            aux[producto].gestionarStockEstanteriaYAlmacen(20);
        }
        else if (ID == 1 && player.IsOwner)
        {
            Dictionary<string, Producto> aux = getDictionaryAccType(tipo);
            aux[producto].stockAlmacen += cantidad;
            aux[producto].gestionarStockEstanteriaYAlmacen(20);
        }
    }

    public void InstanceBag(Vector3 position)
    {
        //print("instanciando bolsa basura");
        Instantiate(bolsaBasura, position, new Quaternion(270, 0, 0, 0));
    }
}
