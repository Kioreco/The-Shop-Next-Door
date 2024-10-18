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
        happinessProgress = 50.0f;
        friendshipProgress = 30.0f;
        developmentProgress = 0.0f;
        romanticProgress = 0.0f;
    }


    public void UpdateLifeProgress(float rom, float friend, float grow, float happy, float rest)
    {
        romanticProgress = Mathf.Clamp(romanticProgress + rom, 0.0f, 100.0f);
        friendshipProgress = Mathf.Clamp(friendshipProgress + friend, 0.0f, 100.0f);
        developmentProgress = Mathf.Clamp(developmentProgress + grow, 0.0f, 100.0f);
        happinessProgress = Mathf.Clamp(happinessProgress + happy, 0.0f, 100.0f);
        restProgress = Mathf.Clamp(restProgress + rest, 0.0f, 100.0f);
    }
}
