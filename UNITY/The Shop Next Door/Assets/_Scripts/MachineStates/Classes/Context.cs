﻿using System;
using UnityEngine;
using UnityEngine.AI;

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
        bool stopedInShelf = false;
        bool stopedInCajaPago = false;
        public int positionPayQueue;

        public IObjectPool objectPool;

        #region MetodosGenerales
        private void Start()
        {
            tiendaManager = GameObject.FindGameObjectWithTag("TiendaManager").GetComponent<TiendaManager>();
            UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

            inicializar();
            GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            tiendaManager.payQueueChange += MoveInQueue;
        }
        public void inicializar()
        {
            lista.CrearLista();
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
            //GameObject.FindWithTag("Player").GetComponent<PlayerControler>().client.instanciarNPC();
            //GameObject.FindWithTag("Player").GetComponent<PlayerControler>().client.instanciarNPC();
            //GameObject.FindWithTag("Player").GetComponent<PlayerControler>().client.instanciarNPC();
            //Destroy(gameObject);
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
            transform.position = TiendaManager.Instance.npcPositionInitial.position;
            inicializar();
        }
        public IObjectPool getObjectPool()
        {
            return objectPool;
        }

        public IContext Clone(Vector3 p, Quaternion r)
        {
            GameObject obj = Instantiate(gameObject, p, r);
            IContext stc = obj.GetComponent<Context>();
            return stc;
        }
        #endregion
    }
}
