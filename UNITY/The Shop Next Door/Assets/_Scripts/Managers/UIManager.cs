using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //[Header("TITLE SCENE")]

    //[Header("MENU SCENE")]
    //[Header("MATCHMAKING SCENE")]
    //[Header("MATCHMAKING SCENE")]
    //[SerializeField] private GameObject

    //[SerializeField] private bool telephoneMini;
    [Header("INGAME SCENE")]
    [SerializeField] private TextMeshProUGUI dineroJugador_text;
    [SerializeField] private TextMeshProUGUI nombreTienda_text;


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

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void UpdateDineroJugador()
    {
        dineroJugador_text.SetText(GameManager.Instance.dineroJugador.ToString());
        if(GameManager.Instance.dineroJugador < 0) { dineroJugador_text.color = Color.red; }
    }
}
