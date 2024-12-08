using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

namespace Assets.Scripts.MachineStates.Classes
{
    public class ContextClienteGenerico : MonoBehaviour, IContext
    {
        public IState currentState;
        ListaCompra lista = new ListaCompra();
        [SerializeField] TiendaManager TiendaManager;
        [SerializeField] UIManager UIManager;
        [SerializeField] GameManager GameManager;
        Vector3 currentEstanteria;

        //caja pago:
        public float dineroCompra = 0;
        bool stopedInShelf = false;
        public bool stopedInCajaPago = false;
        public int positionPayQueue;
        public bool isInPayQueue = false;

        //dudas:
        int porcentajeDuda = 20; //20
        bool tieneDuda = false;
        string productoDuda;

        //pila estados:
        Stack<IState> pilaStates = new Stack<IState>();

        //enfado;
        [Header("Plumbobs")]
        [SerializeField] private GameObject plumbobFelicidadMAX;
        [SerializeField] private GameObject plumbobFelicidadMID;
        [SerializeField] private GameObject plumbobFelicidadMIN;
        int felicidad = 100;
        int maxEnfado = 0;
        int umbralPropinaFelicidad = 65;
        [Header("Emociones")]
        [SerializeField] private GameObject canvasEmociones;
        [SerializeField] private GameObject emocionDuda;
        [SerializeField] private GameObject emocionEnfado;
        [SerializeField] private GameObject emocionMalvado;
        [SerializeField] private GameObject emocionTacaEnfadado;
        [SerializeField] private GameObject emocionTacaFeliz;

        /*
         por producto -> 15
         por mancha -> 3
         por esperar en la cola -> 10
         */

        //objectpool
        public IObjectPool<IContext> objectPool;
        bool isReset;

        //variables control pagar si hay cajero
        public bool hayCajeroEnCaja = false;

        //variables tipo de cliente
        public bool isKaren;
        bool canComplain = false;

        //Tacaño
        public bool isTacanio;
        bool canMakeShow = false;
        public float presupuestoTacanio;


        //cierre tienda
        bool shopIsClosed = false;

        //Animator
        public Animator clientAnimator;

        //si estoy donde el jugador o no
        bool imInPlayer = false;
        bool dudaResuelta = false;

        //si el trabajador está atendiendo
        bool workerInPay = false;
        bool workerFinishPay = false;

        #region MetodosGenerales
        private void Start()
        {
            TiendaManager = GameObject.FindGameObjectWithTag("TiendaManager").GetComponent<TiendaManager>();
            UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            
            clientAnimator = GetComponent<Animator>();

            inicializar();
            GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            if(TiendaManager.ID == 0) TiendaManager.payQueueChangeP1 += MoveInQueue;
            if(TiendaManager.ID == 1) TiendaManager.payQueueChangeP2 += MoveInQueue;
            GameManager._player.GetComponent<PlayerControler>().eventPlayerIsInPayBox += updateIfExistCajero;
            GameManager._player.GetComponent<PlayerControler>().eventPlayerFinishPay += updateIfPlayerFinishPay;
            UIManager.Instance.eventoDudaResuelta += eventUpdateDudaResuelta;

            //métodos hora salida tienda 
            UIManager.schedule.eventTwoHoursLeft += recieverEventTwoHoursLeft;
            UIManager.schedule.eventTenMinutesLeft += recieverEventTenMinutesLeft;
            UIManager.Instance.schedule.dayFinish += destruirClientesRestantes;
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

            //Plumbobs
            plumbobFelicidadMAX.SetActive(true);
            plumbobFelicidadMID.SetActive(false);
            plumbobFelicidadMIN.SetActive(false);
            plumbobChanged = false;


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
                presupuestoTacanio = UnityEngine.Random.Range(60.0f, 175.0f);
            }

            TiendaManager.clientesTotales += 1;

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

        public Vector3 randomPositionShelf(Vector3 positionShelf)
        {
            float radius = 2;
            Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-radius, radius), 0, UnityEngine.Random.Range(-radius, radius));

