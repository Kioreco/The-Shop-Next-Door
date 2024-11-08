using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class VidaPersonalManager : MonoBehaviour
{
    [Header("Life Progress")]
    [HideInInspector] public float romanticProgress;
    [HideInInspector] public float friendshipProgress;
    [HideInInspector] public float developmentProgress;
    [HideInInspector] public float happinessProgress;
    [HideInInspector] public float restProgress;

    [Header("Life Posibilities")]
    [HideInInspector] public bool hasPartner;

    #region Singleton & Awake
    public static VidaPersonalManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void InitializeProgress()
    {
        restProgress = 100.0f;
        happinessProgress = 100.0f;
        friendshipProgress = 100.0f;
        developmentProgress = 100.0f;
        romanticProgress = 100.0f;
    }


    public void UpdateLifeProgress(int totalAct, float rom, float friend, float grow, float happy, float rest)
    {
        romanticProgress    += (rom / totalAct) * 20.0f;
        friendshipProgress  += (friend / totalAct) * 20.0f;
        developmentProgress += (grow / totalAct) * 20.0f;
        happinessProgress   += (happy / totalAct) * 20.0f;
        restProgress        += (rest / totalAct) * 20.0f;
    }
}
