using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class AdvertManager : MonoBehaviour
{
    public GameObject panelPublicity;     
    public VideoPlayer videoPlayer;        
    public List<VideoClip> trailerVideos;       
    private bool _isReward;
    public int reward = 100;
    private int _random;

    void Start()
    {
        Debug.Log("Recompensa inicial: " + reward);
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void ShowSimulatedAdd()
    {
        if (trailerVideos.Count == 0)
        {
            Debug.LogWarning("No hay vídeos"); //Por si acaso se nos olvida meter o se cambian nombres de variables y nos rayamos
            return;
        }

        _isReward = false;          
        panelPublicity.SetActive(true); 

        _random = Random.Range(0, trailerVideos.Count);
        videoPlayer.clip = trailerVideos[_random];

        videoPlayer.Play();            
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        if (!_isReward)
        {
            MultiplyReward();      
            _isReward = true;  
        }
        panelPublicity.SetActive(false);
    }

    private void MultiplyReward()
    {
        reward *= 2;
        Debug.Log("Recompensa multiplicada: " + reward);
    }
}
