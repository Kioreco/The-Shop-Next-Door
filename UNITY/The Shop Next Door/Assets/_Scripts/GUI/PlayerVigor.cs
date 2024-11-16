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

    public bool vigorEnabled = true;
    public bool vigorFill = false;
    public bool vigorDiminish = true;

    private void Update()
    {
        if (!vigorEnabled) return;

        if (vigorDiminish) 
        {
            if (!stressLevelMAX)
            {
                DiminishVigor();
            }
            else
            {
                GameManager.Instance._player.disableMovement();
                vigorDiminish = false;
                vigorFill = true;
            }
        }

        if (vigorFill)
        {
            ReplenishVigor();
            if (vigor_bar.fillAmount == 1)
            {
                vigorFill = false;
                stressLevel = 0;
                stressLevelChanged = true;

                if (stressLevelMAX)
                {
                    GameManager.Instance._player.enableMovement(false);
                    stressLevelMAX = false;
                    vigorDiminish = true;
                }
            }
        }
        
    }

    private void DiminishVigor()
    {
        if (stressLevelMAX) 
        {
            GameManager.Instance._player.disableMovement();
            ReplenishVigor();
            if (vigor_bar.fillAmount == 1) { stressLevelMAX = false; GameManager.Instance._player.enableMovement(false); stressLevel = 0; }

            return; 
        }

        if (stressLevel == 0)
        {
            if (stressLevelChanged) 
            { 
                ChangeVigorColor(stressLevel);
                GameManager.Instance._player.ChangePlayerSpeed(5f);
                stressLevelChanged = false;
                //Cambiar el plumbob del player
            }
            vigor_bar.fillAmount -= Time.deltaTime * 0.02f;
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
                GameManager.Instance._player.ChangePlayerSpeed(2.5f);
                stressLevelChanged = false;
                //Cambiar el plumbob del player
            }
            vigor_bar.fillAmount -= Time.deltaTime * 0.04f;
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
                GameManager.Instance._player.ChangePlayerSpeed(1.5f);
                stressLevelChanged = false;
                //Cambiar el plumbob del player
            }

            vigor_bar.fillAmount -= Time.deltaTime * 0.06f;
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

    private void ReplenishVigor()
    {
        vigor_bar.fillAmount += Time.deltaTime * 0.08f;
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
