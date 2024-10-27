using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkDayCycle : MonoBehaviour
{
    public float gameTime = 0.0f;
    private int currentDay = 0;
    private int totalDays = 4;
    private float realTimePerDay = 240f; //60
    private float gameStartTime = 9f;
    private float gameClientTime = 10f;
    private float gameEndTime = 15f;
    private float gameHoursPerDay = 6f;

    private float realTimePassed = 0f; // Tiempo real transcurrido en segundos
    private bool cycleCompleted = false;
    public bool timeStopped = false;

    public ClientPrototype npcClient;

    public string[] dayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

    void Update()
    {
        if (timeStopped) return;
        if (cycleCompleted) return;
        // Incrementar el tiempo real
        realTimePassed += Time.deltaTime;

        // Calcular el tiempo en el juego (de 9:00 a 15:00)
        float timeRatio = realTimePassed / realTimePerDay;
        gameTime = Mathf.Lerp(gameStartTime, gameEndTime, timeRatio);

        //gameTime==10.0f entran clientes el lunes
        //gameTime==9.3f entran clientes el resto de dias
        
        //print(currentDay);
        if(gameTime >= 10 && currentDay == 0) // lunes
        {
            npcClient.isEnable = true;
        }
        else if (gameTime >= 9.05f && currentDay > 0)
        {
            npcClient.isEnable = true;
        }

        // Actualizar la UI para mostrar la hora del juego
        UpdateTimeText();

        // Si hemos alcanzado el final del día (15:00 en el juego)
        if (realTimePassed >= realTimePerDay)
        {
            realTimePassed = 0f; // Reiniciar el tiempo real para el próximo día
            currentDay++; // Pasar al siguiente día

            if (currentDay > totalDays)
            {
                cycleCompleted = true;
                Debug.Log("El ciclo de 5 días ha terminado.");
            }
            else
            {
                timeStopped = true;
                // Actualizar el texto del día en el UI
                UpdateDayText();
                GameManager.Instance.EndDay();

            }
        }
    }

    // Actualizar el texto del tiempo (formato: 9:00, 10:30, etc.)
    void UpdateTimeText()
    {
        int hours = Mathf.FloorToInt(gameTime);
        int minutes = Mathf.FloorToInt((gameTime - hours) * 60);

        //if (minutes == 30) print(gameTime);

        UIManager.Instance.UpdateTime_UI(hours, minutes);
    }

    // Actualizar el texto del día, mostrando tanto el número como el nombre
    void UpdateDayText()
    {
        string dayName = dayNames[(currentDay) % dayNames.Length];

        UIManager.Instance.UpdateDay_UI(dayName);
    }

}