using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Fuentes de Sonido")]
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource effectsPlayer;

    [Header("Clips de Sonido")]
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip[] effectClips;

    //Variables Singleton
    public static new AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic("Musica_Menu");
    }

    public void PlaySound(String effect)
    {
        AudioClip _audioClip = Array.Find(effectClips, clip => clip.name == effect);

        if (_audioClip == null)
        {
            Debug.LogWarning("No se ha encontrado un EFECTO con ese nombre");
        }
        else
        {
            effectsPlayer.PlayOneShot(_audioClip);
        }
    }

    public void PlayBackgroundMusic(String backgroundMusic)
    {
        print("inicio background music");

        AudioClip _audioClip = Array.Find(musicClips, clip => clip.name == backgroundMusic);

        if (_audioClip == null)
        {
            Debug.LogWarning("No se ha encontrado una MUSICA con ese nombre");
        }
        else
        {
            musicPlayer.clip = _audioClip;
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
    }

    public void StopBackgroundMusic(String backgroundMusic)
    {
        print("stop background music");
        AudioClip _audioClip = Array.Find(musicClips, clip => clip.name == backgroundMusic);

        if (_audioClip == null)
        {
            Debug.LogWarning("No se ha encontrado una MUSICA con ese nombre que parar");
        }
        else
        {
            musicPlayer.clip = _audioClip;
            musicPlayer.Stop();
        }
    }

    public bool isPlaying(String backgroundMusic)
    {
        AudioClip _audioClip = Array.Find(musicClips, clip => clip.name == backgroundMusic);

        if (_audioClip == null)
        {
            //No hay ninguna cancion de fondo sonando
            return false;
        }
        else
        {
            musicPlayer.clip = _audioClip;
            return musicPlayer.isPlaying;
        }
    }

}
