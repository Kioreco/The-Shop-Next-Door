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
        [SerializeField] TiendaManager TiendaManager;
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
        int porcentajeDuda = 20; //20
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

        //objectpool
        public IObjectPool<IContext> objectPool;
        bool isReset;

        //variables control pagar si hay cajero
        bool hayCajeroEnCaja = false;

        //variables tipo de cliente
        public bool isKaren;
        bool canComplain = false;

        //Tacaño
        public bool isTacanio;
        bool canMakeShow = false;
        public float presupuestoTacanio;


        #region MetodosGenerales
        private void Start()
        {
            //print("start");
            TiendaManager = GameObject.FindGameObjectWithTag("TiendaManager").GetComponent<TiendaManager>();
            UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

            inicializar();
            GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            if(TiendaManager.ID == 0) TiendaManager.payQueueChangeP1 += MoveInQueue;
            if(TiendaManager.ID == 1) TiendaManager.payQueueChangeP2 += MoveInQueue;
            GameManager._player.GetComponent<PlayerControler>().eventPlayerIsInPayBox += updateIfExistCajero;
            GameManager._player.GetComponent<PlayerControler>().eventPlayerFinishPay += updateIfPlayerFinishPay;

            //métodos hora salida tienda 
            UIManager.schedule.eventTwoHoursLeft += recieverEventTwoHoursLeft;
            UIManager.schedule.eventTenMinutesLeft += recieverEventTenMinutesLeft;
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
            felicidad = 100;
            stopedInShelf = false;
            stopedInCajaPago = false;
            positionPayQueue = 0;
            isInPayQueue = false;
            pilaStates.Clear();
            canComplain = false;
            canMakeShow = false;

            if (!isKaren & !isTacanio)
            {
                int random = UnityEngine.Random.Range(0, 100);
                if (random < porcentajeDuda)
                {
                    tieneDuda = true;
                    var productos = new List<string>(lista.lista.Keys);
                    productoDuda = productos[UnityEngine.Random.Range(0, lista.lista.Count - 1)];
                }
                else
                {
                    tieneDuda = false;
                    productoDuda = null;
                }
            }
            if (isTacanio & !isKaren)
            {
                //establecer presupuesto
                presupuestoTacanio = UnityEngine.Random.Range(80.0f,250.0f);
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

        #region pay box methods
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
            positionPayQueue = TiendaManager.getPositionPayQueue(this);
        }
        public int getPositionPay()
        {
            return positionPayQueue;
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
        public bool getHayCajeroEnCaja()
        {
            return hayCajeroEnCaja;
        }
        public void setHayCajeroEnCaja(bool b)
        {
            //print($"set hay cajero?: {b}");
            hayCajeroEnCaja = b;
        }
        #endregion

        #region Compra
        public ListaCompra getLista()
        {
            return lista;
        }
        public float getDineroCompra()
        {
            return dineroCompra;
        }
        public void sumDineroCompra(float d)
        {
            dineroCompra += d;
        }
        #endregion

        #region Estanterias
        public Vector3 getCurrentEstanteria()
        {
            return currentEstanteria;
        }
        public void setCurrentEstanteria(Vector3 e)
        {
            currentEstanteria = e;
        }
        public bool getIsInColliderShelf()
        {
            return stopedInShelf;
        }
        public void setIsInColliderShelf(bool b)
        {
            stopedInShelf = b;
        }
        #endregion

        #region metodosEspecificos
        public NavMeshAgent getNavMesh()
        {
            return gameObject.GetComponent<NavMeshAgent>();
        }
        public TiendaManager getTiendaManager()
        {
            return TiendaManager;
        }
        public UIManager getUIManager()
        {
            return UIManager;
        }
        public GameManager getGameManager()
        {
            return GameManager;
        }
        public Vector3 getPosition()
        {
            return transform.position;
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
        public Stack<IState> getPilaState()
        {
            return pilaStates;
        }
        public float calculateHeuristicDistance(Vector3 posClient, Vector3 posWorker)
        {
            return MathF.Abs(posClient.x - posWorker.x) + MathF.Abs(posClient.z - posWorker.z);
        }
        #endregion

        #region tipo cliente variables
        //karen
        public bool getIsKaren()
        {
            return isKaren;
        }
        public bool getCanComplain()
        {
            return canComplain;
        }
        public void setCanComplain(bool b)
        {
            canComplain = b;
        }
        //generico
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
        //tacaño
        public bool getIsTacanio()
        {
            return isTacanio;
        }
        public bool getCanMakeShow()
        {
            return canMakeShow;
        }
        public void setCanMakeShow(bool b)
        {
            canMakeShow = b;
        }
        public float getPresupuesto()
        {
            return presupuestoTacanio;
        }
        #endregion

        #region felicidad
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
        public float calcularFelicidadCliente()
        {
            return (GameManager.reputation * (TiendaManager.clientesTotales - 1) + getFelicidad()) / TiendaManager.clientesTotales;
        }
        #endregion

        #region objectpool and prototype
        public void Destruir()
        {
            objectPool?.Release(this);
        }
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
            if (TiendaManager.Instance.ID == 0 && TiendaManager.Instance.player.IsOwner) transform.position = TiendaManager.Instance.npcPositionInitialP1.position;
            else if (TiendaManager.Instance.ID == 1 && TiendaManager.Instance.player.IsOwner) transform.position = TiendaManager.Instance.npcPositionInitialP2.position;
        }
        public void setObjectPool(IObjectPool<IContext> o)
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

        #region events
        public void updateIfExistCajero(object s, EventArgs e)
        {
            //print("hay cajero");
            setHayCajeroEnCaja(true);
        }        
        public void updateIfPlayerFinishPay(object s, EventArgs e)
        {
            //print("cajero acaba pagar");
            setHayCajeroEnCaja(false);
        }
        public void recieverEventTwoHoursLeft(object s, EventArgs e)
        {
            lista.maxCantidadProductos = 2;
        }        
        public void recieverEventTenMinutesLeft(object s, EventArgs e)
        {
            //irse de la tienda
        }

        #endregion
    }
}
