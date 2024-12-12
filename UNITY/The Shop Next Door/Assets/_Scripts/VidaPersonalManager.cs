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
        restProgress = 0.0f;
        happinessProgress = 0.0f;
        friendshipProgress = 0.0f;
        developmentProgress = 0.0f;
        romanticProgress = 0.0f;
    }


    public void UpdateLifeProgress(int totalAct, float rom, float friend, float grow, float happy, float rest)
    {
        romanticProgress    += Mathf.Clamp((rom / totalAct) * 23.0f, 0, 100);
        friendshipProgress  += Mathf.Clamp((friend / totalAct) * 23.0f, 0, 100);
        developmentProgress += Mathf.Clamp((grow / totalAct) * 23.0f, 0, 100);
        happinessProgress   += Mathf.Clamp((happy / totalAct) * 23.0f, 0, 100);
        restProgress        += Mathf.Clamp((rest / totalAct) * 23.0f, 0, 100);
    }
}
