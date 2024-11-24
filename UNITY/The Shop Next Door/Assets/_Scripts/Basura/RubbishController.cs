using Assets.Scripts.MachineStates.Classes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RubbishController : MonoBehaviour, IPooleableObject, IPrototype<RubbishController>
{
    [Header("Variables NPCs Movimiento")]
    float slowDownAmount = 3;
    [Header("Limites Posiciones")]
    public int minX;
    public int maxX;
    public int minZ;
    public int maxZ;
    public float maxDistanceNavMesh = 1.5f;
    [Header("Object Pool")]
    public IObjectPool<RubbishController> objectPool;
    bool isReset;
    bool clicked = false;


    [SerializeField] private Image progressImage;
    private void Start()
    {
        GameManager.Instance._player.GetComponent<PlayerControler>().eventPlayerIsInRubbish += updateIfPlayerIsInRubbish;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            //Debug.Log("reduc velocidad npc");
            if (other.gameObject.GetComponent<ContextClienteGenerico>().getIsKaren())
            {
                //print("es karen y ha pillado basura");
                other.gameObject.GetComponent<ContextClienteGenerico>().reducirFelicidad(10);
            }
            else other.gameObject.GetComponent<ContextClienteGenerico>().reducirFelicidad(3);

            other.gameObject.GetComponent<NavMeshAgent>().speed -= slowDownAmount;
            StartCoroutine(delaySpeedAgent(other.gameObject));
            
        }
    }
    IEnumerator delaySpeedAgent(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        obj.GetComponent<NavMeshAgent>().speed += slowDownAmount;
        //print("quitado efecto velocidad npc");

        if (obj.gameObject.GetComponent<ContextClienteGenerico>().getIsKaren())
        {
            //print("se va a ir a quejar...");
            obj.gameObject.GetComponent<ContextClienteGenerico>().setCanComplain(true);
        }
    }
    #region object pool
    public bool isActive { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
    public RubbishController Clone(Vector3 pos, Quaternion rot)
    {
        GetSceneLimit();
        GameObject obj = Instantiate(gameObject, calculateRandomPosition(), gameObject.transform.rotation);
        RubbishController stc = obj.GetComponent<RubbishController>();
        return stc;
    }

    public void Reset()
    {
        print("reseted");
        isReset = true;
        clicked = false;
        progressImage.fillAmount = 0;
        //gameObject.SetActive(false);

        //GetSceneLimit();
        transform.position = calculateRandomPosition();
    }
    public void setObjectPool(IObjectPool<RubbishController> o)
    {
        objectPool = o;
    }

    private void OnEnable()
    {
        if (isReset)
        {
            isReset = false;
            //calculateRandomPosition();
        }
        //progressImage.enabled = true;
    }

    public void Destruir()
    {
        objectPool?.Release(this);
        GameManager.Instance._player.enableMovement(false);
    }

    public void GetSceneLimit()
    {
        //print("estabelciendo limites escena");
        if (TiendaManager.Instance.ID == 0 && TiendaManager.Instance.player.IsOwner)
        {
            minX = (int)TiendaManager.Instance.minXP1.position.x;
            maxX = (int)TiendaManager.Instance.maxXP1.position.x;
            minZ = (int)TiendaManager.Instance.minZP1.position.z;
            maxZ = (int)TiendaManager.Instance.maxZP1.position.z;
        }
        else if (TiendaManager.Instance.ID == 1 && TiendaManager.Instance.player.IsOwner)
        {
            minX = (int)TiendaManager.Instance.minXP2.position.x;
            maxX = (int)TiendaManager.Instance.maxXP2.position.x;
            minZ = (int)TiendaManager.Instance.minZP2.position.z;
            maxZ = (int)TiendaManager.Instance.maxZP2.position.z;
        }
    }
    public void CleanRubbish()
    {
        clicked = true;
        GameManager.Instance._player.WalkToPosition(transform.position, false);
    }
    #endregion

    public Vector3 calculateRandomPosition()
    {
        int randomX = UnityEngine.Random.Range(minX, maxX);
        int randomZ = UnityEngine.Random.Range(minZ, maxZ);
        Vector3 position = new Vector3(randomX, transform.position.y, randomZ);

        if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxDistanceNavMesh, NavMesh.AllAreas))
        {
            //Instantiate(manchaPrefab, hit.position, Quaternion.identity);
            position = hit.position;
        }
        return position;
    }

    public void updateIfPlayerIsInRubbish(object s, EventArgs e)
    {
        if(clicked) UIManager.Instance.UpdateCleaning_UI(progressImage, this);
    }
}
