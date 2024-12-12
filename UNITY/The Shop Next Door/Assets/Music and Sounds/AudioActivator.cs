using UnityEngine;

public class AudioActivator : MonoBehaviour
{
    public bool isMenuInicio;
    private void OnEnable()
    {
        if (isMenuInicio)
        {
            if (AudioManager.Instance.isPlaying("Musica_InGame"))
            {
                AudioManager.Instance.StopBackgroundMusic("Musica_InGame");
            }
            else if (AudioManager.Instance.isPlaying("MusicaFinal"))
            {
                AudioManager.Instance.StopBackgroundMusic("MusicaFinal");
            }
            if (!AudioManager.Instance.isPlaying("Musica_Menu")) { AudioManager.Instance.PlayBackgroundMusic("Musica_Menu"); print("no esta sonando musica menu"); }
        }
        else
        {
            AudioManager.Instance.StopBackgroundMusic("Musica_Menu");
            AudioManager.Instance.PlayBackgroundMusic("Musica_InGame");
        }
    }
}