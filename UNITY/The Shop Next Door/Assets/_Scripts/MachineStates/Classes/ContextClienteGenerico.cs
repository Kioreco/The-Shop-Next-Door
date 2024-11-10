using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.MachineStates.Classes
{
    public class ContextClienteGenerico : MonoBehaviour, IContext
    {
        private IState currentState;
        [SerializeField] float speed;
        ListaCompra lista = new ListaCompra();
        [SerializeField] TiendaManager tiendaManager;
        [SerializeField] UIManager UIManager;
        [SerializeField] GameManager GameManager;
        Vector3 currentEstanteria;

        //caja pago:
        public float dineroCompra = 0;
        bool stopedInShelf = false;
        bool stopedInCajaPago = false;
        public int positionPayQueue;
        public bool isInPayQueue = false;

        //dudas:
        int porcentajeDuda = 20;
        bool tieneDuda = false;
        string productoDuda;

        //pila estados:
        Stack<IState> pilaStates = new Stack<IState>();

        //enfado;
        int felicidad = 100;
        int maxEnfado = 0;
        int umbralPropinaFelicidad = 65;
        /*
         por producto -> 15
         por mancha -> 3
         por esperar en la cola -> 10
         */

        public IObjectPool objectPool;
        bool isReset;

        #region MetodosGenerales
        private void Start()
        {
            //print("start");
            tiendaManager = GameObject.FindGameObjectWithTag("TiendaManager").GetComponent<TiendaManager>();
            UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

            inicializar();
            GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            tiendaManager.payQueueChangeP1 += MoveInQueue;
        }
        private void OnEnable()
        {
            if (isReset)
            {
                isReset = false;
                inicializar();
            }
        }
        public void inicializar()
        {
            lista.lista.Clear();
            lista.CrearLista();
            dineroCompra = 0;
            felicidad = 0;
            stopedInShelf = false;
            stopedInCajaPago = false;
            positionPayQueue = 0;
            isInPayQueue = false;
            pilaStates.Clear();

            int random = UnityEngine.Random.Range(0, 100);
            if(random < porcentajeDuda)
            {
                tieneDuda = true;
                var productos = new List<string>(lista.lista.Keys);
                productoDuda = productos[UnityEngine.Random.Range(0, lista.lista.Count-1)];
            }
            else
            {
                tieneDuda = false;
                productoDuda = null;
            }

            SetState(new SearchShelf(this));
        }

        private void Update()
        {
            currentState.Update();
        }
        private void FixedUpdate()
        {
            currentState.FixedUpdate();
        }
        public void SetState(IState cntx)
        {
            currentState = cntx;
            currentState.Enter();
        }
        #endregion

        #region metodosEspecificos
        public NavMeshAgent getNavMesh()
        {
            return gameObject.GetComponent<NavMeshAgent>();
        }
        public ListaCompra getLista()
        {
            return lista;
        }
        public TiendaManager getTiendaManager()
        {
            return tiendaManager;
        }
        public Vector3 getCurrentEstanteria()
        {
            return currentEstanteria;
        }
        public void setCurrentEstanteria(Vector3 e)
        {
            currentEstanteria = e;
        }
        public IState GetState()
        {
            return currentState;
        }
        public float getDineroCompra()
        {
            return dineroCompra;
        }
        public void sumDineroCompra(float d)
        {
            dineroCompra += d;
        }
        public UIManager getUIManager()
        {
            return UIManager; 
        }
        public GameManager getGameManager()
        {
            return GameManager;
        }
        public void Destuir()
        {
            objectPool?.Release(this);
        }
        public Vector3 getPosition()
        {
            return transform.position;
        }
        public bool getIsInColliderShelf()
        {
            return stopedInShelf;
        }
        public void setIsInColliderShelf(bool b)
        {
            stopedInShelf = b;
        }
        public bool getIsInColliderCajaPago()
        {
            return stopedInCajaPago;
        }
        public void setIsInColliderCajaPago(bool b)
        {
            stopedInCajaPago = b;
        }
        public void MoveInQueue(object s, EventArgs e)
        {
            positionPayQueue = tiendaManager.getPositionPayQueue(this);
        }
        public int getPositionPay()
        {
            return positionPayQueue;
        }
        public GameObject getPlayer()
        {
            PlayerControler player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
            if (player.IsOwner) return player.gameObject;
            return null;
        }
        public Transform GetTransform()
        {
            return transform;
        }
        public string getProductoDuda()
        {
            return productoDuda;
        }
        public bool getTieneDuda()
        {
            return tieneDuda;
        }
        public void setTieneDuda(bool duda)
        {
            tieneDuda = duda;
        }
        public Stack<IState> getPilaState()
        {
            return pilaStates;
        }
        public int getFelicidad()
        {
            return felicidad;
        }
        public int getMaxEnfado()
        {
            return maxEnfado;
        }
        public void reducirFelicidad(int enfado)
        {
            //print($"reduzcfo felicidad, felicidad: {felicidad} enfado: {enfado}");
            felicidad -= enfado;
            //print($"felicidad reducia: {felicidad}");
        }
        public int getUmbralPropina()
        {
            return umbralPropinaFelicidad;
        }
        public bool getIsInPayQueue()
        {
            return isInPayQueue;
        }
        public void setIsInPayQueue(bool b)
        {
            isInPayQueue = b;
        }
        #endregion

        #region objectpool and prototype
        public bool isActive
        {
            get
            {
                return gameObject.activeSelf;
            }

            set
            {
                gameObject.SetActive(value);
            }
        }
        public void Reset()
        {
            isReset = true;
            if(TiendaManager.Instance.ID == 0 && TiendaManager.Instance.player.IsOwner) transform.position = TiendaManager.Instance.npcPositionInitialP1.position;
            else if(TiendaManager.Instance.ID == 1 && TiendaManager.Instance.player.IsOwner) transform.position = TiendaManager.Instance.npcPositionInitialP2.position;
        }
        public void setObjectPool(IObjectPool o)
        {
            objectPool = o;
        }

        public IContext Clone(Vector3 p, Quaternion r)
        {
            GameObject obj = Instantiate(gameObject, p, r);
            IContext stc = obj.GetComponent<ContextClienteGenerico>();
            return stc;
        }



        #endregion
    }
}
