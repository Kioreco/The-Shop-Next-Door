using System.Collections;
using System.Collections.Generic;
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

}
