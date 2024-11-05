using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkDayCycle : MonoBehaviour
{
    public float gameTime = 0.0f;
    private int currentDay = 0;
    private int totalDays = 4;
    public string[] dayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

    private float realTimePerDay = 15f;    // TIEMPO POR DÍA -----> 240

    private float gameStartTime = 9f;       // HORA DE ENTRAR A LA TIENDA
    private float gameClientTime = 10f;     // HORA EN LA QUE ENTRAN LOS CLIENTES
    private float gameEndTime = 15f;        // HORA DE CIERRE
    private float gameHoursPerDay = 6f;     // HORAS DE TRABAJO POR DÍA

    private float realTimePassed = 0f;      // TIEMPO EN SEGUNDOS QUE HAN PASADO

    private bool cycleCompleted = false;    // SEMANA COMPLETA
    public bool timeStopped = false;        // TIEMPO PARADO

    public ClientPrototype npcClient;
    [SerializeField] private GameObject[] pastDay_Bg;
    [SerializeField] private GameObject[] followingDay_Bg;


    void Update()
    {
        if (timeStopped) return;

        // SEMANA COMPLETADA
        if (cycleCompleted)
        {
            gameObject.SetActive(false);
            // ACTIVAR FINAL
        }

        // Se actualiza el tiempo que ha pasado
        realTimePassed += Time.deltaTime;

        // Calcular el tiempo en el juego (de 9:00 a 15:00)
        float timeRatio = realTimePassed / realTimePerDay;
        gameTime = Mathf.Lerp(gameStartTime, gameEndTime, timeRatio);

        //gameTime==10.0f entran clientes el lunes
        //gameTime==9.3f entran clientes el resto de dias
        

        if(gameTime >= 10 && currentDay == 0) // Lunes
        {
            npcClient.isEnable = true;
        }
        else if (gameTime >= 9.05f && currentDay > 0)
        {
            npcClient.isEnable = true;
        }

        // Actualizar la UI para mostrar la hora del juego
        UpdateTimeText();

        // SE HA ACABADO EL DÍA
        if (realTimePassed >= realTimePerDay)
        {
            realTimePassed = 0f; // Reiniciar el tiempo real para el próximo día
            currentDay++; // Pasar al siguiente día

            if (currentDay > totalDays)
            {
                cycleCompleted = true;
            }
            else
            {
                timeStopped = true;
                // Actualizar el texto del día en el UI
                UpdateDayText();
                GameManager.Instance.EndDay();

                pastDay_Bg[currentDay--].SetActive(true);
                followingDay_Bg[currentDay--].SetActive(false);
            }
        }
    }

    // Actualizar el texto del tiempo
    void UpdateTimeText()
    {
        int hours = Mathf.FloorToInt(gameTime);
        int minutes = Mathf.FloorToInt((gameTime - hours) * 60);

        UIManager.Instance.UpdateTime_UI(hours, minutes);
    }

    // Actualizar el texto del día
    void UpdateDayText()
    {
        string dayName = dayNames[(currentDay) % dayNames.Length];

        UIManager.Instance.UpdateDay_UI(dayName);
        UIManager.Instance.telephone.calendar.UpdateDayCalendar(dayName);
    }

}