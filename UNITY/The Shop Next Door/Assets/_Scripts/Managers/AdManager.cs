using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

// refs: https://www.youtube.com/watch?v=y-lr1loC5Js

public class AdManager : MonoBehaviour
{
    public GameObject panelPublicity;     
    public VideoPlayer videoPlayer;        
    public List<string> trailerVideoUrls;       
    private bool _isReward;
    public int reward;
    private int _random;

    void Start()
    {
        reward = 10;
        reward = GameManager.Instance.inheritance;
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void ShowAd()
    {
        if (trailerVideoUrls.Count == 0)
        {
            Debug.LogWarning("No hay vídeos");
            return;
        }
        
        AudioManager.Instance.StopBackgroundMusic("Musica_InGame");
        _isReward = false;          
        panelPublicity.SetActive(true); 

        _random = Random.Range(0, trailerVideoUrls.Count);
        videoPlayer.url = trailerVideoUrls[_random];

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
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
        AWSManager.Instance.gemsAmount += reward;
        AWSManager.Instance.UpdateGems(AWSManager.Instance.gemsAmount);
        AudioManager.Instance.PlayBackgroundMusic("Musica_InGame");
    }

    private void MultiplyReward()
    {
        reward *= 2;
        UIManager.Instance.inheritance_text.SetText(reward.ToString());
        UIManager.Instance.ButtonDuplicateReward.SetActive(false);
    }
}
