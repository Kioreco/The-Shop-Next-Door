using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace Assets.Scripts.MachineStates.Classes
{
    public class Context : MonoBehaviour, IContext
    {
        private IState currentState;
        [SerializeField] float speed;
        ListaCompra lista = new ListaCompra();
        [SerializeField] TiendaManager tiendaManager;
        [SerializeField] UIManager UIManager;
        [SerializeField] GameManager GameManager;
        Vector3 currentEstanteria;
        public float dineroCompra = 0;

        #region MetodosGenerales
        private void Start()
        {
            tiendaManager = GameObject.FindGameObjectWithTag("TiendaManager").GetComponent<TiendaManager>();
            UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

            lista.CrearLista();
            //lista.listaPrueba();
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
        public float getSpeed()
        {
            return speed;
        }
        public void setSpeed(float s)
        {
            speed = s;
        }
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
        public Vector3 getCajaPagar()
        {
            return tiendaManager.cajaPago.transform.position;
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
            Destroy(gameObject);
        }
        public Vector3 getPosition()
        {
            return transform.position;
        }
        #endregion
    }
}
