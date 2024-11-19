using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkDayCycle : MonoBehaviour
{
    public float gameTime = 0.0f;
    public int currentDay = 0;
    private int totalDays = 4;
    public string[] dayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

    private float realTimePerDay = 90f;    // TIEMPO POR DÍA -----> 240
    private float realTimePerNight = 15f;    // TIEMPO POR DÍA -----> 15 segundos

    private float gameStartTime = 9f;       // HORA DE ENTRAR A LA TIENDA
    //private float gameClientTime = 10f;     // HORA EN LA QUE ENTRAN LOS CLIENTES
    private float gameEndTime = 15f;        // HORA DE CIERRE
    //private float gameHoursPerDay = 6f;     // HORAS DE TRABAJO POR DÍA
    private float nightStartTime = 22f;       // HORA DE ENTRAR A LA TIENDA
    private float nightEndTime = 24f;       // HORA DE ENTRAR A LA TIENDA

    private float realTimePassed = 0f;      // TIEMPO EN SEGUNDOS QUE HAN PASADO

    private bool cycleCompleted = false;    // SEMANA COMPLETA
    public bool timeStopped = false;        // TIEMPO PARADO

    public ClientPrototype npcClient;
    [SerializeField] private GameObject[] pastDay_Bg;
    [SerializeField] private GameObject[] followingDay_Bg;

    private bool isNightTime = false;

    void Update()
    {
        if (isNightTime)
        {
            NightTimer();
        }

        if (timeStopped) return;

        // SEMANA COMPLETADA
        if (cycleCompleted)
        {
            gameObject.SetActive(false);
            // ACTIVAR FINAL
        }

        DayTimer();
    }

    private void DayTimer()
    {
        // Se actualiza el tiempo que ha pasado
        realTimePassed += Time.deltaTime;

        // Calcular el tiempo en el juego (de 9:00 a 15:00)
        float timeRatio = realTimePassed / realTimePerDay;
        gameTime = Mathf.Lerp(gameStartTime, gameEndTime, timeRatio);

        //gameTime==10.0f entran clientes el lunes
        //gameTime==9.3f entran clientes el resto de dias


        if (gameTime >= 10 && currentDay == 0) // Lunes
        {
            npcClient.isEnable = true;
        }
        else if (gameTime >= 9.05f && currentDay > 0)
        {
            npcClient.isEnable = true;
        }

        if (gameTime >= 14f)
        {
            npcClient.isEnable = false;
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

                UpdateDayText();
                isNightTime = true;
                GameManager.Instance.EndDay();

                pastDay_Bg[currentDay-1].SetActive(true);
                followingDay_Bg[currentDay-1].SetActive(false);
            }
        }
    }

    private void NightTimer()
    {
        realTimePassed += Time.deltaTime;

        float timeRatio = realTimePassed / realTimePerNight;
        gameTime = Mathf.Lerp(nightStartTime, nightEndTime, timeRatio);

        UpdateTimeText();

        if (realTimePassed >= realTimePerNight)
        {
            realTimePassed = 0f; // Reiniciar el tiempo real para el próximo día

            isNightTime = false;
        }
    }

    // Actualizar el texto del tiempo
    void UpdateTimeText()
    {
        int hours = Mathf.FloorToInt(gameTime);
        int minutes = Mathf.FloorToInt((gameTime - hours) * 60);

        if (!isNightTime) { UIManager.Instance.UpdateTime_UI(hours, minutes); }
        else { UIManager.Instance.UpdateNightTime_UI(hours, minutes); }
    }

    // Actualizar el texto del día
    void UpdateDayText()
    {
        string dayName = dayNames[(currentDay) % dayNames.Length];
        string dayNameNight = dayNames[(currentDay-1) % dayNames.Length];

        UIManager.Instance.UpdateDay_UI(dayName, dayNameNight);
    }

}