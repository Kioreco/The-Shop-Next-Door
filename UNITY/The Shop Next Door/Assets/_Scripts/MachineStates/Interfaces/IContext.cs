using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public interface IContext : IPooleableObject, IPrototype<IContext>
{
    public void SetState(IState cntx);
    public NavMeshAgent getNavMesh();
    public ListaCompra getLista();
    public TiendaManager getTiendaManager();
    public Vector3 getCurrentEstanteria();
    public void setCurrentEstanteria(Vector3 e);
    public float getDineroCompra();
    public void sumDineroCompra(float d);
    public UIManager getUIManager();
    public GameManager getGameManager();
    public void Destruir();
    public Vector3 getPosition();
    public bool getIsInColliderShelf();
    public void setIsInColliderShelf(bool b);    
    public bool getIsInColliderCajaPago();
    public void setIsInColliderCajaPago(bool b);
    public void MoveInQueue(object s, EventArgs e);
    public int getPositionPay();
    public void setObjectPool(IObjectPool<IContext> obj);
    public GameObject getPlayer();
    public Transform GetTransform();
    public string getProductoDuda();
    public bool getTieneDuda();
    public void setTieneDuda(bool duda);
    public Stack<IState> getPilaState();
    public int getFelicidad();
    public int getMaxEnfado();
    public void reducirFelicidad(int enfado);
    public int getUmbralPropina();
    public bool getIsInPayQueue();
    public void setIsInPayQueue(bool b);
    public bool getHayCajeroEnCaja();
    public void setHayCajeroEnCaja(bool b);
    public bool getIsKaren();
    public bool getCanComplain();
    public void setCanComplain(bool b);
    public float calculateHeuristicDistance(Vector3 posClient, Vector3 posWorker);
    public float calcularFelicidadCliente();
    public bool getIsTacanio();
    public bool getCanMakeShow();
    public void setCanMakeShow(bool b);
    public float getPresupuesto();
    public bool getIfShopIsClosed();
    public IContext GetContext();
    public Vector3 randomPositionShelf(Vector3 positionShelf);
    public bool getIfImInPlayer();
    public void setIfImInPlayer(bool b);
    public bool getIfDudaResuelta();
    public void setIfDudaResuelta(bool b);
    public void activarCanvasDuda();
    public void activarCanvasEnfado();
    public void activarCanvasTacanioEnfadado();
    public void activarCanvasTacanioFeliz();


}
