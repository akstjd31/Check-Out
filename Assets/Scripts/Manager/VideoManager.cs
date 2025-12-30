using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : Singleton<VideoManager>
{
    [SerializeField] private VideoClip walker;
    [SerializeField] private VideoClip siren;
    [SerializeField] private VideoClip echo;
    [SerializeField] private VideoClip mannequin;
    [SerializeField] private RenderTexture monsterTexture;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Image Image;

    private bool isPlaying = false;

    public void Init() => Image = FindAnyObjectByType<RawImageTracker>().GetComponentInChildren<Image>(true);
    
    private void OnEnable()
    {
        videoPlayer.loopPointReached += OnVideoEnded;
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoEnded;
    }

    public void OnVideoEnded(VideoPlayer vp)
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SoundManager.Instance.StopSound();
        SoundManager.Instance.PlayPlayerDeathSound();
        FadeManager.Instance.FadeStartedInvoke();
        FadeManager.Instance.StartFadeOut();
    }

    public void PlayDeathVideo(Monster monster)
    {
        GameManager.Instance.isGameOver = true;
        AudioListener.pause = true;
        Time.timeScale = 0f;
        ImageActivate();
        if (monster == null)
            return;

        if (monster is WalkerModel)
        {
            isPlaying = true;
            videoPlayer.clip = walker;
        }
        else if (monster is SirenModel)
        {
            isPlaying = true;
            videoPlayer.clip = siren;
        }
        else if (monster is MannequinModel)
        {
            isPlaying = true;
            videoPlayer.clip = mannequin;
        }
        else if (monster is EchoModel)
        {
            isPlaying = true;
            videoPlayer.clip = echo;
        }

        videoPlayer.Play();
    }

    public void Update()
    {
    }

    public void ImageActivate()
    {
        Image.gameObject.SetActive(true);
    }

    public void ImageDeactivate()
    {
        Image.gameObject.SetActive(false);
    }

}
