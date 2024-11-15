using UnityEngine;
using UnityEngine.UI;

public class PlayerVigor : MonoBehaviour
{

    [Header("Vigor UI")]
    [SerializeField] private Image vigor_bar;
    [SerializeField] private Image vigor_background;

    [Header("Faces - Gemma")]
    [SerializeField] private GameObject[] facesGemma;

    [Header("Faces - Emma")]
    [SerializeField] private GameObject[] facesEmma;

    private Color greenColor = new Color(0.05f, 0.78f, 0.29f);
    private Color yellowColor = new Color(0.74f, 0.68f, 0f);
    private Color orangeColor = new Color(0.92f, 0.47f, 0f);

    private int stressLevel = 0;
    private bool stressLevelChanged = true;
    private bool stressLevelMAX = false;

    private void DecreaseVigor()
    {
        if (stressLevelMAX) { return; //Descanso obligatorio
                                      }

        if (stressLevel == 0)
        {
            if (stressLevelChanged) 
            { 
                ChangeVigorColor(stressLevel);
                stressLevelChanged = false;
                //Cambiar la burbuja del player y la velocidad
            }
            vigor_bar.fillAmount -= Time.deltaTime * 0.2f;
            if (vigor_bar.fillAmount <= 0.5f)
            {
                stressLevel = 1;
                stressLevelChanged = true;
            }
        }

        if (stressLevel == 1)
        {
            if (stressLevelChanged)
            {
                ChangeVigorColor(stressLevel);
                stressLevelChanged = false;
                //Cambiar la burbuja del player y la velocidad
            }
            vigor_bar.fillAmount -= Time.deltaTime * 0.4f;
            if (vigor_bar.fillAmount <= 0.25f)
            {
                stressLevel = 2;
                stressLevelChanged = true;
            }
        }

        if (stressLevel == 2)
        {
            if (stressLevelChanged)
            {
                ChangeVigorColor(stressLevel);
                stressLevelChanged = false;
                //Cambiar la burbuja del player y la velocidad
            }
            
            vigor_bar.fillAmount -= Time.deltaTime * 0.6f;
            if (vigor_bar.fillAmount <= 0.05f)
            {
                stressLevelMAX = true;
            }
        }
    }


    private void ChangeVigorColor(int level)
    {
        // Verde
        if (level == 0)
        {
            vigor_background.color = greenColor;
        }
        // Amarillo
        else if (level == 1)
        {
            vigor_background.color = yellowColor;
        }
        //Naranja
        else if (level == 2)
        {
            vigor_background.color = orangeColor;
        }
    }

    public void SetNewFaceGemma(int numberState)
    {
        foreach (GameObject face in facesGemma) { face.SetActive(false); }
        facesGemma[numberState].SetActive(true);
    }
    public void SetNewFaceEmma(int numberState)
    {
        foreach (GameObject face in facesEmma) { face.SetActive(false); }
        facesEmma[numberState].SetActive(true);
    }

}
