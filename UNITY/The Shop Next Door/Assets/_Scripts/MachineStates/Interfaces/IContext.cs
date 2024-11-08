using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public interface IContext : IPooleableObject, IPrototype
{
    public void SetState(IState cntx);
    public float getSpeed();
    public void setSpeed(float s);
    public NavMeshAgent getNavMesh();
    public ListaCompra getLista();
    public TiendaManager getTiendaManager();
    public Vector3 getCurrentEstanteria();
    public void setCurrentEstanteria(Vector3 e);
    public IState GetState();
    public float getDineroCompra();
    public void sumDineroCompra(float d);
    public UIManager getUIManager();
    public GameManager getGameManager();
    public void Destuir();
    public Vector3 getPosition();
    public bool getIsInColliderShelf();
    public void setIsInColliderShelf(bool b);    
    public bool getIsInColliderCajaPago();
    public void setIsInColliderCajaPago(bool b);
    public void MoveInQueue(object s, EventArgs e);
    public int getPositionPay();
    public void setObjectPool(IObjectPool obj);
    public GameObject getPlayer();
    public Transform GetTransform();
    public string getProductoDuda();
    public bool getTieneDuda();
    public void setTieneDuda(bool duda);
    public Stack<IState> getPilaState();
    public int getEnfado();
    public int getMaxEnfado();
    public void aumentarEnfado(int enfado);
    public int getUmbralPropina();
}
