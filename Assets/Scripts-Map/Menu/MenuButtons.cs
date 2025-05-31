using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip audioClip;  
    private void OnEnable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void Start()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        audioSource.Play();
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }

    public void OnStartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("click");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
