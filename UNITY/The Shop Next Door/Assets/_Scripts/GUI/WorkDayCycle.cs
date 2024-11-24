using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkDayCycle : MonoBehaviour
{
    public float gameTime = 0.0f;
    public int currentDay = 0;
    private int totalDays = 4;
    public string[] dayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

    private float realTimePerDay = 30f;    // TIEMPO POR DÍA -----> 240
    private float realTimePerNight = 5f;    // TIEMPO POR NOCHE -----> 15 segundos

    private float gameStartTime = 9f;       // HORA DE ENTRAR A LA TIENDA
    //private float gameClientTime = 10f;     // HORA EN LA QUE ENTRAN LOS CLIENTES
    private float gameEndTime = 15f;        // HORA DE CIERRE
    //private float gameHoursPerDay = 6f;     // HORAS DE TRABAJO POR DÍA
    private float nightStartTime = 22f;       // HORA DE ENTRAR A LA TIENDA
    private float nightEndTime = 24f;       // HORA DE ENTRAR A LA TIENDA

    private float realTimePassed = 0f;      // TIEMPO EN SEGUNDOS QUE HAN PASADO

    private bool cycleCompleted = false;    // SEMANA COMPLETA
    public bool timeStopped = false;        // TIEMPO PARADO

    public ClientPrototype npcClientGenerico;
    public ClientPrototype npcClientKaren;
    public ClientPrototype npcClientTacanio;
    [SerializeField] private GameObject[] pastDay_Bg;
    [SerializeField] private GameObject[] followingDay_Bg;

    private bool isNightTime = false;

    //eventos para los clientes
    public event EventHandler eventTwoHoursLeft;
    public event EventHandler eventThirtyMinutesLeft;
    public event EventHandler eventTenMinutesLeft;
    bool eventLanzadoTwoHours = false;
    bool eventLanzadoOneHours = false;
    bool eventLanzadoTenMinutes = false;
    bool eventLanzadoInicioDia = false;
    bool eventLanzadoThirtyMinutes = false;

    public event EventHandler dayFinish;


    void Update()
    {
        if (!eventLanzadoInicioDia)
        {
            dayFinish?.Invoke(this, EventArgs.Empty);
            //print("alerta inciial");
            eventLanzadoInicioDia = true;
            UIManager.Instance.ActivateAlertBuySupplies(5f);
        }
        if (isNightTime)
        {
            NightTimer();
        }

        if (timeStopped) return;

        // SEMANA COMPLETADA
        if (cycleCompleted)
        {
            gameObject.SetActive(false);
            GameManager.Instance.FinalResult();
            SceneManager.LoadScene("4 - FinalScene");
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

        if (gameTime >= 10 && gameTime < 10.05 && currentDay == 0) // Lunes
        {
            npcClientGenerico.isEnable = true;
            npcClientKaren.isEnable = true;
            npcClientTacanio.isEnable = true;
        }
        else if (gameTime >= 9.3f && gameTime < 9.35f && currentDay > 0)
        {
            npcClientGenerico.isEnable = true;
            npcClientKaren.isEnable = true;
            npcClientTacanio.isEnable = true;
        }

        if (gameTime >= 13f && !eventLanzadoTwoHours)
        {
            print("evento 2 horas");

            eventLanzadoTwoHours = true;
            eventTwoHoursLeft?.Invoke(this, EventArgs.Empty);
        }
        
        if (gameTime >= 13.5f && !eventLanzadoOneHours)
        {
            eventLanzadoOneHours = true;
            //eventThirtyMinutesLeft?.Invoke(this, EventArgs.Empty);
            npcClientGenerico.isEnable = false;
            npcClientKaren.isEnable = false;
            npcClientTacanio.isEnable = false;
        }
        if (gameTime>= 14.5f && !eventLanzadoThirtyMinutes)
        {
            //print("evento 30 minutos");
            eventLanzadoThirtyMinutes = true;
            UIManager.Instance.ActivateAlertThirtyMinutesLeft(5f);
        }

        if (gameTime >= 14.75f && !eventLanzadoTenMinutes)
        {
            //print("quedan 10 minutos");
            UIManager.Instance.ActivateAlertGoOutShop(5f);
            eventLanzadoTenMinutes = true;
            eventTenMinutesLeft?.Invoke(this, EventArgs.Empty);
        }


        // Actualizar la UI para mostrar la hora del juego
        UpdateTimeText();

        // SE HA ACABADO EL DÍA
        if (realTimePassed >= realTimePerDay)
        {
            cycleCompleted = true; //QUITAR
            eventLanzadoTwoHours = false;
            eventLanzadoOneHours = false;
            eventLanzadoTenMinutes = false;
            eventLanzadoInicioDia = false;
            eventLanzadoThirtyMinutes = false;

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