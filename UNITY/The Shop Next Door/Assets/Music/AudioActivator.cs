using UnityEngine;

public class AudioActivator : MonoBehaviour
{
    public bool isMenuInicio;
    private void Awake()
    {
        if (isMenuInicio)
        {
            if (AudioManager.Instance.isPlaying("Musica_InGame"))
            {
                AudioManager.Instance.StopBackgroundMusic("Musica_InGame");
            }
            if (!AudioManager.Instance.isPlaying("Musica_Menu")) AudioManager.Instance.PlayBackgroundMusic("Musica_Menu");
        }
        else
        {
            AudioManager.Instance.StopBackgroundMusic("Musica_Menu");
            AudioManager.Instance.PlayBackgroundMusic("Musica_InGame");
        }
    }
}