using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;


namespace Assets.Scripts.MachineStates.Classes
{
    public class Context : MonoBehaviour, IContext
    {
        public List<GameObject> estanteriaList;
        private IState currentState;
        [SerializeField] float speed;
        ListaCompra lista;

        #region MetodosGenerales
        private void Awake()
        {
            estanteriaList = GameObject.FindGameObjectsWithTag("Estanteria").ToList();
            lista.CrearLista();

            Assert.IsTrue(estanteriaList.Count > 0, "Estanterias no encontradas");

            SetState(new SearchShelf(this));
        }

        public void FixedUpdate()
        {
        }

        public void Update()
        {
        }

        public void SetState(IState cntx)
        {
            currentState = cntx;
            currentState.Enter();
        }
        #endregion

        #region metodosEspecificos
        public List<GameObject> getEstanterias()
        {
            return estanteriaList;
        }
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
        #endregion
    }
}
