using System;
using UnityEngine;

public class GarbageBagController : MonoBehaviour
{
    //timer:
    float secondsToSeek = 10f; //tiempo de la animación
    float lastSeek = 0f;

    public float moneyShop;
    float maxShop = 0.7f;
    //others:
    bool isCollected = false;
    
    private void Update()
    {
        if (!isCollected) lastSeek += Time.deltaTime;

        if (lastSeek >= secondsToSeek)
        {
            lastSeek = 0f;
            TiendaManager.Instance.InstanceGarbage(transform);
            Destroy(gameObject);//CAMBIARLO POR DEVOLVER AL OBJECT POOL
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player");
            isCollected = true;
            if (lastSeek < 1) GameManager.Instance.dineroJugador += moneyShop * maxShop;
            else
            {
                float porcentaje = maxShop - (float)(Math.Round(lastSeek) * 0.1f);
                if (porcentaje < 0) porcentaje = 0;
                GameManager.Instance.dineroJugador += moneyShop * porcentaje;
                print($"dinero: {moneyShop}  tiempo: {lastSeek} division: {Math.Round(lastSeek)}  porcentaje: {(float)(Math.Round(lastSeek) * 0.1f)}   operacion: {moneyShop * porcentaje}  dinero jugador: {GameManager.Instance.dineroJugador}");

            }

            UIManager.Instance.UpdatePlayersIngameMoney_UI();
            Destroy(gameObject);
            //HABRÍA QUE PONER QUE SE DEVUELVA AL OBJECT POOL
        }
    }
}
