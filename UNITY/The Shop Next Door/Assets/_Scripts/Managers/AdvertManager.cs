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
    public int reward;
    private int _random;

    void Start()
    {
        //Debug.Log("Recompensa inicial: " + reward);
        reward = GameManager.Instance.inheritance;
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void ShowAdd()
    {
        if (trailerVideos.Count == 0)
        {
            Debug.LogWarning("No hay vídeos");
            return;
        }
        AudioManager.Instance.StopBackgroundMusic("Musica_InGame");
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
        AudioManager.Instance.PlayBackgroundMusic("Musica_InGame");
    }

    private void MultiplyReward()
    {
        reward *= 2;
        Debug.Log("Recompensa multiplicada: " + reward);
        UIManager.Instance.inheritance_text.SetText(reward.ToString());
        UIManager.Instance.ButtonDuplicateReward.SetActive(false);
    }
}