            Vector3 randomPosition = positionShelf + randomOffset;

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                randomPosition = hit.position;
            }

            return randomPosition;
        }
        #endregion

        #region metodosEspecificos
        public IContext GetContext()
        {
            return this;
        }

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

        bool plumbobChanged = false;
        public void reducirFelicidad(int enfado)
        {
            //print($"reduzcfo felicidad, felicidad: {felicidad} enfado: {enfado}");
            felicidad -= enfado;

            if (felicidad <= 60 && felicidad > 35 && !plumbobChanged) 
            {
                activarCanvasEnfado();
                plumbobFelicidadMAX.SetActive(false); 
                plumbobFelicidadMID.SetActive(true); 
                plumbobChanged = true; 
            }
            if (felicidad <= 35 && plumbobChanged) 
            {
                activarCanvasEnfado();
                plumbobFelicidadMID.SetActive(false); 
                plumbobFelicidadMIN.SetActive(true);
                plumbobChanged = false; 
            }
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

        #region cierre de tienda
        public bool getIfShopIsClosed()
        {
            return shopIsClosed;
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
            //print($"two hours left       maxcantidad productos antes: {lista.maxCantidadProductos}");
            lista.maxCantidadProductos = 2;
            //print($"maxcantidad productos despues: {lista.maxCantidadProductos}");

        }
        public void recieverEventTenMinutesLeft(object s, EventArgs e)
        {
            //irse de la tienda
            //print("ten minutes left");
            shopIsClosed = true;
            if (isActiveAndEnabled) SetState(new LeaveAngry(this));
        }

        public void destruirClientesRestantes(object s, EventArgs e)
        {
            if (isActiveAndEnabled)
            {
                Destruir();
            }
        }
        #endregion

        #region detectar al player
        public bool getIfImInPlayer()
        {
            return imInPlayer;
        }        
        public void setIfImInPlayer(bool b)
        {
            imInPlayer = b;
        }

        private void OnTriggerEnter(Collider other)
        {
            //print("on trigger enter");
            
            //if (other.CompareTag("Player") && tieneDuda && !isKaren)
            //{
            //    //print("estoy en el player");
            //    imInPlayer = true;
            //    other.gameObject.GetComponent<PlayerControler>().disableMovement();
            //}
            //if(other.CompareTag("Player") && isKaren && canComplain)//lo que sea
            //{
            //    imInPlayer = true;
            //    other.gameObject.GetComponent<PlayerControler>().disableMovement();
            //}
        }
        private void OnTriggerExit(Collider other)
        {
            //print("on trigger exit");
            if (other.CompareTag("Player"))
            {
                //print("se va player");
                imInPlayer = false;
                //other.gameObject.GetComponent<PlayerControler>().enableMovement(false);
            }
        }

        public void eventUpdateDudaResuelta(object s, EventArgs e)
        {
            dudaResuelta = true;
            clientAnimator.SetBool("tieneDuda", false);
        }

        public bool getIfDudaResuelta()
        {
            return dudaResuelta;
        }
        public void setIfDudaResuelta(bool b)
        {
            dudaResuelta = b;
        }

        public void activarDelayDuda()
        {
            StartCoroutine(delayDuda());
        }

        public IEnumerator delayDuda()
        {
            yield return new WaitForSeconds(4f);
            //print("delay después ya pueden preguntar");
            TiendaManager.Instance.yaHayDuda = false;

        }
        #endregion

        #region feedback canvas visual
        public void activarCanvasDuda()
        {
            canvasEmociones.SetActive(true);
            emocionDuda.SetActive(true);
            StartCoroutine(delayDesactivar(3f, emocionDuda));
        }
        public void activarCanvasEnfado()
        {
            canvasEmociones.SetActive(true);
            emocionEnfado.SetActive(true);
            StartCoroutine(delayDesactivar(3f, emocionEnfado));
        }
        public void activarCanvasTacanioEnfadado()
        {
            canvasEmociones.SetActive(true);
            emocionTacaEnfadado.SetActive(true);
            StartCoroutine(delayDesactivar(3f, emocionTacaEnfadado));
        }
        public void activarCanvasTacanioFeliz()
        {
            canvasEmociones.SetActive(true);
            emocionTacaFeliz.SetActive(true);
            StartCoroutine(delayDesactivar(3f, emocionTacaFeliz));
        }

        public IEnumerator delayDesactivar(float delay, GameObject canvas)
        {
            yield return new WaitForSeconds(delay);
            canvas.SetActive(false);
            canvasEmociones.SetActive(false);
        }

        public GameObject getCanvasDuda()
        {
            return emocionDuda;
        }        
        public GameObject getCanvasQueja()
        {
            return emocionEnfado;
        }
        #endregion

        #region animator y gameobject
        public Animator GetAnimator()
        {
            return clientAnimator;
        }
        public GameObject GetGameObject()
        {
            return gameObject;
        }
        #endregion

        #region worker
        public bool getWorkerInPay()
        {
            return workerInPay;
        }
        public void setWorkerInPay(bool b)
        {
            workerInPay = b;
        }
        public bool isFinishPayWorker()
        {
            return workerFinishPay;
        }
        public void setFinishPayWorker(bool b)
        {
            workerFinishPay = b;
        }
        #endregion
    }
}
