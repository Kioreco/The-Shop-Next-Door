using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("TITLE SCENE")]
    [SerializeField] private GameObject TSND_Logo;

    [SerializeField] private GameObject LoginCreateButtons;

    [SerializeField] private GameObject LoginCreatePanel;

    public static UIManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CloseTitleScreen()
    {
        TSND_Logo.SetActive(false);
        LoginCreateButtons.SetActive(true);
    }

    public void OpenLoginCampus()
    {
        LoginCreateButtons.SetActive(false);
        LoginCreatePanel.SetActive(true);
    }

}
